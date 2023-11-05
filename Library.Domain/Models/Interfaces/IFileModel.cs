using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models.Interfaces
{
    public interface IFileModel
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string StaticFolder { get; set; }
    }
}
