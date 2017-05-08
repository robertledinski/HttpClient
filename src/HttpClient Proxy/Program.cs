using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient_Proxy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Proxy proxy;
            HttpClientHandler handler;
            string proxyUrl = "";

            proxy = new Proxy(proxyUrl);
            handler = new HttpClientHandler {
                Proxy = proxy,
                UseProxy = true
            };

            HttpClient httpClient = new HttpClient(handler);

            Uri requestUri = new Uri("", UriKind.Absolute);

            Task<HttpResponseMessage> httpMsg = null;

            httpMsg = httpClient.PostAsync(requestUri, new StringContent("", Encoding.UTF8, "application/x-www-form-urlencoded"));
            string result = null;

            httpMsg.Wait();
            HttpResponseMessage msgResult = httpMsg.Result;

            Task<string> readResultTask = msgResult.Content.ReadAsStringAsync();
            readResultTask.Wait();

            result = readResultTask.Result;


        }
    }

    class Proxy : System.Net.IWebProxy
    {
        private string _proxy;
        public System.Net.ICredentials Credentials { get; set; }

        public Proxy(string proxy)
        {
            _proxy = proxy;
        }

        public Uri GetProxy(Uri destination)
        {
            return new Uri(_proxy);
        }

        public bool IsBypassed(Uri host)
        {
            return true;
        }
    }
}
