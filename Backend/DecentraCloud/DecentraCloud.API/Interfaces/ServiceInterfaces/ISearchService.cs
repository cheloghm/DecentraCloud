using DecentraCloud.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<FileSearchResultDto>> SearchFiles(string userId, string query);
        Task<IEnumerable<SearchResultDto>> SearchData(string query);
    }
}
