using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ECS.Model.Domain.Ricp
{
    #region Ricp Common Type

    [Serializable]
    public class RicpRequest
    {
        [JsonProperty("JobNumber")]
        public string JobNumber { get; set; }

        [JsonProperty("unique id")]
        public string unique_id { get; set; } //yyyymmdd-hhmiss.000

        [JsonProperty("Item")]
        public string Item { get; set; }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
    [Serializable]
    public class RicpRequsetBoxDispatch : RicpRequest
    {
        [JsonProperty("BoxID")]
        public string BoxID { get; set; }
    }
    [Serializable]
    public class RicpResponse
    {
        [JsonProperty("jobNumber")]
        public int jobNumber;

        [JsonProperty("unique id")]
        public string unique_id; //yyyymmdd-hhmiss.000

        [JsonProperty("success")]
        public bool success;

        [JsonProperty("errorCode")]
        public HttpStatusCode errorCode = HttpStatusCode.BadRequest;

        [JsonProperty("code")]
        public int code;

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
    #endregion
}
