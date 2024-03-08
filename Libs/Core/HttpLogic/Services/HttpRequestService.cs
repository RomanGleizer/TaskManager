using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net;
using System.Text;
using Core.HttpLogic.Services.Interfaces;
using Core.TraceLogic.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Core.HttpLogic.Services;

public enum ContentType
{
    ///
    Unknown = 0,

    ///
    ApplicationJson = 1,

    ///
    XWwwFormUrlEncoded = 2,

    ///
    Binary = 3,

    ///
    ApplicationXml = 4,

    ///
    MultipartFormData = 5,

    /// 
    TextXml = 6,

    /// 
    TextPlain = 7,

    ///
    ApplicationJwt = 8
}

public record HttpRequestData
{
    /// <summary>
    /// Тип метода
    /// </summary>
    public HttpMethod Method { get; set; }

    /// <summary>
    /// Адрес запроса
    /// </summary>\
    public Uri Uri { set; get; }

    /// <summary>
    /// Тело метода
    /// </summary>
    public object Body { get; set; }

    /// <summary>
    /// content-type, указываемый при запросе
    /// </summary>
    public ContentType ContentType { get; set; } = ContentType.ApplicationJson;

    /// <summary>
    /// Заголовки, передаваемые в запросе
    /// </summary>
    public IDictionary<string, string> HeaderDictionary { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// Коллекция параметров запроса
    /// </summary>
    public ICollection<KeyValuePair<string, string>> QueryParameterList { get; set; } =
        new List<KeyValuePair<string, string>>();
}

public record BaseHttpResponse
{
    /// <summary>
    /// Статус ответа
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Заголовки, передаваемые в ответе
    /// </summary>
    public HttpResponseHeaders Headers { get; set; }

    /// <summary>
    /// Заголовки контента
    /// </summary>
    public HttpContentHeaders ContentHeaders { get; init; }

    /// <summary>
    /// Является ли статус код успешным
    /// </summary>
    public bool IsSuccessStatusCode
    {
        get
        {
            var statusCode = (int)StatusCode;

            return statusCode >= 200 && statusCode <= 299;
        }
    }
}

public record HttpResponse<TResponse> : BaseHttpResponse
{
    /// <summary>
    /// Тело ответа
    /// </summary>
    public TResponse Body { get; set; }
}

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

    public async Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, HttpConnectionData connectionData)
    {
        using var client = _httpConnectionService.CreateHttpClient(connectionData);

        var httpRequestMessage = BuildRequestMessage(requestData);
        var response = await client.SendAsync(httpRequestMessage);
        var responseBody = await response.Content.ReadAsStringAsync();

        TResponse deserializedResponse = default;
        try
        {
            deserializedResponse = JsonConvert.DeserializeObject<TResponse>(responseBody);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing response: {ex.Message}");
        }

        return new HttpResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            Headers = response.Headers,
            ContentHeaders = response.Content.Headers,
            Body = deserializedResponse
        };
    }

    private static HttpContent PrepairContent(object body, ContentType contentType)
    {
        switch (contentType)
        {
            case ContentType.ApplicationJson:
                {
                    if (body is string stringBody)
                    {
                        body = JToken.Parse(stringBody);
                    }

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
                    {
                        throw new Exception(
                            $"Body for content type {contentType} must be {typeof(IEnumerable<KeyValuePair<string, string>>).Name}");
                    }

                    return new FormUrlEncodedContent(list);
                }
            case ContentType.ApplicationXml:
                {
                    if (body is not string s)
                    {
                        throw new Exception($"Body for content type {contentType} must be XML string");
                    }

                    return new StringContent(s, Encoding.UTF8, MediaTypeNames.Application.Xml);
                }
            case ContentType.Binary:
                {
                    if (body.GetType() != typeof(byte[]))
                    {
                        throw new Exception($"Body for content type {contentType} must be {typeof(byte[]).Name}");
                    }

                    return new ByteArrayContent((byte[])body);
                }
            case ContentType.TextXml:
                {
                    if (body is not string s)
                    {
                        throw new Exception($"Body for content type {contentType} must be XML string");
                    }

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
                      ? new StringContent(JsonConvert.SerializeObject(requestData.Body), Encoding.UTF8, requestData.ContentType.ToString())
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