using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp06C.Abstracts
{
    public interface IMessageSource
    {
        Task SendAsyncNetMes(NetMessage message, IPEndPoint ep);
        NetMessage ReceiveNetMes(ref IPEndPoint ep);
    }
}
