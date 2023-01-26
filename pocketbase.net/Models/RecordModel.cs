using pocketbase.net.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace pocketbase.net.Models
{
    public class RecordModel : PbBaseModel
    {
        public IDictionary<string, IEnumerable<RecordModel>> Expand { get; set; } = new Dictionary<string, IEnumerable<RecordModel>>();


        public RecordModel(IDictionary<string, object> _data) { 
         
            ExpandData(_data);
        }

        private void ExpandData(IDictionary<string, object> data)
        {
           Expand = new Dictionary<string, IEnumerable<RecordModel>>();

           if (data != null)
            {
                foreach (var item in data)
                {
                    //TODO
                 //Expand.Add(item.Key,)
                }
            }
        }
    }
}
