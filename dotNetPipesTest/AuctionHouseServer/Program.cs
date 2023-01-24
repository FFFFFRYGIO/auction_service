using System.Diagnostics;
using System.IO.Pipes;
// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

namespace AuctionHouseServer
{
    internal class Program
    {
        //private static NamedPipeServerStream pipe;
        private static PipeServer pipeServer;
        
        static void Main(string[] args)
        {
            Console.WriteLine("halko");

            //pipe = new NamedPipeServerStream("demoPipe", PipeDirection.InOut, 1);
            pipeServer = new PipeServer("demo2pipe");
            run();
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken) // ..
        {
            Console.WriteLine("Server is started");

            await Demo2();
            Console.ReadKey();
        }

        private static async void run()
        {
            await testConnection();
        }

        protected static async Task testConnection()
        {
            Console.WriteLine("Server is started");

            await Demo2();
            Console.ReadKey();
        }

        private static async Task Demo2()
        {
            Console.WriteLine("Executing server-demo2");
            Console.WriteLine(Process.GetCurrentProcess().Id);

            string? msg;
            for (int i = 0; i < 40; i++)
            {
                msg = Console.ReadLine();
                pipeServer.WriteIfConnected(msg);
                //Console.WriteLine("server message");
                //Thread.Sleep(1000);
            }
        }
    }
}

