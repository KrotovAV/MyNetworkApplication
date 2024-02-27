using ConsoleApp06S.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp06S.Services
{
    public class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        private readonly IMessageSource _messageSourse;
        private IPEndPoint ep;
        public Server()
        {
            _messageSourse = new UdpMessageSource();
            ep = new IPEndPoint(IPAddress.Any, 0);
        }

        public Server(IMessageSource sourse)
        {
            _messageSourse = sourse;
            ep = new IPEndPoint(IPAddress.Any, 0);
        }

        bool work = true;
        public void Stop()
        {
            work = false;
        }
        private async Task Register(NetMessage message)
        {
            Console.WriteLine($"Message Reister, name =  {message.NickNameFrom}");
            if (clients.TryAdd(message.NickNameFrom, message.EndPoint))
            {
                using (ChatContext chatContext = new ChatContext())
                {
                    chatContext.Add(new User { FullName = message.NickNameFrom });
                    await chatContext.SaveChangesAsync();
                }
            }
        }
        private async Task RelyMessage(NetMessage message)
        {
            if (clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
            {
                int? id = 0;
                using (ChatContext ctx = new ChatContext())
                {
                    User fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                    User toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
                    Message msg = new Message() { UserFrom = fromUser, UserTo = toUser, IsSent = false, Text = message.Text };

                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();

                    id = msg.MessageId;
                }
                message.Id = id;
                await _messageSourse.SendAsync(message, ep);
                Console.WriteLine($"Message Relied, from =  {message.NickNameFrom} to  {message.NickNameTo} ");
            }
            else
            {
                Console.WriteLine($"Пользватель не найден");
            }
        }
        async Task ConfirmMessageReceived(int? id)
        {
            Console.WriteLine($"Message confirmation id =  + {id}");
            using (var ctx = new ChatContext())
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
            switch(message.Command)
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
            while(work)
            {
                try
                {
                   var message = _messageSourse.Receive(ref ep);
                    Console.WriteLine($"* * * {message.ToString()} * * *");
                    await ProcessMessage(message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
       
        
    }
}
