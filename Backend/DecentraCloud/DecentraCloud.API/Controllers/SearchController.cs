using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("data")]
        public async Task<IActionResult> SearchData([FromQuery] string query)
        {
            var results = await _searchService.SearchData(query);
            return Ok(results);
        }
    }
}
