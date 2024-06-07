using Library.Models;
using Library.Models.APIResponse;
using Library.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Library.Services
{
    public class FakePersonService : IFakePersonService
    {
        private readonly HttpClient _httpClient;
        private readonly IConsoleService _consoleService;

        public FakePersonService(HttpClient httpClient, IConsoleService consoleService)
        {
            _httpClient = httpClient;
            _consoleService = consoleService;
        }

        /// <summary>
        /// Fetches a random driver from an API.
        /// </summary>
        public async Task<Driver> GetRandomDriverAsync()
        {
            try
            {
                _httpClient.BaseAddress = new Uri("https://randomuser.me/api/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var fakePersonResponse = JsonConvert.DeserializeObject<FakePersonResponseDTO>(responseBody);

                    if (fakePersonResponse?.Results != null && fakePersonResponse.Results.Count > 0)
                    {
                        var user = fakePersonResponse.Results[0];
                        return new Driver
                        {
                            Title = user.Name.Title,
                            FirstName = user.Name.First,
                            LastName = user.Name.Last
                        };
                    }
                    else
                    {
                        _consoleService.DisplayError("Inget resultat i response!.");
                        return null;
                    }
                }
                else
                {
                    _consoleService.DisplayError($"API hämtning misslyckades med statuskod: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _consoleService.WriteLine($"Fel inträffade vid hämtning från APIet: {httpRequestException.Message}");
                return null;
            }
            catch (JsonReaderException jsonReaderException)
            {
                _consoleService.WriteLine($"Fel inträffade vid deserializing av responsen från APIet: {jsonReaderException.Message}");
                return null;
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                _consoleService.WriteLine($"Fel inträffade vid deserializing av responsen från APIet: {jsonSerializationException.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _consoleService.DisplayError($"Oväntat fel: {ex.Message}");
                return null;
            }
        }
    }
}
