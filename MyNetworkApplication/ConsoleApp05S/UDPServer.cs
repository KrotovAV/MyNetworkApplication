using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp05S
{
    internal class Server
    {
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        UdpClient udpClient;
        public void Register(NetMessage message, IPEndPoint fromep)
        {
            Console.WriteLine($"Message Reister, name =  {message.NickNameFrom}");
            clients.Add(message.NickNameFrom, fromep);

            using (var ctx = new ChatContext())
            {
                if (ctx.Users.FirstOrDefault(x => x.FullName == message.NickNameFrom) != null) return;

                ctx.Add(new User { FullName = message.NickNameFrom });

                ctx.SaveChanges();
            }
        }
        public void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine($"Message confirmation id =  + {id}");
            using(var ctx = new ChatContext())
            {
                var msg = ctx.Messages.FirstOrDefault(m => m.MessageId == id);
                if(msg != null)
                {
                    msg.IsSent = true;
                    ctx.SaveChanges();
                }
            }
        }
        public void RelyMessage(NetMessage message)
        { 
            int? id = null;
            if(clients.TryGetValue(message.NickNameTo, out IPEndPoint ep))
            {
                using (var ctx = new ChatContext())
                {
                    var fromUser = ctx.Users.First(x => x.FullName == message.NickNameFrom);
                    var toUser = ctx.Users.First(x => x.FullName == message.NickNameTo);
                    var msg = new Message() { UserFrom = fromUser, UserTo = toUser, IsSent = false, Text = message.Text };

                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();

                    id = msg.MessageId;
                }
                message.Id = (int)id;
                var forwardMessageJson = new NetMessage() { Id = (int)id, Command = Command.Message, NickNameTo = message.NickNameTo, NickNameFrom = message.NickNameFrom, Text = message.Text }.SerializeMessageToJSON();
                byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);

                udpClient.Send(forwardBytes, forwardBytes.Length, ep);
            }
            else
            {
                Console.WriteLine($"Пользватель не найден");
            }
        }
        public void ProcessMessage(NetMessage message, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {message.NickNameFrom} для {message.NickNameTo} с командой {message.Command}");
            Console.WriteLine($"* {message.Text} *");
            if(message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromep.Address, fromep.Port));
            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine($"Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }

        }
            public void Work()
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("UDP Клиент ждет сообщений");

            while (true)
            {
                byte[] reciveBytes = udpClient.Receive(ref remoteEndPoint);
                string reciveData = Encoding.ASCII.GetString(reciveBytes);
                Console.WriteLine($"Сообщение {reciveData}");


                try
                {
                    var message = NetMessage.DeserializeMessageFromJSON(reciveData);
                    ProcessMessage(message, remoteEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения");

                }

            }
        }
    }
}
