using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp06C
{
    public class NetMessage
    {
        
        public int? Id;
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string? NickNameFrom { get; set; }
        public string? NickNameTo { get; set; }

        public IPEndPoint? EndPoint { get; set; }
        public Command Command { get; set; }

        public string SerializeMessageToJSON() => JsonSerializer.Serialize(this);

        public static NetMessage? DeserializeMessageFromJSON(string message) => JsonSerializer.Deserialize<NetMessage>(message);

        //public void PrintGetMessageFrom()
        //{
        //    Console.WriteLine(ToString());
        //}

        public void PrintGetMessageFrom()
        {
            if (OperatingSystem.IsWindows())
            {
                var position = Console.GetCursorPosition();
                int left = position.Left;
                int top = position.Top;
                Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
                Console.SetCursorPosition(0, top);
                Console.WriteLine($"{DateTime} от {NickNameFrom}\nПолучено сообщение * {Text} * \n");
                Console.Write("Введите имя получателя: ");
                Console.SetCursorPosition(left, top + 2);
            }
            else Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return $"{DateTime} от {NickNameFrom}\nПолучено сообщение * {Text} * ";
        }
        
    }
}
