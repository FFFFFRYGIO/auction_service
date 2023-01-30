using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ConsoleApplication1
{
    public class AuctionList<T>
    {
        private T _idCounter;
        private T _idStep;
        private List<Auction> _auctionList = new List<Auction>();

        public AuctionList(T auctionListIdCounter, T idStep)
        {
            _idCounter = auctionListIdCounter;
            _idStep = idStep;
        }

        public int CreateAuction(string auctionName, int initialValue, int auctionTime, int ownerId)
        {
            if (string.IsNullOrEmpty(auctionName))
            {
                Console.WriteLine("Error, Auction name cannot be empty");
                return -1;
            }
            else if (initialValue <= 0)
            {
                Console.WriteLine("Error, Initial value must be greater than zero");
                return -1;
            }
            else if (auctionTime <= 0)
            {
                Console.WriteLine("Error, Auction time must be greater than zero");
                return -1;
            }
            else
            {
                var auction = new Auction(Convert.ToInt32(_idCounter), auctionName, initialValue, auctionTime, ownerId);

                if (auction.Id == -1) return -1;

                _auctionList.Add(auction);
                
                var x = Expression.Parameter(typeof(T), "_idCounter");
                var y = Expression.Parameter(typeof(T), "_idStep");
                var body = Expression.Add(x, y);
                var incrementIdCounter = Expression.Lambda<Func<T, T, T>>(body, x, y).Compile();
                _idCounter = incrementIdCounter(_idCounter, _idStep);

                return auction.Id;
            }
        }
        private void DeleteAuction(int auctionId, List<Client> clientsList)
        {
            var auctionToRemove = _auctionList.FirstOrDefault(a => a.Id == auctionId);
            if (auctionToRemove.WinnerId != null)
            {
                clientsList.Find(c => c.GetId() == auctionToRemove.OwnerId).UpdateMoney('+', auctionToRemove.Cost);
            }
            if (auctionToRemove != null)
            {
                _auctionList.Remove(auctionToRemove);
            }
            else
            {
                Console.WriteLine("Error, Auction with given id of {0} was not found for delete", auctionId);
            }
        }

        public string Bid(int auctionId, int amount, int clientId, List<Client> clientsList)
        {
            if (clientsList.Find(c => c.GetId() == clientId).GetMoney() < amount)
            {
                Console.WriteLine("Error, Client {0} doesn't have enough money ({1})", clientId, amount);
                return "Error, Client " + clientId + " doesn't have enough money (" + amount + ")";
            }
            else
            {
                var auction = _auctionList.FirstOrDefault(a => a.Id == auctionId);
                if (auction != null)
                {
                    auction.Bid(amount, clientId, clientsList);
                }
                else
                {
                    Console.WriteLine("Error, Auction with id {0} was not found", auctionId);
                    return "Error, Auction with id " + auctionId + " was not found";
                }
            }

            return "Bid Accepted";
        }
        
        public List<String> PrintAllAuctions()
        {
            var auctionStrings = _auctionList.StringRepresentation();
            Console.WriteLine("Auctions printed successfully");
            foreach (var auctionString in auctionStrings)
            {
                Console.WriteLine(auctionString);
            }

            return auctionStrings;
        }
        
        public void CloseAuctions(List<Client> clientList)
        {
            List<int> toDelete = new List<int>();
            foreach (var auction in _auctionList)
            {
                var warsawTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var currentTimeInWarsaw = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, warsawTimeZone.Id);

                if (auction.TimeToEnd < currentTimeInWarsaw)
                {
                    toDelete.Add(auction.Id);
                    
                }
            }

            foreach (var auctionId in toDelete)
            {
                DeleteAuction(auctionId, clientList);
            }
        }
    }
}