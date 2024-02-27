using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp06S.Abstracts
{
    public interface IMessageSource
    {
        Task SendAsync(NetMessage message, IPEndPoint ep);
        NetMessage Receive(ref IPEndPoint ep);
    }
}
