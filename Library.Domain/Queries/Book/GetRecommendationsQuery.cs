using Library.Domain.Dtos.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Queries.Book
{
    public class GetRecommendationsQuery : IRequest<List<BookViewModel>>
    {
        public Guid UserId { get; set; }
    }
}
