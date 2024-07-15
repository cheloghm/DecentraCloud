using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
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
        private readonly TokenHelper _tokenHelper;
        private readonly INodeRepository _nodeRepository;

        public NodesController(INodeService nodeService, TokenHelper tokenHelper, INodeRepository nodeRepository)
        {
            _nodeService = nodeService;
            _tokenHelper = tokenHelper;
            _nodeRepository = nodeRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterNode([FromBody] NodeRegistrationDto nodeRegistrationDto)
        {
            try
            {
                var node = await _nodeService.RegisterNode(nodeRegistrationDto);

                // Generate JWT token
                var token = _tokenHelper.GenerateJwtToken(node);
                node.Token = token;

                // Save the node with the token
                await _nodeRepository.AddNode(node);

                return Ok(new { node, token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginNode([FromBody] NodeLoginDto nodeLoginDto)
        {
            try
            {
                var node = await _nodeRepository.GetNodeById(nodeLoginDto.NodeId);
                if (node == null)
                {
                    return Unauthorized(new { message = "Node not found." });
                }

                // Assume Password validation here
                if (node.Password != nodeLoginDto.Password)
                {
                    return Unauthorized(new { message = "Invalid password." });
                }

                // Generate new JWT token
                var token = _tokenHelper.GenerateJwtToken(node);
                node.Token = token;
                await _nodeRepository.UpdateNode(node);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
