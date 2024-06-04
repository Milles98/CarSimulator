using Library.Models;
using Library.Services.Interfaces;
using Newtonsoft.Json;

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
        /// Hämtar en slumpmässig förare från ett API.
        /// </summary>
        public async Task<Driver?> GetRandomDriverAsync()
        {
            string url = "https://randomuser.me/api/";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                var randomUserResponse = JsonConvert.DeserializeObject<RandomUserResponse>(response);

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
