using Library.Domain.Dtos.User;
using Library.Domain.Models.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.User
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public UserUpdateDto User { get; set; }
    }
}
