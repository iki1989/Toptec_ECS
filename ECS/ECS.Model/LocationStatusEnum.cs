using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public enum LocationStatusEnum
    {
        PickingStation1_6_90 = 0,
        PickingStation7_13 = 1,

        RollTainer1_4 = 2,
        RollTainer5_6 = 3,
    }

    public class LocationStatus
    {
        public string IoName { get; set; }

        public string ShellCode { get; set; }

        public DateTime UpdateTime { get; set; }

        public string WorkId { get; set; }

        public PushWorkStatusCdEnum StatusCd { get; set; }

        public RollTainterTowerLampEnum RollTainterTowerLamp { get; set; }

        public bool FinishStatusPlcWrote { get; set; }


        public LocationStatus(DataRow row)
        {
            if (row == null) return;

            this.ShellCode = row["SHELLCODE"].ToString().Trim();
            this.IoName = row["IONAME"].ToString().Trim();
            this.UpdateTime = DateTime.Parse(row["UPDATETIME"].ToString().Trim());

            if (!row.IsNull("PUSHWORKID"))
                this.WorkId = row["PUSHWORKID"].ToString().Trim();

            if (Enum.TryParse(row["PUSHWORKSTATUSCD"].ToString().Trim(), out PushWorkStatusCdEnum statusCd))
                this.StatusCd = statusCd;

            switch (this.StatusCd)
            {
                case PushWorkStatusCdEnum.READY:
                    this.RollTainterTowerLamp = RollTainterTowerLampEnum.A;
                    break;
                case PushWorkStatusCdEnum.ING:
                    this.RollTainterTowerLamp = RollTainterTowerLampEnum.D;
                    break;
                case PushWorkStatusCdEnum.FINISH:
                    this.RollTainterTowerLamp = RollTainterTowerLampEnum.N;
                    break;
            }
        }

        public LocationStatus() { }

        public LocationStatus Clone()
        {
            LocationStatus clone = new LocationStatus();

            clone.IoName = this.IoName;
            clone.ShellCode = this.ShellCode;
            clone.UpdateTime = this.UpdateTime;
            clone.WorkId = this.WorkId;
            clone.StatusCd = this.StatusCd;

            return clone;
        }
    }
}
