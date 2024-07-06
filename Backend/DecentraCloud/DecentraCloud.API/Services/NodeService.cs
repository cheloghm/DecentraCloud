using DecentraCloud.API.DTOs;
using DecentraCloud.API.Helpers;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class NodeService : INodeService
    {
        private readonly IUserService _userService;
        private readonly INodeRepository _nodeRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly HttpClient _httpClient;

        public NodeService(IUserService userService, INodeRepository nodeRepository, TokenHelper tokenHelper)
        {
            _userService = userService;
            _nodeRepository = nodeRepository;
            _tokenHelper = tokenHelper;
            _httpClient = new HttpClient();
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
                Storage = nodeRegistrationDto.Storage,
                Endpoint = nodeRegistrationDto.Endpoint
            };

            await _nodeRepository.AddNode(node);

            var token = _tokenHelper.GenerateJwtToken(node);
            node.Token = token;

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

        public async Task<bool> UploadFileToNode(FileUploadDto fileUploadDto)
        {
            var node = await _nodeRepository.GetNodeById(fileUploadDto.NodeId);

            if (node == null)
            {
                return false;
            }

            var url = $"{node.Endpoint}/upload";
            var content = new StringContent(JsonSerializer.Serialize(fileUploadDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFileFromNode(FileOperationDto fileOperationDto)
        {
            var node = await _nodeRepository.GetNodeById(fileOperationDto.NodeId);

            if (node == null)
            {
                return false;
            }

            var url = $"{node.Endpoint}/delete";
            var content = new StringContent(JsonSerializer.Serialize(fileOperationDto), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetFileContentFromNode(FileOperationDto fileOperationDto)
        {
            var node = await _nodeRepository.GetNodeById(fileOperationDto.NodeId);

            if (node == null)
            {
                return null;
            }

            var url = $"{node.Endpoint}/view/{fileOperationDto.Filename}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<FileSearchResultDto>> SearchFilesInNode(FileSearchDto fileSearchDto)
        {
            var node = await _nodeRepository.GetNodeById(fileSearchDto.NodeId);

            if (node == null)
            {
                return null;
            }

            var url = $"{node.Endpoint}/search?query={fileSearchDto.Query}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<FileSearchResultDto>>(content);
        }

        public async Task<List<SearchResultDto>> SearchFilesInNode(Node node, string query)
        {
            var url = $"{node.Endpoint}/search?query={query}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<SearchResultDto>>(content);
        }

        public async Task<IEnumerable<Node>> GetNodesByUser(string userId)
        {
            return await _nodeRepository.GetNodesByUser(userId);
        }

        public async Task<bool> UpdateNode(Node node)
        {
            return await _nodeRepository.UpdateNode(node);
        }

        public async Task<bool> DeleteNode(string nodeId)
        {
            return await _nodeRepository.DeleteNode(nodeId);
        }

        public async Task<IEnumerable<Node>> GetAllNodes()
        {
            return await _nodeRepository.GetAllNodes();
        }
    }
}
