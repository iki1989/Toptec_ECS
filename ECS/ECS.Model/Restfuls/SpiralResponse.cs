using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    [Serializable]
    public class SpiralResponse
    {
        [JsonProperty("success")]
        public bool success;

        [JsonProperty("errorCode")]
        public string errorCode;

        [JsonProperty("errorMsg")]
        public string errorMsg;

        [JsonProperty("work_Id")]
        public string work_Id = DateTime.Now.ToString("yyyyMMdd-hhmmssfff");

        public void SetSuccess()
        {
            this.success = true;
            this.errorCode = string.Empty;
            this.errorMsg = string.Empty;
            this.work_Id = DateTime.Now.ToString("yyyyMMdd-hhmmssfff");
        }

        public SpiralResponse SetBadRequset()
        {
            this.success = true;
            this.errorCode = ErrorCode.BadRequset.ToString();
            this.errorMsg = string.Empty;
            this.work_Id = DateTime.Now.ToString("yyyyMMdd-hhmmssfff");

            return this;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
