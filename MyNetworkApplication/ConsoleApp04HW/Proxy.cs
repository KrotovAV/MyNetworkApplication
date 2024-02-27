using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp04HW
{
    //Proxy
    interface ISubject
    {
        void Request(string s);
        
    }


    class Proxy : ISubject
    {
        private Server server;

        public Proxy()
        {
            server = new Server();
        }

        public void Request(string s)
        {
            Console.WriteLine("Proxy получает запрос");
            server.Request(s);
            
        }
    }
}
