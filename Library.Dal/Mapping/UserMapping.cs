using AutoMapper;
using Library.Dal.Models;
using Library.Domain.Dtos.User;
using Library.Domain.Models.Implementataions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Dal.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserEntity, UserModel>().ReverseMap();

            CreateMap<UserEntity, UserViewModel>().ReverseMap();
            CreateMap<UserModel, UserViewModel>().ReverseMap();

            CreateMap<UserEntity, AuthorViewModel>().ReverseMap();
            CreateMap<UserModel, AuthorViewModel>().ReverseMap();
        }
    }
}
