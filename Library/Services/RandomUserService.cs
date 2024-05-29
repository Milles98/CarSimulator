using Library.Models;
using Library.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library.Services
{
    public class RandomUserService : IRandomUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConsoleService _consoleService;

        public RandomUserService(HttpClient httpClient, IConsoleService consoleService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
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
                    _consoleService.WriteLine("No results found in the response.");
                    return null;
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _consoleService.WriteLine($"An error occurred while fetching data from the API: {httpRequestException.Message}");
                return null;
            }
            catch (JsonReaderException jsonReaderException)
            {
                _consoleService.WriteLine($"An error occurred while deserializing the response: {jsonReaderException.Message}");
                return null;
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                _consoleService.WriteLine($"An error occurred while deserializing the response: {jsonSerializationException.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _consoleService.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
