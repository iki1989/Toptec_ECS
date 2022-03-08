using ECS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ECS.Core.Util
{
    public class SharpTimer : Timer
    {
        private int runHour = 0;
        private int runMinute = 0;
        private int runSecond = 0;

        public SharpTimer(int runHour, int runMinute, int runSecond) : this(null) { }

        public SharpTimer(Organize organize) 
        {
            organize = organize ?? new Organize();

            this.SetSarpInterval(organize.Hour, organize.Miniute, organize.Second);
        }

        public double SetSarpInterval(int runHour, int runMinute, int runSecond)
        {
            this.runHour = runHour;
            this.runMinute = runMinute;
            this.runSecond = runSecond;

            var now = DateTime.Now;
            DateTime setDate = new DateTime(now.Year, now.Month, now.Day, runHour, runMinute, runSecond);
            var span = now - setDate;
            if (span.TotalSeconds > 0)
            {
                //인터벌 이벤트 발생 시간이 현재시각 이전 일 때
                //ex : 현재        시간 = 10:20:30
                //     이벤트 발생 시간 = 05:06:07
                var tomorrowSpan = setDate.AddDays(1) - now;
                this.Interval = tomorrowSpan.TotalMilliseconds;
            }
            else
            {
                //인터벌 이벤트 발생 시간이 현재시각 이후 일 때
                //ex : 현재        시간 = 10:20:30
                //     이벤트 발생 시간 = 20:00:00
                this.Interval = Math.Abs(span.TotalMilliseconds);
            }

            return this.Interval;
        }

        public void SetSarpInterval() => this.SetSarpInterval(this.runHour, this.runMinute, this.runSecond);
    }
}
