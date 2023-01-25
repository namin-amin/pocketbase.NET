using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace pocketbase.net.Models.ResponseHelpers
{
    public class Response<T>
        where T: class
    {
        public int  Code{ get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
