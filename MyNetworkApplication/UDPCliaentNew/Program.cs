using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UDPCliaentNew
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            IPAddress localAddress = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Введите свое имя: ");
            //string? username = Console.ReadLine();
            string username = "Tolia";

            Console.Write("Введите порт для приема сообщений: ");
            //if (!int.TryParse(Console.ReadLine(), out var localPort)) return;
            if (!int.TryParse("12345", out var localPort)) return;
            Console.WriteLine(localPort.ToString());

            Console.Write("Введите порт для отправки сообщений: ");
            //if (!int.TryParse(Console.ReadLine(), out var remotePort)) return;
            if (!int.TryParse("23456", out var remotePort)) return;
            Console.WriteLine(remotePort.ToString());
            Console.WriteLine();
 
            // запускаем получение сообщений
            Task.Run(ReceiveMessageAsync);
            // запускаем ввод и отправку сообщений
            await SendMessageAsync();

            // отправка сообщений в группу
            async Task SendMessageAsync()
            {
                using UdpClient sender = new UdpClient();
                Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
                // отправляем сообщения
                while (true)
                {
                    var message = Console.ReadLine(); // сообщение для отправки
                                                      // если введена пустая строка, выходим из цикла и завершаем ввод сообщений
                    if (string.IsNullOrWhiteSpace(message)) break;
                    // иначе добавляем к сообщению имя пользователя
                    message = $"{username}: {message}";
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    // и отправляем на 127.0.0.1:remotePort
                    await sender.SendAsync(data, new IPEndPoint(localAddress, remotePort));
                }
            }
            // отправка сообщений
            async Task ReceiveMessageAsync()
            {
                using UdpClient receiver = new UdpClient(localPort);
                while (true)
                {
                    // получаем данные
                    var result = await receiver.ReceiveAsync();
                    var message = Encoding.UTF8.GetString(result.Buffer);
                    // выводим сообщение
                    Console.WriteLine(message);
                }
            }
        
           
        }
    }
}