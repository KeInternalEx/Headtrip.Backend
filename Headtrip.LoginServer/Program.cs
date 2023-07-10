using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Headtrip.LoginServer.Data;
using Headtrip.LoginServer.Areas.Identity.Data;

namespace Headtrip.LoginServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("HeadtripLoginServerContextConnection") ?? throw new InvalidOperationException("Connection string 'HeadtripLoginServerContextConnection' not found.");

            builder.Services.AddDbContext<HeadtripLoginServerContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<HeadtripLoginServerUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<HeadtripLoginServerContext>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}