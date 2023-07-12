using Headtrip.WebApiInitialization;

namespace Headtrip.LoginServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebAppBuilder.Create(args).Run();
        }
    }
}