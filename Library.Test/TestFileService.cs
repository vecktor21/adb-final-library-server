using Library.Bll.Services;
using Library.Domain.Interfaces.Services;
using Library.Domain.Models.Implementataions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Test
{
    internal class TestFileService
    {
        IFileService fileService;
        [SetUp]
        public void SetUp()
        {
            //fileService = new FileService();
        }

        [Test]
        public void TestSafeFile()
        {
            fileService.SafeFile(new FileModel());
        }
    }
}
