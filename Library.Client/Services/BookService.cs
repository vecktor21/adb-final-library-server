using Amazon.Runtime.Internal.Transform;
using Library.Client.Models;
using Library.Domain.Dtos.Book;
using Library.Domain.Dtos.User;

namespace Library.Client.Services
{
    public class BookService
    {
        private readonly HttpClientService httpClientService;

        public BookService(HttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }


        public async Task<ResponseData<BookViewModel>> GetBook(Guid bookId)
        {
            return await httpClientService.SendGetRequest<BookViewModel>($"api/books/{bookId}", new Dictionary<string, object>
            {
            });
        }

        public async Task<ResponseData<List<BookViewModel>>> GetBooksByFilter(string filter)
        {
            return await httpClientService.SendGetRequest<List<BookViewModel>>("api/books/filter", new Dictionary<string, object>
            {
                { "filter", filter}
            });
        }

        public async Task<ResponseData<List<BookViewModel>>> GetAllBooks()
        {
            return await httpClientService.SendGetRequest<List<BookViewModel>>("api/books", new Dictionary<string, object>
            {
                
            });
        }


        public async Task AddBookToHistory(Guid userId, Guid bookId)
        {
            await httpClientService.SendPostRequest<object, bool>($"api/users/{userId}/history/{bookId}", null);
        }

        public async Task<ResponseData<UserHistoryViewModel>> GetUserHistory(Guid userId)
        {
            return await httpClientService.SendGetRequest<UserHistoryViewModel>($"api/users/{userId}/history", new Dictionary<string, object>());
        }
    }
}
