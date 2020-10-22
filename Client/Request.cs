using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    /*
     * This class contains the Json request format we use to uphold the protocol.
     */

    public class Request
    {
        // Public fields
        public string Method;
        public string Path;
        public long Date;
        public string Name;
        public int Id;


        // Constructor for initializing request.
        public Request(string method, string path, int id, string name)
        {
            Method = method;
            Path = path;
            Date = GetCurrentDate();
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Call method "RequestToJson" to pass constructor variables to Json string. Use "this" keyword to reference the current scope.
        /// </summary>
        /// <returns>Object to Json</returns>
        public string ToJson()
        {
            var jsonRequest = JsonConvert.SerializeObject(this);
            return jsonRequest;
        }

        /// <summary>
        /// Cast date to current time stamp (seconds since 1970)
        /// </summary>
        /// <returns>returns long</returns>
        public long GetCurrentDate()
        {
            long date = DateTimeOffset.Now.ToUnixTimeSeconds();
            return date;
        }
    }
}