using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Inkject
{
    public class CounterInfo
    {
        public string Start { get; set; }

        public string Stop { get; set; }

        public string Current { get; set; }

        public DirectionEnum Direction { get; set; }

        public TypeEnum Type { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Start)}={this.Start},");
            sb.Append($"{nameof(this.Stop)}={this.Stop},");
            sb.Append($"{nameof(this.Current)}={this.Current},");
            sb.Append($"{nameof(this.Direction)}={this.Direction},");
            sb.Append($"{nameof(this.Type)}={this.Type}");

            return sb.ToString();
        }
    }
}
