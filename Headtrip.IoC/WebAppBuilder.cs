using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Repositories.Abstract;
using Headtrip.Repositories;
using Headtrip.Utilities.Abstract;
using Headtrip.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Services.Abstract;
using Headtrip.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;

namespace Headtrip.WebApiInitialization
{
    public static class WebAppBuilder
    {
        private static void RegisterServices(IServiceCollection services)
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


            services.AddSingleton<Services.Abstract.IAuthenticationService, Services.AuthenticationService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IAccountService, AccountService>();

            services.AddSingleton<ILogging<HeadtripLoginServerContext>, Logging<HeadtripLoginServerContext>>();
            services.AddSingleton<ILogging<HeadtripGameServerContext>, Logging<HeadtripGameServerContext>>();

        }



        public static WebApplication Create(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            RegisterServices(builder.Services);



            builder.Services.AddControllers();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT";
                options.RequireAuthenticatedSignIn = false;
            }).AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>("JWT", options =>
            {
                options.SchemeName = "JWT";
            });


            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();


            return app;

        }






    }
}
