using System;

namespace ConsoleApplication1
{
    public class AuctioneerVip : Client
    {
        public AuctioneerVip(int initMoney,string name) : base(initMoney,name)
        {
            Console.WriteLine("AuctioneerVIP with id {0} crated", Id);
        }

        public override string CreateAuction(string auctionName, int initialValue, int auctionTime,
            AuctionList<int> auctionList)
        {
            var auctionId = auctionList.CreateAuction(auctionName, initialValue, auctionTime, Id);
            if (auctionId != -1)
            {
                //Console.WriteLine("AuctioneerVIP {0} successfully created auction with id {1}", Id, auctionId);
                return "AuctioneerVIP " + Id + " successfully created auction with id " + auctionId;
            }
                
            return "An error occured during creating new auction";  
        }
        
    }
}