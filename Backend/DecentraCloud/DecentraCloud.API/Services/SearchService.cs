using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Services
{
    public class SearchService : ISearchService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly INodeService _nodeService;

        public SearchService(INodeRepository nodeRepository, INodeService nodeService)
        {
            _nodeRepository = nodeRepository;
            _nodeService = nodeService;
        }

        public async Task<List<SearchResultDto>> SearchData(string query)
        {
            var results = new List<SearchResultDto>();

            var nodes = await _nodeRepository.GetAllNodes();
            foreach (var node in nodes)
            {
                var nodeResults = await _nodeService.SearchFilesInNode(node, query);
                results.AddRange(nodeResults);
            }

            return results;
        }
    }
}
