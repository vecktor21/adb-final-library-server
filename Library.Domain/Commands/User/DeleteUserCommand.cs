using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.User
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class ClearUsers : IRequest<bool>
    {

    }
}
