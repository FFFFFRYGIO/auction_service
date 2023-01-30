using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public abstract class Client
    {
        protected readonly int Id;
        private int _money;
        private static int _idCounter = 1;

        protected Client(int initMoney)
        {
            Id = _idCounter++;
            _money = initMoney;
        }

        public int GetMoney()
        {
            return _money;
        }
        
        public int GetId()
        {
            return Id;
        }
        
        public void Bid(int auctionId, int amount, AuctionList<int> auctionList, List<Client> clientsList)
        {
            auctionList.Bid(auctionId, amount, Id, clientsList);
        }
        
        public void UpdateMoney(char op, int amount)
        {
            switch (op)
            {
                case '+':
                    _money += amount;
                    break;
                case '-':
                    if (amount > _money)
                        Console.WriteLine("Error, Client {0} don't have enough money", Id);
                    else
                        _money -= amount;
                    break;
                default:
                    Console.WriteLine("Error, wrong operator");
                    break;
            }
        }
        
        public void PrintAllAuctions(AuctionList<int> auctions)
        {
            auctions.PrintAllAuctions();
        }
        
        public abstract void CreateAuction(string auctionName, int initialValue, int auctionTime,
            AuctionList<int> auctionList);
    }
}