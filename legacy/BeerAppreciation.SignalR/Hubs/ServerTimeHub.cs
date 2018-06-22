using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BeerAppreciation.SignalR.Hubs
{
    public class ServerTimeHub : Hub
    {
        public string GetServerTime()
        {
            return DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        }
    }
}
