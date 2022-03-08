using System;
using System.Data.Common;
using System.IO;
using System.Windows;
using Urcis.SmartCode;
using Urcis.SmartCode.Database;
using Urcis.SmartCode.Database.Sqlite;
using Urcis.SmartCode.Wpf;
using ECS.Model;
using ECS.Core.Equipments;
using ECS.Core.Restful;
using System.Collections.Generic;
using System.Data;
using Urcis.SmartCode.Database.SqlServer;
using Newtonsoft.Json;

namespace ECS.Core.Managers
{
    #region EcsTouchAppManager
    public class EcsTouchAppManager : WpfAppManager
    {

        #region Static
        public new static EcsTouchAppManager Instance => AppManager.Instance as EcsTouchAppManager;
        #endregion

        #region Field
        #endregion

        #region Constructor
        public EcsTouchAppManager(Application app) : base(app) { }
        #endregion

        #region Property
        public DataBaseManagerForTouch DataBaseManager { get; set; }
        public new EcsTouchAppManagerSetting Setting => base.Setting as EcsTouchAppManagerSetting;

        public EquipmentDictionary Equipments = new EquipmentDictionary();
        #endregion

        #region Method

        #region private
        private DataBaseManagerSetting LoadDatabaseSetting()
        {
            DataBaseManagerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.Database))
                    setting = FileSettingManager.Instance.LoadByName<DataBaseManagerSetting>(EcsSettingFile.Database);
                else
                {
                    setting = new DataBaseManagerSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.Database);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }
        }

        private EquipmentCollectionForTouchSetting LoadEquipmentsSetting()
        {
            EquipmentCollectionForTouchSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.Equipments))
                    setting = FileSettingManager.Instance.LoadByName<EquipmentCollectionForTouchSetting>(EcsSettingFile.Equipments);
                else
                {
                    setting = new EquipmentCollectionForTouchSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.Equipments);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }
        }
        #endregion
        #region life cycle
        protected override void OnAfterApplySetting()
        {
            base.OnAfterApplySetting();

            var setting = this.Setting;
            DataBaseManagerSetting dataBaseManagerSetting = this.LoadDatabaseSetting();
            if (dataBaseManagerSetting != null)
                setting.DataBaseManagerSetting = dataBaseManagerSetting;

            var dbExecutor = new SqlServerDbExecutor(dataBaseManagerSetting.SqlConnectionStringBuilder.ConnectionString);
            dbExecutor.CheckConnection();

            EquipmentCollectionForTouchSetting equipmentsSetting = this.LoadEquipmentsSetting();
            if (equipmentsSetting != null)
                setting.EquipmentCollectionSetting = equipmentsSetting;
        }
        protected override bool OnInitialize()
        {
            if (!base.OnInitialize())
                return false;

            var setting = this.Setting;

            switch (setting.TouchType)
            {
                case EcsTouchType.BcrLcd:
                case EcsTouchType.InvoiceReject:
                case EcsTouchType.CaseErect:
                case EcsTouchType.WeightCheck:
                case EcsTouchType.SmartPacking:
                    this.DataBaseManager = new DataBaseManagerForTouch(setting.DataBaseManagerSetting);
                    break;
            }

            switch (setting.TouchType)
            {
                case EcsTouchType.InvoiceReject:
                case EcsTouchType.CaseErect:
                    var eqSetting = setting.EquipmentCollectionSetting.LabelPrinterZebraEquipmentSetting;
                    this.Equipments.Add(eqSetting.Name, new LabelPrinterZebraZt411Equipment(eqSetting));
                    break;
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSetting;
                this.Equipments.Add(eqSetting.Name, new ServerPcEquipment(eqSetting));
            }
            return true;
        }
        protected override void OnCreate()
        {
            base.OnCreate();

            foreach (var equipment in this.Equipments.Values)
                equipment.Create();
        }
        protected override void OnPrepare()
        {
            base.OnPrepare();

            foreach (var equipment in this.Equipments.Values)
            {
                equipment.Prepare();
                equipment.Start();
            }
        }
        protected override void OnTerminate()
        {
            base.OnTerminate();

            foreach (var equipment in this.Equipments.Values)
                equipment?.Terminate();
        }
        #endregion
        #endregion
    }
    #endregion

    public enum EcsTouchType { BcrLcd, InvoiceReject, WeightCheck, CaseErect, Conveyor, SmartPacking }
    #region EcsEouchAppManagerSetting
    [Serializable]
    public class EcsTouchAppManagerSetting : WpfAppManagerSetting
    {
        public EcsTouchType TouchType { get; set; }
        public string Password { get; set; } = "1";

        [JsonIgnore]
        public DataBaseManagerSetting DataBaseManagerSetting { get; set; } = new DataBaseManagerSetting();

        [JsonIgnore]
        public EquipmentCollectionForTouchSetting EquipmentCollectionSetting { get; set; }

        public EcsTouchAppManagerSetting() : base()
        {
            this.EquipmentCollectionSetting = new EquipmentCollectionForTouchSetting();

            this.DisableDiskDefragmentation = true;
            this.DisableSleepModeAndDisplayOff = true;
            this.UseProcessRespondingMonitoring = true;
            this.ThreadCultureName = "ko-KR";
            this.UiCultureName = this.ThreadCultureName;
        }
    }
    #endregion
}
