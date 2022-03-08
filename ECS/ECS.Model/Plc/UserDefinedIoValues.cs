using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Urcis.SmartCode.Helpers;
using Urcis.SmartCode.Io;

namespace ECS.Model.Plc
{
    #region PLC
    #region ActUtlInputResultObject
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ActUtlInputResultObject : IUserDefinedIoValue
    {
        #region Field
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private char[] m_BoxID;
        public string BoxID
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxID)); }
            set { this.m_BoxID = StringHelper.ToFixedLengthCharArray(value, 20); }
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        private char[] m_BoxType;
        public string BoxType
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxType)); }
            set { this.m_BoxType = StringHelper.ToFixedLengthCharArray(value, 10); }
        }
        public int Volume;
        public int Height;
        public short Result;
        public short NGCode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        private char[] m_Reserved;
        public string Reserved
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_Reserved)); }
            set { this.m_Reserved = StringHelper.ToFixedLengthCharArray(value, 22); }
        }
        #endregion

        #region Constructor
        public ActUtlInputResultObject()
        {
            this.BoxID = string.Empty;
            this.BoxType = string.Empty;
            this.Volume = 0;
            this.Height = 0;
            this.Result = 0;
            this.NGCode = 0;
            this.Reserved = string.Empty;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("BoxID: {0}", this.BoxID);
            sb.AppendFormat(", BoxType: {0}", this.BoxType);
            sb.AppendFormat(", Volume: {0}", this.Volume.ToString());
            sb.AppendFormat(", Height: {0}", this.Height.ToString());
            sb.AppendFormat(", Result: {0}", this.Result.ToString());
            sb.AppendFormat(", NGCode: {0}", this.NGCode.ToString());
            sb.AppendFormat(", Reserved: {0}", this.Reserved);
         
            return sb.ToString();
        }
        #endregion
    }
    #endregion
    #region ActUtlOutputResultObject
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class ActUtlOutputResultObject : IUserDefinedIoValue
    {
        #region Field
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private char[] m_BoxID;
        public string BoxID
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxID)); }
            set { this.m_BoxID = StringHelper.ToFixedLengthCharArray(value, 20); }
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        private char[] m_BoxType;
        public string BoxType
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxType)); }
            set { this.m_BoxType = StringHelper.ToFixedLengthCharArray(value, 10); }
        }
        public short Result;
        public short BufferCount;
        #endregion

        #region Constructor
        public ActUtlOutputResultObject()
        {
            this.BoxID = string.Empty;
            this.BoxType = string.Empty;
            this.Result = 0;
            this.BufferCount = 0;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("BoxID: {0}", this.BoxID);
            sb.AppendFormat(", BoxType: {0}", this.BoxType);
            sb.AppendFormat(", Result: {0}", this.Result.ToString());
            sb.AppendFormat(", BufferCount: {0}", this.BufferCount.ToString());

            return sb.ToString();
        }
        #endregion
    }
    #endregion
    #endregion
    #region PC
    #region BCRReadingResult
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class ActUtlBCRReadingResult : IUserDefinedIoValue
    {
        #region Field
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        private char[] m_BoxID;
        public string BoxID
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxID)); }
            set { this.m_BoxID = StringHelper.ToFixedLengthCharArray(value, 20); }
        }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        private char[] m_BoxType;
        public string BoxType
        {
            get { return StringHelper.TrimZeroOrWhiteSpace(new string(this.m_BoxType)); }
            set { this.m_BoxType = StringHelper.ToFixedLengthCharArray(value, 10); }
        }
        public short Result;
        public short NGCode;
        #endregion

        #region Constructor
        public ActUtlBCRReadingResult()
        {
            this.BoxID = string.Empty;
            this.BoxType = string.Empty;
            this.Result = 0;
            this.NGCode = 0;
        }
        #endregion

        #region Method
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("BoxID: {0}", this.BoxID);
            sb.AppendFormat(", BoxType: {0}", this.BoxType);
            sb.AppendFormat(", Result: {0}", this.Result.ToString());
            sb.AppendFormat(", NGCode: {0}", this.NGCode.ToString());

            return sb.ToString();
        }
        #endregion
    }
    #endregion
    #endregion
}
