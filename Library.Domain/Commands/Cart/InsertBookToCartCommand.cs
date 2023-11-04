﻿using Library.Domain.Dtos.Cart;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Commands.Cart
{
    public class InsertBookToCartCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
