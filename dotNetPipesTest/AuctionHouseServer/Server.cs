using System.Diagnostics;

namespace AuctionHouseServer
{
    public class Server
    {
        //private static NamedPipeServerStream pipe;
        private PipeServer pipeServer;
        private List<PipeServer> pipeServers;

        public Server()
        {
            pipeServer = new PipeServer("demo2pipe");
            pipeServers = new List<PipeServer>();
        }
        
        /*
        static void Main(string[] args)
        {
            Console.WriteLine("halko");

            //pipe = new NamedPipeServerStream("demoPipe", PipeDirection.InOut, 1);
            
            run();
        }
        */

        /*
         protected async Task ExecuteAsync(CancellationToken stoppingToken) // ..
        {
            Console.WriteLine("Server is started");

            //await Demo2();
            Console.ReadKey();
        }
        */

        public async void run()
        {
            Thread connecting = new Thread(ConnectClient);
            connecting.Start();
            
            await ClientCommunication();
        }

        private void ConnectClient()
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
                    
                    pipeServers.Add(new PipeServer(msg));
                    
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

        private async Task ClientCommunication()
        {
            while (true)
            {
                int i = 0;
                foreach (var pipe in pipeServers)
                {
                    Console.WriteLine("komunikacja");
                    if (pipe.isConnected())
                    {
                        pipe.WriteIfConnected("hello " + i);
                    }

                    i++;
                }

                Thread.Sleep(1000);
            }
        }
    }
}

