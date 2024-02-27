using ConsoleApp04HW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ConsoleApp04HW
{

    public class Server : ISubject
    {
        public void Loger(string path, string toLog)
        {
            using (StreamWriter writer = File.AppendText(path))
                writer.WriteLine(toLog);
        }
        public void Request(string s)
        {
            Console.WriteLine("Server отрабатывает запрос");
            Loger("D:\\GeekBrains\\My Git\\NetworkApplicationCSharp\\04_Lesson_HW\\ProxyData.txt", s);
            Console.WriteLine(s);
        }

        public void MyServer(CancellationToken token)
        {

            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                 
            Task.Run(() =>
            {
                Console.WriteLine("Сервер ждет сообщение от клиента");
                while (!token.IsCancellationRequested)
                {
                    byte[] buffer = udpClient.Receive(ref iPEndPoint);
                    if (buffer == null) break;
                    string messageText = Encoding.UTF8.GetString(buffer);

                    MyMessage? message = MyMessage.DeserializeMessageFromJson(messageText);
                    message?.PrintMessageFrom();

                    if (message != null)
                    {
                        byte[] bufferOut = Encoding.UTF8.GetBytes($"*** Сообщение - {message.Text} - получено сервером {message.DateTime}.\n ");
                        udpClient.Send(bufferOut, bufferOut.Length, iPEndPoint);
            
                        if (message.Text.Contains("Exit") || message.Text.Contains("exit"))
                        {
                            Console.WriteLine($"Клиент {message.NickNameFrom} будет выключен.");
                            Console.WriteLine("Сервер выключен.");
                            Console.WriteLine();
                            token.ThrowIfCancellationRequested();
                            Console.WriteLine("Для выключения приложения нажмите любую клавишу.");
                        }
                    }
                }
            }, token);
        }
    }
}
