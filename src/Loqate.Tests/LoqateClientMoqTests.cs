using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Loqate.Models;
using Moq;
using System.Linq;

namespace Loqate.Tests
{
    [TestClass]
    public class LoqateClientMoqTests
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

            Client = new LoqateClient(apiKey, baseUrl, initialFindResource, subsequentFindResource, retrieveResource);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Client = null;
        }

        [TestMethod]
        public async Task Moq_InitialFindAsync_ForGBandHA29HA_ReturnsNotNull()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";
            List<AddressSummary> addresses = new List<AddressSummary>
            {
                new AddressSummary
                {
                    Id = "GB|RM|ENG|9HA-HA2",
                    Type = "Postcode",
                    Text = "HA2 9HA",
                    Highlight = "0-3,4-7",
                    Description = "Malvern Avenue, Harrow - 8 Addresses",
                }
            };

            var mock = new Mock<ILoqateClient>();
            mock.Setup(x => x.InitialFindAsync(country, postcode)).Returns(Task.FromResult(addresses));

            // Act
            List<AddressSummary> addressSummaries = await mock.Object.InitialFindAsync(country, postcode);

            // Assert
            Assert.AreNotEqual(addressSummaries, null);
        }

        [TestMethod]
        public async Task Moq_InitialFindAsync_ForGBandHA29HA_Returns8Addresses()
        {
            // Arrange
            var country = "GB";
            var postcode = "HA2 9HA";
            var numberOfHousesInHa29Ha = 8;

            List<AddressSummary> addresses = new List<AddressSummary>
            {
                new AddressSummary
                {
                    Id = "GB|RM|ENG|9HA-HA2",
                    Type = "Postcode",
                    Text = "HA2 9HA",
                    Highlight = "0-3,4-7",
                    Description = "Malvern Avenue, Harrow - 8 Addresses",
                }
            };

            var mock = new Mock<ILoqateClient>();
            mock.Setup(x => x.InitialFindAsync(country, postcode)).Returns(Task.FromResult(addresses));

            // Act
            List<AddressSummary> addressSummaries = await mock.Object.InitialFindAsync(country, postcode);

            // Assert
            Assert.AreEqual(GetNumberFromString(addressSummaries[0].Description), numberOfHousesInHa29Ha);
        }

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
