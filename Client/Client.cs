using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ClassLibrary1;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Client
{
    class ClientProgram
    {
        public string Status;

        static void Main(string[] args)
        {
            var path = @"C:\Users\45535\Desktop\RAWDATA\C#\Projects\Assignment_3_Vers01\Client\Test.json";

            // Define and start TCP client
            using var tcpClient = new TcpClient();

            // Client connect to local via. port 5000.
            tcpClient.Connect(IPAddress.Loopback, 5000);
            Console.WriteLine("Client started");


            //Define our request
            Request request = new Request("Update", path, 2, "Bent");

            //Send request to server - see Util.cs
            Util.SendRequest(tcpClient, request.ToJson());

            //Console.WriteLine($"Message to the server: {}");
            Console.WriteLine("We send this to the client: \n " + request.ToJson());
            //File.WriteAllText(path, request.ToJson());
            while (true)
            {
                try
                {
                    //Read responses from the server.
                    string readResponse = ClientReadResponse(tcpClient, tcpClient.GetStream());
                    ResponseContainer r3 = JsonConvert.DeserializeObject<ResponseContainer>(readResponse);
                    Console.WriteLine($"The server responds:  \n + {r3.Status} \n {r3.Body}");
                    break;
                }
                catch (Exception e)
                {

                }



                //We send respond to a JSON formatted string.


                //Now we desirialize it, so it will be stored in our "responseContainer.cs" OBS WAIT UNTIL WE SEND right response from server.
            }
        }

        public static string ClientReadResponse(TcpClient client, NetworkStream stream)
        {
            byte[] data = new byte[client.ReceiveBufferSize];

            var cnt = stream.Read(data);

            var msg = Encoding.UTF8.GetString(data, 0, cnt);
            return msg;
        }

        public static string GetServerResponse(TcpClient tcpClient)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[2048];
            int bytesToRead = stream.Read(buffer, 0, buffer.Length);
            var responseFromServer = Encoding.UTF8.GetString(buffer, 0, bytesToRead);
            return responseFromServer;
        }


        public string IntepretStatus()
        {
            const string ok = "ok";
            const string created = "created";
            const string updated = "Updated";
            const string badRequest = "badRequest";
            const string notFound = "notFound";
            const string error = "error";

            if (Status == "1")
            {
                Console.WriteLine(ok);
            }
            else if (Status == "2")
            {
                Console.WriteLine(created);
            }
            else if (Status == "3")
            {
                Console.WriteLine(updated);
            }
            else if (Status == "4")
            {
                Console.WriteLine(badRequest);
            }
            else if (Status == "5")
            {
                Console.WriteLine(notFound);
            }
            else if (Status == "6")
            {
                Console.WriteLine(error);
            }

            return Status;
        }

        public static ResponseContainer ReadResponse(TcpClient client)
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
                return JsonSerializer.Deserialize<ResponseContainer>(responseData,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }
    }
}