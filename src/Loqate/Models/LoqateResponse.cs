using System.Collections.Generic;

namespace Loqate.Models
{
    public class LoqateResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
    }
}
