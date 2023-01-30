using System;
using System.Collections.Generic;
using Regex = System.Text.RegularExpressions.Regex;

namespace ConsoleApplication1
{
    public class Auction : IValidateName, IDisposable
    {
        public readonly int Id;
        public string Name;
        public int Cost;
        public int OwnerId;
        public int? WinnerId;
        public DateTime TimeToEnd;
        
        private bool disposed = false;

        public Auction(int id, string name, int cost, int auctionTime, int ownerId)
        {
            if (!ValidateName(name.Trim()))
            {
                Console.WriteLine("Error, wrong auction name input ({0})", name);
                Id = -1;
            }
            else
            {
                Id = id;
                Name = Regex.Replace(name.Trim(), @"\s+", " ");
                Cost = cost;
                var warsawTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var currentTimeInWarsaw = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, warsawTimeZone.Id);

                TimeToEnd = currentTimeInWarsaw.AddMinutes(auctionTime);
                OwnerId = ownerId;
            }
        }

        public void Bid(int amount, int clientId, List<Client> clientsList)
        {
            if (amount > Cost)
            {
                if (WinnerId == null)
                {
                    WinnerId = clientId;
                    clientsList.Find(c => c.GetId() == clientId).UpdateMoney('-', amount);
                    Console.WriteLine("Client {0} successfully executed first bid for auction {1}", clientId, Id);
                }
                else if (!Equals(clientId, WinnerId))
                {
                    clientsList.Find(c => c.GetId() == clientId).UpdateMoney('-', amount);
                    clientsList.Find(c => c.GetId() == WinnerId).UpdateMoney('+', Cost);
                    WinnerId = clientId;
                    Console.WriteLine("Client {0} successfully bid auction {1}", clientId, Id);
                }
                else
                {
                    clientsList.Find(c => c.GetId() == clientId).UpdateMoney('-', amount - Cost);
                    Console.WriteLine("Client {0} bid his own price {1}", clientId, Id);
                }
                Cost = amount;
            }
            else if (amount == Cost)
            {
                Console.WriteLine("Error, bid amount ({0}) is equal to current cost ({1})", amount, Cost);
            }
            else
            {
                Console.WriteLine("Error, bid amount ({0}) is lower than current cost ({1})", amount, Cost);
            }
        }

        public bool ValidateName(string name)
        {
            var regex = new Regex("^[A-Z][a-z]+\\b(?:\\s+[A-Z][a-z]+\\b)*$");
            return regex.IsMatch(name);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            
            if(!this.disposed)
            {
                disposed = true;
            }
        }

        ~Auction()
        {
            Dispose(disposing: false);
        }
    }
}