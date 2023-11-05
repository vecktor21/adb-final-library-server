using Library.Bll.Services;
using Library.Cache.Repositories;
using Library.Dal;
using Library.Dal.Repositories;
using Library.Domain.Interfaces.Repositories;
using Library.Domain.Interfaces.Services;
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
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFileService, FileService>();

        }
    }
}