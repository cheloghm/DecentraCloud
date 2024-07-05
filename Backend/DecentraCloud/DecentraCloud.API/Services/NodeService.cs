using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using DecentraCloud.API.Helpers;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class NodeService : INodeService
    {
        private readonly IUserService _userService;
        private readonly INodeRepository _nodeRepository;
        private readonly TokenHelper _tokenHelper;

        public NodeService(IUserService userService, INodeRepository nodeRepository, TokenHelper tokenHelper)
        {
            _userService = userService;
            _nodeRepository = nodeRepository;
            _tokenHelper = tokenHelper;
        }

        public async Task<Node> RegisterNode(NodeRegistrationDto nodeRegistrationDto)
        {
            var user = await _userService.AuthenticateUser(new UserLoginDto
            {
                Email = nodeRegistrationDto.Username,
                Password = nodeRegistrationDto.Password
            });

            if (user == null)
            {
                return null;
            }

            var node = new Node
            {
                UserId = user.Id,
                Storage = nodeRegistrationDto.Storage
            };

            await _nodeRepository.AddNode(node);

            // Generate token for the node
            var token = _tokenHelper.GenerateJwtToken(node);

            node.Token = token; // Assuming Node model has a Token property

            return node;
        }

        public async Task<bool> UpdateNodeStatus(NodeStatusDto nodeStatusDto)
        {
            var node = await _nodeRepository.GetNodeById(nodeStatusDto.NodeId);

            if (node == null)
            {
                return false;
            }

            node.Uptime = nodeStatusDto.Uptime;
            node.Downtime = nodeStatusDto.Downtime;
            node.StorageStats = new StorageStats
            {
                UsedStorage = nodeStatusDto.StorageStats.UsedStorage,
                AvailableStorage = nodeStatusDto.StorageStats.AvailableStorage
            };
            node.OnlineStatus = nodeStatusDto.OnlineStatus;
            node.CauseOfDowntime = nodeStatusDto.CauseOfDowntime;

            return await _nodeRepository.UpdateNode(node);
        }
    }
}
