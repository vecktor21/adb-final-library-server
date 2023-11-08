using Library.Domain.Dtos.User;

namespace Library.Client
{
    public class ContextData
    {
        public UserViewModel CurrentUser { get; set; } = new();
        public bool IsAuthorized { get; set; } = false;
        public string JwtToken { get; set; } = "";

    }
}
