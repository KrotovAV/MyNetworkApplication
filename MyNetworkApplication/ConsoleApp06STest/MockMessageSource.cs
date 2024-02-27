using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp06S;
using ConsoleApp06S.Abstracts;
using ConsoleApp06S.Services;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace ConsoleApp06STest
{
    public class MockMessageSource : IMessageSource
    {
        private Server server;
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
        private Queue<NetMessage> messages = new Queue<NetMessage>();
        
        

        public MockMessageSource() 
        {
            messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Вася" });
            messages.Enqueue(new NetMessage { Command = Command.Register, NickNameFrom = "Юля" });
            messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Юля", NickNameTo = "Вася", Text = "Привет, Василий"});
            messages.Enqueue(new NetMessage { Command = Command.Message, NickNameFrom = "Вася", NickNameTo = "Юля", Text = "Привет, Юлька!!!" });
        }
        public void AddServer(Server srv)
        {
            server = srv;
        }
        public async Task SendAsync(NetMessage message, IPEndPoint ep)
        {

        }

        public NetMessage Receive(ref IPEndPoint ep)
        {
            ep = endPoint;
            if(messages.Count == 0)
            {
                server.Stop();
                return null;
            }
            NetMessage msg = messages.Dequeue();
            return msg;
        }
    }
}
