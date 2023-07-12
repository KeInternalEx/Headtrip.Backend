
using Headtrip.WebApiInitialization;

namespace Headtrip.GameServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebAppBuilder.Create(args).Run();
        }
    }
}