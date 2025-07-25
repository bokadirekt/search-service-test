using Microsoft.AspNetCore.Mvc;
using SearchEngine.Models;
using SearchEngine.Services;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SearchEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchServiceController : ControllerBase
    {

        public SearchServiceController()
        {
            
        }

        /// <summary>
        /// Get latest uploaded DTC:s from ECC for ProductId decorated with content data
        /// </summary>        
        /// <param name="serviceName" example="Massage">The Service Name</param>
        /// <param name="location" example="Stockholm">The Location of the service</param>
        /// <returns>matched services name,locations and others details</returns>
        [HttpGet("service-details-by-name")]
        [ProducesResponseType(typeof(ServiceOutputResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetServiceDetailsbyName([Required][FromQuery][FromRoute(Name = "service-name")] string serviceName,
            [Required][FromQuery][FromRoute(Name = "location")] string location,[FromServices] ISearchServiceListService searchServiceListService)
        {
            try
            {
                var result = await searchServiceListService.GetServiceDetailsByName(serviceName, location);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
