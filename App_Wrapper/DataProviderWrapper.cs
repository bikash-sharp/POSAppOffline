using App_BAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App_Wrapper
{
    public class DataProviderWrapper
    {
        private static readonly DataProviderWrapper instance = new DataProviderWrapper();
        static DataProviderWrapper() { }
        private DataProviderWrapper() { }

        public static string DataBaseName = "";
        public static int InstanceNo = 0;

        public static DataProviderWrapper Instance
        {
            get
            {
                return instance;
            }
        }

        public string GetData(string Url, Verbs verb, string DataWrite = "")
        {
            string BaseUrl = Url;
            string Token = string.Empty;
            Url = BaseUrl + (verb == Verbs.GET ? DataWrite : "");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = verb.ToString();
            request.ContentType = "application/json";
            //Token = ConfigurationManager.AppSettings["Token"];

            var bytes = Encoding.UTF8.GetBytes(Token);
            string Base64Token = Convert.ToBase64String(bytes);

            //request.Headers.Add("Authorization", "Custom " + Base64Token);

            if (verb != Verbs.GET)
            {
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DataWrite);
                }
            }
            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();

                            return response;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
                return e.Message;
            }
            return "Success";
        }

        public string PostData(string url, string postData)
        {
            string responseXml = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var multipartFormDataContent = new MultipartFormDataContent())
                    {
                        var values = new[]{ new KeyValuePair<string, string>("products", postData)};                        
                        foreach (var keyValuePair in values)
                        {
                            multipartFormDataContent.Add(new StringContent(keyValuePair.Value),String.Format("\"{0}\"", keyValuePair.Key));
                        }
                        var  postProduct = client.PostAsync(url, multipartFormDataContent).Result.Content.ReadAsStringAsync();
                        responseXml = postProduct.Result;                       
                    }
                }
                return responseXml;
            }
            catch (Exception webEx)
            {
                var err = webEx.Message;
            }
            return responseXml;
        }
    }
}
