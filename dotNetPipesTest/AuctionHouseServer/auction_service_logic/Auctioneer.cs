using System;

namespace ConsoleApplication1
{
    public class Auctioneer : Client
    {

        public Auctioneer(int initMoney, string name) : base(initMoney,name)
        {
            Console.WriteLine("Auctioneer with id {0} crated", Id);
        }

        public override void CreateAuction(string auctionName, int initialValue, int auctionTime,
            AuctionList<int> auctionList)
        {
            if (initialValue > 1000)
            {
                Console.WriteLine("Error, Non-VIP auctioneer is not allowed to create auctions with that high initial amount, id: {0}", Id);
            }
            else
            {
                var auctionId = auctionList.CreateAuction(auctionName, initialValue, auctionTime, Id);
                if (auctionId != -1)
                    Console.WriteLine("Auctioneer {0} successfully created auction with id {1}", Id, auctionId);
            }
        }
    }
}