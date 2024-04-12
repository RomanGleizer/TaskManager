using System.Net.Mime;
using System.Text;
using System.Web;
using Core.HttpLogic.Services.Interfaces;
using Core.TraceLogic.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.HttpLogic.Services;

internal class HttpRequestService : IHttpRequestService
{
    private readonly IHttpConnectionService _httpConnectionService;
    private readonly IEnumerable<ITraceWriter> _traceWriterList;

    public HttpRequestService(
        IHttpConnectionService httpConnectionService,
        IEnumerable<ITraceWriter> traceWriterList)
    {
        _httpConnectionService = httpConnectionService;
        _traceWriterList = traceWriterList;
    }

    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData,
        HttpConnectionData connectionData)
    {
        using var client = _httpConnectionService.CreateHttpClient(connectionData);

        var httpRequestMessage = BuildRequestMessage(requestData);
        var response = await client.SendAsync(httpRequestMessage);
        var responseBody = await response.Content.ReadAsStringAsync();

        TResponse? deserializedResponse = default;
        try
        {
            deserializedResponse = JsonConvert.DeserializeObject<TResponse?>(responseBody);
        }
        catch (JsonException ex)
        {
            throw new JsonException(ex.Message);
        }

#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        return new HttpResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            Headers = response.Headers,
            ContentHeaders = response.Content.Headers,
            Body = deserializedResponse
        };
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
    }

    private static HttpContent PrepairContent(object body, ContentType contentType)
    {
        switch (contentType)
        {
            case ContentType.ApplicationJson:
            {
                if (body is string stringBody) body = JToken.Parse(stringBody);

                var serializeSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                var serializedBody = JsonConvert.SerializeObject(body, serializeSettings);
                var content = new StringContent(serializedBody, Encoding.UTF8, MediaTypeNames.Application.Json);
                return content;
            }

            case ContentType.XWwwFormUrlEncoded:
            {
                if (body is not IEnumerable<KeyValuePair<string, string>> list)
                    throw new Exception(
                        $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");

                return new FormUrlEncodedContent(list);
            }
            case ContentType.ApplicationXml:
            {
                if (body is not string s)
                    throw new Exception($"Body for content type {contentType} must be XML string");

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
            }
            case ContentType.Binary:
            {
                if (body.GetType() != typeof(byte[]))
                    throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");

                return new ByteArrayContent((byte[])body);
            }
            case ContentType.TextXml:
            {
                if (body is not string s)
                    throw new Exception($"Body for content type {contentType} must be XML string");

                return new StringContent(s, Encoding.UTF8, MediaTypeNames.Text.Xml);
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null);
        }
    }

    private HttpRequestMessage BuildRequestMessage(HttpRequestData requestData)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = requestData.Method,
            RequestUri = requestData.Uri,
            Content = requestData.Body != null
                ? new StringContent(JsonConvert.SerializeObject(requestData.Body), Encoding.UTF8,
                    requestData.ContentType.ToString())
                : null
        };

        // Headers
        foreach (var header in requestData.HeaderDictionary)
            httpRequestMessage.Headers.Add(header.Key, header.Value);

        // Query Parameters
        var uriBuilder = new UriBuilder(httpRequestMessage.RequestUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var queryParameter in requestData.QueryParameterList)
            query[queryParameter.Key] = queryParameter.Value;

        uriBuilder.Query = query.ToString();
        httpRequestMessage.RequestUri = uriBuilder.Uri;

        // Trace Headers
        foreach (var traceWriter in _traceWriterList)
            httpRequestMessage.Headers.Add(traceWriter.Name, traceWriter.GetValue());

        return httpRequestMessage;
    }
}