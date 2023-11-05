using Library.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces.Services
{
    public interface IFileService
    {
        Task SafeFile(IFileModel file);
        void DeleteFile(string path);
    }
}
