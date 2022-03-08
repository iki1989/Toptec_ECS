using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    [Serializable]
    public class WcsResponse
    {
        [JsonProperty("result_cd")]
        public int result_cd;

        [JsonProperty("result_msg")]
        public string result_msg;

        public void SetSuccess()
        {
            this.result_cd = (int)ErrorCode.Success;
            this.result_msg = ErrorCode.Success.ToString();
        }

        public WcsResponse SetBadRequset()
        {
            this.result_cd = (int)ErrorCode.BadRequset;
            this.result_msg = ErrorCode.BadRequset.ToString();

            return this;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
