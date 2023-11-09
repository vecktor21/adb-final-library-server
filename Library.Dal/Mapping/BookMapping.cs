using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Dtos.Book;
using Library.Domain.Models.Implementataions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Mapping
{
    public class BookMapping : Profile
    {
        public BookMapping()
        {
            CreateMap<BookModel, BookEntity>();
            CreateMap<BookEntity, BookModel>().ForMember(x=>x.Images, opt =>
            {
                opt.MapFrom(x=>x.Images.Select(img=> new FileModel
                {
                    Content = null,
                    ContentType = img.ContentType,
                    StaticFolder = img.StaticFolder,
                    FileName = img.FileName,
                    FileType = img.FileType,
                }));
            });

            CreateMap<BookEntity, BookViewModel>().ReverseMap();
            CreateMap<BookModel, BookViewModel>().ReverseMap();
        }
    }
}
