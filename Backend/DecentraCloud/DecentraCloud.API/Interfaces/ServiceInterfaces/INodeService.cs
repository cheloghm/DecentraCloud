﻿using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface INodeService
    {
        Task<Node> RegisterNode(NodeRegistrationDto nodeRegistrationDto);
        Task<string> LoginNode(NodeLoginDto nodeLoginDto);
        Task<bool> UpdateNodeStatus(NodeStatusDto nodeStatusDto);
        Task<long> GetFileSize(string nodeId, string filename);
        Task<bool> UploadFileToNode(FileUploadDto fileUploadDto);
        Task<IEnumerable<NodeDto>> GetAllNodes();
        Task<IEnumerable<Node>> GetNodesByUser(string userId);
        Task<Node> GetNodeById(string nodeId);
        Task<bool> UpdateNode(Node node);
        Task<bool> DeleteNode(string nodeId);
        Task<Node> GetRandomNode();
        Task<Node> GetRandomOnlineNode(); // Add this line
    }
}
