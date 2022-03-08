using ECS.Core.Managers;
using ECS.Model.Pcs;
using ECS.Core.Equipments;
using ECS.Core.Util;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode;
using System.Windows.Threading;

namespace ECS.Core.ViewModels.Viewer
{
    public class EcsViewerViewModel : Notifier, IHaveLogger
    {
        protected Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        protected DataBaseManagerForViewer dbm => DataBaseManagerForViewer.Instance;

        public Logger Logger { get; protected set; }

        #region constructor
        public EcsViewerViewModel()
        {
            this.Logger = Log.GetLogger("ECS_Viewer") ?? new Logger("ECS_Viewer", AppDirectory.Instance.Log);
            this.Logger.Write("EcsViewerViewModel Start");
        }
        #endregion


    }
}
