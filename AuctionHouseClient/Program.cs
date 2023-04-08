// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System.Diagnostics;
using System.IO.Pipes;
using System.Text.Json;

namespace AuctionHouseClient
{
    internal static class Program
    {
        private static string _name;
        private static NamedPipeClientStream _privateCommunicationPipe;
        private static bool _loop = true;

        private static void Main()
        {
            Console.WriteLine("Client Started");
            
            ConnectServer();
            if (_name != "")
            {
                PrivateConnection();
            }
            
            Console.ReadKey();
        }

        private static void ConnectServer()
        {
            Console.Write("Enter username: ");
            _name = Console.ReadLine() ?? string.Empty;
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

            var sw = new StreamString(pipe);

            sw.WriteString(_name);
            
            pipe.Dispose();
        }

        private static void PrivateConnection()
        {
            _privateCommunicationPipe = new NamedPipeClientStream(".", _name, PipeDirection.InOut);
            
            Console.WriteLine("Connecting to private line");
            while (!_privateCommunicationPipe.IsConnected)
            {
                try
                {
                    _privateCommunicationPipe.Connect();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("Connected to the private communication line");
            
            var pipeWr = new StreamString(_privateCommunicationPipe);

            while (_loop)
            {
                Menu.DrawMenu(_name);
                string ask = Menu.Select();

                pipeWr.WriteString(ask);
                _privateCommunicationPipe.Flush();
                
                if (ask == ClientActions.Quit.ToString())
                {
                    _loop = false;
                    _privateCommunicationPipe.Close();
                    _privateCommunicationPipe.Dispose();
                }
                else
                {
                    var msg = pipeWr.ReadString();
                    var reply = JsonSerializer.Deserialize<Response>(msg);
                    if (reply != null && reply.Message != "list")
                    {
                        Console.WriteLine(reply.Message);
                    }
                    else
                    {
                        foreach (var auctionString in JsonSerializer.Deserialize<List<string>>(reply.AuctionList))
                        {
                            Console.WriteLine(auctionString);
                        }
                    }
                }
                
                

                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}