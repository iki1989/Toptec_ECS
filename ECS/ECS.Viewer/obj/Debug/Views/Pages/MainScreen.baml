   M S B A M L   `   `   `  ����F  BECS.Viewer, Version=1.11.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ECS.Viewer.Views.PagesECS.Viewer.Views.Pages  5      E'clr-namespace:ECS.Viewer.Views.ControlsECS.Viewer.Views.Controls  5      D @ECS.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:ECS.Model.Controls;assembly=ECS.ModelECS.Model.Controls 5      C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.CoreECS.Core.ViewModels.Viewer 5	      M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089/$clr-namespace:System;assembly=SystemSystem 5
      '    !ECS.Viewer.Views.Pages.MainScreen   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:ECS.Viewer.Views.Pages     5      <controls'clr-namespace:ECS.Viewer.Views.Controls     5      MmodelControls3clr-namespace:ECS.Model.Controls;assembly=ECS.Model     5      Ivm:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.Core     5	      7system$clr-namespace:System;assembly=System     5
        N� Title$  
MainScreen��5       � 	Resources 5         Uri.Screen.Monitoring  Uri.Screen.OrderSearch  Uri.Screen.WeightReject  Uri.Screen.InvoiceReject ! Uri.Screen.SmartPackingReject  Uri.Screen.OutManage  Uri.Screen.TrackingSearch   Uri.Screen.TrackingLogSearch  Uri.Screen.OutChart��%)  &	        &	 1     &	 c     &	 �     &	 �     &	     &	 3    &	 h    &	 �    @�5   
   MonitoringScreen.xaml?�5      @�5   
   OrderSearchScreen.xaml?�5      @�5   
   WeightRejectScreen.xaml?�5      @�5   
   InvoiceRejectScreen.xaml?�5      @�5   
   !SmartPackingRejectScreen.xaml?�5      @�5   
   OutManageScreen.xaml?�5       @�5!   
   TrackingSearchScreen.xaml?�5#      @�5$   
    TrackingLogSearchScreen.xaml?�5&      @�5'   
   OutChartScreen.xaml?�5)      5*      .��5+      �  � DataContext 5,   
   .  (ECS.Core.ViewModels.Viewer.MainViewModel  5-      -   5        	ViewModel��5-       � ColumnDefinitions 5/   
   �� 50      $	��230 �6   �� 51      $��* �6    � RowDefinitions 53   
   �� 54      $��70 �6   �� 55      $��* �6   .+�58   
   � $��2��6   ��59      �� 5:       �� ImageSource$! /Images/MainBackground1.png��6   .+�5<      (� $
��Left=�6   .��5=      �� -   5         ��Name$ 	txbMenuNm��5=      $��5q�6j    	 Font.Common#��[	 6u   ��60   �� *SelectedItem.Text+ �� FallbackValue$ 작업 모니터링��6u   5>      (� 5?      $��Right=�6   .��5@      �� $��스마트층��6   $��3q�6(   
�������$!��#CJ ONLYONE NEW 제목 Bold)�6F   5A      5B      � 5D   
   $��1��6   $��0��6   ���	#FF007AC3 K� ZIndex$ 1��6C    5E      �� 5F      $
��0.7* �6    �� 5G      $
��1.8* �6    �� 5H      $	��40* �6    .+�5O      �� -   5        	 ��Name$
	 Menu��5O   U   $��3��6    
 Brush.CjBlue#��[
 6*   $��10q�6c   .+�5P      1   +ECS.Viewer.Views.Controls.EcsViewerMenuItem  -   5        
  Name$

 work��5P   -   1   +ECS.Viewer.Views.Controls.EcsViewerMenuItem   SubMenuClicked$ Menu_SubMenuClicked��6J      Header$ 작업 현황��6;      Items. 5Q      -  'ECS.Model.Controls.EcsViewerMenuSubItem     Text$ 작업 모니터링��69      Screen# [  6H   5R        5S      $ Menu_SubMenuClicked��6<   $ 주문 관리��6-   . 5T        $ 주문 조회��69   # [ 6F   5U        5V      $ Menu_SubMenuClicked��6<   $ 출고 관리��6-   . 5W        $ 중량검수 리젝��69   # [ 6H     5X      $  스마트 완충재 리젝��69   # [ 6K     5Y      $ 상면검증 리젝��69   # [ 6H     5Z      $ 상품출고 관리��69   # [ 6H   5[        5\      $ Menu_SubMenuClicked��6<   $ 작업 조회��6-   . 5]        $ 작업별 이력 조회��69   # [ 6J     5^      $ 시간별 작업 조회��69   # [ 6J     5_      $ 출고차트 조회��69   # [ 6H   5`      5a      5b       � 5c   
   $��1��6   $��1��6     � Source 6-   �� *SelectedItem.Screen+# [  5d      5e      