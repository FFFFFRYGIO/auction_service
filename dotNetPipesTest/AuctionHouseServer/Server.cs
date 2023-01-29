using System.Diagnostics;

namespace AuctionHouseServer
{
    public class Server
    {
        //private static NamedPipeServerStream pipe;
        private PipeServer pipeServer;
        private List<privPipe> pipeServers;

        public Server()
        {
            pipeServer = new PipeServer("demo2pipe");
            pipeServers = new List<privPipe>();
        }

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
            
            ClientCommunication();
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
                    
                    pipeServers.Add(new privPipe(new PipeServer(msg)));

                    Console.WriteLine(msg);
                    pipeServer.close();
                    pipeServer = new PipeServer("demo2pipe");
                    //pipeServer.WaitConnection();
                }
                //pipeServer.WriteIfConnected(msg);
                //Console.WriteLine("server message");
                Thread.Sleep(1000);
            }
            //Console.ReadKey();
        }

        private void ClientCommunication()
        {
            while (true)
            {
                foreach (var pipeServer in pipeServers)
                {
                    //Console.WriteLine(pipeServer.isCreated);
                    //Console.WriteLine("isCOnnecvted: " + pipe.isConnected());
                    if (false) //pipeServer.isCreated
                    {
                        Console.WriteLine("Client: " + pipeServer.pipe.getName() + " status: " + pipeServer.task.Status);
                    }
                    if (!pipeServer.isCreated)
                    {
                        pipeServer.task = new Task(() =>
                        {
                            communicate(pipeServer.pipe);
                        });
                        
                        pipeServer.task.Start();
                        pipeServer.isCreated = true;
                    }
                    else if (pipeServer.task.Status != TaskStatus.Running)
                    {
                        pipeServer.task.Dispose();
                        pipeServer.task = new Task(() =>
                        {
                            communicate(pipeServer.pipe);
                        });
                        
                        pipeServer.task.Start();
                    }
                }

                Thread.Sleep(500);
            }
        }

        private async Task communicate(PipeServer pipe)
        {
            string msg;
            if (pipe.isConnected())
            {
                //pipe.WriteIfConnected("hello ");
                pipe.WaitConnection();
                msg = pipe.Read();
                CommandJSON message = new CommandJSON();
                message.message = msg;
                //Console.WriteLine("From: " + pipe.getName() + " " + msg);
                //pipe.WriteIfConnected("From: " + pipe.getName() + " " + msg);
                pipe.WriteIfConnected(message);
            }
        }
    }
}

