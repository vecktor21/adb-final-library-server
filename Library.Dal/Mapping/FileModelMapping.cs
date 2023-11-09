using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Dtos;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
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
            CreateMap<FileModel, FileModelEntity>();
            CreateMap<FileModelEntity, FileDto>().ReverseMap();
            CreateMap<FileModelEntity, FileModel>().ForMember(x=>x.Content, opt=>opt.Ignore());
            CreateMap<FileModel, FileDto>().ReverseMap();
        }
    }
}
