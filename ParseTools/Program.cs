using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;


namespace ParseTools
{
    class Program
    {
        static void Main(string[] args)
        {

            WebHeaderCollection Header = new WebHeaderCollection();
            Header.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:56.0) Gecko/20100101 Firefox/56.0");
            Header.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            Header.Add(HttpRequestHeader.AcceptLanguage, "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
            Header.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br");
            Header.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.ru");
            request.Method = "GET";
            request.Headers = new WebHeaderCollection();
            request.CookieContainer = new CookieContainer();

            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            Stream dataStream = responce.GetResponseStream();
            string HtmlResponse;
            WebHeaderCollection rHeaders = responce.Headers;
            using (StreamReader reader = new StreamReader(dataStream))
            {
                HtmlResponse = reader.ReadToEnd();
            }
            CookieCollection cookie = responce.Cookies;
            responce.Close();

           
            HtmlWeb htmlWeb = new HtmlWeb();


            HttpConnection requ =  HttpConnection.Create();
            requ.Responce.Headers

        }

    }
    public class HttpConnection
    {
        public class HttpTransfer
        {
            protected WebHeaderCollection _headers;
            public WebHeaderCollection Headers
            {
                get{ return _headers; }
                //set { _headers = value; }
            }
        }
        public class HttpRequest:HttpTransfer
        {
            public WebHeaderCollection Headers
            {
                set { _headers = value;  }
            }
        }
        public class HttpResponce:HttpTransfer
        {

        }

        HttpRequest _request;
        HttpResponce _responce;
        string _method;
        string _url;
        CookieCollection _cookie;
        WebHeaderCollection _headers;

        CookieCollection _wcookie;
        WebHeaderCollection _responceHeaders;


        public static HttpConnection  Create()
        {
            HttpConnection result = new HttpConnection();
            result.Request = new HttpRequest();
            result.Responce = new HttpResponce();
            //_cookie = null;
            //result.Headers = new WebHeaderCollection();
            //result.CookieContainer = new CookieContainer();
            return result;
        }
        public HttpRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }
        public HttpResponce Responce
        {
            get { return _responce; }
            set { _responce = value; }
        }
        public CookieCollection Cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }
        public WebHeaderCollection Headers
        {
            get { return _headers; }
            set { _headers = value; }
        }
        public string  Method
        {
            get { return _method; }
            set { _method = value; }
        }
        public HtmlDocument GET(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            request.Method = "GET";
            request.Headers = new WebHeaderCollection();
            request.CookieContainer = new CookieContainer();
            //Set cookie
            foreach (Cookie c in Cookie)
            {
                request.CookieContainer.Add(c);
            }

            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            Stream dataStream = responce.GetResponseStream();
            string HtmlResponse;
            WebHeaderCollection rHeaders = responce.Headers;
            using (StreamReader reader = new StreamReader(dataStream))
            {
                HtmlResponse = reader.ReadToEnd();
            }
            CookieCollection cookie = responce.Cookies;
            responce.Close();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(HtmlResponse);
        }

        HttpConnection()
        {

        }

    }
    public class UserAgent
    {

        public UserAgent()
        {

        }
    }

}
