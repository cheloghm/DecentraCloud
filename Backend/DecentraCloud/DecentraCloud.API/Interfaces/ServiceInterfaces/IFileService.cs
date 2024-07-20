using System.Collections.Generic;
using System.Threading.Tasks;
using DecentraCloud.API.DTOs;
using DecentraCloud.API.Models;

namespace DecentraCloud.API.Interfaces.ServiceInterfaces
{
    public interface IFileService
    {
        Task<FileOperationResult> UploadFile(FileUploadDto fileUploadDto);
    }
}
