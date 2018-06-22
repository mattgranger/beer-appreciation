namespace BeerAppreciation.Domain
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Data.EF.Domain;

    [Table("EventBeverages", Schema = "BA")]
    public class EventBeverage : BaseEntityWithState<int>
    {
        public int EventId { get; set; }
        public int EventRegistrationId { get; set; }
        public int BeverageId { get; set; }
        public int DrinkingOrder { get; set; }
        public decimal? Score { get; set; }

        public Event Event { get; set; }
        public EventRegistration EventRegistration { get; set; }
        public Beverage Beverage { get; set; }
    }
}
