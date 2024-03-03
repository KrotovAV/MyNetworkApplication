namespace DebugTest
{
    internal class Program
    {
        public static int Calc(int num, int a){ 
            for(int i = 0; i < a; i++)
            {
                num = num + a;
                Console.WriteLine(num);
            }
            return num;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            int a = 5;
            int b = Calc(a, 5);
            Console.WriteLine();
            Console.WriteLine(b);
        }
    }
}