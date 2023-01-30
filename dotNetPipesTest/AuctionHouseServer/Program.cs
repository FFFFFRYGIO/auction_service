using System.Diagnostics;
using System.IO.Pipes;
using System.Threading;
using AuctionHouseServer;

// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

namespace AuctionHouseServer
{
    internal partial class Program
    {
        private static Server server;
        
        static void Main(string[] args)
        {
            Console.WriteLine("HALOOOO");

            server = new Server();
            server.run();

            //pipe = new NamedPipeServerStream("demoPipe", PipeDirection.InOut, 1);
            //pipeServer = new PipeServer("demo2pipe");
            //run();
        }

    }
}

