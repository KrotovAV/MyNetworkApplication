using System.Net.Sockets;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Server");

            using var udpServer = new UdpClient(12345);
            Console.WriteLine("UDP-сервер запущен...");

            // получаем данные
            var result = await udpServer.ReceiveAsync();

            // предположим, что отправлена строка, преобразуем байты в строку
            var message = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine($"Получено {result.Buffer.Length} байт");
            Console.WriteLine($"С удаленного адреса: {result.RemoteEndPoint}");
            Console.WriteLine(message);

            byte[] data = Encoding.UTF8.GetBytes("Подтверждение о получении");
            int bytes = await udpServer.SendAsync(data, result.RemoteEndPoint);
            Console.WriteLine($"Отправлено {bytes} байт");
        }
    }
}