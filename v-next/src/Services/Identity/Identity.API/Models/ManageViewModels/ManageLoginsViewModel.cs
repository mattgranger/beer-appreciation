namespace BeerAppreciation.Services.BeerAppreciation.Services.Identity.API.Models.ManageViewModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
    }
}
