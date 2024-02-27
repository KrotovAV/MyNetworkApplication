using System.Net.Sockets;
using System.Net;
using System.Text;
using ConsoleApp04HW;

namespace ConsoleApp04HW_02
{
    internal class Program
    {
        public static void SendMessage(string From, string ip)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            while (true)
            {
                IBuilderMyMessage builderMyMessage = new ConcreteBuilder();
                builderMyMessage.BuildNickNameFrom(From);
                builderMyMessage.BuildNickNameTo();
                builderMyMessage.BuildText();
                MyMessage myMessage = builderMyMessage.Build();

                //----------
                //string message;
                //do
                //{
                    //Console.WriteLine("Введите сообщение для отправки");
                    //message = Console.ReadLine();
                //} while (string.IsNullOrEmpty(message));
                //MyMessage myMessage = new MyMessage() { Text = message, DateTime = DateTime.Now, NickNameFrom = From, NickNameTo = "Server" };
                //-----------
                
                string jsonMyMessage = myMessage.SerializeMessageToJson();

                byte[]? bytes = Encoding.UTF8.GetBytes(jsonMyMessage);
                udpClient.Send(bytes, bytes.Length, ipEndPoint);

                byte[] bufferIn = udpClient.Receive(ref ipEndPoint);


                if (bufferIn.Length != 0)
                {
                    var reportStr = Encoding.UTF8.GetString(bufferIn);
                    Console.WriteLine(reportStr);
                    if (reportStr.Contains("Exit") || reportStr.Contains("exit"))
                    {
                        Console.WriteLine($"Клиент {From} успешно отключен от сервера");
                        break;
                    }
                }

            }
        }
        static void Main(string[] args)
        {

            string ip = "127.0.0.1";
            string from = "Kolia";
            Console.WriteLine();
            Console.WriteLine(from);
            Console.WriteLine("--------------");

            SendMessage(from, ip);
        }
    }
}