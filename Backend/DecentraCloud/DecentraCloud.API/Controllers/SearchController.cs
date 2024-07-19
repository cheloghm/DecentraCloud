using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("search-files")]
        public async Task<IActionResult> SearchFiles(string query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var results = await _searchService.SearchFiles(userId, query);
            return Ok(results);
        }
    }
}
