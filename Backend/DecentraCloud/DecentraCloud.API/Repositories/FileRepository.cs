using DecentraCloud.API.Data;
using DecentraCloud.API.DTOs;
using DecentraCloud.API.Interfaces.RepositoryInterfaces;
using DecentraCloud.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DecentraCloud.API.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DecentraCloudContext _context;

        public FileRepository(DecentraCloudContext context)
        {
            _context = context;
        }

        public async Task AddFileRecord(FileRecord fileRecord)
        {
            await _context.Files.InsertOneAsync(fileRecord);
        }

        public async Task<FileRecord> GetFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            return await _context.Files.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FileRecord> GetFileRecordById(string fileId)
        {
            return await _context.Files.Find(f => f.Id == fileId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FileRecord>> GetFilesByUserId(string userId)
        {
            return await _context.Files.Find(f => f.UserId == userId).ToListAsync();
        }

        public async Task<bool> DeleteFileRecord(string userId, string filename)
        {
            var filter = Builders<FileRecord>.Filter.And(
                Builders<FileRecord>.Filter.Eq(f => f.UserId, userId),
                Builders<FileRecord>.Filter.Eq(f => f.Filename, filename)
            );
            var result = await _context.Files.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            return new HttpClient(handler);
        }

        public async Task<bool> UploadFileToNode(FileUploadDto fileUploadDto, Node node)
        {
            var httpClient = CreateHttpClient();
            var url = $"{node.Endpoint}/storage/upload";
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(fileUploadDto.Data);
            content.Add(fileContent, "file", fileUploadDto.Filename);
            content.Add(new StringContent(fileUploadDto.UserId), "userId");
            content.Add(new StringContent(fileUploadDto.Filename), "filename");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", node.Token);

            var response = await httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<byte[]> DownloadFileFromNode(string userId, string fileId, Node node)
        {
            var httpClient = CreateHttpClient();
            var url = $"{node.Endpoint}/storage/download/{userId}/{fileId}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", node.Token);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> ViewFileOnNode(string userId, string fileId, Node node)
        {
            var httpClient = CreateHttpClient();
            var url = $"{node.Endpoint}/storage/view/{userId}/{fileId}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", node.Token);
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
