using ECS.Core.Equipments;
using ECS.Core.Managers;
using ECS.Core.Util;
using ECS.Model.Domain.Touch;
using ECS.Model.Pcs;
using ECS.Model.Plc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.ViewModels.Touch
{
    public class ConveyorViewModel : EcsTouchViewModel
    {
        #region field
        private ConveyorSpeedEnum m_CurrentConveyor;
        #endregion

        #region property

        private int m_TabIndex = 0;
        public int TabIndex
        {
            get => this.m_TabIndex;
            set
            {
                this.m_TabIndex = value;
                this.OnPropertyChanged(nameof(this.TabIndex));
            }
        }


        private bool m_PlcConnection = true;
        public bool PlcConnection
        {
            get => this.m_PlcConnection;
            set
            {
                this.m_PlcConnection = value;
                this.OnPropertyChanged(nameof(this.PlcConnection));
            }
        }


        private bool m_DbConnection = true;
        public bool DbConnection
        {
            get => this.m_DbConnection;
            set
            {
                this.m_DbConnection = value;
                this.OnPropertyChanged(nameof(this.DbConnection));
            }
        }


        private bool m_Property;
        public bool Property
        {
            get => this.m_Property;
            set
            {
                this.m_Property = value;
                this.OnPropertyChanged(nameof(this.Property));
            }
        }


        private bool m_ShowConveyorSpeedPopup;
        public bool ShowConveyorSpeedPopup
        {
            get => this.m_ShowConveyorSpeedPopup;
            set
            {
                this.m_ShowConveyorSpeedPopup = value;
                this.OnPropertyChanged(nameof(this.ShowConveyorSpeedPopup));
            }
        }


        private string m_ConveyorSpeedPopupTitle;
        public string ConveyorSpeedPopupTitle
        {
            get => this.m_ConveyorSpeedPopupTitle;
            set
            {
                this.m_ConveyorSpeedPopupTitle = value;
                this.OnPropertyChanged(nameof(this.ConveyorSpeedPopupTitle));
            }
        }


        private double m_PV;
        public double PV
        {
            get => this.m_PV;
            set
            {
                this.m_PV = value;
                this.OnPropertyChanged(nameof(this.PV));
            }
        }


        private double m_SV;
        public double SV
        {
            get => this.m_SV;
            set
            {
                this.m_SV = value;
                this.OnPropertyChanged(nameof(this.SV));
            }
        }


        private bool m_IsRatioMode = false;
        public bool IsRatioMode
        {
            get => this.m_IsRatioMode;
            set
            {
                this.m_IsRatioMode = value;
                this.OnPropertyChanged(nameof(this.IsRatioMode));
            }
        }


        private int m_NormalRatio;
        public int NormalRatio
        {
            get => this.m_NormalRatio;
            set
            {
                this.m_NormalRatio = value;
                this.OnPropertyChanged(nameof(this.NormalRatio));
            }
        }


        private int m_SmartRatio;
        public int SmartRatio
        {
            get => this.m_SmartRatio;
            set
            {
                this.m_SmartRatio = value;
                this.OnPropertyChanged(nameof(this.SmartRatio));
            }
        }


        private int m_CurrentNormalRatio;
        public int CurrentNormalRatio
        {
            get => this.m_CurrentNormalRatio;
            set
            {
                this.m_CurrentNormalRatio = value;
                this.OnPropertyChanged(nameof(this.CurrentNormalRatio));
            }
        }


        private int m_CurrentSmartRatio;
        public int CurrentSmartRatio
        {
            get => this.m_CurrentSmartRatio;
            set
            {
                this.m_CurrentSmartRatio = value;
                this.OnPropertyChanged(nameof(this.CurrentSmartRatio));
            }
        }


        #region password

        private bool m_ShowPasswordBox;
        public bool ShowPasswordBox
        {
            get => this.m_ShowPasswordBox;
            set
            {
                this.m_ShowPasswordBox = value;
                this.OnPropertyChanged(nameof(this.ShowPasswordBox));
            }
        }
        #endregion

        #endregion

        #region constructor
        public ConveyorViewModel() : base()
        {
            this.Logger.Write("ConveyorViewModel Start");
            this.EnrollEventHandler();
        }
        #endregion

        #region method

        #region public
        public void ConveyorClick(ConveyorSpeedEnum conveyorType, string title)
        {
            if (this.ServerConnection)                     
            {
                this.ConveyorSpeedPopupTitle = title;
                this.m_CurrentConveyor = conveyorType;
                this.server.CvSpeedRequest(conveyorType);
            }
            else
            {
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");
            }
        }
        public void ConveyorSubmit()
        {
            this.Logger.Write($"ConveyorViewModel : ConveyorSubmit({this.m_CurrentConveyor}, {this.SV})");
            if (this.ServerConnection)
            {
                if (1 <= this.SV && this.SV <= 60)
                {

                    this.server.ConveyorCvSpeed(this.m_CurrentConveyor, this.SV);
                    this.ShowErrorMessageBox("컨베이어 속도 변경을 요청했습니다.");
                }
                else
                    this.ShowErrorMessageBox("값이 범위를 벗어났습니다.");
            }
            else
            {
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");
            }
        }
        public void RouteModeClick()
        {
            this.Logger.Write($"ConveyorViewModel : RouteModeClick()");
            if (this.ServerConnection)
            {
                server.RouteModeRequest();
            }
            else
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");
        }
        public void ToggleClick()
        {
            this.Logger.Write($"ConveyorViewModel : ToggleClick({this.IsRatioMode})");
            if (this.ServerConnection)
            {
                server.RouteMode(IsRatioMode ? "Auto" : "Ratio", CurrentSmartRatio, CurrentNormalRatio);
                this.IsRatioMode ^= true;
                this.ShowErrorMessageBox("배출 분기 모드 변경을 요청했습니다.");
            }
            else
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");
        }
        public void RouteModeSubmit()
        {
            this.Logger.Write($"ConveyorViewModel : RouteModeSubmit({this.IsRatioMode}, {this.SmartRatio}, {this.NormalRatio})");
            if (this.ServerConnection)
            {
                if (!this.IsRatioMode)
                {
                    this.ShowErrorMessageBox("배출 분기 모드가 자동입니다.");
                    return;
                }
                if (SmartRatio < 0 || SmartRatio > 10 || NormalRatio < 0 || NormalRatio > 10)
                {
                    this.ShowErrorMessageBox("배출 비율이 범위를 벗어났습니다.");
                    return;
                }
                if (SmartRatio == 0 && NormalRatio == 0)
                {
                    this.ShowErrorMessageBox("배출 비율이 0:0은 가능하지 않습니다.");
                    return;
                }
                server.RouteMode("Ratio", SmartRatio, NormalRatio);
                this.ShowErrorMessageBox("배출 비율 변경을 요청했습니다.");
            }
            else
                this.ShowErrorMessageBox("서버와 연결되지 않았습니다.");

        }
        #endregion

        #region event handler
        private void EnrollEventHandler()
        {
            this.server.ConveyorCvSpeedReceived += OnConveyorCvSpeedRecieved;
            this.server.RouteModeReceived += OnRouteModeRecieved;
        }
        private void OnConveyorCvSpeedRecieved(ConveyorCvSpeed conveyorCvSpeed)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnConveyorCvSpeedRecieved : {conveyorCvSpeed}");
                if (conveyorCvSpeed.ConveyorSpeed == (int)this.m_CurrentConveyor)
                {
                    this.ShowConveyorSpeedPopup = true;
                    this.PV = conveyorCvSpeed.Pv;
                    this.SV = conveyorCvSpeed.Sv;
                }
            }
            );
        }
        private void OnRouteModeRecieved(RouteMode routeMode)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Logger.Write($"OnRouteModeRecieved : {routeMode}");
                this.IsRatioMode = (routeMode.Mode == "Ratio");
                this.CurrentNormalRatio = routeMode.NormalRatio;
                this.NormalRatio = routeMode.NormalRatio;
                this.CurrentSmartRatio = routeMode.SmartRatio;
                this.SmartRatio = routeMode.SmartRatio;
                this.TabIndex = 2;
            });
        }
        #endregion

        #endregion
    }
}
