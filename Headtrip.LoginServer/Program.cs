using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Headtrip.LoginServer.Data;
using Headtrip.LoginServer.Areas.Identity.Data;
using Headtrip.Utilities.Abstract;
using Headtrip.GameServerContext;
using Headtrip.LoginServerContext;
using Headtrip.Utilities;

namespace Headtrip.LoginServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IContext<HeadtripGameServerContext>, HeadtripGameServerContext>();
            builder.Services.AddTransient<IContext<HeadtripLoginServerContext>, HeadtripLoginServerContext>();

            builder.Services.AddTransient<IUnitOfWork<HeadtripGameServerContext>, UnitOfWork<HeadtripGameServerContext>>();
            builder.Services.AddTransient<IUnitOfWork<HeadtripLoginServerContext>, UnitOfWork<HeadtripLoginServerContext>>();

            builder.Services.AddTransient<
                IUnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext>,
                UnitOfWork<HeadtripLoginServerContext, HeadtripGameServerContext>>();

            builder.Services.AddTransient<
                IUnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext>,
                UnitOfWork<HeadtripGameServerContext, HeadtripLoginServerContext>>();

            builder.Services.AddControllers();


            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}