using Library.Models;
using Library.Services.Interfaces;
using Newtonsoft.Json;

namespace Library.Services
{
    public class RandomUserService : IRandomUserService
    {
        private readonly HttpClient httpClient;

        public RandomUserService()
        {
            httpClient = new HttpClient();
        }

        public async Task<Driver> GetRandomDriverAsync()
        {
            string url = "https://randomuser.me/api/";
            try
            {
                var response = await httpClient.GetStringAsync(url);
                var randomUserResponse = JsonConvert.DeserializeObject<RandomUserResponse>(response);

                if (randomUserResponse?.Results != null && randomUserResponse.Results.Count > 0)
                {
                    var user = randomUserResponse.Results[0];
                    return new Driver
                    {
                        FirstName = user.Name.First,
                        LastName = user.Name.Last
                    };
                }
                else
                {
                    Console.WriteLine("No results found in the response.");
                    return null;
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                Console.WriteLine($"An error occurred while fetching data from the API: {httpRequestException.Message}");
                return null;
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                Console.WriteLine($"An error occurred while deserializing the response: {jsonSerializationException.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
