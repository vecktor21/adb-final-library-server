using Library.Domain.Dtos.Cart;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.Cart
{
    public class CreateEmptyCartCommand : IRequest<CartViewModel>
    {
        public Guid UserId { get; set; }
    }
}
