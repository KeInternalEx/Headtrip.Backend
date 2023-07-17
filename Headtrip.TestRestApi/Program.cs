namespace Headtrip.TestRestApi
{
    internal class Program
    {

   

        static void Main(string[] args)
        {

            var s = new Stack<string>();

            s.Push("a");
            s.Push("b");


            foreach (var a in s.ToList())
            {
                Console.WriteLine(a);
            }





        }
    }
}