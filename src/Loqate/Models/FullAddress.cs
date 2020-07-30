using System.Collections.Generic;

namespace Loqate.Models
{
    public class FullAddress
    {
        public string BuildingNumber { get; set; }
        public string BuildingName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string PostalCode { get; set; }
        public string CountryIso2 { get; set; }
        public string CountryName { get; set; }
        public string Type { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
    }

    public class FullAddressResponse
    {
        public IEnumerable<FullAddress> Items { get; set; }
    }
}
/*
      "Id": "GB|RM|A|10454867",
      "DomesticId": "10454867",
      "Language": "ENG",
      "LanguageAlternatives": "ENG",
      "Department": "",
      "Company": "",
      "SubBuilding": "",
      "BuildingNumber": "115",
      "BuildingName": "",
      "SecondaryStreet": "",
      "Street": "Malvern Avenue",
      "Block": "",
      "Neighbourhood": "",
      "District": "",
      "City": "Harrow",
      "Line1": "115 Malvern Avenue",
      "Line2": "",
      "Line3": "",
      "Line4": "",
      "Line5": "",
      "AdminAreaName": "Harrow",
      "AdminAreaCode": "",
      "Province": "",
      "ProvinceName": "",
      "ProvinceCode": "",
      "PostalCode": "HA2 9HA",
      "CountryName": "United Kingdom",
      "CountryIso2": "GB",
      "CountryIso3": "GBR",
      "CountryIsoNumber": 826,
      "SortingNumber1": "77112",
      "SortingNumber2": "",
      "Barcode": "(HA29HA1GT)",
      "POBoxNumber": "",
      "Label": "115 Malvern Avenue\nHARROW\nHA2 9HA\nUNITED KINGDOM",
      "Type": "Residential",
      "DataLevel": "Premise",
      "Field1": "",
      "Field2": "",
      "Field3": "",
      "Field4": "",
      "Field5": "",
      "Field6": "",
      "Field7": "",
      "Field8": "",
      "Field9": "",
      "Field10": "",
      "Field11": "",
      "Field12": "",
      "Field13": "",
      "Field14": "",
      "Field15": "",
      "Field16": "",
      "Field17": "",
      "Field18": "",
      "Field19": "",
      "Field20": ""
*/

/*
{
  "Items": [
    {
      "Id": "US|US|A|V211903284|2",
      "DomesticId": "V211903284",
      "Language": "ENG",
      "LanguageAlternatives": "ENG",
      "Department": "",
      "Company": "",
      "SubBuilding": "Ste 2",
      "BuildingNumber": "1312",
      "BuildingName": "",
      "SecondaryStreet": "",
      "Street": "Boylston St",
      "Block": "",
      "Neighbourhood": "",
      "District": "",
      "City": "Boston",
      "Line1": "1312 Boylston St Ste 2",
      "Line2": "",
      "Line3": "",
      "Line4": "",
      "Line5": "",
      "AdminAreaName": "Suffolk",
      "AdminAreaCode": "025",
      "Province": "MA",
      "ProvinceName": "Massachusetts",
      "ProvinceCode": "MA",
      "PostalCode": "02215-4300",
      "CountryName": "United States",
      "CountryIso2": "US",
      "CountryIso3": "USA",
      "CountryIsoNumber": 840,
      "SortingNumber1": "",
      "SortingNumber2": "",
      "Barcode": "",
      "POBoxNumber": "",
      "Label": "1312 Boylston St Ste 2\nBOSTON MA 02215-4300\nUNITED STATES",
      "Type": "Residential",
      "DataLevel": "Range",
      "Field1": "",
      "Field2": "",
      "Field3": "",
      "Field4": "",
      "Field5": "",
      "Field6": "",
      "Field7": "",
      "Field8": "",
      "Field9": "",
      "Field10": "",
      "Field11": "",
      "Field12": "",
      "Field13": "",
      "Field14": "",
      "Field15": "",
      "Field16": "",
      "Field17": "",
      "Field18": "",
      "Field19": "",
      "Field20": ""
    }
  ]
}*/
