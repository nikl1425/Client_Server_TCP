using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class RequestContainer
    {
        public string Method {
            get;
            set;
        }
        public string Path{
            get;
            set;
        }
        public string Date  {
            get;
            set;
        }
        
        public string Body { get; set; }





        /*
       [JsonProperty("results")]
       public Results Results { get; set; }
    }

    public class Results
    {
        [JsonProperty("jobCodes")]
        public Dictionary<string, JobCode> JobCodes { get; set; }
    }

    public class JobCode
    {
        [JsonProperty ("Method")]
        public string Method { get; set; }
        [JsonProperty ("Path")]
        public string Path { get; set; }
        [JsonProperty ("Date")] 
        public long Date { get; set; }
        [JsonProperty ("Name")]
        public string Name { get; set; }
        [JsonProperty ("Id")]
        public string Id { get; set; }
        */
    }
    
}
