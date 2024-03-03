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
            Console.WriteLine("Register в Сервере сработал ");

            if (clients.TryAdd(message.NickNameFrom, message.EndPoint))
            {
                Console.WriteLine($"Пользователь {message.NickNameFrom} зарегистрирован");
                Console.WriteLine("----------------");
                using (ChatContext ctx = new ChatContext())
                {
                    User? user = ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom);
                    if (user == null)
                    {
                        ctx.Add(new User {FullName = message.NickNameFrom});
                        await ctx.SaveChangesAsync();
                    }
                }
                var messageRegConfirm = new NetMessage()
                {
                    NickNameFrom = "Server",
                    NickNameTo = message.NickNameFrom,
                    Text = "Зарегистрирован на сервере"
                };

                await _messageSourse.SendAsyncNetMes(messageRegConfirm, message.EndPoint);

                Console.WriteLine("Register сработал у Cliaent");
            }
        }
        private async Task RelyMessage(NetMessage message)
        {
            if (clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
            {
                Console.WriteLine($"RelyMessage Пользователь Найден");
                int? id = 0;
                using (ChatContext ctx = new ChatContext())
                {
                    User fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                    User toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);

                    Message msg = new Message() { 
                        UserFrom = fromUser, 
                        UserTo = toUser, 
                        IsSent = false, 
                        Text = message.Text 
                    };
                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();
                    id = msg.MessageId;
                }
                message.Id = id;

                NetMessage messageTestRelied = new NetMessage()
                {
                    Id = message.Id,
                    NickNameFrom = message.NickNameFrom,
                    NickNameTo = message.NickNameTo,
                    Text = message.Text
                };
              
                await _messageSourse.SendAsyncNetMes(messageTestRelied, ep);
                //await _messageSourse.SendAsyncNetMes(message, ep);
                Console.WriteLine($"Message Relied, from =  {message.NickNameFrom} to  {message.NickNameTo} ");
            }
            else
            {
                Console.WriteLine($"Пользватель не найден или сейчас не в сети");
            }
        }
        async Task ConfirmMessageReceived(int? id)
        {
            Console.WriteLine($"Message confirmation id =  + {id}");
            using (ChatContext ctx = new ChatContext())
            {
                var msg = ctx.Messages.FirstOrDefault(m => m.MessageId == id);
                if (msg != null)
                {
                    msg.IsSent = true;
                    await ctx.SaveChangesAsync();
                }
            }
        }
        public async Task ProcessMessage(NetMessage message)
        {
            Console.WriteLine("ProcessMessage в Сервере сработал ");
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
            while (work)
            {
                try
                {
                    //-------------------
                    //using (ChatContext ctx = new ChatContext())
                    //{
                    //    Console.WriteLine("Users------------------------");
                    //    foreach (var row in ctx.Users)
                    //        Console.WriteLine(row.Id + " : " + row.FullName);
                    //    Console.WriteLine("Messages------------------------");
                    //    foreach (var row in ctx.Messages)
                    //        Console.WriteLine(row.UserFromId + " : " + row.UserToId + " : " + row.Text);
                    //    Console.WriteLine("--------------------------------");

                    //}
                    //------------------
                    NetMessage? message = _messageSourse.ReceiveNetMes(ref ep);
                    message.EndPoint = ep;

                    await ProcessMessage(message);
                    //------------------
                    foreach (var c in clients)
                    {
                        Console.WriteLine(c.Key + ": " + c.Value);
                    }
                    Console.WriteLine("------------------------");
                    //------------------------------------
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
       
        
    }
}
