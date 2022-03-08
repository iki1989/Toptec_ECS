﻿using ECS.Core.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ECS.Server.Views
{
    /// <summary>
    /// Interaction logic for ConnectionInfoRicpRestfulScreen.xaml
    /// </summary>
    public partial class ConnectionInfoRicpRestfulScreen : Page
    {
        public ConnectionInfoRicpRestfulScreen()
        {
            InitializeComponent();

            if (this.DataContext is ConnectionInfoRicpRestfulViewModel viewModel)
                this.propertyGrid.SelectedObject = viewModel.Manager.Setting;
        }
    }
}
