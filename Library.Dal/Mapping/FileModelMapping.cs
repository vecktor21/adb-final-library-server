using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Dtos;
using Library.Domain.Models.Implementataions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Mapping
{
    public class FileModelMapping : Profile
    {
        public FileModelMapping()
        {
            CreateMap<FileModel, FileModelEntity>().ReverseMap();
            CreateMap<FileModelEntity, FileDto>().ReverseMap();
            CreateMap<FileModel, FileDto>().ReverseMap();
        }
    }
}
