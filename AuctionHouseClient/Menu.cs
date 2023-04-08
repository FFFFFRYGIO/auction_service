using System.Text.Json;

namespace AuctionHouseClient;

public static class Menu
{
    public static void DrawMenu(string clientName)
    {
        Console.WriteLine("Welcome {0}", clientName);
        Console.WriteLine();
        Console.WriteLine("Choose option:");
        Console.WriteLine("1. Create auction");
        Console.WriteLine("2. See all auctions");
        Console.WriteLine("3. Bid auction");
        Console.WriteLine("4. Fund your account");
        Console.WriteLine("5. Quit");
    }

    public static string Select()
    {
        Console.Write("Input action: ");
        var action = (Console.ReadLine() ?? string.Empty);
        switch (action)
        {
            case "1":
                var newAuction = new CreateAuction();
                newAuction.Type = ClientActions.Create.ToString();
                Console.Write("Input auction name ");
                newAuction.Name = Console.ReadLine() ?? string.Empty;
                Console.Write("Input auction value ");
                newAuction.Value = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Input auction time ");
                newAuction.Time = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                return JsonSerializer.Serialize(newAuction);
            case "2":
                var order = new ShowAuctions();
                order.Type = ClientActions.ShowAuctions.ToString();
                return JsonSerializer.Serialize(order);
            case "3":
                var newBid = new BidAuction();
                newBid.Type = ClientActions.Bid.ToString();
                Console.Write("Input auction id ");
                newBid.AuctionId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.Write("Input bid amount ");
                newBid.BidValue = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                return JsonSerializer.Serialize(newBid);
            case "4":
                var addFund = new Fund();
                addFund.Type = ClientActions.AddFunds.ToString();
                Console.Write("Input amount ");
                addFund.Value = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                return JsonSerializer.Serialize(addFund);
            case "5":
                return ClientActions.Quit.ToString();
            default:
                Console.Write("Error, wrong action");
                return ClientActions.Error.ToString();
        }
    }
}