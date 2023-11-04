using Library.Domain.Dtos.Cart;
using Library.Domain.Models.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Queries.Cart
{
    public class GetCartQuery : IRequest<CartViewModel>
    {
        public Guid UserId { get; set; }
    }
    public class GetAllCartsQuery : IRequest<List<CartViewModel>>
    {
    }
}
