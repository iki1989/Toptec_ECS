using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Util
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        #region Import
        [DllImport("kernel32.dll")]
        private static extern void GetLocalTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME lpSystemTime);
        #endregion

        #region Field
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
        #endregion

        public static bool SetLocalTime(DateTime value)
        {
            SYSTEMTIME st = new SYSTEMTIME();

            GetLocalTime(ref st);

            st.wYear = (ushort)value.Year;
            st.wMonth = (ushort)value.Month;
            st.wDayOfWeek = (ushort)value.DayOfWeek;
            st.wDay = (ushort)value.Day;
            st.wHour = (ushort)value.Hour;
            st.wMinute = (ushort)value.Minute;
            st.wSecond = (ushort)value.Second;
            st.wMilliseconds = (ushort)value.Millisecond;

            return SetLocalTime(ref st);
        }
    }
}
