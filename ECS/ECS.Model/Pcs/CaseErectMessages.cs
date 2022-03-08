using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Pcs
{
    #region Touch -> Server
    [Serializable]
    public class InkjectIgnorelackOfInk
    {
        public int Line { get; set; }

        public bool Ignore { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Line)} : {this.Line}");
            sb.Append($"{nameof(this.Ignore)} : {this.Ignore}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class InkjectResume
    {
        public int Line { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Line)} : {this.Line}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class ManualBoxValidationRequest
    {
        public string BoxId { get; set; }
        public string BoxType { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.BoxId)} : {this.BoxId}");
            sb.Append($"{nameof(this.BoxType)} : {this.BoxType}");

            return sb.ToString();
        }
    }

    [Serializable]
    // 채번 업데이트
    public class BoxNumber
    {
        public int Line { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Line)} : {this.Line}");

            return sb.ToString();
        }
    }
    #endregion

    #region Touch <- Server
    [Serializable]
    public class CaseErectBcrRead
    {
        public long CaseErectIndex { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.CaseErectIndex)} : {this.CaseErectIndex}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class InkjectInk
    {
        public int Line { get; set; }

        public int InkPercent { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Line)} : {this.Line}");
            sb.Append($"{nameof(this.InkPercent)} : {this.InkPercent}");

            return sb.ToString();
        }
    }
    [Serializable]
    public class ErectorConnectionState : BaseConnectionState
    {
        public bool Erector1Connection { get; set; }
        public bool Erector2Connection { get; set; }
        public bool Inkjet1Connection { get; set; }
        public bool Inkjet2Connection { get; set; }
        public bool ErectorBcr1Connection { get; set; }
        public bool ErectorBcr2Connection { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Erector1Connection)} : {this.Erector1Connection}");
            sb.Append($"{nameof(this.Erector2Connection)} : {this.Erector2Connection}");
            sb.Append($"{nameof(this.Inkjet1Connection)} : {this.Inkjet1Connection}");
            sb.Append($"{nameof(this.Inkjet2Connection)} : {this.Inkjet2Connection}");
            sb.Append($"{nameof(this.ErectorBcr1Connection)} : {this.ErectorBcr1Connection}");
            sb.Append($"{nameof(this.ErectorBcr2Connection)} : {this.ErectorBcr2Connection}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class ManualBoxValidationResponse
    {
        public bool Result { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{nameof(this.Result)} : {this.Result}");

            return sb.ToString();
        }
    }
    #endregion
}
