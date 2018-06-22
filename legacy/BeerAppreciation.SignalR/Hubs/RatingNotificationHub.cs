namespace BeerAppreciation.SignalR.Hubs
{
    using System;
    using System.Globalization;
    using Messages;
    using Microsoft.AspNet.SignalR;

    public class RatingNotificationHub : Hub
    {
        public void RateBeverage(BeverageRating rating)
        {
            this.Clients.All.beverageRated(rating);
        }
    }
}
