using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Restfuls;
using Newtonsoft.Json;
using System;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;

namespace ECS.Core.Restful
{
    public class RestfulSpiralRequester : RestfulRequester
    {
        public RestfulSpiralRequester(string domainName, string urlOption, string name) : base(domainName, urlOption)
        {
            this.Name = name;

            if (string.IsNullOrEmpty(this.Name))
                this.Name = "Spiral";

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.RestfulRequesterSpiralLog));
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }

        public RestfulSpiralRequester(string domainName, string urlOption) : this(domainName, urlOption, null) { }

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
            SpiralResponse obj = null;

            try
            {
                obj = JsonConvert.DeserializeObject<SpiralResponse>(responseJson);
            }
            catch (Exception ex)
            {
                this.Logger?.Write(ex.Message);
            }

            return obj;
        }
    }
}