using SearchEngine.Models;

namespace SearchEngine.Services
{
    public interface ISearchServiceListService
    {
        public Task<ServiceOutputResponse> GetServiceDetailsByName(string serviceName , string location);
    }
}