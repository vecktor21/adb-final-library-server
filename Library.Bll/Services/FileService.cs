using Library.Domain.Interfaces.Services;
using Library.Domain.Models.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library.Bll.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment env;

        public FileService(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async Task SafeFile(IFileModel file)
        {
            string path = Path.Combine(env.WebRootPath, file.StaticFolder, $"{file.FileName}" );

            await File.WriteAllBytesAsync(path, file.Content);

        }

        public void DeleteFile(string path)
        {
            File.Delete(Path.Combine(env.WebRootPath, path));

        }
    }
}
