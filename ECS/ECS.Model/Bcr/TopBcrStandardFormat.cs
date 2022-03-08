using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Bcr
{
    public class TopBcrStandardFormat
    {
        public static byte Header { get; } = 0x02; //STX

        public static byte[] Terminator { get; } = new byte[2] { 0x0D , 0x0A }; //CR, LF

        public static byte[] Seperator { get; } = new byte[2] { 0x0D, 0x0A }; //CR, LF

        public static byte[] MultipleLabelSeperator { get; } = new byte[2] { 0x0D, 0x0A }; //CR, LF
    }
}
