using ECS.Model.Plc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace ECS.Model.Restfuls
{
    #region 물동량송신
    [Serializable]
    public class SpiralBuffer2f : RicpFormat
    {
        [JsonProperty("eqp_id")]
        [StringLength(40)]
        [Description("설비 ID")]
        public readonly string eqp_id = "GP_ECS_022";

        [JsonProperty("work_Id")]
        [StringLength(40)]
        [Description("작업 ID")]
        public string work_Id;

        [JsonProperty("setBoxCount")]
        [Description("설비 설정 수량")]
        public double setBoxCount;

        [JsonProperty("currentBoxCount")]
        [Description("박스 수량")]
        public double currentBoxCount;

        [JsonProperty("transportQuantity")]
        [Description("물동량")]
        public double transportQuantity;

        [JsonProperty("QueuingTime")]
        [Description("대기시간")]
        public TimeSpan QueuingTime = TimeSpan.MinValue;

        //Send할 필드가 아님
        [JsonIgnore]
        public DateTime QueuinguUpdateTime = DateTime.Now;

        [JsonProperty("eqp_Status")]
        [Description("가동 상태")]
        public EquipmentStateEnum eqp_Status = EquipmentStateEnum.Run;
    }
    #endregion
}
