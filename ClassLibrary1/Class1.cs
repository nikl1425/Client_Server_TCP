using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using Server;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ClassLibrary1
{

    public static class Util
    {
        public static string ToJson(this object data)
        {
            return JsonSerializer.Serialize(data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }




        public static T FromJson<T>(this string element)
        {
            return JsonSerializer.Deserialize<T>(element,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        public static void SendRequest(this TcpClient client, string request)
        {
            var msg = Encoding.UTF8.GetBytes(request);
            client.GetStream().Write(msg, 0, msg.Length);
        }

        public static Response ReadResponse(TcpClient client)
        {
            var strm = client.GetStream();
            strm.ReadTimeout = 250;
            byte[] resp = new byte[2048];
            using (var memStream = new MemoryStream())
            {
                int bytesread = 0;
                do
                {
                    bytesread = strm.Read(resp, 0, resp.Length);
                    memStream.Write(resp, 0, bytesread);

                } while (bytesread == 2048);

                var responseData = Encoding.UTF8.GetString(memStream.ToArray());
                return JsonSerializer.Deserialize<Response>(responseData,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }


        }




    }
}

