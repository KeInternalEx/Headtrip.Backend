namespace Headtrip.Daemon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // todo: start topshelf service
            // todo: split on super daemon flag
            // todo: if we're the super daemon, we run the contract transformation task
            // todo: if we're not a super daemon, then we manage the server spinning tasks


        }
    }
}