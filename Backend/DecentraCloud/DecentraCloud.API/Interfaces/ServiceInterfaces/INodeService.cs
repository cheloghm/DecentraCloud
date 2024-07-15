using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface INodeService
    {
        Task<Node> RegisterNode(NodeRegistrationDto nodeRegistrationDto);
        Task<bool> UpdateNodeStatus(NodeStatusDto nodeStatusDto);
        Task<long> GetFileSize(string nodeId, string filename);
        Task<bool> UploadFileToNode(FileUploadDto fileUploadDto);
        Task<bool> DeleteFileFromNode(FileOperationDto fileOperationDto);
        Task<string> GetFileContentFromNode(FileOperationDto fileOperationDto);
        Task<IEnumerable<FileSearchResultDto>> SearchFilesInNode(FileSearchDto fileSearchDto);
        Task<bool> RenameFileInNode(FileRenameDto fileRenameDto);
        Task<IEnumerable<Node>> GetNodesByUser(string userId);
        Task<bool> UpdateNode(Node node);
        Task<bool> DeleteNode(string nodeId);
        Task<IEnumerable<Node>> GetAllNodes();
        Task<Node> GetRandomNode();
    }
}
