   M S B A M L   `   `   `  ���� E  AECS.Viewer, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ECS.Viewer.Views.PagesECS.Viewer.Views.Pages  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.CoreECS.Core.ViewModels.Viewer .    (ECS.Viewer.Views.Pages.OrderSearchScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:ECS.Viewer.Views.Pages     6sys&clr-namespace:System;assembly=mscorlib     Ivm:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.Core       N� Title$  OrderSearchScreen�� N� 
Background
 �   �   ViewModel.OrderSearch � DataContext# [  .��� 
������� � RowDefinitions �� $	��120 ��� $��* �.+��� $��0��$��7q����	#FFF2F2F2._�]� .+���  �� Orientation$ 
Horizontal=�$��5 4 5 4q�$��Center=�$
��Left=�.+��� .+��� $ 
Horizontal=�.+��� $��기간 :��$��Right=�$��Center=� �� TextAlignment$ Right=� �� Padding$ 4q�$��23(�$��70��$��45��(  "System.Windows.Controls.DatePicker  -   $��Begin��  DatePickerIconResizingStyle#��[  J� RenderTransformOrigin$ 0.5,1&�$��0 0 0 0q�$��23(�$	��200��$��45��   SelectedDate �� *Begin+�� $��~ ��$��Center=�$��Center=�$ Center=�$ 2q�$��25(�$��70��$��45��  -   $	��End��#��[ $ 0.5,1&�$��0 0 0 0q�$��23(�$	��200��$��45�� �� *End+�� $��전체 :��$��Center=�$��Center=�$ Right=�$ 2q�$��10 0 0 0q�$��23(�$��70��$��45���� $��Center=�$��Center=�$
 Left=�$ 2q�$��10 0 0 0q�$��23(�$��80��$��45������ *	DataCount+�� $��출고 :��$��Center=�$��Center=�$ Right=�$ 2q�$��10 0 0 0q�$��23(�$��70��$��45���� $��Center=�$��Center=�$
 Left=�$ 2q�$��10 0 0 0q�$��23(�$��80��$��45������ *
OutCount+�� $��미출고 :��$��Center=�$��Center=�$ Right=�$ 2q�$��10 0 0 0q�$��23(�$��70��$��45���� $��Center=�$��Center=�$
 Left=�$ 2q�$��10 0 0 0q�$��23(�$��80��$��45������ *NonOutCount+�� $��주문취소 :��$��Center=�$��Center=�$ Right=�$ 2q�$��10 0 0 0q�$��23(�$	��120��$��45���� $��Center=�$��Center=�$
 Left=�$ 2q�$��10 0 0 0q�$��23(�$��70��$��45������ *
CanelCount+�� $ 
Horizontal=�$��0 6 0 0q�.+��� $ 
Horizontal=�.+��� $��출고 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$��70��$��45���� $��Center=�$��Center=�$��23(�  Font.Common#��[ $	��200��$��45��	 �� SelectedItem	 �� *	OutFilter+.@���
전체����
출고����	미출고����주문취소���� -   $��Center=�$��Center=�$��10 0 0 0q�$��20(�$	��150��$��45������ *CstCd+.������ 
 �� 
TargetType"
 ��.���  �� Property � �� �� Value# [  �� 	Resources   CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual  Brush.LightGray��%g   &	       7  !� $
 Left=�$ Center=�$
 None=� �� $��고객코드��#���  8��  $��20(� �� Triggers P�  P� Property � �� P� Value$  ��. ���  � ��# [ P�  �@J�IsKeyboardFocused . . ���  � ��
 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��20(�$	��180��$��45������ *
CstOrdNo+.������ "
 ��.���  � ��# [  ��%m   &	       7  !� $
 Left=�$ Center=�$
 None=� �� $��고객주문번호��#���  8��  $��20(� P�  � ��$  ��. ���  � ��# [ P�  �@J�IsKeyboardFocused . . ���  � ��
 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��20(�$	��180��$��45������ *WaveNo+.������ "
 ��.���  � ��# [  ��%j   &	       7  !� $
 Left=�$ Center=�$
 None=� �� $��웨이브번호��#���  8��  $��20(� P�  � ��$  ��. ���  � ��# [ P�  �@J�IsKeyboardFocused . . ���  � ��
 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��20(�$	��150��$��45������ *BoxId+.������ "
 ��.���  � ��# [  ��%c   &	       7  !� $
 Left=�$ Center=�$
 None=� �� $��박스ID��#���  8��  $��20(� P�  � ��$  ��. ���  � ��# [ P�  �@J�IsKeyboardFocused . . ���  � ��
 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��20(�$	��150��$��45������ *	InvoiceId+.������ "
 ��.���  � ��# [  ��%g   &	       7  !� $
 Left=�$ Center=�$
 None=� �� $��송장번호��#���  8��  $��20(� P�  � ��$  ��. ���  � ��# [ P�  �@J�IsKeyboardFocused . . ���  � ��
 �����.���� $ 
Horizontal=�$��5 4 5 4q�$��Center=�$��Right=�.+��� $ 
Horizontal=�.+��� .+��� $ 
Horizontal=�.+��� $��Page : ��$��Center=�$��Center=�$ Right=�$ 1q�$��23(�$	��110��$��45���� -   $��Center=�$��Center=� �� TextAlignment$ Center=�$��23(�$��45��$��80������ *Page+�� $	�� / ��$��Center=�$��Center=�$ Center=�$ 1q�$��23(�$��20��$��45���� $��Center=�$��Center=�$ Center=�$ 1q�$��23(�$��45��$��80������ *	MaxPage+�� $ 
Horizontal=�$��0 6 0 0q�.+��� $��항목 수 : ��$��Center=�$��Center=�$ Right=�$ 1q�$��23(�$	��110��$��45����  �� SelectedIndex$ 0��$��Center=�$��Center=�$��23(�#��[ $��45��$	��180��	 �� *
PageSize+.@���25����50����100���� $��6 0 0 0q�.+��� -	     Style.Button.Up#��[ $��46��$��46���� -
     Style.Button.Down#��[ $��0 6 0 0q�$��46��$��46���� $��6 0 0 0q�.+��� -   $��65��$��46������  �� ImageSource$2 ,/ECS.Viewer;component/Images/Icon_Search.png������ "
 ��.���  � �� ��  �� 
TargetType" ��.R���  ��Name$ border��  Brush.DarkGray#��[ $��1q�#��z��._��� $��Center=�$��Center=��� -   $��0 6 0 0q�$��65��$��46������ $ /Images/Excel.png��$ Uniform=����� "
 ��.���  � �� �� " ��.R��� $ border��#��[ $��1q�#��z��._��� $��Center=�$��Center=��� $
��7,10q�$��1��$��0q�#��[ ._�&   System.Windows.Controls.DataGrid    Style.DataGrid.Common#��[ $��20(�"   VerticalScrollBarVisibility$
 Auto=�!   EnableColumnVirtualization .    EnableRowVirtualization . $
��2560��$
��1600������ *OrderSearchList+ �� IsAsync .  � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell��%�   '        '  '     �� "
  .���  � ��$ Center=��� "
  .���  � �� �� "  .R�� #��z��.+��� $��Center=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn    Width5  /System.Windows.Controls.DataGridLengthConverter$  82 !   Header$! No��1  +System.Windows.Controls.DataGridBoundColumn"   Binding" �� *NO+  $	  105 $! 창고 코드��" �� *WH_ID+  $	  120 $! 웨이브 번호��" �� *	WAVE_NO+  $	  110 $! 고객 코드��" �� *CST_CD+  $	  210 $! 고객 주문번호��" �� *
CST_ORD_NO+  $	  160 $! 박스ID��" �� *BOX_ID+  $	  170 $! 송장번호��" �� *
INVOICE_ID+  $	  100 $! 취소여부��" �� *ORDER_CANCEL+  $	  100 $! 박스유형��" �� *BOX_TYPE_CD+  $	  120 $! 중량 기준 값��" �� *
WEIGHT_SUM+  $	  190 $! 주문 일시��" �� *
ORDER_TIME+  $	  190 $! 출고 일시��" �� *
OUT_TIME+