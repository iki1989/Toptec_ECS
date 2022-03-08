using ECS.Core.Managers;
using ECS.Model;
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
    public class RestfulWcsRequester : RestfulRequester
    {
        public RestfulWcsRequester(string domainName, string urlOption, string name) : base(domainName, urlOption) 
        {
            this.Name = name;

            if (string.IsNullOrEmpty(this.Name))
                this.Name = "WCS";

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.RestfulRequesterWcsLog));
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }

        public RestfulWcsRequester(string domainName, string urlOption) : this(domainName, urlOption, null) { }


        public override object PostHttp<T>(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            string responseJson = this.PostHttpJson(json);

            if (string.IsNullOrEmpty(responseJson))
                return null;

            return this.ParseResposeJson(responseJson);
        }

        private object ParseResposeJson(string responseJson)
        {
            WcsResponse obj = null;

            try
            {
                obj = JsonConvert.DeserializeObject<WcsResponse>(responseJson);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return obj;
        }
    }
}