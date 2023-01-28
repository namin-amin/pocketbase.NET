using pocketbase.net.Models.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Schema;

namespace pocketbase.net.Models
{
    public class RecordModel : PbBaseModel
    {
        public IDictionary<string, IEnumerable<RecordModel>> Expand { get; set; } = new Dictionary<string, IEnumerable<RecordModel>>();

        [JsonIgnore]
        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();


        public RecordModel()
        {

        }

        public RecordModel(IDictionary<string, object> _data) { 
         
            ExpandData(_data);
        }

        //this function to be called recursively
        public static RecordModel ExpandData(IDictionary<string, object> data)
        {
           var model = new RecordModel(data);
           var result =  new List<RecordModel>();

            foreach (var item in data) 
            {
                if ( item.Value is IEnumerable )
                {
                    foreach (var it in (IEnumerable)item.Value )
                    {
                        result.Add(RecordModel.ExpandData((IDictionary<string, object>)it));
                    }
                }

                if (item.Value is IDictionary)
                {
                    result.Add(RecordModel.ExpandData((IDictionary<string, object>)item.Value));
                }

                model.Expand[item.Key] = result;
            }

            /**
             Todo: check dart sdk and remove extra fields this methos is to remove all the things other than expand things 
             
             
             */




            return model;
        }
    }
}
