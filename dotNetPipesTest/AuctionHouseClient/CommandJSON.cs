namespace AuctionHouseClient;

public class CommandJSON
{
    public string Message { get; set; }
}

public class Response : CommandJSON
{
    public string AuctionList { get; set; }
}

public class Base
{
    public string Type { get; set; }
}

public class CreateAuction : Base
{
    public string Name { get; set; }
    public int Value { get; set; }
    public int Time { get; set; }
}

public class ShowAuctions : Base { }

public class BidAuction : Base
{
    public int AuctionId { get; set; }
    public int BidValue { get; set; }
}

public class Fund : Base
{
    public int Value { get; set; }
}