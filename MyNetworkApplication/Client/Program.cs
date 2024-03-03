using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, Client");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(new string('.', i));
                await Task.Delay(1000);
            }

            using var udpClient = new UdpClient();

            // отправляемые данные
            string message = "Hello METANIT.COM";

            // преобразуем в массив байтов
            byte[] data = Encoding.UTF8.GetBytes(message);

            // определяем конечную точку для отправки данных
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            // отправляем данные
            int bytes = await udpClient.SendAsync(data, remotePoint);
            Console.WriteLine($"Отправлено {bytes} байт");

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(new string('.', i));
                await Task.Delay(1000);
            }

            //БЛОК ПОЛУЧЕНИЯ СООБЩЕНИЙ КЛИЕНТОМ
            var result = await udpClient.ReceiveAsync();

            // предположим, что отправлена строка, преобразуем байты в строку
            var messageFromServer = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine($"Получено {result.Buffer.Length} байт");
            Console.WriteLine($"С удаленного адреса: {result.RemoteEndPoint}");
            Console.WriteLine(messageFromServer);
        }
    }
}