using Headtrip.UeService.IPCMessages.FromUeService;

namespace Headtrip.TestRestApi
{
    internal class Program
    {

   

        static void Main(string[] args)
        {

            var s = new DIPC_CLOSE_INSTANCE();

            s = DIPC_CLOSE_INSTANCE.Decode("{\"Type\":\"notreal\"}");
            // s.Verify();

            Console.WriteLine(s.Encode());



        }
    }
}