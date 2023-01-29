// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.IO.Pipes;

namespace AuctionHouseClient
{
    internal class Program
    {
        private static string name;
        private static NamedPipeClientStream privateCommunicationPipe;
        static void Main(string[] args)
        {
            Console.WriteLine("Client Started");
            
            ConnectServer();
            if (name != "")
            {
                PrivateConnection();
            }
            
            //Communication();
            
            Console.ReadKey();
        }

        private static void ConnectServer()
        {
            name = Console.ReadLine();
            //connect
            var pipe = new NamedPipeClientStream(".", "demo2pipe", PipeDirection.InOut);
            
            Console.WriteLine("Waiting for connection");
            while (!pipe.IsConnected)
            {
                try
                {
                    pipe.Connect();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Connected to the sever");
            Console.WriteLine(Process.GetCurrentProcess().Id);
            
            //inform sever

            StreamString sw = new StreamString(pipe);

            sw.WriteString(name);
            
            pipe.Close();

            /*
            
            */
        }

        private static void PrivateConnection()
        {
            privateCommunicationPipe = new NamedPipeClientStream(".", name, PipeDirection.InOut);
            
            Console.WriteLine("Connecting to private line");
            while (!privateCommunicationPipe.IsConnected)
            {
                try
                {
                    privateCommunicationPipe.Connect();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Connected to the private communication line");
            
            StreamString pipeWR = new StreamString(privateCommunicationPipe);
            string? msg;

            while (true)
            {
                msg = Console.ReadLine();
                pipeWR.WriteString(msg);
                privateCommunicationPipe.WaitForPipeDrain();
                privateCommunicationPipe.Flush();
                msg = pipeWR.ReadString();
                Console.WriteLine(msg);
            }
        }

        private static void Communication()
        {
            StreamString sr = new StreamString(privateCommunicationPipe);

            string? msg;
            while ((msg = sr.ReadString()) != null)
            {
                Console.WriteLine(msg);
            }
        }
    }
}