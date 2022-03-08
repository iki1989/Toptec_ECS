using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.DynamicScales
{
    public class TLW150DataFormat
    {
        /// <summary>
        /// Dynamic weight
        /// Right aligned, 10 bytes, filled with spaces for insufficient bits
        /// </summary>
        public string IW0104 { get; set; }

        /// <summary>
        /// Blank
        /// The corresponding hex code is 0x20, filled with spaces for insufficient bits
        /// </summary>
        public static char Space { get; } = ' ';

        /// <summary>
        /// Weight unit
        /// Left aligned, 3 characters
        /// </summary>
        public string WT0103 { get; set; }

        /// <summary>
        /// CRLF line break
        /// The corresponding hex codes are 0xOD and 0x0A
        /// </summary>
        public static byte[] CRLF { get; } = new byte[2] { 0x0D, 0X0A};
    }
}
