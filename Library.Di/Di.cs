using Library.Dal;
using Library.Dal.Options;
using Library.Dal.Repositories;
using Library.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Library.Di
{
    public static class Di
    {
        public static void AddServices(IServiceCollection services)
        {


            services.AddScoped<Database>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}