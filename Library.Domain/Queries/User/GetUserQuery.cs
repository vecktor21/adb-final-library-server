using Library.Domain.Dtos.User;
using Library.Domain.Models.Implementataions;
using Library.Domain.Models.Interfaces;
using MediatR;

namespace Library.Domain.Queries.User
{
    public class GetUserQuery : IRequest<UserViewModel?>
    {
        public Guid Id { get; set; }
    }
    public class GetUserByFilterQuery : IRequest<UserModel?>
    {
        public string Filter { get; set; }
    }
    public class GetUsersQuery : IRequest<List<UserViewModel>>
    {

    }
}
