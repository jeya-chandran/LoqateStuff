using System.Collections.Generic;

namespace Loqate.Models
{
    public class AddressSummary
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Highlight { get; set; }
        public string Description { get; set; }
        public long NoOfAddresses { get; set; } = 0;
        public string DisplayText { get { return (Text + " " + Description); } }
    }

    public class AddressSummaryResponse
    {
        public IEnumerable<AddressSummary> Items { get; set; }
    }
}

