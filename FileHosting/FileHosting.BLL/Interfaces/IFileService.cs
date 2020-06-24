using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileHosting.BLL.Interfaces
{
    public interface IFileService
    {
        Task<FileModel> AddFile(IFormFile file, string WebRootPath, string userId);
        Task DeleteFile(int fileId, string WebRootPath);
        Task<IEnumerable<FileModel>> GetFiles();
        Task<IEnumerable<FileModel>> GetUserFiles(string userId);
        Task<string> GetFilePath(int FileId);
#warning MarkDelete
    }
}
