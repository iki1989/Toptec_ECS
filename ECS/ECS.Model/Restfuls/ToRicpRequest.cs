using ECS.Model.Plc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ECS.Model.Restfuls
{
    #region 설비가동상태
    [Serializable]
    public class DeviceStatus : RicpFormat
    {
        [JsonProperty("deviceList")]
        public List<DeviceInfo> deviceList { get; set; } = new List<DeviceInfo>();
    }

    [Serializable]
    public class DeviceInfo : RicpFormat
    {
        [JsonProperty("deviceId")]
        public string deviceId = string.Empty;

        [JsonProperty("deviceName")]
        public string deviceName = string.Empty;

        [JsonProperty("deviceTypeId")]
        public string deviceTypeId = string.Empty;

        [JsonProperty("deviceTypeName")]
        public string deviceTypeName = string.Empty;

        [JsonProperty("deviceStatusCd")]
        public int deviceStatusCd;

        [JsonProperty("deviceErrorMsg")]
        public string deviceErrorMsg = string.Empty;
    }
    #endregion

    #region 제함 박스 아이디 스캔
    [Serializable]
    public class ContainerScan : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;

        [JsonProperty("containerTypeCd")]
        public string containerTypeCd = string.Empty;

    }
    #endregion

    #region 중량검사기 앞 박스 아이디 스캔
    [Serializable]
    public class WeightInvoiceContainerScan : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;
    }
    #endregion

    #region 중량검수 결과 전송
    [Serializable]
    public class WeightResultContaierScan : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;

        [JsonProperty("containerTypeCd")]
        public string containerTypeCd = string.Empty;

        [JsonProperty("checkResult")]
        public bool checkResult = false;

        [JsonProperty("checkValue")]
        public string checkValue = string.Empty;
    }
    #endregion

    #region 송장번호 스캔
    [Serializable]
    public class InvoiceScan : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;

        [JsonProperty("invoiceNo")]
        public string invoiceNo = string.Empty;

        [JsonProperty("scanResult")]
        public bool scanResult = false;
    }
    #endregion

    #region 출고 송장 스캔
    [Serializable]
    public class OutInvoiceScan : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;

        [JsonProperty("invoiceNo")]
        public string invoiceNo = string.Empty;
    }
    #endregion

    #region 지점 버튼 푸시
    [Serializable]
    public class LocationPointPush : RicpFormat
    {
        [JsonProperty("locationPointCode")]
        public string locationPointCode = string.Empty;
    }
    #endregion

    #region 지점 버튼 상태
    [Serializable]
    public class LocationPointStatus : RicpFormat
    {
        [JsonProperty("locationPointCode")]
        public string locationPointCode = string.Empty;

        [JsonProperty("pushWorkId")]
        public string pushWorkId = string.Empty;
    }
    #endregion

    #region 지점 롤테이너 센서 상태
    [Serializable]
    public class RolltainerSensorStatus : RicpFormat
    {
        [JsonProperty("locationPointCode")]
        public string locationPointCode = string.Empty;

        [JsonProperty("sensorStatus")]
        public string sensorStatus = string.Empty;
    }
    #endregion

    #region RMS 상태 설정
    [Serializable]
    public class RmsStatusSetting : RicpFormat
    {
        [JsonProperty("systemCode")]
        public string systemCode = string.Empty;

        [JsonProperty("instruction")]
        public string instruction = string.Empty;
    }
    #endregion

    #region 친환경충진 결과전송
    [Serializable]
    public class PackageResult : RicpFormat
    {
        [JsonProperty("containerCode")]
        public string containerCode = string.Empty;

        [JsonProperty("containerTypeCd")]
        public string containerTypeCd = string.Empty;

        [JsonProperty("containerInputTime")]
        public DateTime containerInputTime = DateTime.Now;

        [JsonProperty("resultCode")]
        public string resultCode = string.Empty;

        [JsonProperty("resultInVolume")]
        public string resultInVolume = string.Empty;

        [JsonProperty("resultHeight")]
        public string resultHeight = string.Empty;

        [JsonProperty("resultManualWork")]
        public bool resultManualWork = false;

        [JsonProperty("resultByPass")]
        public bool resultByPass = false;

        [JsonProperty("resultBufferCount")]
        public double resultBufferCount = 0;

        [JsonProperty("resultOutputTime")]
        public DateTime resultOutputTime = DateTime.Now;

        [JsonProperty("resultImagePath")]
        public string resultImagePath = string.Empty;
    }
    #endregion
}
