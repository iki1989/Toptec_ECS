using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain.Touch
{
    public struct BoxInfoData : IReaderConvertable
    {
        public static BoxInfoData None = new BoxInfoData() { BoxTypeCd = "", Name = "", FirstNormalTo = 8999999, ManualFrom = 9000000, ManualTo = 9999999 };
        public int No { get; set; }
        public string BoxTypeCd { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int HeightSensor { get; set; }
        public int FirstNormalFrom { get; set; }
        public int FirstNormalCurrent { get; set; }
        public int FirstNormalTo { get; set; }
        public int SecondNormalFrom { get; set; }
        public int SecondNormalCurrent { get; set; }
        public int SecondNormalTo { get; set; }
        public int ManualFrom { get; set; }
        public int ManualCurrent { get; set; }
        public int ManualTo { get; set; }
        public void Convert(SqlDataReader reader)
        {
            this.No = 0;
            this.BoxTypeCd = reader["BOX_TYPE_CD"].ToString();
            this.Name = reader["NAME"].ToString();
            this.Length = double.Parse(reader["LENGTH"].ToString());
            this.Weight = double.Parse(reader["WEIGHT"].ToString());
            this.Width = double.Parse(reader["WIDTH"].ToString());
            this.Height = double.Parse(reader["HEIGHT"].ToString());
            this.HeightSensor = int.Parse(reader["H_SENSOR"].ToString());
            this.FirstNormalFrom = int.Parse(reader["FIRST_NORMAL_FROM"].ToString());
            this.FirstNormalCurrent = int.Parse(reader["FIRST_NORMAL_CURRENT"].ToString());
            this.FirstNormalTo = int.Parse(reader["FIRST_NORMAL_TO"].ToString());
            this.SecondNormalFrom = int.Parse(reader["SECOND_NORMAL_FROM"].ToString());
            this.SecondNormalCurrent = int.Parse(reader["SECOND_NORMAL_CURRENT"].ToString());
            this.SecondNormalTo = int.Parse(reader["SECOND_NORMAL_TO"].ToString());
            this.ManualFrom = int.Parse(reader["MANUAL_FROM"].ToString());
            this.ManualCurrent = int.Parse(reader["MANUAL_CURRENT"].ToString());
            this.ManualTo = int.Parse(reader["MANUAL_TO"].ToString());
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
