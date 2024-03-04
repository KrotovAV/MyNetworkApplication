using ConsoleApp06C.Services;


namespace UDPClient2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Thread.Sleep(5500);
            Console.WriteLine("Second Client start!");

            string ip = "127.0.0.2";
            string name = "M";
            int portL = 0;
            Console.WriteLine();
            Console.WriteLine(name);
            Console.WriteLine("--------------");

            Client clientFirst = new Client(name, ip, 12345, portL);

            clientFirst.Start();
        }
    }
}