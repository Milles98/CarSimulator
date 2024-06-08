using Library.Models;
using Library.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Library.Models.DTO;

namespace Library.Services;

public class FakePersonService(HttpClient httpClient, IConsoleService consoleService) : IFakePersonService
{
    /// <summary>
    /// Fetches a random driver from an API.
    /// </summary>
    public async Task<Driver?> GetRandomDriverAsync()
    {
        try
        {
            httpClient.BaseAddress = new Uri("https://randomuser.me/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var fakePersonResponse = JsonConvert.DeserializeObject<FakePersonResponseDTO>(responseBody);

                if (fakePersonResponse?.Results is { Count: > 0 })
                {
                    var user = fakePersonResponse.Results[0];
                    return new Driver
                    {
                        Title = user.Name.Title,
                        FirstName = user.Name.First,
                        LastName = user.Name.Last
                    };
                }
                consoleService.DisplayError("Inget resultat i response!.");
                return null;
            }
            consoleService.DisplayError($"API hämtning misslyckades med statuskod: {response.StatusCode}");
            return null;
        }
        catch (HttpRequestException httpRequestException)
        {
            consoleService.DisplayError($"Fel inträffade vid hämtning från APIet: {httpRequestException.Message}");
            return null;
        }
        catch (JsonReaderException jsonReaderException)
        {
            consoleService.DisplayError($"Fel inträffade vid deserializing av responsen från APIet: {jsonReaderException.Message}");
            return null;
        }
        catch (JsonSerializationException jsonSerializationException)
        {
            consoleService.DisplayError($"Fel inträffade vid deserializing av responsen från APIet: {jsonSerializationException.Message}");
            return null;
        }
        catch (Exception ex)
        {
            consoleService.DisplayError($"Oväntat fel: {ex.Message}");
            return null;
        }
    }
}