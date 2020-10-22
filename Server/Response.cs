using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class Response
    {
        public string status { get; set; }
        public string reason { get; set;  }
        

        public Response(string Status, string Reason)
        {
            status = Status;
            reason = Reason;
        }
        
         


       
    }
}