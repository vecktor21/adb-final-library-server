using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Dtos;
using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.Cart;
using Library.Domain.Models.Implementataions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Mapping
{
    public class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<CartModel, CartEntity>()
                .ForMember(x=>x.Books, opt=>opt.Ignore());

            CreateMap<CartEntity, CartModel>()
                .ForMember(x => x.Books, opt => opt.Ignore());

            CreateMap<CartViewModel, CartEntity>()
                .ForMember(x => x.Books, (x => x.MapFrom(m => m.Books.Select(s=>new CartBookEntity { BookId = s.Book.Id, Count = s.Count}))));

            CreateMap<CartModel, CartViewModel> ()
                .ForMember(x => x.Books, (x => x.MapFrom(m => m.Books.Select(s=> new CartBookViewModel
                {
                    Book = new BookViewModel{
                        Author = s.Book.Author,
                        CoverType = s.Book.CoverType,
                        CreateDate = s.Book.CreateDate,
                        Description = s.Book.Description,
                        Discount = s.Book.Discount,
                        Genre = s.Book.Genre,
                        Id= s.Book.Id,
                        Likes = s.Book.Likes,
                        Pages = s.Book.Pages,
                        PiblishCity = s.Book.PiblishCity,
                        Price = s.Book.Price,
                        Publisher = s.Book.Publisher,
                        Title = s.Book.Title,
                        UpdateDate = s.Book.UpdateDate,
                        Year = s.Book.Year,
                        Images = s.Book.Images.Select(im=> new FileDto
                        {
                            ContentType = im.ContentType,
                            FileName = im.FileName,
                            StaticFolder = im.StaticFolder,
                            FileType = im.FileType,
                        }).ToList()
                    },
                    Count = s.Count
                }))))
                .ForMember(x=>x.Discount, (s=>s.MapFrom(m=>m.CountDiscount())))
                .ForMember(x => x.IntermediatePrice, (s => s.MapFrom(m => m.CountIntermediatePrice())))
                .ForMember(x => x.Total, (s => s.MapFrom(m => m.CountTotal())));
        }
    }
}
