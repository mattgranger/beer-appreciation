using System.Linq;

namespace BeerAppreciation.Data.IntegrationTests.Helpers
{
    using Repositories.Context;
    using Domain;

    public static class BeverageTestHelper
    {

        /// <summary>
        /// Uses the WebApi to add a beverage to the database
        /// </summary>
        /// <param name="beverage">The beverage.</param>
        /// <returns>
        /// The id of the beverage added to database
        /// </returns>
        public static int AddBeverage(Beverage beverage)
        {
            using (var dbContext = new DatabaseContext())
            {
                dbContext.Beverages.Add(beverage);
                dbContext.SaveChanges();
            }

            return beverage.Id;
        }

        /// <summary>
        /// Gets the specified beverage.
        /// </summary>
        /// <param name="beverageId">The beverage identifier.</param>
        /// <returns>
        /// The specified beverage
        /// </returns>
        public static Beverage GetBeverage(int beverageId)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Beverages
                    .Include("Manufacturer")
                    .Include("Type")
                    .Include("Style")
                    .FirstOrDefault(b => b.Id == beverageId);
            }
        }

        /// <summary>
        /// Deletes the beverage.
        /// </summary>
        /// <param name="beverageId">The beverage identifier.</param>
        public static void DeleteBeverage(int beverageId)
        {
            using (var dbContext = new DatabaseContext())
            {
                dbContext.Database.ExecuteSqlCommand("DELETE FROM BA.Beverages WHERE ID = {0}", beverageId);
            }
        }
    }
}
