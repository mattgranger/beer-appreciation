using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeerAppreciation.Web.Models
{
    // Models returned by MeController actions.
    public class GetViewModel
    {
        public string DrinkingName { get; set; }
    }
}