using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public abstract class Client
    {
        protected readonly int Id;
        private int _money;
        private static int _idCounter = 1;
        private string _Name;

        protected Client(int initMoney, string name)
        {
            Id = _idCounter++;
            _money = initMoney;
            _Name = name;
        }

        public int GetMoney()
        {
            return _money;
        }
        
        public int GetId()
        {
            return Id;
        }
        
        public string GetName()
        {
            return _Name;
        }
        
        
        public string Bid(int auctionId, int amount, AuctionList<int> auctionList, List<Client> clientsList)
        {
            return auctionList.Bid(auctionId, amount, Id, clientsList);
        }
        
        public string UpdateMoney(char op, int amount)
        {
            switch (op)
            {
                case '+':
                    _money += amount;
                    return "Balance updated";
                    break;
                case '-':
                    if (amount > _money)
                    {
                        Console.WriteLine("Error, Client {0} don't have enough money", Id);
                        return "Error, Client " + Id + " don't have enough money";
                    }
                    else
                    {
                        _money -= amount;
                        return "Operation succeeded";
                    }
                        
                    break;
                default:
                    Console.WriteLine("Error, wrong operator");
                    return "Error, wrong operator";
                    break;
            }
        }

        public abstract string CreateAuction(string auctionName, int initialValue, int auctionTime,
            AuctionList<int> auctionList);
    }
}