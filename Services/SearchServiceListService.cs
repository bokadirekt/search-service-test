using Newtonsoft.Json;
using SearchEngine.Models;
using System.Xml.Linq;

namespace SearchEngine.Services
{
    public class SearchServiceListService : ISearchServiceListService
    {
        private readonly GeoPositionService _geocoder;
        private readonly string _jsonFilePath;
        public SearchServiceListService(GeoPositionService geocoder) 
        {
            _geocoder = geocoder;
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");

        }

        public async Task<ServiceOutputResponse> GetServiceDetailsByName(string serviceName, string location)
        {
            var (lat, lng) = await _geocoder.GetCoordinatesAsync(location);
            var allItems = await LoadDataAsync();
            // filter the name based on name passed from input
            var filtered = allItems
       .Where(x =>
           x.Name != null &&
           x.Name.Contains(serviceName, StringComparison.OrdinalIgnoreCase)
       );

            var matching = filtered
                .Select(x =>
                {
                    // Calculate similarity score (0–100)
                    int score = FuzzyMatchHelper.CalculateScore(serviceName, x.Name);

                    // Calculate distance
                    double distanceKm = GeoUtils.CalculateDistanceInKm(lat, lng, x.Position.Lat, x.Position.Lng);
                    x.Distance = distanceKm < 1 ? $"{Math.Round(distanceKm * 1000)}m" : $"{distanceKm:F2}km";
                    x.Score = score;

                    return x;
                })
                .Where(x => x != null)
                .OrderByDescending(x => x.Score)
                .ThenBy(x =>
                {
                    double.TryParse(x.Distance?.Replace("km", "")?.Replace("m", ""), out double numericDistance);
                    return numericDistance;
                })
                .ToList();

            return new ServiceOutputResponse
            {
                TotalHits = matching.Count,
                TotalDocuments = allItems.Count,
                Results = matching
            };
        }
        private async Task<List<ResultItem>> LoadDataAsync()
        {
            if (!File.Exists(_jsonFilePath))
                throw new FileNotFoundException("Location data not found.", _jsonFilePath);

            var json = await File.ReadAllTextAsync(_jsonFilePath);
            return JsonConvert.DeserializeObject<List<ResultItem>>(json) ?? new List<ResultItem>();
        }
    }
}
