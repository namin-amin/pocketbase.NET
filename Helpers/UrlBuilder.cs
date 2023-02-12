using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pocketbase.net.Helpers
{
    internal class UrlBuilder
    {

        public UrlBuilder(string collectionName)
        {
            this.CollectionName = collectionName;

        }
        private string CollectionName { get; set; } = string.Empty;

        /// <summary>
        /// builds the request url for records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string CollectionUrl(string id = "")
        {
            return "collections/" + CollectionName.ToLower() + "/records/" + id;
        }
    }
}