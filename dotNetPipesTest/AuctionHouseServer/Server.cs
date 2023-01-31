using System.Diagnostics;
using System.Text.Json;
using AuctionHouseClient;
using ConsoleApplication1;

namespace AuctionHouseServer
{
    public class Server
    {
        //private static NamedPipeServerStream pipe;
        private PipeServer _pipeServer;
        private readonly List<PrivPipe> _pipeServers;

        private readonly AuctionList<int> _auctionList;
        private readonly List<Client> _clientsList;

        public Server()
        {
            _pipeServer = new PipeServer("demo2pipe");
            _pipeServers = new List<PrivPipe>();
            _auctionList = new AuctionList<int>(1, 1);
            _clientsList = new List<Client>();
        }
        
        public void Run()
        {
            var connecting = new Thread(ConnectClient);
            connecting.Start();
            
            ClientCommunication();
        }

        private void ConnectClient()
        {
            Console.WriteLine("Server is started");
            Console.WriteLine(Process.GetCurrentProcess().Id);

            string? msg;
            var vipClientSwitch = false;
            while(true)
            {
                if (_pipeServer.IsConnected())
                {
                    _pipeServer.WaitConnection();
                    msg = _pipeServer.Read();
                    
                    _pipeServers.Add(new PrivPipe(new PipeServer(msg)));
                    if (!_clientsList.Exists(e => e.GetName() == msg))
                    {
                        if (vipClientSwitch)
                        {
                            _clientsList.Add(new AuctioneerVip(2000, msg));
                            vipClientSwitch = false;
                        }
                        else
                        {
                            _clientsList.Add(new Auctioneer(2000, msg));
                            vipClientSwitch = true;
                        }
                    }

                    Console.WriteLine(msg);
                    _pipeServer.Close();
                    _pipeServer.Dispose();
                    _pipeServer = new PipeServer("demo2pipe");
                }
                Thread.Sleep(1000);
            }
        }

        private void ClientCommunication()
        {
            var toDelete = new List<PrivPipe>();
            while (true)
            {
                _auctionList.CloseAuctions(_clientsList);
                foreach (var pipeServer in _pipeServers)
                {
                    if (!pipeServer.IsCreated)
                    {
                        pipeServer.Task = new Task(() =>
                        {
                            Communicate(pipeServer.Pipe);
                        });
                        
                        pipeServer.Task.Start();
                        pipeServer.IsCreated = true;
                    }
                    else if (pipeServer.Task.Status != TaskStatus.Running && pipeServer.Task.Status != TaskStatus.WaitingForActivation
                             && pipeServer.Task.Status != TaskStatus.WaitingToRun)
                    {
                        pipeServer.Task.Dispose();
                        if (pipeServer.Pipe.IsConnected() &&
                            !pipeServer.Pipe.ToDelete)
                        {
                            pipeServer.Task = new Task(() =>
                            {
                                Communicate(pipeServer.Pipe);
                            });
                        
                            pipeServer.Task.Start();
                        }
                        else if(pipeServer.Pipe.ToDelete)
                        {
                            toDelete.Add(pipeServer);
                        }
                        
                    }
                }

                foreach (var privPipe in toDelete)
                {
                    _pipeServers.Remove(privPipe);
                    privPipe.Dispose();
                }
                toDelete.Clear();

                Thread.Sleep(500);
            }
        }

        private async Task Communicate(PipeServer pipe)
        {
            if (pipe.IsConnected() && !pipe.ToDelete)
            {
                pipe.WaitConnection();
                var msg = pipe.Read();
                var message = new Response();
                
                var rec = JsonSerializer.Deserialize<Base>(msg);

                if (rec != null)
                    switch (rec.Type)
                    {
                        case "Create":
                            var received = JsonSerializer.Deserialize<CreateAuction>(msg);
                            if (received != null)
                                message.Message = _clientsList.Find(c => c.GetName() == pipe.GetName()).CreateAuction(
                                    received.Name, received.Value, received.Time, _auctionList);
                            break;
                        case "ShowAuctions":
                            var send = _auctionList.PrintAllAuctions();
                            var stringList = JsonSerializer.Serialize(send);
                            message.Message = send.Any() ? "list" : "No auctions currently running!";

                            message.AuctionList = stringList;
                            break;
                        case "Bid":
                            var received3 = JsonSerializer.Deserialize<BidAuction>(msg);
                            if (received3 != null)
                                message.Message = _clientsList.Find(c => c.GetName() == pipe.GetName())
                                    .Bid(received3.AuctionId, received3.BidValue, _auctionList, _clientsList);
                            break;
                        case "AddFunds":
                            var received4 = JsonSerializer.Deserialize<Fund>(msg);
                            if (received4 != null)
                                message.Message = _clientsList.Find(c => c.GetName() == pipe.GetName())
                                    .UpdateMoney('+', received4.Value);
                            break;
                        case "Quit":
                            pipe.ToDelete = true;
                            message.Message = "Disconnected";
                            break;
                    }
                pipe.WriteIfConnected(JsonSerializer.Serialize(message));
            }
        }
    }
}

