using Library.Models;
using Newtonsoft.Json;

namespace Library.Services
{
    public class RandomUserService
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<Driver> GetRandomDriverAsync()
        {
            string url = "https://randomuser.me/api/";
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
            return null;
        }
    }
}
