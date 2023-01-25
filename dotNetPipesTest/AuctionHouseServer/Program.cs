using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using AuctionHouseServer;

// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

namespace AuctionHouseServer
{
    internal class Program
    {
        //private static NamedPipeServerStream pipe;
        private static PipeServer pipeServer;
        private static Server server;
        
        static void Main(string[] args)
        {
            Console.WriteLine("halko");

            server = new Server();
            server.run();

            //pipe = new NamedPipeServerStream("demoPipe", PipeDirection.InOut, 1);
            //pipeServer = new PipeServer("demo2pipe");
            //run();
        }

        /*
         protected async Task ExecuteAsync(CancellationToken stoppingToken) // ..
        {
            Console.WriteLine("Server is started");

            //await Demo2();
            Console.ReadKey();
        }
        */

        /*
        private static async void run()
        {
            Thread connecting = new Thread(ConnectClient);
            connecting.Start();
            
            await ClientCommunication();
        }

        private static void ConnectClient()
        {
            Console.WriteLine("Server is started");
            Console.WriteLine(Process.GetCurrentProcess().Id);

            string? msg;
            while(true)
            {
                if (pipeServer.isConnected())
                {
                    pipeServer.WaitConnection();
                    //msg = Console.ReadLine();
                    msg = pipeServer.Read();
                    Console.WriteLine(msg);
                    pipeServer.close();
                    pipeServer = new PipeServer("demo2pipe");
                }
                //pipeServer.WriteIfConnected(msg);
                //Console.WriteLine("server message");
                Thread.Sleep(1000);
            }
            Console.ReadKey();
        }

        private static async Task ClientCommunication()
        {
            while (true)
            {
                Console.WriteLine("komunikacja");
                Thread.Sleep(1000);
            }
        }
        */
        
    }
}

