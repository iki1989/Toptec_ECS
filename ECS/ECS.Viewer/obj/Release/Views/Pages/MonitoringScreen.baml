   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  E AECS.Viewer, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ECS.Viewer.Views.PagesECS.Viewer.Views.Pages E'clr-namespace:ECS.Viewer.Views.ControlsECS.Viewer.Views.Controls C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.CoreECS.Core.ViewModels.Viewer -   'ECS.Viewer.Views.Pages.MonitoringScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     6local$clr-namespace:ECS.Viewer.Views.Pages     <controls'clr-namespace:ECS.Viewer.Views.Controls     Ivm:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.Core     -      ViewModel.Counting  � DataContext#  [   N� Title$ MonitoringScreen��.���  � ColumnDefinitions �� $	��40* ��� $	��40* ��� $	��40* � � RowDefinitions �� $��50 ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ 	TodayDate��$��0��$��2��$��
2021.09.09��$��Right=�$��0,0,30,0q�$��30(� �� TextAlignment$ Right=�$��Center=��� $��1��$��0��$��15q� �� CornerRadius$ 3��$��5q����	#FF007DC3._��  �� $	��40* ��� $	��40* � �� $��40 ��� $	��300 ��� $��50 �.+��� $��2��
�������$��일별 오더 현황��$ Center=�$��25(����	#FF007DC3$
��-1 0q�$��40��7  1System.Windows.Forms.Integration.WindowsFormsHost  -     Name$ dailyOrderChart��$��1��$��2��$��10,12,10,10q�   
Foreground Z� 	   Child.	 +  %ECS.Viewer.Views.Controls.SimpleChart  -   
OrderChart���� $��2��$��0��$��2��
 �� Orientation$
 
Horizontal=�$��Center=�$��0,3,20,0q�.+��� $��당일 오더 수량 : ��$ Right=����	#FF545557$��24(��� -   $ TodayOrderCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��1��$��1��$��15q�$ 3��$��5q����	#FF007DC3._��  �� $��9* ��� $
��254* ��� $
��261* � �� $��40 ��� $	��30* ��� $	��30* ��� $	��30* ��� $	��30* �.+��� $��0��$��3��
�������$��Reject 현황��$ Center=�$��25(�$
��-1 0q����	#FF007DC3$��40���� $��1��$��1��$��2��$
 
Horizontal=�$��Center=�$��Center=�.+��� $��제함 리젝 수량 : ��$ Right=����	#FF545557$��24(��� -   $ TodayCaseErectRejectCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��2��$��1��$��2��$
 
Horizontal=�$��Center=�$��Center=�.+��� $��중량 리젝 수량 : ��$ Right=����	#FF545557$��24(��� -   $ TodayWeightRejectCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��3��$��1��$��2��$
 
Horizontal=�$��Center=�$��Center=�.+��� $��충진 리젝 수량 : ��$ Right=����	#FF545557$��24(��� -   $" TodaySmartPackingRejectCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��4��$��1��$��2��$
 
Horizontal=�$��Center=�$��Center=�.+��� $��상면 리젝 수량 : ��$ Right=����	#FF545557$��24(��� -	   $ TodayTopRejectCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��1��$��2��$��15q�$ 3��$��5q�  Brush.CjBlue#��[ ._��  �� $	��40* ��� $	��40* � �� $��40 ��� $	��30* �.+��� $��0��$��2��
�������$��시간당 출하량��$ Center=�$��25(�#��[ $	��Top=�$��2��$
��-1 0q�$��40���� $��1��$��0��$��2��$
 
Horizontal=�$��Center=�$��Center=�.+��� $ ��현재 시각 출하량 : ��$ Right=�  Brush.DarkGray#��[ $��24(��� -
   $ HourlyOutCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��2��$��0��$��15q�$ 3��$��5q�  Brush.CjRed#��[ ._��  �� $	��40* ��� $	��40* � �� $��40 ��� $	��300 ��� $��50 �.+��� $��2��
�������$��전일 미출고 현황��$ Center=�$��25(�#��[ $	��Top=�$
��-1 0q�$��40��  -   $ yesterdayNotOutChart��$��1��$��2��$��10q� Z� �� $��2��$��0��$��2��$
 
Horizontal=�$��Center=�$��0,3,20,0q�.+��� $��전일 미출하량 : ��$ Right=����	#FF545557$��24(��� -   $ NonOutCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��2��$��1��$��15q�$ 3��$��5q�#��[ ._��  �� $	��40* ��� $	��40* � �� $��40 ��� $	��300 ��� $��50 �.+��� $��2��
�������$��금일 출고 현황��$ Center=�$��25(�#��[ $��2��$	��Top=�$
��-1 0q�$��40��  -   $ todayOutChart��$��1��$��2��$��10q� Z� �� $��2��$��0��$��2��$
 
Horizontal=�$��Center=�$��0,3,20,0q�.+��� $��금일 총 출하량 : ��$ Right=����	#FF545557$��24(��� -   $ TodayOutCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50���� $��2��$��2��$��15q�$ 3��$��5q�  Brush.CjYellow#��[ ._��  �� $	��40* ��� $	��40* � �� $��40 ��� $	��300 ��� $��50 �.+��� $��2��
�������$��누적 현황(Total)��$ Center=�$��25(�#��[ $	��Top=�$
��-1 0q�$��40��  -   $ totalOutChart��$��1��$��2��$��10q� Z� �� $��2��$��0��$��2��$
 
Horizontal=�$��Center=�$��0,3,20,0q�.+��� $ ��전체 누적 출하량 : ��$ Right=����	#FF545557$��24(��� -   $ TotalOutCount��$��0��$
��Left=�$��24(��� $��EA��$��24(�$
 Left=�$��50��