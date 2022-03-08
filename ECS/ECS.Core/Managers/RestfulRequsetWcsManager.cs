using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urcis.SmartCode;
using ECS.Core.Restful;
using ECS.Model.Hub;
using ECS.Model.Restfuls;
using ECS.Model;

namespace ECS.Core.Managers
{
    public class RestfulRequsetWcsManager : RestfulRequsetManager
    {
        #region Field
        private RestfulWcsRequester BoxIdHistoryRequester;
        private RestfulWcsRequester WeightRequester;
        private RestfulWcsRequester InvoiceRequester;
        #endregion

        #region Prop

        public new RestfulRequsetWcsManagerSetting Setting
        {
            get => base.Setting as RestfulRequsetWcsManagerSetting;
            set => base.Setting = value;
        }
        #endregion

        #region Ctor
        public RestfulRequsetWcsManager(RestfulRequsetWcsManagerSetting setting)
        {
            this.Setting = setting ?? new RestfulRequsetWcsManagerSetting();

            this.Name = HubServiceName.WcsPost;

            string logName = string.Empty;
            logName = "WCS CaseEretor";
            this.BoxIdHistoryRequester = new RestfulWcsRequester(this.Setting.DomainName, this.Setting.BoxIdHistory, logName);

            this.WeightRequester = new RestfulWcsRequester(this.Setting.DomainName, this.Setting.Weight);
            this.InvoiceRequester = new RestfulWcsRequester(this.Setting.DomainName, this.Setting.Invoice);
        }
        #endregion

        #region Method

        private async void CaseErectBoxIdPostAsync(CaseErectBcrResultArgs args)
        {
            if (args == null) return;

            BoxID boxID = new BoxID();
            boxID.meta.SetToWcs();

            BoxID.DataClass dataClass = new BoxID.DataClass();
            dataClass.box_id = args.BoxId;
            dataClass.box_type = args.BoxType;
            dataClass.floor = "2";
            dataClass.eqp_id = args.EqpId;
            boxID.data.Add(dataClass);

            await this.BoxIdHistoryRequester.PostHttpAsync<BoxID>(boxID);
        }

        public async void WeightPostAsync(WeightAndInvoice weightAndInvoice)
        {
            weightAndInvoice.meta.SetToWcs();
            await this.WeightRequester.PostHttpAsync<WeightAndInvoice>(weightAndInvoice);
        }

        public async void InvoicePostAsync(WeightAndInvoice weightAndInvoice)
        {
            weightAndInvoice.meta.SetToWcs();
            await this.InvoiceRequester.PostHttpAsync<WeightAndInvoice>(weightAndInvoice);
        }

        public override void OnHub_Recived(EventArgs e)
        {
            if (e is CaseErectBcrResultArgs caseErectBcrResultArgs)
            {
                this.CaseErectBoxIdPostAsync(caseErectBcrResultArgs);
            }
        }
        #endregion
    }

    [Serializable]
    public class RestfulRequsetWcsManagerSetting : RestfulRequsetManagerSetting
    {
        [DisplayName("박스ID 생성 송신")]
        public string BoxIdHistory { get; set; } = "api/wcs/boxID";

        [DisplayName("중량검수 정보 송신")]
        public string Weight { get; set; } = "api/wcs/rsltWgt";

        [DisplayName("운송장 정보 송신")]
        public string Invoice { get; set; } = "api/wcs/rsltWaybill";
    }
}
