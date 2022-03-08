using System;
using System.Text;
using Urcis.SmartCode.Net.Tcp;
using ECS.Model.LabelPrinter;
using ECS.Model;

namespace ECS.Core.Communicators
{
    public class LabelPrinter_ZebraZt411Communicator : TcpCommunicator
    {
        #region Event
        public event EventHandler<HostStatusReturnArg> HostStatusReturnRecived;
        #endregion

        #region Field
        private const string STARTFORMAT = "^XA";
        private const string ENDFORMAT = "^XZ";

        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;
        #endregion

        #region Prop
        public new LabelPrinter_ZebraZT411CommunicatorSetting Setting => base.Setting as LabelPrinter_ZebraZT411CommunicatorSetting;
        #endregion

        #region Ctor
        public LabelPrinter_ZebraZt411Communicator(object owner) : base(owner) { }
        #endregion

        #region Method
        public void ApplySetting(LabelPrinter_ZebraZT411CommunicatorSetting setting)
        {
            setting = setting ?? new LabelPrinter_ZebraZT411CommunicatorSetting();
            
            base.ApplySetting(setting);
        }

        protected override bool OnReceive()
        {
            byte[] bytes = new byte[82];
            int result = 0;
            try
            {
                if (this.Client != null)
                    result = this.Client.Receive(bytes);

                if (result == 0)
                {
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);
                    return false;
                }

                if (result == bytes.Length)
                    this.HostStatusReturnProcess(bytes);

                return true;
            }
            catch (Exception ex)
            {

                if (ex is ObjectDisposedException || ex is NullReferenceException) { }
                else
                    this.StartRestart(RestartReasonEnum.CommunicationFailure);

                return false;
            }
        }

        protected void HostStatusReturnProcess(byte[] bytes)
        {
            if (bytes[0] == STX && bytes[33] == ETX && bytes[34] == CR && bytes[35] == LF
                        && bytes[36] == STX && bytes[69] == ETX && bytes[70] == CR && bytes[71] == LF
                        && bytes[72] == STX && bytes[79] == ETX && bytes[80] == CR && bytes[81] == LF)
            {
                string string1 = Encoding.ASCII.GetString(bytes, 1, 32 - 1);
                string string2 = Encoding.ASCII.GetString(bytes, 37, 68 - 37);
                string string3 = Encoding.ASCII.GetString(bytes, 73, 78 - 73);

                HostStatusReturnArg arg = this.PaseHostStatusReturn(string1, string2, string3);
                if (arg != null)
                    this.HostStatusReturnRecived?.Invoke(this, arg);
            }
        }

        private HostStatusReturnArg PaseHostStatusReturn(string string1, string string2, string string3)
        {
            string[] splited1 = string1.Split(',');
            if (splited1.Length != 12) return null;
            
            string[] splited2 = string2.Split(',');
            if (splited2.Length != 11) return null;

            string[] splited3 = string3.Split(',');
            if (splited3.Length != 2) return null;

            HostStatusReturnArg hsr = new HostStatusReturnArg();
            #region String1

            #region Communication(Interface) Settings
            if (byte.TryParse(splited1[0], out byte aaa) == false)
                return null;

            if ((aaa & (byte)Baud.Baud_19200) == (byte)Baud.Baud_19200)
                hsr.Baud = Baud.Baud_19200;
            else if ((aaa & (byte)Baud.Baud_9600) == (byte)Baud.Baud_9600)
                hsr.Baud = Baud.Baud_9600;
            else if ((aaa & (byte)Baud.Baud_4800) == (byte)Baud.Baud_4800)
                hsr.Baud = Baud.Baud_4800;
            else if ((aaa & (byte)Baud.Baud_2400) == (byte)Baud.Baud_2400)
                hsr.Baud = Baud.Baud_2400;
            else if ((aaa & (byte)Baud.Baud_1200) == (byte)Baud.Baud_1200)
                hsr.Baud = Baud.Baud_1200;
            else if ((aaa & (byte)Baud.Baud_600) == (byte)Baud.Baud_600)
                hsr.Baud = Baud.Baud_600;
            else if ((aaa & (byte)Baud.Baud_300) == (byte)Baud.Baud_300)
                hsr.Baud = Baud.Baud_300;
            else if ((aaa & (byte)Baud.Baud_110) == (byte)Baud.Baud_110)
                hsr.Baud = Baud.Baud_110;

            if ((aaa & (byte)HandShake.DTR) == (byte)HandShake.DTR)
                hsr.HandShake = HandShake.DTR;

            if ((aaa & (byte)Parity.Even) == (byte)Parity.Even)
                hsr.Parity = Parity.Even;

            if ((aaa & (byte)Enable.Enable) == (byte)Enable.Enable)
                hsr.Enable = Enable.Enable;

            if ((aaa & (byte)StopBit.Bits1) == (byte)StopBit.Bits1)
                hsr.StopBit = StopBit.Bits1;

            if ((aaa & (byte)DataBit.Bits8) == (byte)DataBit.Bits8)
                hsr.DataBit = DataBit.Bits8;
            #endregion


            hsr.PaperOutFlag = splited1[1] == "1" ? true : false;
            hsr.PauseFlag = splited1[2] == "1" ? true : false;

            if (int.TryParse(splited1[3], out int labelLength) == false)
                return null;
            hsr.LabelLength = labelLength;

            if (int.TryParse(splited1[4], out int numberOfFormatsInReceiveBuffer) == false)
                return null;
            hsr.NumberOfFormatsInReceiveBuffer = numberOfFormatsInReceiveBuffer;

            hsr.BufferFullFlag = splited1[5] == "1" ? true : false;
            hsr.CommunicationsDiagnosticModeFlag = splited1[6] == "1" ? true : false;
            hsr.PartialFormatFlag = splited1[7] == "1" ? true : false;
            hsr.CorruptRamFalg = splited1[9] == "1" ? true : false;

            if (int.TryParse(splited1[4], out int temperatureRangeOver) == false)
                return null;
            hsr.TemperatureRangeOver = temperatureRangeOver;

            if (int.TryParse(splited1[4], out int temperatureRangeUnder) == false)
                return null;
            hsr.TemperatureRangeUnder = temperatureRangeUnder;
            #endregion

            #region String2
            hsr.FunctionSettings = splited2[0];
            hsr.HeadUpFlag = splited1[2] == "1" ? true : false;
            hsr.RibbonOutFlag = splited1[3] == "1" ? true : false;
            hsr.ThermalTransferModeFlag = splited1[4] == "1" ? true : false;

            if (HostStatusReturnArg.PrintModes.TryGetValue(splited1[5], out PrintModeEnum printmode) == false)
                return null;
            hsr.PrintMode = printmode;

            hsr.PrintWidthMode = splited1[6];
            hsr.LabelWatingFlag = splited1[7] == "1" ? true : false;
            hsr.LabelRemainingInBatch = splited1[8];
            hsr.FormatWhilePrintingFlag = splited1[9] == "1" ? true : false;

            if (int.TryParse(splited1[10], out int numberofGrapgicImagesStoredInMemory) == false)
                return null;
            hsr.NumberofGrapgicImagesStoredInMemory = numberofGrapgicImagesStoredInMemory;
            #endregion

            #region String3
            hsr.Password = splited2[0];
            hsr.StaticRamInstalled = splited2[1] == "1" ? true : false;
            #endregion

            return hsr;
        }

        public bool Print(string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(STARTFORMAT);
            //sb.Append("^SEE:UHANGUL.DAT^FS");
            //sb.Append("^CW0,E:KFONT3.FNT^CI26^FS");
            sb.Append(message);
            sb.Append(ENDFORMAT);

            return this.SendMessage(sb.ToString());
        }

        public bool SendMessage(string message)
        {
            if (this.TcpConnectionState == TcpConnectionStateEnum.Connected)
            {
                byte[] bytes = Encoding.GetEncoding(949).GetBytes(message);

                int result = 0;
                if (this.Client != null && this.Client.Connected)
                    result = this.Client.Send(bytes);

                if (result == bytes.Length)
                    return true;
            }

            return false;
        }
        #endregion
    }

    [Serializable]
    public class LabelPrinter_ZebraZT411CommunicatorSetting : PingTcpCommunicatorSetting
    {
        public LabelPrinter_ZebraZT411CommunicatorSetting()
        {
            this.Name = "Zebra ZT411"; //(Zebra ZE500-4)
            this.Port = 6101;

            this.Active = true;
        }
    }
}
