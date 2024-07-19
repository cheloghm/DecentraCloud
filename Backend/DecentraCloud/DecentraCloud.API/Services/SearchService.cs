using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Interfaces.ServiceInterfaces;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<FileSearchResultDto>> SearchFiles(string userId, string query)
        {
            var nodes = await _nodeService.GetNodesByUser(userId);
            var searchResults = new List<FileSearchResultDto>();

            foreach (var node in nodes)
            {
                var nodeResults = await _nodeService.SearchFilesInNode(new FileSearchDto { NodeId = node.Id, Query = query });
                if (nodeResults != null)
                {
                    searchResults.AddRange(nodeResults);
                }
            }

            return searchResults;
        }

        public async Task<IEnumerable<SearchResultDto>> SearchData(string query)
        {
            var results = new List<SearchResultDto>();

            var nodes = await _nodeService.GetAllNodes();
            foreach (var node in nodes)
            {
                var nodeResults = await _nodeService.SearchFilesInNode(new FileSearchDto { NodeId = node.Id, Query = query });
                if (nodeResults != null)
                {
                    results.AddRange(nodeResults.Select(r => new SearchResultDto
                    {
                        NodeId = node.Id,
                        Filename = r.Filename,
                        Snippet = r.Snippet
                    }));
                }
            }

            return results;
        }
    }
}
