using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Models.User;
using Headtrip.Repositories;
using Headtrip.Repositories.Abstract;
using Headtrip.Services;
using Headtrip.Services.Abstract;
using Headtrip.Utilities;
using Headtrip.Utilities.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Headtrip.IoC
{
    public static class IoC
    {
        public static void RegisterForLoginServer(IServiceCollection services)
        {
            services.AddScoped<IContext<HeadtripGameServerContext>, HeadtripGameServerContext>();
            services.AddScoped<IContext<HeadtripLoginServerContext>, HeadtripLoginServerContext>();

            services.AddTransient<IUnitOfWork<HeadtripGameServerContext>, UnitOfWork<HeadtripGameServerContext>>();
            services.AddTransient<IUnitOfWork<HeadtripLoginServerContext>, UnitOfWork<HeadtripLoginServerContext>>();

            services.AddTransient<
                IUnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext>,
                UnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext>>();

            services.AddTransient<
                IUnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext>,
                UnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext>>();


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();


            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddSingleton<ILogging<HeadtripLoginServerContext>, Logging<HeadtripLoginServerContext>>();
            services.AddSingleton<ILogging<HeadtripGameServerContext>, Logging<HeadtripGameServerContext>>();


        }

        public static void RegisterForGameServer(IServiceCollection services)
        {



        }
    }
}