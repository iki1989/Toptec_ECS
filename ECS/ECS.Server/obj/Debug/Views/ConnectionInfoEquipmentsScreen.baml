   M S B A M L   `   `   `  ����C  ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.CoreECS.Core.ViewModels.Server  5      F BECS.Server, Version=2.10.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ECS.Server.ViewsECS.Server.Views 5      5   /ECS.Server.Views.ConnectionInfoEquipmentsScreen   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ivm:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.Core     5      0localclr-namespace:ECS.Server.Views     5      -   5          N� Title$$  ConnectionInfoEquipmentsScreen��5	       � DataContext 5      >   8ECS.Core.ViewModels.Server.ConnectionInfoEquipmentsModel  5   
   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5      {�.     
IsReadOnly .    CanUserSortColumns .     AutoGenerateColumns .  ��6}   �� *EquipmentConnectonInfos+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 5   
   0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ No��6!   	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 6-   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6:   �� *No+  5      $ ID��6!   $
	 Auto 6-   
 6:   �� *Id+  5      $ 이름��6!   $
	 Auto 6-   
 6:   �� *Name+  5      $ IP��6!   $
	 Auto 6-   
 6:   �� *Ip+  5      $
 Port��6!   $
	 Auto 6/   
 6<   �� *Port+  5      $ Active��6!   $
	 Auto 61   
 6>   �� *Active+  5      $ 연결상태��6!   $	 * 6/   
 69   �� *IsConnected+5      5      