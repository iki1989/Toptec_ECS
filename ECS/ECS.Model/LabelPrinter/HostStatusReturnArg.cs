using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.LabelPrinter
{
    public class HostStatusReturnArg
    {
        #region String1
        #region Communication(Interface) Settings
        public Baud Baud { get; set; }
        public HandShake HandShake { get; set; }
        public Parity Parity { get; set; }
        public Enable Enable { get; set; }
        public StopBit StopBit { get; set; }
        public DataBit DataBit { get; set; }
        #endregion

        public bool PaperOutFlag { get; set; }
        public bool PauseFlag { get; set; }
        public int LabelLength { get; set; }
        public int NumberOfFormatsInReceiveBuffer { get; set; }
        public bool BufferFullFlag { get; set; }
        public bool CommunicationsDiagnosticModeFlag { get; set; }
        public bool PartialFormatFlag { get; set; }
        //public string Unused { get; set; }
        public bool CorruptRamFalg { get; set; }
        public int TemperatureRangeOver { get; set; }
        public int TemperatureRangeUnder { get; set; }
        #endregion

        #region String2
        public string FunctionSettings { get; set; }
        //public string Unused { get; set; }
        public bool HeadUpFlag { get; set; }
        public bool RibbonOutFlag { get; set; }
        public bool ThermalTransferModeFlag { get; set; }
        public PrintModeEnum PrintMode { get; set; }
        public string PrintWidthMode { get; set; }
        public bool LabelWatingFlag { get; set; }
        public string LabelRemainingInBatch { get; set; }
        public bool FormatWhilePrintingFlag { get; set; }
        public int NumberofGrapgicImagesStoredInMemory { get; set; }
        #endregion

        #region String3
        public string Password { get; set; }
        public bool StaticRamInstalled { get; set; }
        #endregion

        public static Dictionary<string, PrintModeEnum> PrintModes { get; } = new Dictionary<string, PrintModeEnum>(){
          {"0", PrintModeEnum.Rewind }
          ,{"1", PrintModeEnum.Peel_Off }
          ,{"2", PrintModeEnum.Tear_Off }
          ,{"3", PrintModeEnum.Cutter }
          ,{"4", PrintModeEnum.Applicator }
          ,{"5", PrintModeEnum.DelayedCut }
          ,{"6", PrintModeEnum.LinerlessPeel }
          ,{"7", PrintModeEnum.LinerlessRewind }
          ,{"8", PrintModeEnum.PartialCutter }
          ,{"9", PrintModeEnum.RFID }
          ,{"K", PrintModeEnum.Kiosk }
          ,{"S", PrintModeEnum.Stream }};
    }

    [Flags]
    public enum HandShake : byte
    {
        XonXoff = 0,
        DTR = 0b_1000_0000,
    }

    [Flags]
    public enum Parity : byte
    {
        Odd = 0,
        Even = 0b_010_0000,
    }

    [Flags]
    public enum Enable : byte
    {
        Disable = 0,
        Enable = 0b_0010_0000,
    }

    [Flags]
    public enum StopBit : byte
    {
        Bits2 = 0,
        Bits1 = 0b_0001_0000,
    }

    [Flags]
    public enum DataBit : byte
    {
        Bits7 = 0,
        Bits8 = 0b_0000_1000,
    }

    [Flags]
    public enum Baud : byte
    {
        Baud_110 = 0b_0000_0000,
        Baud_300 = 0b_0000_0001,
        Baud_600 = 0b_0000_0010,
        Baud_1200 = 0b_0000_0011,
        Baud_2400 = 0b_0000_0100,
        Baud_4800 = 0b_0000_0101,
        Baud_9600 = 0b_0000_0110,
        Baud_19200 = 0b_0000_0111,
    }

    public enum PrintModeEnum
    {
        Rewind,
        Peel_Off,
        Tear_Off,
        Cutter,
        Applicator,
        DelayedCut,
        LinerlessPeel,
        LinerlessRewind,
        PartialCutter,
        RFID,
        Kiosk,
        Stream,
    }
}
