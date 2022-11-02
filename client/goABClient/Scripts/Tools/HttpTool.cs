using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace goABClient.Scripts.Tools
{
    public class HttpTool
    {
        public static string PostHttp(string url,object req)
        {
            WebRequest request = WebRequest.Create(url);
            request.Timeout = 3000;
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(JsonConvert.SerializeObject(req));
                sw.Flush();
                sw.Close();
            }

            WebResponse response = request.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
               return sr.ReadToEnd();
            }
        }



    }
}
