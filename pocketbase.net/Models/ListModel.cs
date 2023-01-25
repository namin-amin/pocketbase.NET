using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models
{
    public class ListModel<T> 
        where T:BaseModel
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
