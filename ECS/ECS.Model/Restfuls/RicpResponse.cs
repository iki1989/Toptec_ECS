using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ECS.Model.Restfuls
{
    [Serializable]
    public class RicpResponse
    {
        [JsonProperty("success")]
        public bool success;

        [JsonProperty("errorCode")]
        public string errorCode;

        [JsonProperty("errorMsg")]
        public string errorMsg;

        public void SetSuccess()
        {
            this.success = true;
            this.errorCode = string.Empty;
            this.errorMsg = string.Empty;
        }

        public RicpResponse SetBadRequset()
        {
            this.success = false;
            this.errorCode = ErrorCode.BadRequset.ToString();
            this.errorMsg = string.Empty;

            return this;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class RicpLocationPointPushResponse : RicpResponse
    {
        public class result
        {
            [JsonProperty("pushWorkId")]
            public string pushWorkId;

            [JsonProperty("locationPointCode")]
            public string locationPointCode;
        }

        public new RicpLocationPointPushResponse SetBadRequset()
        {
            this.success = true;
            this.errorCode = ErrorCode.BadRequset.ToString();
            this.errorMsg = string.Empty;

            return this;
        }

        [JsonProperty("resultData")]
        public result resultData;
    }

    [Serializable]
    public class RicpLocationPointStatusResponse : RicpResponse
    {
        public class result
        {
            [JsonProperty("pushWorkId")]
            public string pushWorkId;

            [JsonProperty("locationPointCode")]
            public string locationPointCode = string.Empty;

            [JsonProperty("pushWorkStatusCd")]
            public PushWorkStatusCdEnum pushWorkStatusCd;
        }

        public new RicpLocationPointStatusResponse SetBadRequset()
        {
            this.success = true;
            this.errorCode = ErrorCode.BadRequset.ToString();
            this.errorMsg = string.Empty;

            return this;
        }

        [JsonProperty("resultData")]
        public result resultData;
    }

    [Serializable]
    public class RicpRmsStatusSettingResponse : RicpResponse
    {
        [JsonProperty("requestId")]
        public string requestId = string.Empty;

        public new RicpRmsStatusSettingResponse SetBadRequset()
        {
            this.success = true;
            this.errorCode = ErrorCode.BadRequset.ToString();
            this.errorMsg = string.Empty;

            return this;
        }
    }

    public enum PushWorkStatusCdEnum : short
    {
        READY = 0, 
        ING = 1, 
        FINISH = 2,
    }

    public enum RollTainterTowerLampEnum : short
    {
        A = 1,      // 도착예정
        D = 3,      // 출발예정
        N = 5,      // 예정없음
    }
}
