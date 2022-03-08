using ECS.Core.Managers;
using ECS.Model;
using ECS.Model.Restfuls;
using Newtonsoft.Json;
using System;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;

namespace ECS.Core.Restful
{
    public class RestfulRicpRequester : RestfulRequester
    {
        #region Prop
        private Type RequsetType { get; set; }
        #endregion

        #region Ctor
        public RestfulRicpRequester(string domainName, string urlOption, string name) : base(domainName, urlOption)
        {
            this.Name = name;

            if (string.IsNullOrEmpty(this.Name))
                this.Name = "RICP";

            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(this.Name, EcsAppDirectory.RestfulRequesterRicpLog));
            this.Logger.Setting.KeepingDays = EcsServerAppManager.Instance.Logger.Setting.KeepingDays;
        }

        public RestfulRicpRequester(string domainName, string urlOption) : this(domainName, urlOption, null) { }
        #endregion

        #region Method
        public override object PostHttp<T>(object obj)
        {
            this.RequsetType = typeof(T);
            string json = JsonConvert.SerializeObject(obj);
            string responseJson = this.PostHttpJson(json);

            if (string.IsNullOrEmpty(responseJson))
                return null;

            return this.ParseResposeJson(responseJson);
        }

        private object ParseResposeJson(string responseJson)
        {
            object obj = null;

            if (this.RequsetType == typeof(LocationPointPush))
            {
                try
                {
                    obj = JsonConvert.DeserializeObject<RicpLocationPointPushResponse>(responseJson);
                }
                catch (Exception ex)
                {
                    this.Logger?.Write(ex.Message);
                }
            }
            else if (this.RequsetType == typeof(LocationPointStatus))
            {
                try
                {
                    obj = JsonConvert.DeserializeObject<RicpLocationPointStatusResponse>(responseJson);
                }
                catch (Exception ex)
                {
                    this.Logger?.Write(ex.Message);
                }
            }
            else if (this.RequsetType == typeof(RmsStatusSetting))
            {
                try
                {
                    obj = JsonConvert.DeserializeObject<RicpLocationPointStatusResponse>(responseJson);
                }
                catch (Exception ex)
                {
                    this.Logger?.Write(ex.Message);
                }
            }
            else
            {
                try
                {
                    obj = JsonConvert.DeserializeObject<RicpResponse>(responseJson);
                }
                catch (Exception ex)
                {
                    this.Logger?.Write(ex.Message);
                }
            }

            return obj;
        }
        #endregion
    }
}