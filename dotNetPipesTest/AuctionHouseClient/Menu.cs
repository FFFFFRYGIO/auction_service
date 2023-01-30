using System.Text.Json;

namespace AuctionHouseClient;

public static class Menu
{
    public static void drawMenu(string clientName)
    {
        Console.WriteLine("Welcome {0}", clientName);
        Console.WriteLine();
        Console.WriteLine("Choose option:");
        Console.WriteLine("1. Create auction");
        Console.WriteLine("2. See all auctions");
        Console.WriteLine("3. Bid auction");
        Console.WriteLine("4. Fund your account");
    }

    public static string select()
    {
        Console.Write("Input action: ");
        var action = int.Parse(Console.ReadLine() ?? string.Empty);
        switch (action)
        {
            case '1':
                CreateAuction newAuction = new CreateAuction();
                newAuction.Type = "create";
                Console.Write("Input auction name");
                newAuction.Name = Console.ReadLine();
                Console.WriteLine("Input auction value");
                newAuction.Value = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.WriteLine("Input auction time");
                newAuction.Time = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                // CreateAuction(auctionName, auctionValue, auctionTime)
                return JsonSerializer.Serialize(newAuction);
                break;
            case '2':
                // ShowAuctions()
                ShowAuctions order = new ShowAuctions();
                order.Type = "showauctions";
                return JsonSerializer.Serialize(order);
                break;
            case '3':
                BidAuction newBid = new BidAuction();
                newBid.Type = "bid";
                Console.WriteLine("Input auction id");
                newBid.auctionId = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.WriteLine("Input bid amount");
                newBid.bidValue = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                // BidAuction(auctionId, bidAmount)
                return JsonSerializer.Serialize(newBid);
                break;
            case '4':
                Fund addFund = new Fund();
                addFund.Type = "addfunds";
                Console.WriteLine("Input amount");
                addFund.value = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                // FundAccount(fundAmount)
                return JsonSerializer.Serialize(addFund);
                break;
            default:
                Console.WriteLine("Error, wrong action");
                break;
        }
    }
}