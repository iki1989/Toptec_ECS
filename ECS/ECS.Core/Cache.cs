using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model;
using ECS.Model.Databases;
using ECS.Model.Restfuls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Urcis.SmartCode;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode.Threading;

namespace ECS.Core
{
    public class Cache : ILifeCycleable, IDrive, IHaveLogger
    {
        #region Field
        private Timer locationStatusScanTimer = new Timer(3 * 1000); //3s
        private Timer spiralReportTimer = new Timer(60 * 1000); //1minute
        #endregion

        #region Prop
        public Dictionary<string, ProductInfo> ProductInfos { get; set; } = new Dictionary<string, ProductInfo>();

        public Dictionary<string, LocationStatus>[] LocationStatusPikcingStations { get; set; } = new Dictionary<string, LocationStatus>[2];

        public Dictionary<string, LocationStatus>[] LocationStatusRollTainers { get; set; } = new Dictionary<string, LocationStatus>[2];

        public SpiralBuffer2f SpiralBuffer2f { get; set; } = new SpiralBuffer2f();

        public Logger ProductInfosLogger { get; private set; }
        public Logger Logger { get; protected set; }

        private LifeCycleStateEnum m_LifeState;
        public LifeCycleStateEnum LifeState
        {
            get => this.m_LifeState;
            protected set
            {
                if (this.m_LifeState == value) return;

                this.m_LifeState = value;
                this.Logger?.Write(this.m_LifeState.ToString());
            }
        }
       
        #endregion

        #region Ctor
        public Cache()
        {
            string name = "ProductInfo";
            this.ProductInfosLogger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.CacheLog));

            name = "Cache";
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo(name, EcsAppDirectory.CacheLog));

            for (int i = 0; i < this.LocationStatusPikcingStations.Length; i++)
                this.LocationStatusPikcingStations[i] = new Dictionary<string, LocationStatus>();

            for (int i = 0; i < this.LocationStatusRollTainers.Length; i++)
                this.LocationStatusRollTainers[i] = new Dictionary<string, LocationStatus>();

        }
        #endregion

        #region Method
        #region ProductInfo
        public void ProductInfoReLoadAsync()
        {
            ScTask.Run(() => this.ProductInfoReLoad());      
        }

        public void ProductInfoReLoad()
        {
            lock (this.ProductInfos)
            {
                this.ProductInfos.Clear();

                using (DataTable dataTable = EcsServerAppManager.Instance.DataBaseManagerForServer.SelectAfterPicking())
                    this.ProductInfoLoad(dataTable);
            }
        }
        public void ProductInfoLoad(DataTable dataTable)
        {
            try
            {
                lock (this.ProductInfos)
                {
                    foreach (var row in dataTable.AsEnumerable())
                    {
                        ProductInfo productInfo = new ProductInfo();
                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            var columnName = dataTable.Columns[i].ColumnName;
                            var prop = productInfo.GetType().GetProperty(columnName);
                            if (prop != null)
                            {

                                if (row.ItemArray[i] != DBNull.Value)
                                {
                                    if (prop.Name == "WEIGHT_SUM")
                                    {
                                        if (double.TryParse(row.ItemArray[i].ToString(), out double result))
                                            prop.SetValue(productInfo, result);
                                    }
                                    else
                                        prop.SetValue(productInfo, row.ItemArray[i].ToString());
                                }

                            }
                        }

                        if (string.IsNullOrEmpty(productInfo.BOX_ID) == false)
                            productInfo.BOX_ID = productInfo.BOX_ID.Trim();

                        if (string.IsNullOrEmpty(productInfo.INVOICE_ID) == false)
                            productInfo.INVOICE_ID = productInfo.INVOICE_ID.Trim();

                        StringBuilder sb = new StringBuilder();
                        if (this.ProductInfos.ContainsKey(productInfo.BOX_ID) == false)
                        {
                            this.ProductInfos.Add(productInfo.BOX_ID, productInfo);
                            sb.Clear();
                            sb.Append($"ProductInfos Add : ");
                            sb.Append($"BOX_ID = {productInfo.BOX_ID}, ");
                            sb.Append($"INVOICE_ID = {productInfo.INVOICE_ID}, ");
                            sb.Append($"MNL_PACKING_FLAG = {productInfo.MNL_PACKING_FLAG}, ");
                            sb.Append($"WEIGHT_SUM = {productInfo.WEIGHT_SUM}, ");
                            sb.Append($"WT_CHECK_FLAG = {productInfo.WT_CHECK_FLAG}");

                            this.ProductInfosLogger?.Write(sb.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProductInfosLogger?.Write(ex.Message);
            }
        }

        public void ProductInfoLoadAsync(DataTable dataTable)
        {
            ScTask.Run(() => this.ProductInfoLoad(dataTable));
        }

        public void ProductInfoRemove(string boxId)
        {
            lock (this.ProductInfos)
            {
                if (this.ProductInfos.ContainsKey(boxId))
                {
                    this.ProductInfos.Remove(boxId);
                    this.ProductInfosLogger?.Write($"ProductInfos Remove : BOX_ID = {boxId}");
                }
            }
        }
        #endregion

        #region LocationStatus
        private void LoadLocationStatus(LocationStatusEnum locationStatusEnum)
        {
            using (var table = EcsServerAppManager.Instance.DataBaseManagerForServer.GetSelectLocationStatusPickingStation(locationStatusEnum))
            {
                if (table != null)
                {
                    try
                    {
                        foreach (var row in table.AsEnumerable())
                        {
                            var key = row["SHELLCODE"].ToString().Trim();
                            int index = (int)locationStatusEnum;

                            switch (locationStatusEnum)
                            {
                                case LocationStatusEnum.PickingStation1_6_90:
                                    if (this.LocationStatusPikcingStations[0].ContainsKey(key) == false)
                                        this.LocationStatusPikcingStations[0].Add(key, new LocationStatus(row));
                                    break;
                                case LocationStatusEnum.PickingStation7_13:
                                    if (this.LocationStatusPikcingStations[1].ContainsKey(key) == false)
                                        this.LocationStatusPikcingStations[1].Add(key, new LocationStatus(row));
                                    break;
                                case LocationStatusEnum.RollTainer1_4:
                                    if (this.LocationStatusRollTainers[0].ContainsKey(key) == false)
                                        this.LocationStatusRollTainers[0].Add(key, new LocationStatus(row));
                                    break;
                                case LocationStatusEnum.RollTainer5_6:
                                    if (this.LocationStatusRollTainers[1].ContainsKey(key) == false)
                                        this.LocationStatusRollTainers[1].Add(key, new LocationStatus(row));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Logger?.Write(ex.Message);
                    }
                }
            }
        }

        public LocationStatus GetLocationStatusPickingStationButton(string shellCode)
        {
            foreach (var locationStatusPikcingStation in this.LocationStatusPikcingStations)
            {
                if (locationStatusPikcingStation.ContainsKey(shellCode))
                    return locationStatusPikcingStation[shellCode];
            }

            return null;
        }

        public LocationStatus GetLocationStatusRollTainer(string shellCode)
        {
            foreach (var locationStatusPikcingStation in this.LocationStatusRollTainers)
            {
                if (locationStatusPikcingStation.ContainsKey(shellCode))
                    return locationStatusPikcingStation[shellCode];
            }

            return null;
        }

        public LocationStatusEnum GetLocationStatusPickingStationEnum(string shellCode)
        {
            for (int i = 0; i < this.LocationStatusPikcingStations.Length; i++)
            {
                if (this.LocationStatusPikcingStations[i].ContainsKey(shellCode))
                {
                    if (i == 0)
                        return LocationStatusEnum.PickingStation1_6_90;
                    else
                        return LocationStatusEnum.PickingStation7_13;
                }
            }

            return default;
        }

        public LocationStatusEnum GetLocationStatusRollTainerEnum(string shellCode)
        {
            for (int i = 0; i < this.LocationStatusRollTainers.Length; i++)
            {
                if (this.LocationStatusRollTainers[i].ContainsKey(shellCode))
                {
                    if (i == 0)
                        return LocationStatusEnum.RollTainer1_4;
                    else
                        return LocationStatusEnum.RollTainer5_6;
                }
            }

            return default;
        }


        #endregion

        #region Spiral
        public async void SpiralBuffer2fPostAsync()
        {
            this.SpiralBuffer2f.currentBoxCount = this.GetCurrentBoxCount();

            if (this.SpiralBuffer2f.currentBoxCount > 0)
                this.SpiralBuffer2f.transportQuantity = this.SpiralBuffer2f.currentBoxCount / this.SpiralBuffer2f.setBoxCount;
            else
                this.SpiralBuffer2f.transportQuantity = 0;

            this.SpiralBuffer2f.work_Id = DateTime.Now.ToString("yyyyMMdd-hhmmssfff");

            var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetSpiralManager>();
            if (manager != null)
            {
                var response = await manager.SpiralBuffer2fRequester.PostHttpAsync<SpiralResponse>(this.SpiralBuffer2f) as SpiralResponse;
            }
            
        }

        private int GetCurrentBoxCount()
        {
            int count = 0;
            lock (this.ProductInfos)
            {
                count = this.ProductInfos
               .Where(d => d.Value.STATUS == $"{(int)statusEnum.top_invoice}")
               .Count();
            }

            return count;
        }

        public void UpdateQueuingTime()
        {
            var now = DateTime.Now;

            this.SpiralBuffer2f.QueuingTime = now - this.SpiralBuffer2f.QueuinguUpdateTime;
            this.SpiralBuffer2f.QueuinguUpdateTime = now;
        }
        #endregion

        #region Interface
        public void Start()
        {
            if (this.LifeState != LifeCycleStateEnum.Prepared)
                return;

            if (Debugger.IsAttached == false)
            {
                this.locationStatusScanTimer?.Start();
                this.spiralReportTimer?.Start();
            }
        }

        public void Stop()
        {
            this.locationStatusScanTimer?.Stop();
            this.spiralReportTimer?.Stop();
        }

        public Task StartAsync() => Task.Run(() => this.Start());

        public Task StopAsync() => Task.Run(() => this.Stop());

        public void Create() { }

        public ScTask CreateAsync() => ScTask.Run(() => this.Create());

        public void Prepare()
        {
            if (this.LifeState != LifeCycleStateEnum.Created)
                this.Create();

            this.LifeState = LifeCycleStateEnum.Preparing;

            this.LoadLocationStatus(LocationStatusEnum.PickingStation1_6_90);
            this.LoadLocationStatus(LocationStatusEnum.PickingStation7_13);
            this.LoadLocationStatus(LocationStatusEnum.RollTainer1_4);
            this.LoadLocationStatus(LocationStatusEnum.RollTainer5_6);

            #region pushbutton 초기화
            for (int i = 0; i < this.LocationStatusPikcingStations.Length; i++)
            {
                foreach (LocationStatus button in this.LocationStatusPikcingStations[i].Values)
                {
                    if (button.StatusCd == PushWorkStatusCdEnum.READY)
                    {
                        var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationEnum(button.ShellCode);
                        switch (statusEnum)
                        {
                            case LocationStatusEnum.PickingStation1_6_90:
                                {
                                    var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment] as PlcPickingEquipment;
                                    if (eq != null)
                                        eq.ButtonsResposeSetValue(button);
                                }
                                break;
                            case LocationStatusEnum.PickingStation7_13:
                                {
                                    var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                                    if (eq != null)
                                        eq.ButtonsResposeSetValue(button);
                                }
                                break;
                            case LocationStatusEnum.RollTainer1_4:
                            case LocationStatusEnum.RollTainer5_6:
                                break;
                        }
                    }
                }
            }
            #endregion

            this.locationStatusScanTimer.Elapsed += LocationStatusScanTimer_Elapsed;

            this.SpiralBuffer2f.setBoxCount = EcsServerAppManager.Instance.Setting.SpiralBufferSettingBoxCount;
            this.spiralReportTimer.Elapsed += SpiralReportTimer_Elapsed;

            this.LifeState = LifeCycleStateEnum.Prepared;
        }

        public ScTask PrepareAsync() => ScTask.Run(() => this.Prepare());

        public void Terminate()
        {
            this.LifeState = LifeCycleStateEnum.Terminating;
            this.Stop();

            if (this.locationStatusScanTimer != null)
            {
                this.locationStatusScanTimer.Elapsed -= LocationStatusScanTimer_Elapsed;
                this.locationStatusScanTimer.Dispose();
            }

            if (this.spiralReportTimer != null)
            {
                this.spiralReportTimer.Elapsed -= SpiralReportTimer_Elapsed;
                this.spiralReportTimer.Dispose();
            }

            this.LifeState = LifeCycleStateEnum.Terminated;
        }

        public ScTask TerminateAsync() => ScTask.Run(() => this.Terminate());
        #endregion
        #endregion

        #region Event Handler
        private void LocationStatusScanTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < this.LocationStatusPikcingStations.Length; i++)
            {
                foreach (LocationStatus button in this.LocationStatusPikcingStations[i].Values)
                {
                    TimeSpan timeLimit = DateTime.Now - button.UpdateTime;
                    if ((timeLimit.TotalMinutes > 5)
                        && (button.StatusCd == PushWorkStatusCdEnum.ING)
                        && (button.WorkId != null))
                    {
                        var manager = EcsServerAppManager.Instance.RestfulRequsetManagers.GetByRestfulRequsetManagerType<RestfulRequsetRicpManager>();
                        if (manager != null)
                            manager.LocationPointStatusPostAsync(button);
                    }
                       
                    else if (button.StatusCd == PushWorkStatusCdEnum.FINISH)
                    {
                        if (timeLimit.TotalSeconds > 20)
                        {
                            if (button.FinishStatusPlcWrote == false)
                            {
                                LocationStatus buttonClone = button.Clone();
                                buttonClone.StatusCd = PushWorkStatusCdEnum.READY;

                                var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationEnum(buttonClone.ShellCode);
                                switch (statusEnum)
                                {
                                    case LocationStatusEnum.PickingStation1_6_90:
                                        {
                                            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment];
                                            if (eq != null)
                                            {
                                                if (eq is PlcPickingEquipment pickingEq)
                                                    pickingEq.ButtonsResposeSetValue(button);
                                            }
                                        }
                                        break;
                                    case LocationStatusEnum.PickingStation7_13:
                                        {
                                            var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment];
                                            if (eq != null)
                                            {
                                                if (eq is PlcPickingEquipment pickingEq)
                                                    pickingEq.ButtonsResposeSetValue(button);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            var statusEnum = EcsServerAppManager.Instance.Cache.GetLocationStatusPickingStationEnum(button.ShellCode);
                            switch (statusEnum)
                            {
                                case LocationStatusEnum.PickingStation1_6_90:
                                    {
                                        var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking1Equipment] as PlcPickingEquipment;
                                        if (eq != null)
                                            eq.ButtonsResposeSetValue(button);
                                    }
                                    break;
                                case LocationStatusEnum.PickingStation7_13:
                                    {
                                        var eq = EcsServerAppManager.Instance.Equipments[HubServiceName.PlcPicking2Equipment] as PlcPickingEquipment;
                                        if (eq != null)
                                            eq.ButtonsResposeSetValue(button);
                                    }
                                    break;
                                case LocationStatusEnum.RollTainer1_4:
                                case LocationStatusEnum.RollTainer5_6:
                                    break;
                            }

                            button.FinishStatusPlcWrote = false;
                        }
                    }
                }
            }
        }

        private void SpiralReportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.SpiralBuffer2fPostAsync();
        }
        #endregion
    }
}
