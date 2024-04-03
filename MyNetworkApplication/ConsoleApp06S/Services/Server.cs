using ConsoleApp06S.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp06S.Services
{
    public class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        private readonly IMessageSource _messageSourse;
        private IPEndPoint ep;
        public Server(int portL)
        {
            _messageSourse = new UdpMessageSource(portL);
            ep = new IPEndPoint(IPAddress.Any, 0);
        }

        //public Server(IMessageSource sourse)
        //{
        //    _messageSourse = sourse;
        //    ep = new IPEndPoint(IPAddress.Any, 0);
        //}

        bool work = true;
        public void Stop()
        {
            work = false;
        }
        private async Task Register(NetMessage message)
        {
            if (clients.TryAdd(message.NickNameFrom, message.EndPoint))
            {
                using (ChatContext ctx = new ChatContext())
                {
                    User? user = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom);
                    if (user == null)
                    {
                        ctx.Add(new User {FullName = message.NickNameFrom});
                        await ctx.SaveChangesAsync();
                    }
                    User? u = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom);
                    if (u != null)
                    {
                        Console.WriteLine($"Пользователь {u.Id} зарегистрирован");
                        Console.WriteLine("----------------");
                    }
                    
                    
                }
                
                var messageRegConfirm = new NetMessage()
                {
                    NickNameFrom = "Server",
                    NickNameTo = message.NickNameFrom,
                    DateTime = DateTime.Now,
                    Text = "Зарегистрирован на сервере"
                };

                await _messageSourse.SendAsyncNetMes(messageRegConfirm, message.EndPoint);

                using (ChatContext ctx = new ChatContext())
                {
                    User? toUser = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom);
                    var undelMessages = ctx.Messages.Where(x => x.UserToId == toUser.Id && x.IsSent == false).ToList();
                    if (undelMessages != null)
                    {
                        NetMessage undeliveredMessage = new NetMessage() { };
                        foreach (var mes in undelMessages)
                        {
                            User fromUser = ctx.Users.FirstOrDefault(x => x.Id == mes.UserFromId);

                            undeliveredMessage.Id = mes.MessageId;
                            undeliveredMessage.NickNameFrom = fromUser.FullName;
                            undeliveredMessage.DateTime = mes.DateSend;
                            undeliveredMessage.NickNameTo = toUser.FullName;
                            undeliveredMessage.Text = mes.Text;

                            await _messageSourse.SendAsyncNetMes(undeliveredMessage, message.EndPoint);
                            Console.WriteLine($"Сообщение id:{undeliveredMessage.Id} отправлено пользователю {toUser.Id}.");
                            Console.WriteLine("----------------");
                        }
                    }
                }
            }
        }
        private async Task RelyMessage(NetMessage message)
        {
            using (ChatContext ctx = new ChatContext())
            {
                User toUser = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameTo);
                User fromUser = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom);
                NetMessage messageRelied = new NetMessage(){};
                IPEndPoint messageBackOrRelayEP;
                //Проверяем наличие получателя б.д. users
                if (toUser != null)
                {
                    Console.WriteLine($"Пользователь {toUser.Id} найден.");
                    Console.WriteLine("----------------");
                    //Сохраняем сообщение в б.д. Messages
                    Message msg = new Message()
                    {
                        UserFrom = fromUser,
                        UserTo = toUser,
                        IsSent = false,
                        DateSend = DateTime.Now,
                        Text = message.Text
                    };
                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();
                    message.Id = msg.MessageId;
                    //Проверяем наличие получателя в сети, т.е. есть ли он сейчас в словаре
                    if (clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
                    {
                        //отправляем сообщение получателю
                        messageRelied.Id = msg.MessageId;
                        messageRelied.NickNameFrom = message.NickNameFrom;
                        messageRelied.NickNameTo = message.NickNameTo;
                        messageRelied.Text = message.Text;

                        Console.WriteLine($"Сообщение id: {msg.MessageId} от {msg.UserFromId} для {msg.UserToId} отправлено.");
                        Console.WriteLine("----------------");
                        messageBackOrRelayEP = ep;
                    }
                    else
                    {
                        //Сообщаем отправителю, что сообщение пока не доставлено
                        messageRelied.Id = msg.MessageId;
                        messageRelied.NickNameFrom = "Server";
                        messageRelied.NickNameTo = message.NickNameFrom;
                        messageRelied.Text = $"{message.NickNameTo} находится не в сети\n" +
                            $"Сообщение id = {msg.MessageId} будет отправлено пользователю, когда он появится в сети";
                        messageBackOrRelayEP = clients[message.NickNameFrom];
                        
                        Console.WriteLine($"Сообщение {msg.MessageId} будет отправлено пользователю, когда он появится в сети");
                        Console.WriteLine("----------------");
                    }
                }
                else
                {
                    Console.WriteLine($"Получатель не найден.");
                    Console.WriteLine("----------------");
                    //Сообщаем отправителю, что такой пользователь не найден

                    messageRelied.NickNameFrom = "Server";
                    messageRelied.NickNameTo = message.NickNameFrom;
                    messageRelied.Text = $"Пользователь {message.NickNameTo} не найден.";
                    messageBackOrRelayEP = clients[message.NickNameFrom];
                }
                messageRelied.DateTime = DateTime.Now;
                await _messageSourse.SendAsyncNetMes(messageRelied, messageBackOrRelayEP);
            }
        }
        async Task ConfirmMessageReceived(int? id)
        {
            using (ChatContext ctx = new ChatContext())
            {
                var msg = ctx.Messages.FirstOrDefault(m => m.MessageId == id);
                if (msg != null)
                {
                    msg.IsSent = true;
                    await ctx.SaveChangesAsync();
                //}
                Console.WriteLine($"Сообщение id:{msg.MessageId} доставлено получателю {msg.UserToId}.");
                Console.WriteLine("----------------");
                }
            }
        }
        public async Task ProcessMessage(NetMessage message)
        {
            switch (message.Command)
            {
                case Command.Register: await Register(message); break;
                case Command.Message: await RelyMessage(message); break;
                case Command.Confirmation: await ConfirmMessageReceived(message.Id); break;
                default: break;
            }
        }
        public async Task Start()
        {
            Console.WriteLine("Сервер ожидает сообщения ");
            Console.WriteLine("----------------");
            while (work)
            {
                try
                {
                    NetMessage? message = _messageSourse.ReceiveNetMes(ref ep);
                    message.EndPoint = ep;
                    await ProcessMessage(message);
                }
                catch (SocketException)
                {
                    Console.WriteLine($"Получатель сообщения не в сети.");
                    Console.WriteLine("----------------");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }        
    }
}
