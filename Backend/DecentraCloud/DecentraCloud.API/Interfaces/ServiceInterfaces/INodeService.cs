using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Xml.Linq;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface INodeService
    {
        Task<Node> RegisterNode(NodeRegistrationDto nodeRegistrationDto);
        Task<bool> UpdateNodeStatus(NodeStatusDto nodeStatusDto);
        Task<bool> UploadFileToNode(FileUploadDto fileUploadDto);
        Task<bool> DeleteFileFromNode(FileOperationDto fileOperationDto);
        Task<string> GetFileContentFromNode(FileOperationDto fileOperationDto);
        Task<IEnumerable<FileSearchResultDto>> SearchFilesInNode(FileSearchDto fileSearchDto);
        Task<List<SearchResultDto>> SearchFilesInNode(Node node, string query);
        Task<IEnumerable<Node>> GetNodesByUser(string userId);
        Task<bool> UpdateNode(Node node);
        Task<bool> DeleteNode(string nodeId);
        Task<IEnumerable<Node>> GetAllNodes();
    }


}
