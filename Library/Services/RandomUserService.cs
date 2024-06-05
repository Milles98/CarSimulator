using Library.Models;
using Library.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Library.Services
{
    public class RandomUserService : IRandomUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConsoleService _consoleService;

        public RandomUserService(HttpClient httpClient, IConsoleService consoleService)
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
                    var randomUserResponse = JsonConvert.DeserializeObject<RandomUserResponse>(responseBody);

                    if (randomUserResponse?.Results != null && randomUserResponse.Results.Count > 0)
                    {
                        var user = randomUserResponse.Results[0];
                        return new Driver
                        {
                            Title = user.Name.Title,
                            FirstName = user.Name.First,
                            LastName = user.Name.Last
                        };
                    }
                    else
                    {
                        _consoleService.WriteLine("Inget resultat i response!.");
                        return null;
                    }
                }
                else
                {
                    _consoleService.WriteLine($"API hämtning misslyckades med statuskod: {response.StatusCode}");
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
                _consoleService.WriteLine($"Oväntat fel uppstod: {ex.Message}");
                return null;
            }
        }
    }
}
