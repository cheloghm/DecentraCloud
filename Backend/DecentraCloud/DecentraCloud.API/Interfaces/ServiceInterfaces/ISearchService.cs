using DecentraCloud.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface ISearchService
    {
        Task<List<SearchResultDto>> SearchData(string query);
    }
}
