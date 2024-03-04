using ConsoleApp06C.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace ConsoleApp06C.Services
{
    public class Client
    {
        private readonly string _name;
        
        private readonly IMessageSource _messageSource;
        private IPEndPoint remoteEndPoint;

        public Client(string name, string address, int port, int portL)
        {
            this._name = name;
         
            _messageSource = new UdpMessageSource(portL);
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }

        async Task ClientListener()
        {
            while (true)
            {
                try
                {
                    var messageReceived = _messageSource.ReceiveNetMes(ref remoteEndPoint);
                    messageReceived.PrintGetMessageFrom();
                    await Confirm(messageReceived, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении сообщения: " + ex.Message);
                }
            }
        }



        async Task ClientSender()
        {
            Register(remoteEndPoint);

            while (true)
            {
                try
                {
                    Thread.Sleep(500);
                    Console.WriteLine("***");
                    Console.Write("Введите имя получателя: ");
                    var nameTo = Console.ReadLine();

                    Console.Write("Введите сообщение и нажмите Enter: ");
                    var messageText = Console.ReadLine();

                    var message = new NetMessage() { Command = Command.Message, NickNameFrom = _name, 
                        NickNameTo = nameTo, Text = messageText };

                    await _messageSource.SendAsyncNetMes(message, remoteEndPoint);

                    Console.WriteLine("Сообщение отправлено.");
                    Console.WriteLine("- - -");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }

        public async Task Start()
        {
            Task.Run(ClientListener);
            await ClientSender();
        }
        async Task Confirm(NetMessage message, IPEndPoint remoteEndPoint)
        {
            message.Command = Command.Confirmation;
            await _messageSource.SendAsyncNetMes(message, remoteEndPoint);
        }

        void Register(IPEndPoint remoteEndPoint)
        {
            var messageReg = new NetMessage() { NickNameFrom = _name, NickNameTo = null, 
                Text = "Reg", Command = Command.Register};

            _messageSource.SendAsyncNetMes(messageReg, remoteEndPoint);
        }
    }
}

