using Microsoft.AspNetCore.Mvc;
using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
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

            return Ok(new { node.Id, node.Token });
        }

        [HttpPost("status")]
        [Authorize]
        public async Task<IActionResult> UpdateNodeStatus([FromBody] NodeStatusDto nodeStatusDto)
        {
            // Only allow the node itself to update its status
            var nodeId = HttpContext.Items["Node"]?.ToString();

            if (nodeId != nodeStatusDto.NodeId)
            {
                return Unauthorized(new { message = "You are not authorized to update this node's status." });
            }

            var result = await _nodeService.UpdateNodeStatus(nodeStatusDto);

            if (!result)
            {
                return BadRequest(new { message = "Failed to update node status." });
            }

            return Ok(new { message = "Node status updated successfully." });
        }
    }
}
