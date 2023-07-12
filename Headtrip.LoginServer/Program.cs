namespace Headtrip.LoginServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            IoC.IoC.RegisterForLoginServer(builder.Services);



            builder.Services.AddControllers();



            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}