using ECS.Core.Managers;
using ECS.Model.Pcs;
using ECS.Core.Equipments;
using ECS.Core.Util;
using Urcis.SmartCode.Diagnostics;
using Urcis.SmartCode;
using System.Windows.Media;
using System.Windows.Threading;

namespace ECS.Core.ViewModels.Touch
{
    public class EcsTouchViewModel : Notifier, IHaveLogger
    {
        #region field
        #endregion

        #region property
        protected Dispatcher Dispatcher { get; } = Dispatcher.CurrentDispatcher;
        protected DataBaseManagerForTouch dbm => EcsTouchAppManager.Instance.DataBaseManager;
        protected ServerPcEquipment server { get; }
        public Logger Logger { get; protected set; }

        private bool m_ServerConnection;
        public bool ServerConnection
        {
            get => this.m_ServerConnection;
            set
            {
                this.Logger.Write($"ServerConnection Chenged : {value}");
                this.m_ServerConnection = value;
                this.OnPropertyChanged(nameof(this.ServerConnection));
            }
        }

        private BcrAlarmSetReset m_BcrAlarmSetReset;
        public BcrAlarmSetReset BcrAlarmSetReset
        {
            get => this.m_BcrAlarmSetReset;
            set
            {
                this.Logger.Write($"BcrAlarmSet Chenged : {value}");
                this.m_BcrAlarmSetReset = value;
                this.OnPropertyChanged(nameof(this.BcrAlarmSetReset));
            }
        }

        #region error message

        private bool m_ShowErrorMessage = false;
        public bool ShowErrorMessage
        {
            get => this.m_ShowErrorMessage;
            set
            {
                this.Logger.Write($"ShowErrorMessage Chenged : {value}");
                this.m_ShowErrorMessage = value;
                this.OnPropertyChanged(nameof(this.ShowErrorMessage));
            }
        }

        private string m_ErrorMessage = "Error";
        public string ErrorMessage
        {
            get => this.m_ErrorMessage;
            set
            {
                this.Logger.Write($"ErrorMessage Chenged : {value}");
                this.m_ErrorMessage = value;
                this.OnPropertyChanged(nameof(this.ErrorMessage));
            }
        }

        protected static readonly SolidColorBrush Black = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        protected static readonly SolidColorBrush CjRed = new SolidColorBrush(Color.FromRgb(0xE3, 0x1B, 0x23));



        private SolidColorBrush m_TextBrush = Black;
        public SolidColorBrush TextBrush
        {
            get => this.m_TextBrush;
            set
            {
                this.m_TextBrush = value;
                this.OnPropertyChanged(nameof(this.TextBrush));
            }
        }
        #endregion

        #endregion

        #region constructor
        public EcsTouchViewModel()
        {
            this.Logger = Log.GetOrCreateLogger(new LoggerCreationInfo("ECS_Touch", AppDirectory.Instance.Log));
            this.Logger.Write("EcsTouchViewModel Start");
            this.server = EcsTouchAppManager.Instance.Equipments.GetByEquipmentType<ServerPcEquipment>();
            this.EnrollEventHandler();
        }
        #endregion

        #region method

        #region public
        public void ShowErrorMessageBox(string msg, SolidColorBrush color = null)
        {
            this.TextBrush = color ?? Black;
            this.Logger.Write($"ShowErrorMessageBox : {msg}");
            this.ErrorMessage = msg;
            this.ShowErrorMessage = true;
        }
        public bool CheckPassword(string password)
        {
            return EcsTouchAppManager.Instance.Setting.Password == password;
        }
        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.ServerConnection = server.Communicator.TcpConnectionState == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;
            server.Communicator.TcpConnectionStateChanged += OnServerConnectionChanged;
            server.TimeSynconizeReceived += OnTimeSyncronizeReceived;
            server.BcrAlarmSetResetReceived += OnBcrAlarmSetResetReceived;
        }

        protected virtual void OnServerConnectionChanged(object sender, Urcis.SmartCode.Net.Tcp.TcpConnectionStateChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnServerConnectionChanged : {e.Current}");
                this.ServerConnection = e.Current == Urcis.SmartCode.Net.Tcp.TcpConnectionStateEnum.Connected;
            });
        }
        protected virtual void OnTimeSyncronizeReceived(TimeSyncronize data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnTimeSyncronizeReceived : {data}");
                SYSTEMTIME.SetLocalTime(data.Time);
            });
        }

        protected virtual void OnBcrAlarmSetResetReceived(BcrAlarmSetReset data)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnBcrAlarmSetResetReceived : {data}");
                this.BcrAlarmSetReset = data;
            });
        }
        #endregion

        #endregion
    }
}
