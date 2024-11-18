using System.Text.Json;
using System.Web;

public class BaseClient
{
    public async Task<TOut> Get<TOut>(string urlEndpoint, Dictionary<string, string>? parameters = null)
    {
        var converted = new List<KeyValuePair<string, string>>();
        if (parameters != null)
        {
            converted = parameters.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value)).ToList();
        }

        return await Get<TOut>(urlEndpoint, converted);
    }

    private async Task<TOut> Get<TOut>(string urlEndpoint, List<KeyValuePair<string, string>>? parameters = null)
    {
        var uri = new Uri(urlEndpoint);

        var response = await Execute(HttpMethod.Get, uri, parameters);

        try
        {
            return await TryDeserialize<TOut>(response);

        } catch (Exception ex)
        {
           throw new Exception("Invalid response type.", ex);
        }
    }

    private async Task<HttpResponseMessage> Execute(HttpMethod metode, Uri uri, List<KeyValuePair<string, string>>? parameters)
    {
        var restRequest = new HttpRequestMessage()
        {
            Method = metode,
        };

        var uriBuilder = new UriBuilder(uri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        if (parameters != null && parameters.Count > 0)
        {
            foreach (var parameter in parameters)
            {
                query.Add(parameter.Key, parameter.Value);
            }
        }

        uriBuilder.Query = query.ToString();
        restRequest.RequestUri = uriBuilder.Uri;

        var client = new HttpClient();
        var response = await client.SendAsync(restRequest);

        return response;
    }

    private async Task<TOut> TryDeserialize<TOut>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var convertedResponse = JsonSerializer.Deserialize<TOut>(content, options);

        return convertedResponse;
    }
}