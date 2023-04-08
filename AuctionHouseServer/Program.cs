namespace AuctionHouseServer
{
    internal static class Program
    {
        private static Server _server;

        private static void Main()
        {
            _server = new Server();
            _server.Run();
        }

    }
}

