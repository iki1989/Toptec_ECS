   M S B A M L   `   `   `  ���� C  ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.CoreECS.Core.ViewModels.Server  E AECS.Server, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ECS.Server.ViewsECS.Server.Views 5   /ECS.Server.Views.ConnectionInfoEquipmentsScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Ivm:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.Core     0localclr-namespace:ECS.Server.Views     -     N� Title$$  ConnectionInfoEquipmentsScreen�� � DataContext >   8ECS.Core.ViewModels.Server.ConnectionInfoEquipmentsModel  .��&   System.Windows.Controls.DataGrid  -     Name$ dataGrid��   
IsReadOnly .    CanUserSortColumns .     AutoGenerateColumns .  ���� *EquipmentConnectonInfos+ �� UpdateSourceTrigger$ PropertyChanged=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ No��	   Width5  /System.Windows.Controls.DataGridLengthConverter$
	 Auto 1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 �� *No+  $ ID��$
	 Auto 
 �� *Id+  $ 이름��$
	 Auto 
 �� *Name+  $ IP��$
	 Auto 
 �� *Ip+  $
 Port��$
	 Auto 
 �� *Port+  $ Active��$
	 Auto 
 �� *Active+  $ 연결상태��$	 * 
 �� *IsConnected+