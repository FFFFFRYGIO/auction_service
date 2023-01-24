// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.IO.Pipes;

namespace AuctionHouseClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Started");
            Demo2();
            Console.ReadKey();
        }

        static void Demo2()
        {
            //connect
            var pipe = new NamedPipeClientStream(".", "demo2pipe", PipeDirection.InOut);
            
            Console.WriteLine("Waiting for connection");
            pipe.Connect();
            Console.WriteLine("Connected to the sever");
            Console.WriteLine(Process.GetCurrentProcess().Id);
            
            //read data

            using StreamReader sr = new StreamReader(pipe);

            string? msg;
            while ((msg = sr.ReadLine()) != null)
            {
                Console.WriteLine(msg);
            }
        }
    }
}