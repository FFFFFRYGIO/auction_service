using System;

namespace ConsoleApplication1
{
    public class AuctioneerVip : Client
    {
        public AuctioneerVip(int initMoney) : base(initMoney)
        {
            Console.WriteLine("AuctioneerVIP with id {0} crated", Id);
        }

        public override void CreateAuction(string auctionName, int initialValue, int auctionTime,
            AuctionList<int> auctionList)
        {
            var auctionId = auctionList.CreateAuction(auctionName, initialValue, auctionTime, Id);
            if (auctionId != -1)
                Console.WriteLine("AuctioneerVIP {0} successfully created auction with id {1}", Id, auctionId);
        }
        
    }
}