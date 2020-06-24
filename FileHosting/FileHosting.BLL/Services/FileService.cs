using FileHosting.BLL.Interfaces;
using FileHosting.DAL.EF;
using FileHosting.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHosting.BLL.Services
{
    public class FileService : IFileService
    {
        readonly ApiContext db;
        const string FILES_PATH = "/Files/Storage/";
        public FileService(ApiContext context)
        {
            db = context;
        }
        public async Task<FileModel> AddFile(IFormFile uploadedFile, string WebRootPath, string userId)
        {
            DateTime date = DateTime.Now;
            string path = FILES_PATH + date.Ticks + "_" + uploadedFile.FileName;
            using (var fileStream = new FileStream(WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            FileModel file = new FileModel 
            { 
                Name = date.Ticks + "_" + uploadedFile.FileName, 
                Path = path,
                UserId = userId
            };
            await db.Files.AddAsync(file);
            await db.SaveChangesAsync();
            return file;
        }
        
        public async Task DeleteFile(int fileId, string WebRootPath)
        {
            string path = await GetFilePath(fileId);
            FileInfo fileInf = new FileInfo(WebRootPath + path);
            FileModel file = db.Files.Find(fileId);
            if (fileInf.Exists && file != null)
            {
                db.Files.Remove(file);
                await db.SaveChangesAsync();
                fileInf.Delete();
                // альтернатива с помощью класса File
                // File.Delete(path);
            }
            else
            {
                throw new Exception("File not found");
            }
        }

        public async Task<string> GetFilePath(int FileId)
        {
            var File = await db.Files.FindAsync(FileId);
            return File.Path;
        }

        public async Task<IEnumerable<FileModel>> GetFiles()
        {
            var files = await db.Files.ToListAsync();
            return files.OrderByDescending(p=>p.UploadDate);
        }

        public async Task<IEnumerable<FileModel>> GetUserFiles(string userId)
        {
            return (await GetFiles()).Where(p => p.UserId == userId);
        }
    }
}
