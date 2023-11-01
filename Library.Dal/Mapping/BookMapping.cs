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
            CreateMap<BookEntity, BookModel>().ReverseMap();

            CreateMap<BookEntity, BookViewModel>().ReverseMap();
            CreateMap<BookModel, BookViewModel>().ReverseMap();
        }
    }
}
