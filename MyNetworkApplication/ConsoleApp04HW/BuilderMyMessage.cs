using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp04HW
{
    
    public interface IBuilderMyMessage
    {
        void BuildNickNameFrom(string nickNameFrom);
        void BuildNickNameTo();
        void BuildText();
        MyMessage Build();
    }

    public class ConcreteBuilder : IBuilderMyMessage
    {
        private string from;
        private string to;
        private string textMessage;
        
        public MyMessage Build()
        {
            return new MyMessage() { NickNameFrom = from, NickNameTo = to, Text = textMessage, DateTime = DateTime.Now };
        }

        public void BuildNickNameFrom(string nickNameFrom)
        {
            this.from = nickNameFrom;
        }

        public void BuildNickNameTo()
        {
            string nameTo;
            do
            {
                Console.WriteLine("Введите получателя сообщения");
                nameTo = Console.ReadLine();
                
            } while (string.IsNullOrEmpty(nameTo));
            this.to = nameTo;
        }

        public void BuildText()
        {
            string message;
            {
                do
                {
                    Console.WriteLine("Введите сообщение для отправки");
                    message = Console.ReadLine();
                } while (string.IsNullOrEmpty(message));
            }
            this.textMessage = message;
        }
    }
    
}
