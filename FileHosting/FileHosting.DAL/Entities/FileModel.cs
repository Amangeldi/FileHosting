using System;
using System.Collections.Generic;
using System.Text;

namespace FileHosting.DAL.Entities
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string UserId { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}
