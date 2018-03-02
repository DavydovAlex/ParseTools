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
            Header.Add(HttpRequestHeader.Referer, "https://www.google.ru");





            Header.Set(HttpRequestHeader.Referer, "https://help.bars-open.ru/");

            HttpConnection requ =  HttpConnection.Create();            
            requ.Request.Headers = Header;
            HtmlDocument doc = requ.GET("https://help.bars-open.ru/");
            string scemail = "davam@nso.ru";
            string scpassword = "agt4ir696tyu";
            string csrfhash = doc.DocumentNode.SelectSingleNode(@"//input[@name='_csrfhash']").Attributes["value"].Value;
            string redirectAction = @"/Base/User/Login";
            string postData = "_redirectAction=" + redirectAction + "&_csrfhash=" + csrfhash + "&scemail=" + scemail + "&scpassword=" + scpassword;

            HtmlDocument post = requ.POST("https://help.bars-open.ru/index.php?/Base/User/Login", postData);
            
            HtmlDocument Kbase = requ.GET("https://help.bars-open.ru/index.php?/Knowledgebase/List");
        }

    }
    public class HttpConnection
    {

        public class HttpTransfer
        {
             WebHeaderCollection _headers;
             CookieCollection _cookies;
            public HttpTransfer()
            {
                _headers = new WebHeaderCollection();
                _cookies = new CookieCollection();
                
            }
            public WebHeaderCollection Headers
            {
                get { return _headers; }
                set { _headers = value; }
            }
            public CookieCollection Cookies
            {
                get { return _cookies; }
                set { _cookies = value; }
            }
        }
        public class HttpRequest:HttpTransfer
        {
            string _method;
            //string _referer;

            public string Method
            {
                get { return _method; }
                set { _method = value; }
            }
            public string Referer
            {
                get { return Headers.Get("Referer"); }
                set { Headers.Set("Referer", value); }
            }


        }
        public class HttpResponce:HttpTransfer
        {

      
        }

        HttpRequest _request;
        HttpResponce _responce;

        public static HttpConnection  Create()
        {
            HttpConnection result = new HttpConnection();
            result.Request = new HttpRequest();
            result.Responce = new HttpResponce();           
            return result;
        }
        public HttpRequest Request
        {
            get { return _request; }
            private set { _request = value; }
        }
        public HttpResponce Responce
        {
            get { return _responce; }
            private set { _responce = value; }
        }


        private void AddHeaders( HttpWebRequest request)
        {
            request.Headers = new WebHeaderCollection();
            foreach (string header in Request.Headers)
            {
                var value = Request.Headers[header];
                switch (header)
                {
                    case "User-Agent":
                        request.UserAgent = value;
                        break;
                    case "Accept":
                        request.Accept = value;
                        break;
                    case "Content-Type":
                        request.ContentType = value;
                        break;
                    case "Referer":
                        request.Referer = value;
                        break;
                    default:
                        request.Headers.Add(header, value);
                        break;
                }
            }
        }
        private void AddCookies(HttpWebRequest request)
        {
            request.CookieContainer = new CookieContainer();
            foreach (Cookie c in Request.Cookies)
            {
                request.CookieContainer.Add(c);
            }
        }
        public HtmlDocument GET(string Url)
        {
            return RequestGetPost(Url, "GET");
        }
        public HtmlDocument POST(string Url, string body)
        {
            return RequestGetPost(Url, "POST",body);
        }

        HtmlDocument RequestGetPost(string Url, string Method,string body="")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = Method;
            AddHeaders(request);
            AddCookies(request);

            switch(Method)
            {
                case "GET":

                    break;
                case "POST":
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytePostData = encoding.GetBytes(body);
                    request.ContentLength = bytePostData.Length;
                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(bytePostData, 0, bytePostData.Length);
                    }                        
                  
                    break;
            }
            HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
            Stream dataStream = responce.GetResponseStream();
            string HtmlResponse;
            WebHeaderCollection rHeaders = responce.Headers;
            using (StreamReader reader = new StreamReader(dataStream))
            {
                HtmlResponse = reader.ReadToEnd();
            }
            this.Responce.Cookies = responce.Cookies;
            this.Responce.Headers = responce.Headers;

            this.Request.Cookies = Request.Cookies.Count==0?this.Responce.Cookies: this.Request.Cookies;
            this.Request.Referer = responce.ResponseUri.AbsoluteUri;
            responce.Close();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(HtmlResponse);
            return htmlDocument;


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
