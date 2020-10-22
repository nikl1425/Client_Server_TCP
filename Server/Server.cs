using Assignment3TestSuite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {

            // set server to TCP listener on local
            var server = new TcpListener(IPAddress.Loopback, 5000);
            server.Start();
            Console.WriteLine("STARTED SERVER!");


            while (true)
            {
                // set client = new tcp client. 
                var connection = server.AcceptTcpClient();

                Console.WriteLine("Accepted client!");

                var stream = connection.GetStream();
                byte[] data = new byte[connection.ReceiveBufferSize];
                var cnt = stream.Read(data);

                var msg = Encoding.UTF8.GetString(data, 0, cnt);
                Console.WriteLine($"Message from client:  {msg}");

                // Desiralize the request into r and use methods. 

                RequestContainer r = Util.FromJson<RequestContainer>(msg);
                //RequestContainer r = Util.FromJson<RequestContainer>(msg);

                //Throw body variables into bodyContainer object.
                 /*
                var JsonBody = r.Body.ToJson();
                BodyContainer bodyContainer = Util.FromJson<BodyContainer>(JsonBody);
                 */
                 
                Console.WriteLine("the method of msg is: \n" + r.Method + "\n" + r.Body);

                string getRequestBody = r.Body;
                Console.WriteLine(r.Body);

                // RESPOND TYPES:
                Response perfect = new Response("1 OK", "Well done.");
                Response created = new Response("2 Created: ", " ");
                Response updated = new Response("3 Updated: ", " ");
                Response missingAllMethods = new Response("4 missing date, missing method, illegal method ", " ");
                Response missingRessourse = new Response("4 missing resource", " ");
                Response illigalDate = new Response("4 illegal date", " ");
                Response badRequest = new Response("4 Bad Request", null);
                Response missingBody = new Response("illegal body", $"{getRequestBody}");
                Response notFound = new Response("5 Not found: ", " ");
                Response error = new Response("6 error: ", " ");

                Console.WriteLine($"{missingBody.reason}");

                // Legal methods:
                HashSet<string> serverMethods = new HashSet<string>();
                serverMethods.Add("create");
                serverMethods.Add("read");
                serverMethods.Add("update");
                serverMethods.Add("delete");
                serverMethods.Add("echo");

                string dateTimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();


                if (r.Method == null && r.Date == null)
                {
                    var missedmethodJson = Util.ToJson(missingAllMethods);
                    Console.WriteLine(missedmethodJson);
                    byte[] response = Encoding.UTF8.GetBytes(missedmethodJson.ToUpper());
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                    continue;
                }
                else if (!serverMethods.Contains(r.Method))
                {
                    var illegalMethodJson = Util.ToJson(missingAllMethods);
                    Console.WriteLine("Illegal method: " + illegalMethodJson);
                    byte[] response = Encoding.UTF8.GetBytes(illegalMethodJson.ToUpper());
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                    continue;
                }
                else if (serverMethods.Contains(r.Method) && r.Date != null && r.Path == null && r.Body == null)
                {
                    var missingResources = Util.ToJson(missingRessourse);
                    Console.WriteLine("Ressource doesn't exists: " + missingResources);
                    byte[] response = Encoding.UTF8.GetBytes(missingResources);
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                }
                else if (r.Date != dateTimeUnix && serverMethods.Contains(r.Method) && r.Path == "testing" && r.Body != null)
                {
                    var illigaldate = Util.ToJson(illigalDate);
                    byte[] response = Encoding.UTF8.GetBytes(illigaldate.ToUpper());
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                }
                else if (!serverMethods.Contains(r.Method) && r.Path != null && r.Date != null && r.Body != r.Body.ToJson())
                {
                    Console.WriteLine("WE ARE HERE");
                    var missingBod = Util.ToJson(missingBody);
                    byte[] response = Encoding.UTF8.GetBytes(missingBod);
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                }
                else if (r.Method == "echo" && r.Date != null && r.Path == null && r.Body != r.Body.ToJson())
                {
                    Console.WriteLine("WE ARE HERE2");
                    var missingBod = Util.ToJson(missingBody);
                    byte[] response = Encoding.UTF8.GetBytes(missingBod);
                    stream.Write(response);
                    Console.WriteLine(missingBod);
                    connection.Close();
                    stream.Flush();
                }
                else if (r.Method == "read" && IsValidPath(r.Path) == false && r.Date != null)
                {
                    Console.WriteLine("WE ARE HERE3");
                    var badReq = Util.ToJson(badRequest);
                    byte[] response = Encoding.UTF8.GetBytes(badReq);
                    stream.Write(response);
                    Console.WriteLine(badReq);
                    connection.Close();
                    stream.Flush();
                } else if (serverMethods.Contains(r.Method) && IsValidPath(r.Path) == false && r.Date == dateTimeUnix && r.Body != null)
                {
                    Console.WriteLine("WE ARE HERE4");
                    var badReq = Util.ToJson(badRequest);
                    byte[] response = Encoding.UTF8.GetBytes(badReq);
                    stream.Write(response);
                    Console.WriteLine(badReq);
                    connection.Close();
                    stream.Flush();
                } else if (serverMethods.Contains(r.Method) && !IsValidPath(r.Path) && dateTimeUnix == r.Date && r.Body == null)
                {
                    Console.WriteLine("WE ARE HERE4");
                    var badReq = Util.ToJson(badRequest);
                    byte[] response = Encoding.UTF8.GetBytes(badReq);
                    stream.Write(response);
                    Console.WriteLine(badReq);
                    connection.Close();
                    stream.Flush();
                }
                else
                {
                    var perfectJson = Util.ToJson(perfect);
                    Console.WriteLine(perfectJson);
                    byte[] response = Encoding.UTF8.GetBytes(perfectJson.ToUpper());
                    stream.Write(response);
                    connection.Close();
                    stream.Flush();
                    //stream.Write(Encoding.UTF8.GetBytes(methodExist));
                    Console.WriteLine("method is: " + r.Method);
                }


                connection.Close();
            }
        }

        public static bool IsValidPath(string path)
        {
            try
            {
                path = path.Replace(@"\\", ":"); // to cancel out c:\\\\test.text
                string temp = Path.GetPathRoot(path); //For cases like: \text.txt
                if (temp.StartsWith(@"\"))
                    return false;
                string pt = Path.GetFullPath(path);
            }
            catch //(Exception NotSupportedException) // catch specific exception here or not if you want
            {
                return false;
            }

            return true;
        }
    }
}