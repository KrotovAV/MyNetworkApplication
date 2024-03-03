using ConsoleApp06S.Abstracts;
using ConsoleApp06S.Services;
using System.Net;

namespace UDPServer
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





        static async Task Main(string[] args)
        //static void Main(string[] args)
        {
            int portL = 12345;
            //--------------
            Console.WriteLine("Server start!");
            //----------------

            //UdpMessageSource netmsgsorce = new UdpMessageSource();
            Server server = new Server(portL);
            //Server server = new Server(netmsgsorce);
            await server.Start();
            //server.Start();
        }
    }
}