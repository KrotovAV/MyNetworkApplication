using System.Runtime.Intrinsics.X86;

namespace ConsoleApp06S
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            /*
            Подумаем как бы мы могли протестировать сервер. Очевидно что источником данных для 
            сервера является UDP соединение и заменив источник данных мы смогли бы точно 
            выяснить пересылает ли сервер сообщения. Давайте перепишем код сервера таким 
            образом чтобы он получал сообщения от интерфейса IMessageSource и его
            же использовал для отправки
            */
            using (var сtx = new ChatContext())
            {
                var user1 = сtx.Users.FirstOrDefault(x => x.FullName == "Вася");
                var user2 = сtx.Users.FirstOrDefault(x => x.FullName == "Юля");
               
                var msg = сtx.Messages.FirstOrDefault(x => x.UserFrom == user1 && x.UserTo == user2);
                Console.WriteLine(msg.Text);

                Console.WriteLine(user1.FullName);
                Console.WriteLine(user1.MessagesTo.Count);
                Console.WriteLine(user1.MessagesFrom.Count);

                Console.WriteLine(user2.FullName);
                Console.WriteLine(user2.MessagesTo.Count);
                Console.WriteLine(user2.MessagesFrom.Count);

                Console.WriteLine(" * * * --------------");


                var xz = user1.MessagesTo;
                if (xz == null)
                {
                    Console.WriteLine("**************");
                    
                }
                else
                {
                    foreach (var m in xz)
                    {
                        Console.WriteLine("*");
                        Console.WriteLine(m.UserToId);
                        Console.WriteLine(m.ToString);
                    }
                }
                Console.WriteLine(" * * * ");
            }

        }
    }
}