using System.Collections.Generic;
using System.Threading.Tasks;

using Loqate.Models;

namespace Loqate
{
    public interface ILoqateClient
    {
        string BaseUrl { get; set; }
        string ApiKey { get; set; }
        string Countries { get; set; }

        string InitialFindResource { get; set; }
        string SubsequentFindResource { get; set; }
        string RetrieveFullAddressResource { get; set; }

        /// <summary>
        /// Initial search of any address based on the query
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="query">Query to be used for the search</param>
        /// <returns>List of AddressSummary objects</returns>
        List<AddressSummary> InitialFind(string country, string query);

        /// <summary>
        /// Subsequent search of any address 
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="addressSummary">AddressSummary object returned from the InitialFind method call</param>
        /// <returns>List of AddressSummary objects</returns>
        List<AddressSummary> SubsequentFind(string country, AddressSummary addressSummary);

        /// <summary>
        /// Retrieves the full destination address for the id returned by previous InitialFind or SubsequentFind method calls
        /// </summary>
        /// <param name="id">Id of the address to be retrieved</param>
        /// <returns>Full destination address</returns>
        FullAddress RetrieveFullAddressById(string id);

        Task<List<AddressSummary>> InitialFindAsync(string country, string query);
        Task<List<AddressSummary>> SubsequentFindAsync(string country, AddressSummary addressSummary);
        Task<FullAddress> RetrieveFullAddressByIdAsync(string id);
    }
}
