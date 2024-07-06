using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNode([FromBody] NodeRegistrationDto nodeRegistrationDto)
        {
            var node = await _nodeService.RegisterNode(nodeRegistrationDto);

            if (node == null)
            {
                return BadRequest(new { message = "Node registration failed." });
            }

            return Ok(node);
        }

        [HttpPost("status")]
        public async Task<IActionResult> UpdateNodeStatus([FromBody] NodeStatusDto nodeStatusDto)
        {
            var result = await _nodeService.UpdateNodeStatus(nodeStatusDto);

            if (!result)
            {
                return BadRequest(new { message = "Failed to update node status." });
            }

            return Ok(new { message = "Node status updated successfully." });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNodes()
        {
            var nodes = await _nodeService.GetAllNodes();
            return Ok(nodes);
        }
    }
}
