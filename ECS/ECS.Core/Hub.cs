using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Restful;
using ECS.Model;
using ECS.Model.Inkject;
using ECS.Model.Hub;
using ECS.Model.Plc;
using ECS.Model.Restfuls;
using ECS.Model.DynamicScales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ECS.Core
{
    public class Hub
    {
        #region Method      
        public void Send(string targetName, EventArgs e)
        {
            if (HubServiceName.PlcPicking1Equipment == targetName
              || HubServiceName.PlcPicking2Equipment == targetName
              || HubServiceName.PlcCaseErectEquipment == targetName
              || HubServiceName.PlcWeightInvoiceEquipment == targetName
              || HubServiceName.DynamicScaleEquipment == targetName
              || HubServiceName.InkjectEquipment1 == targetName
              || HubServiceName.InkjectEquipment2 == targetName
              || HubServiceName.NormalLabelPrinterZebraZe500Equipment == targetName
              || HubServiceName.SmartLabelPrinterZebraZe500Equipment == targetName
              || HubServiceName.OutLogicalEquipment == targetName
              || HubServiceName.TopBcrEquipment == targetName
              || HubServiceName.RouteLogicalEquipment == targetName
              || HubServiceName.OutLogicalEquipment == targetName
              || HubServiceName.TouchPcCaseErectEquipment == targetName
              || HubServiceName.TouchPcWeightInspectorEquipment == targetName
              || HubServiceName.TouchPcInvoiceRejectEquipment == targetName
              || HubServiceName.TouchPcBcrLcdEquipment == targetName
              || HubServiceName.TouchPcConveyorEquipment1 == targetName
              || HubServiceName.TouchPcConveyorEquipment2 == targetName)
            {
                var eq = EcsServerAppManager.Instance.Equipments[targetName];
                if (eq == null) return;

                eq.OnHub_Recived(e);
            }
            else if (HubServiceName.WcsPost == targetName
                || HubServiceName.RicpPost == targetName
                || HubServiceName.SpiralPost == targetName)
            {
                var manager = EcsServerAppManager.Instance.RestfulRequsetManagers[targetName];
                if (manager == null) return;

                manager.OnHub_Recived(e);
            }
        }
        #endregion
    }
}
