using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model
{
    public class Stuck
    {
        private bool m_IsStuck;
        public bool IsStuck
        {
            get => this.m_IsStuck;
            set
            {
                if (this.m_IsStuck == value)
                    return;

                this.m_IsStuck = value;

                if (this.m_IsStuck)
                    this.StuckedTime = DateTime.Now;
            }
        }

        public DateTime StuckedTime { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.IsStuck)} : ").Append($"{this.IsStuck},");
            sb.Append($"{nameof(this.StuckedTime)} : ").Append($"{this.StuckedTime}");

            return sb.ToString();
        }

    }
}
