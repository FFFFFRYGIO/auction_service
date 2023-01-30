using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    public static class ExtensionMethods
    {
        public static List<string> StringRepresentation(this List<Auction> auctions)
        {
            return auctions.Select(a => $"{a.Id,-2} {a.Name,-10} {a.OwnerId,-2} {a.WinnerId,-2} {a.Cost,-5} {a.TimeToEnd}").ToList();
        }
    }
}
