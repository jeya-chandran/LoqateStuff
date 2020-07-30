using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RestSharp;

using Loqate.Models;

namespace Loqate
{
    public class LoqateClient : ILoqateClient
    {
        private readonly RestClient _client;

        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string Countries { get; set; }

        public string InitialFindResource { get; set; }
        public string SubsequentFindResource { get; set; }
        public string RetrieveFullAddressResource { get; set; }

        #region Constructors

        public LoqateClient(string apiKey, string baseUrl, string initialFindResource, string subsequentFindResource, string retrieveResource)
        {
            BaseUrl = baseUrl;
            ApiKey = apiKey;

            InitialFindResource = initialFindResource;
            SubsequentFindResource = subsequentFindResource;
            RetrieveFullAddressResource = retrieveResource;

            _client = CreateHttpClient(baseUrl);
        }

        #endregion Constructors

        #region Public methods

        #region Sync API calls

        /// <summary>
        /// Initial search of any address based on the query
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="query">Query to be used for the search</param>
        /// <returns>List of AddressSummary objects</returns>
        public List<AddressSummary> InitialFind(string country, string query)
        {
            List<AddressSummary> results = new List<AddressSummary>();

            Countries = country;

            var req = CreateHttpRequest(InitialFindResource, Method.GET, query);
            IRestResponse<LoqateResponse<AddressSummary>> response = _client.Execute<LoqateResponse<AddressSummary>>(req);

            if (response != null)
            {
                results = RetrieveResultsListFromResponse<AddressSummary>(response);
            }

            return (results);
        }

        /// <summary>
        /// Subsequent search of any address 
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="addressSummary">AddressSummary object returned from the InitialFind method call</param>
        /// <returns>List of AddressSummary objects</returns>
        public List<AddressSummary> SubsequentFind(string country, AddressSummary addressSummary)
        {
            List<AddressSummary> results = new List<AddressSummary>();

            Countries = country;

            var req = CreateHttpRequest(SubsequentFindResource, Method.GET, addressSummary.Id);
            IRestResponse<LoqateResponse<AddressSummary>> response = _client.Execute<LoqateResponse<AddressSummary>>(req);

            if (response != null)
            {
                results = RetrieveResultsListFromResponse<AddressSummary>(response);
            }

            return (results);
        }

        /// <summary>
        /// Retrieves the full destination address for the id returned by previous InitialFind or SubsequentFind method calls
        /// </summary>
        /// <param name="id">Id of the address to be retrieved</param>
        /// <returns>Full destination address</returns>
        public FullAddress RetrieveFullAddressById(string id)
        {
            FullAddress results = null;

            var req = CreateHttpRequest(RetrieveFullAddressResource, Method.GET, id);
            IRestResponse<LoqateResponse<FullAddress>> response = _client.Execute<LoqateResponse<FullAddress>>(req);

            if (response != null)
            {
                var addresses = RetrieveResultsListFromResponse<FullAddress>(response);
                if (addresses != null && addresses.Count > 0)
                {
                    results = addresses[0];
                    FormatAddressInto2AddressLines(results);
                }
            }

            return (results);
        }

        #endregion Sync API calls

        #region Async API calls

        /// <summary>
        /// Initial search of any address based on the query
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="query">Query to be used for the search</param>
        /// <returns>List of AddressSummary objects</returns>
        public async Task<List<AddressSummary>> InitialFindAsync(string country, string query)
        {
            List<AddressSummary> results = new List<AddressSummary>();

            Countries = country;

            var req = CreateHttpRequest(InitialFindResource, Method.GET, query);
            IRestResponse<LoqateResponse<AddressSummary>> response = await _client.ExecuteGetTaskAsync<LoqateResponse<AddressSummary>>(req);

            if (response != null)
            {
                results = RetrieveResultsListFromResponse<AddressSummary>(response);
            }

            return (results);
        }

        /// <summary>
        /// Subsequent search of any address 
        /// </summary>
        /// <param name="country">Country to be searched within</param>
        /// <param name="addressSummary">AddressSummary object returned from the InitialFind method call</param>
        /// <returns></returns>
        public async Task<List<AddressSummary>> SubsequentFindAsync(string country, AddressSummary addressSummary)
        {
            List<AddressSummary> results = new List<AddressSummary>();

            Countries = country;

            var req = CreateHttpRequest(SubsequentFindResource, Method.GET, addressSummary.Id);
            IRestResponse<LoqateResponse<AddressSummary>> response = await _client.ExecuteGetTaskAsync<LoqateResponse<AddressSummary>>(req);

            if (response != null)
            {
                results = RetrieveResultsListFromResponse<AddressSummary>(response);
            }

            return (results);
        }

        /// <summary>
        /// Retrieves the full destination address for the id returned by previous InitialFind or SubsequentFind method calls
        /// </summary>
        /// <param name="id">Id of the address to be retrieved</param>
        /// <returns>Full destination address</returns>
        public async Task<FullAddress> RetrieveFullAddressByIdAsync(string id)
        {
            FullAddress results = null;

            var req = CreateHttpRequest(RetrieveFullAddressResource, Method.GET, id);
            IRestResponse<LoqateResponse<FullAddress>> response = await _client.ExecuteGetTaskAsync<LoqateResponse<FullAddress>>(req);

            if (response != null)
            {
                var addresses = RetrieveResultsListFromResponse<FullAddress>(response);
                if (addresses != null && addresses.Count > 0)
                {
                    results = addresses[0];
                    FormatAddressInto2AddressLines(results);
                }
            }

            return (results);
        }

        #endregion Async API calls

        #endregion Public methods

        #region Private methods

        /// <summary>
        /// Creates a RestClient object
        /// </summary>
        /// <param name="baseUrl">Base URL</param>
        /// <returns>RestClient object</returns>
        private RestClient CreateHttpClient(string baseUrl)
        {
            var client = new RestClient(baseUrl);

            return (client);
        }

        /// <summary>
        /// Creates a RestRequest object
        /// </summary>
        /// <param name="resource">Find resource url of the request </param>
        /// <param name="httpMethod">HTTP method</param>
        /// <param name="query">Query to be attached to the resource</param>
        /// <returns>RestRequest object</returns>
        private RestRequest CreateHttpRequest(string resource, Method httpMethod, object query)
        {
            resource = Rts(resource);
            resource = resource.Replace("{QUERY}", query.ToString());

            var request = new RestRequest(resource, httpMethod);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            return (request);
        }

        /// <summary>
        /// Runtime substitution of the resource
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string Rts(string resource)
        {
            if (!string.IsNullOrEmpty(ApiKey))
            {
                resource = resource.Replace("{APIKEY}", ApiKey);
            }

            if (!string.IsNullOrEmpty(Countries))
            {
                resource = resource.Replace("{COUNTRIES}", Countries);
            }

            return (resource);
        }

        /// <summary>
        /// Formats the address into 2 Address Lines address
        /// </summary>
        /// <param name="results"></param>
        private static void FormatAddressInto2AddressLines(FullAddress results)
        {
            if (!string.IsNullOrEmpty(results.Line3))
            {
                results.Line1 = results.Line1 + (!string.IsNullOrEmpty(results.Line2) ? ", " + results.Line2 : "");
                results.Line2 = results.Line3;
                results.Line3 = string.Empty;
            }
        }

        /// <summary>
        /// Retrieves the results list from the RestResponse object
        /// </summary>
        /// <typeparam name="T">Type of list of objects to be returned</typeparam>
        /// <param name="response">Rest response object</param>
        /// <returns>List of T objects</returns>
        private List<T> RetrieveResultsListFromResponse<T>(IRestResponse response)
        {
            List<T> results = new List<T>();

            if (response != null)
            {
                var contents = JsonConvert.DeserializeObject<LoqateResponse<T>>(response.Content);
                results = contents.Items.ToList();
            }

            return (results);
        }

        #endregion Private methods
    }
}
