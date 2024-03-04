using ConsoleApp06S.Abstracts;
using ConsoleApp06S.Services;
using System.Net;

namespace UDPServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int portL = 12345;
            Console.WriteLine("Server start!");
            Console.WriteLine("----------------");
            Console.WriteLine();

            Server server = new Server(portL);
            
            await server.Start();
        }
    }
}