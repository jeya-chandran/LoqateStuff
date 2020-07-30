using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Loqate.Models;
using System.Text.RegularExpressions;
using System.Linq;

namespace Loqate.Tests
{
    [TestClass]
    public class LoqateClientTests
    {
        public LoqateClient Client { get; private set; }

        [TestInitialize]
        public void Init()
        {
            var baseUrl = ConfigurationManager.AppSettings["LoqateBaseUrl"];
            var apiKey = ConfigurationManager.AppSettings["LoqateApiKey"];
            var initialFindResource = ConfigurationManager.AppSettings["LoqateInitialFindResource"];
            var subsequentFindResource = ConfigurationManager.AppSettings["LocateSubsequentFindResource"];
            var retrieveResource = ConfigurationManager.AppSettings["LoqateRetrieveResource"];

            Client =  new LoqateClient(apiKey, baseUrl, initialFindResource, subsequentFindResource, retrieveResource);
        }

        [TestCleanup]
        public void  Cleanup()
        {
            Client = null;
        }

        #region Sync methods

        [TestMethod]
        public void InitialFind_ForGBandHA29HA_ReturnsNotNull()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            // Act
            List<AddressSummary> addresses =  Client.InitialFind(country, postcode);

            // Assert
            Assert.AreNotEqual(addresses, null);
        }

        [TestMethod]
        public void InitialFind_ForGBandHA29HA_Returns8Addresses()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";
            var numberOfHousesInHa29Ha = 8;

            // Act
            List<AddressSummary> addresses = Client.InitialFind(country, postcode);

            // Assert
            Assert.AreEqual(GetNumberFromString(addresses[0].Description), numberOfHousesInHa29Ha);
        }

        [TestMethod]
        public void SubsequentFind_ForGBHA29HA_RetunrsDescriptionAsHarrowHA29HA()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            List<AddressSummary> addressSummaries = Client.InitialFind(country, postcode);

            // Act
            List<AddressSummary> addresses = Client.SubsequentFind(country, addressSummaries[0]);

            // Assert
            Assert.AreEqual(addresses[0].Description, "Harrow, HA2 9HA");
        }

        [TestMethod]
        public void SubsequentFind_ForGBHA29HA_RetunrsCityAsHarrow()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            List<AddressSummary> addressSummaries = Client.InitialFind(country, postcode);
            List<AddressSummary> addresses = Client.SubsequentFind(country, addressSummaries[0]);

            // Act
            FullAddress fullAddress = Client.RetrieveFullAddressById(addresses[0].Id);

            // Assert
            Assert.AreEqual(fullAddress.City, "Harrow");
        }

        #endregion Async methods

        #region Async methods

        [TestMethod]
        public async Task InitialFindAsync_ForGBandHA29HA_ReturnsNotNull()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            // Act
            List<AddressSummary> addresses = await Client.InitialFindAsync(country, postcode);

            // Assert
            Assert.AreNotEqual(addresses, null);
        }

        [TestMethod]
        public async Task InitialFindAsync_ForGBandHA29HA_Returns8Addresses()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";
            var numberOfHousesInHa29Ha = 8;

            // Act
            List<AddressSummary> addresses = await Client.InitialFindAsync(country, postcode);

            // Assert
            Assert.AreEqual(GetNumberFromString(addresses[0].Description), numberOfHousesInHa29Ha);
        }

        [TestMethod]
        public async Task SubsequentFindAsync_ForGBHA29HA_RetunrsDescriptionAsHarrowHA29HA()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            List<AddressSummary> addressSummaries = await Client.InitialFindAsync(country, postcode);

            // Act
            List<AddressSummary> addresses = await Client.SubsequentFindAsync(country, addressSummaries[0]);

            // Assert
            Assert.AreEqual(addresses[0].Description, "Harrow, HA2 9HA");
        }

        [TestMethod]
        public async Task SubsequentFindAsync_ForGBHA29HA_RetunrsCityAsHarrow()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";

            List<AddressSummary> addressSummaries = await Client.InitialFindAsync(country, postcode);
            List<AddressSummary> addresses = await Client.SubsequentFindAsync(country, addressSummaries[0]);

            // Act
            FullAddress fullAddress = await Client.RetrieveFullAddressByIdAsync(addresses[0].Id);

            // Assert
            Assert.AreEqual(fullAddress.City, "Harrow");
        }

        #endregion Async methods

        private long GetNumberFromString(string source)
        {
            long number = 0;

            string[] numbers = Regex.Split(source, @"\D+");
            foreach (string value in numbers.Reverse())
            {
                if (!string.IsNullOrEmpty(value))
                {
                    number = long.Parse(value);
                    break;
                }
            }

            return (number);
        }
    }
}
