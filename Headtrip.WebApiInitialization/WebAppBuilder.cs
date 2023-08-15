using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
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
using Headtrip.Repositories.Repositories.Interface.GameServer;
using Headtrip.Repositories.Repositories.Interface.LoginServer;
using Headtrip.Repositories.Repositories.Implementation.LoginServer;
using Headtrip.Repositories.Repositories.Implementation.GameServer;

namespace Headtrip.WebApiInitialization
{
    public static class WebAppBuilder
    {
        public static string JWT_SCHEME_NAME = "JWT";
        public static string UNREAL_UnrealService_SCHEME_NAME = "API_KEY_UNREAL_UnrealService";
        public static string UNREAL_GAMESERVER_SCHEME_NAME = "API_KEY_UNREAL_GAME_SERVER";


        private static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IContext<HeadtripGameServerContext>, HeadtripGameServerContext>();
            services.AddTransient<IContext<HeadtripLoginServerContext>, HeadtripLoginServerContext>();

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
                options.DefaultScheme = JWT_SCHEME_NAME;
                options.RequireAuthenticatedSignIn = false;
            })
            .AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler>(JWT_SCHEME_NAME, options => { options.SchemeName = JWT_SCHEME_NAME; })
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(UNREAL_UnrealService_SCHEME_NAME, options => {
                options.SchemeName = UNREAL_UnrealService_SCHEME_NAME;
                options.KeyName = "UnrealUnrealServiceApiKey";
            })
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(UNREAL_GAMESERVER_SCHEME_NAME, options => {
                options.SchemeName = UNREAL_GAMESERVER_SCHEME_NAME;
                options.JwtSchemeName = JWT_SCHEME_NAME;
                options.KeyName = "UnrealGameServerApiKey";
            });


            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();


            return app;

        }






    }
}
