using DecentraCloud.API.DTOs;
using System.Threading.Tasks;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IFileService
    {
        Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto);
    }
}
