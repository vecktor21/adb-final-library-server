using Library.Domain.Dtos.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Queries.User
{
    public class GetUserHistoryQuery : IRequest<UserHistoryViewModel>
    {
        public Guid UserId { get; set; }
    }
}
