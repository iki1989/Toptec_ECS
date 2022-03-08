using System;
using System.Data.Common;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Urcis.SmartCode;
using Urcis.SmartCode.Database;
using Urcis.SmartCode.Database.Sqlite;
using Urcis.SmartCode.Wpf;
using Urcis.SmartCode.Database.SqlServer;
using ECS.Model;
using ECS.Core.Equipments;
using System.Text;
using ECS.Core.Util;
using System.Timers;
using System.ComponentModel;
using Urcis.SmartCode.Diagnostics;

namespace ECS.Core.Managers
{
    #region EcsServerAppManager
    public class EcsServerAppManager : WpfAppManager
    {
        #region Static
        public new static EcsServerAppManager Instance => AppManager.Instance as EcsServerAppManager;
        #endregion

        #region Field
        public WebServiceManager WebManager;
        public RestfulRequsetCollection RestfulRequsetManagers = new RestfulRequsetCollection();
        public DataBaseManagerForServer DataBaseManagerForServer;
        public EquipmentDictionary Equipments = new EquipmentDictionary();

        private SharpTimer organizeTimer = null;
        #endregion

        #region Constructor
        public EcsServerAppManager(Application app) : base(app) { }
        #endregion

        #region Property
        public new EcsServerAppManagerSetting Setting => base.Setting as EcsServerAppManagerSetting;

        public Hub Hub { get; set; } = new Hub();
        public Cache Cache { get; set; } = new Cache();
        #endregion

        #region Method
        protected override void OnAfterApplySetting()
        {
            base.OnAfterApplySetting();

            #region Sqlite - IoServerSetting.db3
            string dbFilePath = Path.Combine(AppDirectory.Instance.Setting, "PLC IO Setting.db3");
            string connectionString = SqliteDbExecutor.GetConnectionString(dbFilePath, true);
            DbExecutor.Default = new SqliteDbExecutor(connectionString);
            using (DbConnection connection = DbExecutor.Default.CreateConnection())
            {
                connection.Open();
            }
            #endregion

            var setting = this.Setting;
            DataBaseManagerSetting dataBaseManagerSetting = this.LoadDatabaseSetting();
            if (dataBaseManagerSetting != null)
                setting.DataBaseManagerSetting = dataBaseManagerSetting;

            var dbExecutor = new SqlServerDbExecutor(dataBaseManagerSetting.SqlConnectionStringBuilder.ConnectionString);
            dbExecutor.CheckConnection();

            WebServiceManagerSetting webServiceSetting = this.LoadWebServiceSetting();
            if (webServiceSetting != null)
                setting.WebServiceManagerSetting = webServiceSetting;

            EquipmentCollectionForServerSetting equipmentsSetting = this.LoadEquipmentsSetting();
            if (equipmentsSetting != null)
                setting.EquipmentCollectionSetting = equipmentsSetting;

            RestfulRequsetWcsManagerSetting wcsRestfulManagerSetting = this.LoadWcsRestfulManagerSetting();
            if (wcsRestfulManagerSetting != null)
                setting.RestfulRequsetWcsManagerSetting = wcsRestfulManagerSetting;

            RestfulRequsetRicpManagerSetting ricpRestfulManagerSetting = this.LoadRicpRestfulManagerSetting();
            if (ricpRestfulManagerSetting != null)
                setting.RestfulRequsetRicpManagerSetting = ricpRestfulManagerSetting;

            RestfulRequsetSpiralManagerSetting spiralRestfulManagerSetting = this.LoadSpiralRestfulManagerSetting();
            if (spiralRestfulManagerSetting != null)
                setting.RestfulRequsetSpiralManagerSetting = spiralRestfulManagerSetting;
        }

        protected override bool OnInitialize()
        {
            if (base.OnInitialize() == false) return false;

            var setting = this.Setting;
           
            this.DataBaseManagerForServer = new DataBaseManagerForServer(setting.DataBaseManagerSetting);
            using (DataTable dataTable = this.DataBaseManagerForServer.SelectAfterPicking())
                this.Cache.ProductInfoLoad(dataTable);

            this.WebManager = new WebServiceManager(setting.WebServiceManagerSetting);

            this.RestfulRequsetManagers.Add(HubServiceName.WcsPost, new RestfulRequsetWcsManager(setting.RestfulRequsetWcsManagerSetting));
            this.RestfulRequsetManagers.Add(HubServiceName.RicpPost, new RestfulRequsetRicpManager(setting.RestfulRequsetRicpManagerSetting));
            this.RestfulRequsetManagers.Add(HubServiceName.SpiralPost, new RestfulRequsetSpiralManager(setting.RestfulRequsetSpiralManagerSetting));

            {
                var eqSetting = setting.EquipmentCollectionSetting.PlcEquipmentSettings[0];
                this.Equipments.Add(eqSetting.Name, new PlcPickingEquipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.PlcEquipmentSettings[1];
                this.Equipments.Add(eqSetting.Name, new PlcPickingEquipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.PlcEquipmentSettings[2];
                this.Equipments.Add(eqSetting.Name, new PlcCaseErectEquipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.PlcEquipmentSettings[3];
                this.Equipments.Add(eqSetting.Name, new PlcWeightInvoiceEquipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.PlcEquipmentSettings[4];
                if (eqSetting is PlcSmartPackingSetting smartPackingSetting)
                    this.Equipments.Add(eqSetting.Name, new PlcSmartPackingEquipment(smartPackingSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.DynamicScaleEquipmentSetting;
                this.Equipments.Add(eqSetting.Name, new DynamicScaleEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.InkjectEquipmentSettings[0];
                this.Equipments.Add(eqSetting.Name, new InkjectEquipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.InkjectEquipmentSettings[1];
                this.Equipments.Add(eqSetting.Name, new InkjectEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.LabelPrinterZebraZe500EquipmentSettings[0];
                this.Equipments.Add(eqSetting.Name, new LabelPrinterZebraZe500Equipment(eqSetting));
            }
            {
                var eqSetting = setting.EquipmentCollectionSetting.LabelPrinterZebraZe500EquipmentSettings[1];
                this.Equipments.Add(eqSetting.Name, new LabelPrinterZebraZe500Equipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.TopBcrEquipmentSetting;
                this.Equipments.Add(eqSetting.Name, new TopBcrEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.RouteLogicalEquipmentSetting;
                this.Equipments.Add(eqSetting.Name, new RouteLogicalEquipment(eqSetting));
            }


            {
                var eqSetting = setting.EquipmentCollectionSetting.OutLogicalEquipmentSetting;
                this.Equipments.Add(eqSetting.Name, new OutLogicalEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[0];
                this.Equipments.Add(eqSetting.Name, new TouchPcCaseErectEquipment(eqSetting));
            }


            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[1];
                this.Equipments.Add(eqSetting.Name, new TouchPcWeightInspectorEquipment(eqSetting));
            }


            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[2];
                this.Equipments.Add(eqSetting.Name, new TouchPcInvoiceRejectEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[3];
                this.Equipments.Add(eqSetting.Name, new TouchPcBcrLcdEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[4];
                this.Equipments.Add(eqSetting.Name, new TouchPcConveyorEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[5];
                this.Equipments.Add(eqSetting.Name, new TouchPcConveyorEquipment(eqSetting));
            }

            {
                var eqSetting = setting.EquipmentCollectionSetting.PcEquipmenttSettings[6];
                this.Equipments.Add(eqSetting.Name, new TouchPcSmartPackingEquipment(eqSetting));
            }


            this.organizeTimer = new SharpTimer(setting.Organize);

            return true;
        }

        private WebServiceManagerSetting LoadWebServiceSetting()
        {
            WebServiceManagerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.WebService))
                    setting = FileSettingManager.Instance.LoadByName<WebServiceManagerSetting>(EcsSettingFile.WebService);
                else
                {
                    setting = new WebServiceManagerSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.WebService);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }

        }

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

        private EquipmentCollectionForServerSetting LoadEquipmentsSetting()
        {
            EquipmentCollectionForServerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.Equipments))
                    setting = FileSettingManager.Instance.LoadByName<EquipmentCollectionForServerSetting>(EcsSettingFile.Equipments);
                else
                {
                    setting = new EquipmentCollectionForServerSetting();
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

        private RestfulRequsetWcsManagerSetting LoadWcsRestfulManagerSetting()
        {
            RestfulRequsetWcsManagerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.WcsRestful))
                    setting = FileSettingManager.Instance.LoadByName<RestfulRequsetWcsManagerSetting>(EcsSettingFile.WcsRestful);
                else
                {
                    setting = new RestfulRequsetWcsManagerSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.WcsRestful);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }
        }

        private RestfulRequsetRicpManagerSetting LoadRicpRestfulManagerSetting()
        {
            RestfulRequsetRicpManagerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.RICPRestful))
                    setting = FileSettingManager.Instance.LoadByName<RestfulRequsetRicpManagerSetting>(EcsSettingFile.RICPRestful);
                else
                {
                    setting = new RestfulRequsetRicpManagerSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.RICPRestful);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }
        }

        private RestfulRequsetSpiralManagerSetting LoadSpiralRestfulManagerSetting()
        {
            RestfulRequsetSpiralManagerSetting setting = null;
            try
            {
                if (FileSettingManager.Instance.ExistsByName(EcsSettingFile.SpiralRestful))
                    setting = FileSettingManager.Instance.LoadByName<RestfulRequsetSpiralManagerSetting>(EcsSettingFile.SpiralRestful);
                else
                {
                    setting = new RestfulRequsetSpiralManagerSetting();
                    FileSettingManager.Instance.SaveByName(setting, EcsSettingFile.SpiralRestful);
                }

                return setting;
            }
            catch (Exception e)
            {
                this.Logger?.Write(e.Message);
                return null;
            }
        }

        protected override void OnCreate()
        {
            this.WebManager?.Create();

            this.Equipments.CreateAll();

            this.Cache.Create();

            base.OnCreate();
        }

        protected override void OnPrepare()
        {
            this.WebManager.Prepare();
            this.WebManager.Start();

            this.Equipments.PrepareAndStartAll();

            this.Cache.Prepare();
            this.Cache.Start();

            this.organizeTimer.Elapsed += OrganizeTimer_Elapsed;
            this.organizeTimer.Start();

            base.OnPrepare();
        }

        protected override void OnTerminate()
        {
            base.OnTerminate();

            if (this.organizeTimer != null)
            {
                this.organizeTimer.Stop();
                this.organizeTimer.Elapsed -= OrganizeTimer_Elapsed;
                this.organizeTimer.Dispose();
            }

            this.WebManager?.Terminate();

            this.Equipments.TerminateAll();

            this.Cache?.Terminate();
        }

        public void SaveSetting()
        {
            var setting = this.Setting;
            FileSettingManager.Instance.SaveByName(setting.WebServiceManagerSetting, EcsSettingFile.WebService);
            FileSettingManager.Instance.SaveByName(setting.DataBaseManagerSetting, EcsSettingFile.Database);
            FileSettingManager.Instance.SaveByName(setting.EquipmentCollectionSetting, EcsSettingFile.Equipments);
            FileSettingManager.Instance.SaveByName(setting.RestfulRequsetWcsManagerSetting, EcsSettingFile.WcsRestful);
            FileSettingManager.Instance.SaveByName(setting.RestfulRequsetRicpManagerSetting, EcsSettingFile.RICPRestful);
            FileSettingManager.Instance.SaveByName(setting.RestfulRequsetSpiralManagerSetting, EcsSettingFile.SpiralRestful);

            this.Cache.SpiralBuffer2f.setBoxCount = setting.SpiralBufferSettingBoxCount;
            this.organizeTimer.SetSarpInterval(setting.Organize.Hour, setting.Organize.Miniute, setting.Organize.Second);
            FileSettingManager.Instance.SaveByName(setting, "App Manager.setting");
        }
        #endregion

        #region Event Handler
        private void OrganizeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.organizeTimer.SetSarpInterval();

            this.DataBaseManagerForServer.BackupDatabaseAsync();
            this.Logger?.Write("Backup Database");

            int keepingDays = this.Setting.DataBaseBackUpDeleteDay;

            this.DataBaseManagerForServer.DeleteBackupDatabase(keepingDays);
            this.Logger?.Write($"Delete Backup Database over {keepingDays} days(~{DateTime.Now.AddDays(-keepingDays)})");

            this.DataBaseManagerForServer.DeleteByDayAfter(DateTime.Now.AddDays(-keepingDays));
            this.Logger?.Write($"Delete Database Table over {keepingDays} days(~{DateTime.Now.AddDays(-keepingDays)})");

            this.Cache.ProductInfoReLoad();
            this.Logger?.Write("Product Infos Reload");
        }
        #endregion
    }
    #endregion

    #region EcsServerAppManagerSetting
    [Serializable]
    public class EcsServerAppManagerSetting : WpfAppManagerSetting
    {
        [Browsable(false)]
        [JsonIgnore]
        public WebServiceManagerSetting WebServiceManagerSetting { get; set; } = new WebServiceManagerSetting();

        [Browsable(false)]
        [JsonIgnore]
        public EquipmentCollectionForServerSetting EquipmentCollectionSetting { get; set; } = new EquipmentCollectionForServerSetting();

        [Browsable(false)]
        [JsonIgnore]
        public DataBaseManagerSetting DataBaseManagerSetting { get; set; } = new DataBaseManagerSetting();

        [Browsable(false)]
        [JsonIgnore]
        public RestfulRequsetWcsManagerSetting RestfulRequsetWcsManagerSetting { get; set; } = new RestfulRequsetWcsManagerSetting();

        [Browsable(false)]
        [JsonIgnore]
        public RestfulRequsetRicpManagerSetting RestfulRequsetRicpManagerSetting { get; set; } = new RestfulRequsetRicpManagerSetting();

        [Browsable(false)]
        [JsonIgnore]
        public RestfulRequsetSpiralManagerSetting RestfulRequsetSpiralManagerSetting { get; set; } = new RestfulRequsetSpiralManagerSetting();

        public int SpiralBufferSettingBoxCount { get; set; } = 20;

        public int DataBaseBackUpDeleteDay { get; set; } = 30;

        public string PackageResultImagePath { get; set; } = @"http://192.168.12.21/SP";
        public string PackageResultImageExtension { get; set; } = "jpg";

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Organize Organize { get; set; } = new Organize();

        public EcsServerAppManagerSetting()
        {
            this.DisableDiskDefragmentation = true;
            this.DisableSleepModeAndDisplayOff = true;
            this.UseProcessRespondingMonitoring = true;
            this.ThreadCultureName = "ko-KR";
            this.UiCultureName = this.ThreadCultureName;

            this.Organize.Hour = 5;
        }
    }
    #endregion
}
