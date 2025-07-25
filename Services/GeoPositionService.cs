using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SearchEngine.Services
{
    public class GeoPositionService
    {
        private readonly HttpClient _httpClient = new();
        private const string ApiKey = "5b7519792dbd4a54ba8f14572efa63ca"; // Replace with your actual key

        public async Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string location)
        {
            var encoded = Uri.EscapeDataString(location);
            var url = $"https://api.opencagedata.com/geocode/v1/json?q={encoded}&key={ApiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JObject.Parse(response);

            var firstResult = json["results"]?.FirstOrDefault();
            if (firstResult == null)
                throw new Exception("Location not found");

            var geometry = firstResult["geometry"];
            return ((double)geometry["lat"], (double)geometry["lng"]);
        }
    }
}
