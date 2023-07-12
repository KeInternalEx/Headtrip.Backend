namespace Headtrip.TestRestApi
{
    internal class Program
    {

        static void test<T>()
        {
            Console.WriteLine(typeof(T).Name);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            test<Program>();







        }
    }
}