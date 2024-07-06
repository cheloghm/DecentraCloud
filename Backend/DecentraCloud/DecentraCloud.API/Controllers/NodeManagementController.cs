using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NodeManagementController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeManagementController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet("nodes")]
        public async Task<IActionResult> GetNodesByUser()
        {
            var userId = User.Identity.Name;
            var nodes = await _nodeService.GetNodesByUser(userId);
            return Ok(nodes);
        }

        [HttpPut("node")]
        public async Task<IActionResult> UpdateNode([FromBody] Node node)
        {
            var result = await _nodeService.UpdateNode(node);
            if (result)
            {
                return Ok(new { message = "Node updated successfully" });
            }
            return BadRequest(new { message = "Failed to update node" });
        }

        [HttpDelete("node/{nodeId}")]
        public async Task<IActionResult> DeleteNode(string nodeId)
        {
            var result = await _nodeService.DeleteNode(nodeId);
            if (result)
            {
                return Ok(new { message = "Node deleted successfully" });
            }
            return BadRequest(new { message = "Failed to delete node" });
        }
    }
}
