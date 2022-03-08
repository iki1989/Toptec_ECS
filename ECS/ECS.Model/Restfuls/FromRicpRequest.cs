using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace ECS.Model.Restfuls
{
    #region 피킹 박스 아이디 매핑
    [Serializable]
    public class ContainerMapping : RicpFormat
    {
        [JsonProperty("outOrderCode")]
        public string outOrderCode;

        [JsonProperty("mappingList")]
        public List<Mapping> mappingList = new List<Mapping>();

    }

    [Serializable]
    public class Mapping : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode;

        [JsonProperty("invoiceNo")]
        public string invoiceNo;
    }
    #endregion

    #region 지점 버튼 푸시 콜백
    [Serializable]
    public class LocationPointPushCallback : RicpFormat
    {
        [JsonProperty("pushWorkId")]
        public string pushWorkId;

        [JsonProperty("locationPointCode")]
        public string locationPointCode;
    }
    #endregion

    #region 지점 롤테이너 예정 설정
    [Serializable]
    public class RolltainerScheduleSetting : RicpFormat
    {
        [JsonProperty("locationPointList")]
        public List<RolltainerPoint> locationPointList = new List<RolltainerPoint>();
    }

    [Serializable]
    public class RolltainerPoint : RicpFormat
    {
        [JsonProperty("locationPointCode")]
        public string locationPointCode;

        [JsonProperty("upcomingTypeCode")]
        public string upcomingTypeCode;
    }
    #endregion

    #region 피킹 실적
    [Serializable]
    public class PickingResultsImport : RicpFormat
    {
        public List<Picking.DataClass> data = new List<Picking.DataClass>();
    }
    #endregion

    #region RMS 상태 설정 콜백
    [Serializable]
    public class RmsStatusSettingCallback : RicpFormat
    {
        [JsonProperty("requestId")]
        public string requestId = string.Empty;

        [JsonProperty("systemCode")]
        public string systemCode = string.Empty;

        [JsonProperty("callbackType")]
        public string callbackType = string.Empty;

        [JsonProperty("source")]
        public string source = $"{SourceEnum.INTERFACE}";
    }
    #endregion
}
