﻿using DecentraCloud.API.Models;

namespace DecentraCloud.API.Interfaces.RepositoryInterfaces
{
    public interface INodeRepository
    {
        Task AddNode(Node node);
        Task<Node> GetNodeById(string nodeId);
        Task<bool> UpdateNode(Node node);
    }
}
