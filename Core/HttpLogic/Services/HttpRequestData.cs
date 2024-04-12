namespace Core.HttpLogic.Services;

public record HttpRequestData
{
    /// <summary>
    ///     Тип метода
    /// </summary>
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public HttpMethod Method { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.

    /// <summary>
    ///     Адрес запроса
    /// </summary>
    /// \
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public Uri Uri { set; get; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.

    /// <summary>
    ///     Тело метода
    /// </summary>
#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    public object Body { get; set; }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.

    /// <summary>
    ///     content-type, указываемый при запросе
    /// </summary>
    public ContentType ContentType { get; set; } = ContentType.ApplicationJson;

    /// <summary>
    ///     Заголовки, передаваемые в запросе
    /// </summary>
    public IDictionary<string, string> HeaderDictionary { get; set; } = new Dictionary<string, string>();

    /// <summary>
    ///     Коллекция параметров запроса
    /// </summary>
    public ICollection<KeyValuePair<string, string>> QueryParameterList { get; set; } =
        new List<KeyValuePair<string, string>>();
}