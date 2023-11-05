using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.User.Auth
{
    public class AuthorizeUserCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
    }
}
