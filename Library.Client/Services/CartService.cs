using Library.Client.Models;
using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.Cart;

namespace Library.Client.Services
{
    public class CartService
    {
        private readonly HttpClientService httpClientService;

        public CartService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }


        public async Task<ResponseData<bool>> AddToCart(Guid userId, Guid bookId)
        {
            return await httpClientService.SendPutRequest<object, bool>($"api/carts/{userId}/book/{bookId}",
                new Dictionary<string, object>(),null);
        }

        public async Task<ResponseData<bool>> RemoveOneFromCart(Guid userId, Guid bookId)
        {
            return await httpClientService.SendDeleteRequest< bool>($"api/carts/{userId}/book/{bookId}",
                new Dictionary<string, object>());
        }

        public async Task<ResponseData<CartViewModel>> GetUserCart(Guid userId)
        {
            return await httpClientService.SendGetRequest<CartViewModel>($"api/carts/{userId}", new Dictionary<string, object>());
        } 
    }
}
