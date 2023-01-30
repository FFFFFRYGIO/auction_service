namespace AuctionHouseClient;

public class CommandJSON
{
    public string message { get; set; }
}

public class Base
{
    public string Type { get; set; }
}

public class CreateAuction : Base
{
    //public string Type { get; set; }
    public string Name { get; set; }
    public int Value { get; set; }
    public int Time { get; set; }
}

public class ShowAuctions : Base
{
    //public string Type { get; set; }
}

public class BidAuction : Base
{
    //public string Type { get; set; }
    public int auctionId { get; set; }
    public int bidValue { get; set; }
}

public class Fund : Base
{
    //public string Type { get; set; }
    public int value { get; set; }
}