using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public class TimeBox
    {
        public string BoxId { get; set; } = string.Empty;
        public DateTime BcrReadTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.BoxId)} : ").Append($"{this.BoxId}, ");
            sb.Append($"{nameof(this.BcrReadTime)} : ").Append($"{this.BcrReadTime}");

            return sb.ToString();
        }
    }
}
