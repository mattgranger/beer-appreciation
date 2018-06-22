using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeerAppreciation.Core.Helpers;
using BeerAppreciation.Data.IntegrationTests.Helpers;
using BeerAppreciation.Data.Tests.Mocks;
using BeerAppreciation.Domain;
using NUnit.Framework;

namespace BeerAppreciation.Data.IntegrationTests
{
    /// <summary>
    /// Integration tests relating to beverages
    /// </summary>
    [TestFixture]
    public class BeverageIntegrationTests
    {
        #region Fields and Constants

        /// <summary>
        /// The service endpoint
        /// </summary>
        private readonly string serviceEndpoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BeverageIntegrationTests" /> class.
        /// </summary>
        public BeverageIntegrationTests()
        {
            this.serviceEndpoint = ConfigurationManager.AppSettings["WebApi.Beverage.Endpoint"];
        }

        #endregion

        #region Test Initialisation Methods

        /// <summary>
        /// Method that is run before each test
        /// </summary>
        [TestFixtureSetUp]
        public void TestInitialise()
        {
            // Need this to prevent the FakeDbContext from attempting to create a code first database
            Database.SetInitializer<FakeDbContext>(null);
        }

        #endregion

        #region Tests

        //[Test]
        public void TestBeverageDelete()
        {
            var beverage = new Beverage
            {
                AlcoholPercent = 7.5M,
                Description = "Test Beverage",
                ManufacturerId = 1,
                Name = "Awesome Brew",
                BeverageTypeId = 1,
                BeverageStyleId = 1,
                Volume = 375
            };

            beverage.Id = BeverageTestHelper.AddBeverage(beverage);
            Assert.IsTrue(beverage.Id > 0);

            // Act
            HttpResponseMessage response = HttpRequestHelper.Delete(this.serviceEndpoint, String.Format("api/beverages/{0}", beverage.Id));

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        #endregion
    }
}
