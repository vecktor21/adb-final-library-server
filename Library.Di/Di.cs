using Library.Cache.Repositories;
using Library.Dal;
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

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<Database>();
            services.AddScoped<ICacheRepository, RedisCacheRepository>();


            services.AddMediatR(cfg =>
            {
                var assamblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName).ToArray();
                cfg.RegisterServicesFromAssemblies(assamblies);
            });
            /*services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();*/

        }
    }
}