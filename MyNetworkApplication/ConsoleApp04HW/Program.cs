
using System.Net.Sockets;
using System.Net;
using System.Text;
using ConsoleApp04HW;

namespace ConsoleApp04HW
{
    internal class Program
    {
        

        static void Main(string[] args)
        {

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Console.WriteLine("Нажмите 'Выход' для выключения приложения ");
            Console.WriteLine();
            Console.WriteLine("--------------");


            Server s = new Server();

            //---------
            Proxy proxy = new Proxy();
            proxy.Request("Server Start: " + DateTime.Now.ToString());
            //---------


            s.MyServer(token);

            if (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                cts.Cancel();
            }
            //-------------------
            proxy.Request("Server Stop: " + DateTime.Now.ToString());
            //-------------------
            cts.Dispose();
            
        }
    }
}