using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerAppreciation.SignalR.Messages
{
    public class BeverageRating
    {
        public string Beverage { get; set; }
        public string DrinkingName { get; set; }
        public decimal? Rating { get; set; }
    }
}
