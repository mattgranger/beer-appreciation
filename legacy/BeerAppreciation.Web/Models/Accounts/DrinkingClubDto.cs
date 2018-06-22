using BeerAppreciation.Domain;

namespace BeerAppreciation.Web.Models.Accounts
{
    public class DrinkingClubDto
    {
        public DrinkingClubDto()
        {
        }

        public DrinkingClubDto(DrinkingClub dc)
        {
            this.Id = dc.Id;
            this.Name = dc.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}