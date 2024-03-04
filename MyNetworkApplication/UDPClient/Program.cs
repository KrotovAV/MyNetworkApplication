using ConsoleApp06C;
using ConsoleApp06C.Abstracts;
using ConsoleApp06C.Services;
using System.Net;
using System.Net.Sockets;

namespace UDPClient
{
    internal class Program
    {

        //static async Task Main(string[] args)
        //{
        //    if (args[0].Equals("Server"))
        //    {
        //        var netmsgsorce = new UdpMessageSouce();
        //        var server = new Server(netmsgsorce);
        //        await server.Start();
        //    }
        //    else
        //    {
        //        var client = new Client("Vasya", "172.0.0.1", 12345);
        //        await client.Start();
        //    }
        //}

        static void Main(string[] args)
        {
            //Thread.Sleep(5000);
            Console.WriteLine("First Client start!");

            string ip = "127.0.0.1";
            string name = "N";
            int portL = 0;
            
            Console.WriteLine();
            Console.WriteLine(name);
            Console.WriteLine("--------------");

            Client clientFirst = new Client(name, ip, 12345, portL);
            
            clientFirst.Start();

        }
    }
}