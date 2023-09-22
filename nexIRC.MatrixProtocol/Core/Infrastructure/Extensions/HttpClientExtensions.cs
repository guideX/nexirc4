using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using nexIRC.MatrixProtocol;
/// <summary>
/// Http Client Extensions
/// </summary>
internal static class HttpClientExtensions {
    /// <summary>
    /// Json Serializer Settings
    /// </summary>
    /// <returns></returns>
    private static JsonSerializerSettings GetJsonSettings() {
        var contractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
        var settings = new JsonSerializerSettings {
            ContractResolver = contractResolver,
            NullValueHandling = NullValueHandling.Ignore
        };
        settings.Converters.Add(new StringEnumConverter());
        return settings;
    }
    /// <summary>
    /// Post Async
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public static async Task PostAsync(this HttpClient httpClient,
        string requestUri, CancellationToken cancellationToken) {
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, null, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new ApiException(response.RequestMessage.RequestUri, null, null, response.StatusCode);
    }
    /// <summary>
    /// Post As Json Async
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="httpClient"></param>
    /// <param name="requestUri"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public static async Task<TResponse> PostAsJsonAsync<TResponse>(this HttpClient httpClient, string requestUri, object? model, CancellationToken cancellationToken) {
        string result = null;
        JsonSerializerSettings settings = null;
        settings = GetJsonSettings();
        string json = JsonConvert.SerializeObject(model, settings);
        var content = new StringContent(json, Encoding.Default, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync(requestUri, content, cancellationToken);//.ConfigureAwait(false);
        result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new ApiException(response.RequestMessage.RequestUri, json, result, response.StatusCode);
        return JsonConvert.DeserializeObject<TResponse>(result, settings)!;
    }
    /// <summary>
    /// Put as Json Async
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="httpClient"></param>
    /// <param name="requestUri"></param>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public static async Task<TResponse> PutAsJsonAsync<TResponse>(this HttpClient httpClient,
        string requestUri, object model, CancellationToken cancellationToken) {
        JsonSerializerSettings settings = GetJsonSettings();
        string json = JsonConvert.SerializeObject(model, settings);
        var content = new StringContent(json, Encoding.Default, "application/json");
        HttpResponseMessage response = await httpClient.PutAsync(requestUri, content, cancellationToken);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(response.RequestMessage.RequestUri,
                json, result, response.StatusCode);
        return JsonConvert.DeserializeObject<TResponse>(result, settings)!;
    }
    /// <summary>
    /// Get As Json Async
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="httpClient"></param>
    /// <param name="requestUri"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    public static async Task<TResponse> GetAsJsonAsync<TResponse>(this HttpClient httpClient,
        string requestUri, CancellationToken cancellationToken) {
        HttpResponseMessage response = await httpClient.GetAsync(requestUri, cancellationToken);//.ConfigureAwait(false);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new ApiException(response.RequestMessage.RequestUri,
                null, result, response.StatusCode);
        return JsonConvert.DeserializeObject<TResponse>(result, GetJsonSettings())!;
    }
    /// <summary>
    /// Add Bearer Token
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="bearer"></param>
    public static void AddBearerToken(this HttpClient httpClient, string bearer) =>
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
}