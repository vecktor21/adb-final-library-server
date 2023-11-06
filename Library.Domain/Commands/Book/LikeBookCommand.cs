using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.Book
{
    public class LikeBookCommand : IRequest<bool>
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
