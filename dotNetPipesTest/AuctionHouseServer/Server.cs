using System.Diagnostics;
using System.Text.Json;
using AuctionHouseClient;
using ConsoleApplication1;

namespace AuctionHouseServer
{
    public class Server
    {
        //private static NamedPipeServerStream pipe;
        private PipeServer pipeServer;
        private List<privPipe> pipeServers;

        private AuctionList<int> auctionList;
        private List<Client> clientsList;

        public Server()
        {
            pipeServer = new PipeServer("demo2pipe");
            pipeServers = new List<privPipe>();
            auctionList = new AuctionList<int>(1, 1);
            clientsList = new List<Client>();
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
            bool vipClientSwitch = false;
            while(true)
            {
                if (pipeServer.isConnected())
                {
                    pipeServer.WaitConnection();
                    //msg = Console.ReadLine();
                    msg = pipeServer.Read();
                    
                    pipeServers.Add(new privPipe(new PipeServer(msg)));
                    // odd vips
                    if (!clientsList.Exists(e => e.GetName() == msg))
                    {
                        if (vipClientSwitch)
                        {
                            clientsList.Add(new AuctioneerVip(2000, msg));
                            vipClientSwitch = false;
                        }
                        else
                        {
                            clientsList.Add(new Auctioneer(2000, msg));
                            vipClientSwitch = true;
                        }
                    }

                    Console.WriteLine(msg);
                    pipeServer.close();
                    pipeServer.Dispose();
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
            List<privPipe> toDelete = new List<privPipe>();
            while (true)
            {
                auctionList.CloseAuctions(clientsList);
                foreach (var pipeServer in pipeServers)
                {
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
                    else if (pipeServer.task.Status != TaskStatus.Running && pipeServer.task.Status != TaskStatus.WaitingForActivation
                             && pipeServer.task.Status != TaskStatus.WaitingToRun)
                    {
                        pipeServer.task.Dispose();
                        if (pipeServer.pipe.isConnected() &&
                            !pipeServer.pipe.toDelete)
                        {
                            pipeServer.task = new Task(() =>
                            {
                                communicate(pipeServer.pipe);
                            });
                        
                            pipeServer.task.Start();
                        }
                        else if(pipeServer.pipe.toDelete)
                        {
                            toDelete.Add(pipeServer);
                        }
                        
                    }
                }

                foreach (var privPipe in toDelete)
                {
                    pipeServers.Remove(privPipe);
                    privPipe.Dispose();
                }
                toDelete.Clear();

                Thread.Sleep(500);
            }
        }

        private async Task communicate(PipeServer pipe)
        {
            string msg;
            if (pipe.isConnected() && !pipe.toDelete)
            {
                pipe.WaitConnection();
                msg = pipe.Read();
                Response message = new Response();
                
                var rec = JsonSerializer.Deserialize<Base>(msg);

                switch (rec.Type)
                {
                    case "create":
                        var received = JsonSerializer.Deserialize<CreateAuction>(msg);
                        message.message = clientsList.Find(c => c.GetName() == pipe.getName()).CreateAuction(
                            received.Name, received.Value, received.Time, auctionList);
                        //snd = "Auction Created"; ThingsForAuction.Camera.ToString()
                        //message.message = "Auction Created";
                        break;
                    case "showauctions":
                        //var received2 = JsonSerializer.Deserialize<ShowAuctions>(msg);
                        List<string> send = auctionList.PrintAllAuctions();
                        string stringList = JsonSerializer.Serialize(send);
                        if (send.Count() > 0)
                        {
                            message.message = "list";
                        }
                        else
                        {
                            message.message = "No auctions currently running!";
                        }
                        message.auctionList = stringList;
                        //Console.WriteLine(message.auctionList);
                        //snd = JsonSerializer.Serialize(send);
                        break;
                    case "bid":
                        var received3 = JsonSerializer.Deserialize<BidAuction>(msg);
                        message.message = clientsList.Find(c => c.GetName() == pipe.getName()).Bid(received3.auctionId, received3.bidValue,auctionList,clientsList);
                        break;
                    case "addfunds":
                        var received4 = JsonSerializer.Deserialize<Fund>(msg);
                        message.message = clientsList.Find(c => c.GetName() == pipe.getName()).UpdateMoney('+', received4.value);
                        break;
                    case "quit":
                        //pipe.close();
                        pipe.toDelete = true;
                        message.message = "Disconnected";
                        break;
                }
                //message.message = "msg";
                //Console.WriteLine("From: " + pipe.getName() + " " + msg);
                //pipe.WriteIfConnected("From: " + pipe.getName() + " " + msg);
                pipe.WriteIfConnected(JsonSerializer.Serialize(message));
            }
        }
    }
}

