// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.IO.Pipes;
using System.Text.Json;

namespace AuctionHouseClient
{
    internal class Program
    {
        private static string name;
        private static NamedPipeClientStream privateCommunicationPipe;
        private static bool loop = true;
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
            Console.Write("Enter username: ");
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
            
            //pipe.Close();
            pipe.Dispose();

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

            while (loop)
            {
                Menu.drawMenu(name);
                //msg = Console.ReadLine();
                string ask = Menu.Select();

                pipeWR.WriteString(ask);
                //privateCommunicationPipe.WaitForPipeDrain();
                privateCommunicationPipe.Flush();
                
                if (ask == "quit")
                {
                    loop = false;
                    //privateCommunicationPipe.Close();
                    privateCommunicationPipe.Close();
                    privateCommunicationPipe.Dispose();
                    break;
                }
                
                msg = pipeWR.ReadString();
                Response reply = JsonSerializer.Deserialize<Response>(msg);
                if (reply.message != "list")
                {
                    Console.WriteLine(reply.message);
                }
                else
                {
                    foreach (var auctionString in JsonSerializer.Deserialize<List<string>>(reply.auctionList))
                    {
                        Console.WriteLine(auctionString);
                    }
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}