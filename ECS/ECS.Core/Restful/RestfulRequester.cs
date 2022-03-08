using ECS.Model.Restfuls;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;

namespace ECS.Core.Restful
{
    public class RestfulRequester : IHaveLogger, INameable
    {
        #region Define
        private enum HttpVerb
        {
            GET,
            POST,
            //PUT,
            //DELETE
        }
        #endregion

        #region Prop
        private string m_DomainName;
        public string DomainName
        {
            get => this.m_DomainName;
            private set
            {
                this.m_DomainName = value;

                if (this.m_DomainName.EndsWith("/") == false)
                    this.m_DomainName += "/";
            }
        }

        public string UrlOption { get; set; }

        public Logger Logger { get; protected set; }

        public string Name { get; protected set; }
        #endregion

        #region Ctor
        public RestfulRequester(string domainName, string urlOption, string logName)
        {
            this.DomainName = domainName;
            this.UrlOption = urlOption;
            this.Name = logName;
        }

        public RestfulRequester(string domainName, string urlOption) : this(domainName, urlOption, null) { }

        public RestfulRequester(string domainName) : this(domainName, null) { }
        #endregion

        #region Method

        private HttpWebRequest CreateHttpWebRequest(HttpVerb httpVerb)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{this.DomainName}{this.UrlOption}");

            switch (httpVerb)
            {
                case HttpVerb.GET: request.Method = WebRequestMethods.Http.Get; break;
                case HttpVerb.POST: request.Method = WebRequestMethods.Http.Post; break;
            }
           
            request.Timeout = 10 * 1000; //10s
            request.MaximumAutomaticRedirections = 4; //default : 50
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json";

            //if (this.NetworkCredential != null)
            //    request.Credentials = this.NetworkCredential;

            return request;
        }

        private string GetResponseJson(HttpWebRequest request)
        {
            string json = string.Empty;

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response?.StatusCode == HttpStatusCode.OK || response?.StatusCode == HttpStatusCode.Created)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    json = reader.ReadToEnd();
                                    this.Logger?.Write($"[Recived]HttpWebResponse : {json}");
                                }
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"StatusCode - {response.StatusCode}");
                        sb.Append($"StatusDescription - {response.StatusDescription}");

                        this.Logger?.Write($"[Recived]HttpWebResponse : {sb}");
                    }
                }
            }
            catch (WebException webExcp)
            {
                //통신 실패시
                this.Logger?.Write($"Status = {webExcp.Status}, Message{webExcp.Message}");

                if (webExcp.Response != null)
                {
                    using (var stream = webExcp.Response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                if (reader != null)
                                    this.Logger?.Write(reader.ReadToEnd());
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return json;
        }

        private string GetHttpJson()
        {
            if (string.IsNullOrEmpty(this.DomainName)) return string.Empty;

            HttpWebRequest request = this.CreateHttpWebRequest(HttpVerb.GET);
            
            this.Logger?.Write($"[Sent]HttpWebRequest[{HttpVerb.GET}] : {this.DomainName}{this.UrlOption}");
            return this.GetResponseJson(request);
        }

        protected string PostHttpJson(string json)
        {
            if (string.IsNullOrEmpty(this.DomainName)) return string.Empty;

            HttpWebRequest request = this.CreateHttpWebRequest(HttpVerb.POST);

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytes.Length;

            try
            {
                this.SendWriteLog(HttpVerb.POST, json);
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream?.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
                return null;
            }

            return this.GetResponseJson(request);
        }

        private void SendWriteLog(HttpVerb httperb, string json)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[Send]HttpWebRequest[{httperb}] : {this.DomainName}{this.UrlOption}");
            sb.Append($"{json}");

            this.Logger?.Write(sb.ToString());
        }
        public Task<object> PostHttpAsync<T>(object obj) => Task.Run(() => this.PostHttp<T>(obj));
        public virtual object PostHttp<T>(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            string responseJson = this.PostHttpJson(json);

            if (string.IsNullOrEmpty(responseJson))
                return null;

            WcsResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<WcsResponse>(responseJson);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return response;
        }

        public Task<object> GetHttpAsync<T>() => Task.Run(() => this.GetHttp<T>());
        public object GetHttp<T>()
        {
            string json = this.GetHttpJson();

            WcsResponse response = null;
            try
            {
                response = JsonConvert.DeserializeObject<WcsResponse>(json);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }
            return response;
        }

        #endregion
    }
}