   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  E AECS.Viewer, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:ECS.Viewer.Views.PagesECS.Viewer.Views.Pages C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.CoreECS.Core.ViewModels.Viewer 1   +ECS.Viewer.Views.Pages.TrackingSearchScreen   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6sys&clr-namespace:System;assembly=mscorlib     6local$clr-namespace:ECS.Viewer.Views.Pages     Ivm:clr-namespace:ECS.Core.ViewModels.Viewer;assembly=ECS.Core       N� Title$  TrackingSearchScreen��   ViewModel.Tracking � DataContext# [  .��� 
������� � RowDefinitions �� $	��120 ��� $��* �.+��� $��0��$��7q����	#FFF2F2F2._�]� .+���  �� Orientation$ 
Horizontal=�$��5 4 5 4q�$��Center=�$
��Left=�.+��� .+��� $ 
Horizontal=�.+��� $��기간 :��$��Right=�$��Center=� �� TextAlignment$ Right=� �� Padding$ 4q�$��23(�$��70��$��45��(  "System.Windows.Controls.DatePicker  -   $��Begin��  DatePickerIconResizingStyle#��[  J� RenderTransformOrigin$ 0.5,1&�$��0 0 0 0q�$��23(�$	��200��$��45��   SelectedDate �� *Begin+�� $��~ ��$��Center=�$��Center=�$ Center=�$ 2q�$��25(�$��70��$��45��  -   $	��End��#��[ $ 0.5,1&�$��0 0 0 0q�$��23(�$	��200��$��45�� �� *End+�� -   $��Center=�$��Center=�$��10 0 0 0q�$��23(�$	��180��$��45������ *BoxId+.������  �� 
TargetType" ��.��� 	 �� Property	 � ��  Font.Common
 �� Value#
 [  �� 	Resources   CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch !� Visual  Brush.LightGray��%e   &	       !� $
 Left=�$ Center=�$
 None=� �� $��박스ID��#���  $
��Bold"�$��23(� �� Triggers P�  P� Property � �� P� Value$  ��. ��� 	 � ��#
 [ P�  �@J�IsKeyboardFocused . . ��� 	 � ��

 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��23(�$	��180��$��45������ *	InvoiceId+.������ " ��.��� 	 � ��#
 [  ��%i   &	       !� $
 Left=�$ Center=�$
 None=� �� $��송장번호��#���  $
��Bold"�$��23(� P�  � ��$  ��. ��� 	 � ��#
 [ P�  �@J�IsKeyboardFocused . . ��� 	 � ��

 �����.���� -   $��Center=�$��Center=�$��10 0 0 0q�$��23(�$	��250��$��45������ *
CstOrdNo+.������ " ��.��� 	 � ��#
 [  ��%p   &	       !� $
 Left=�$ Center=�$
 None=� �� $��고객 주문번호��#���  $
��Bold"�$��23(� P�  � ��$  ��. ��� 	 � ��#
 [ P�  �@J�IsKeyboardFocused . . ��� 	 � ��

 �����.���� $ 
Horizontal=�$��0 6 0 0q�.+��� $ 
Horizontal=�.+��� $��주문 취소 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$	��125��$��45���� $��Center=�$��Center=�$��23(�#��[ $	��150��$��45�� �� SelectedItem �� *CancelFilter+.@���
전체����
정상����주문 취소����	미출고���� $ 
Horizontal=�.+��� $��피킹 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$��70��$��45���� $��Center=�$��Center=�$��23(�#��[ $	��150��$��45�� �� *PickingFilter+.@���
전체����피킹 없음����피킹 완료���� $ 
Horizontal=�.+��� $��중량 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$��70��$��45���� $��Center=�$��Center=�$��23(�#��[ $	��150��$��45�� �� *WeightFilter+.@���
전체����중량 없음����중량 실패����중량 성공���� $ 
Horizontal=�.+��� $��발행 종류 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$	��115��$��45���� $��Center=�$��Center=�$��23(�#��[ $	��150��$��45�� �� *PrintFilter+.@���
전체����발행 없음����
일반����	스마트���� $ 
Horizontal=�.+��� $��상면 검증 :��$��Center=�$��Center=�$ Right=�$ 2q�$��23(�$	��125��$��45���� $��Center=�$��Center=�$��23(�#��[ $	��150��$��45�� �� *	TopFilter+.@���
전체����검증 없음����검증 실패����검증 성공���� $ 
Horizontal=�$��5 4 5 4q�$��Center=�$��Right=�.+��� $ 
Horizontal=�.+��� .+��� $ 
Horizontal=�.+��� $��Page : ��$��Center=�$��Center=�$ Right=�$ 1q�$��23(�$	��110��$��45���� -   $��Center=�$��Center=� �� TextAlignment$ Center=�$��23(�$��45��$��80������ *Page+�� $	�� / ��$��Center=�$��Center=�$ Center=�$ 1q�$��23(�$��20��$��45���� $��Center=�$��Center=�$ Center=�$ 1q�$��23(�$��45��$��80������ *	MaxPage+�� $ 
Horizontal=�$��0 6 0 0q�.+��� $��항목 수 : ��$��Center=�$��Center=�$ Right=�$ 1q�$��23(�$	��110��$��45����  �� SelectedIndex$ 0��$��Center=�$��Center=�$��23(�#��[ $��45��$	��180�� �� *
PageSize+.@���25����50����100���� $��6 0 0 0q�.+��� -     Style.Button.Up#��[ $��46��$��46���� -     Style.Button.Down#��[ $��0 6 0 0q�$��46��$��46���� $��6 0 0 0q�.+��� -	   $��65��$��46������  �� ImageSource$2 ,/ECS.Viewer;component/Images/Icon_Search.png������ " ��.��� 	 � ��
 ��  �� 
TargetType" ��.R���  ��Name$ border��  Brush.DarkGray#��[ $��1q�#��z��._��� $��Center=�$��Center=��� -
   $��0 6 0 0q�$��65��$��46������ $ /Images/Excel.png��$ Uniform=����� " ��.��� 	 � ��
 �� " ��.R��� $ border��#��[ $��1q�#��z��._��� $��Center=�$��Center=��� $
��7,10q�$��1��$��0q�#��[ ._�&   System.Windows.Controls.DataGrid    Style.DataGrid.Common#��[ $��20(�"   VerticalScrollBarVisibility$
 Auto=�$   HorizontalScrollBarVisibility$
 Auto=�!   EnableColumnVirtualization .    EnableRowVirtualization . $
��2560��$
��1600������ *TrackingList+ �� IsAsync .  � 	Resources =  7System.Windows.Controls.Primitives.DataGridColumnHeader*  $System.Windows.Controls.DataGridCell��%�   '        '  '     �� "  .��� 	 � ��$
 Center=��� "  .��� 	 � ��
 �� "  .R�� #��z��.+��� $��Center=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn    Width5  /System.Windows.Controls.DataGridLengthConverter$	  145 !   Header$! 박스ID��1  +System.Windows.Controls.DataGridBoundColumn"   Binding" �� *BOX_ID+  $	  175 $! 송장번호��" �� *
INVOICE_ID+  $	  230 $! 고객 주문번호��" �� *
CST_ORD_NO+  $	  200 $! 주문 수신��" �� *
ORDER_TIME+  $	  200 $! 주문 취소��" �� *ORDER_CANCEL_TIME+  $	  200 $! 피킹 완료��" �� *PICKING_TIME+  $	  200 $! 중량 검수��" �� *WEIGHT_TIME+  $	  200 $! 중량 실패��" �� *WEIGHT_REJECT_TIME+  $	  200 $! 스마트 완충재��" �� *SMART_PACKING_TIME+  $	  200 $ ! 스마트 완충재 실패��" �� *SMART_PACKING_REJECT_TIME+  $	  200 $! 스마트 발행��" �� *SMART_PRINT_TIME+  $	  200 $! 일반 발행��" �� *NORMAL_PRINT_TIME+  $	  200 $! 상면 검증��" �� *
TOP_TIME+  $	  200 $! 상면 실패��" �� *TOP_REJECT_TIME+  $	  200 $! 출고 시간��" �� *
OUT_TIME+