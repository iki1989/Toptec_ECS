   M S B A M L   `   `   `  ���� D  @ECS.Touch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ECS.Touch.ViewsECS.Touch.Views  C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.CoreECS.Core.ViewModels.Touch )    #ECS.Touch.Views.InvoiceRejectScreen   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:ECS.Touch.Views     Hvm9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.Core     -   $��control��  N� Title$  InvoiceRejectScreen�� � 	Resources  �� 
TargetType   Style.TextBlock.Font �� BasedOn  Style.TabItem.CustomHead=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value:  4System.Windows.Controls.Primitives.DatePickerTextBox  Stype.DatePicker.Calender&   System.Windows.Controls.Calendar��%�   '��       7   '��      7  '       '  E     &	 q     �� " ��8   �� " ��8   �� "  .���  � ��$ Center=��� "  .���  �@��
IsReadOnly . �� "   � DataContext 6  0ECS.Core.ViewModels.Touch.InvoiceRejectViewModel  -   	ViewModel��.���  � RowDefinitions �� $��70 ��� $��* �.+�� ����  �� ImageSource$! /Images/MainBackground1.png��.+�(� $
��Left=�(� $��Right=��� $��2��$��0,0,0,0q����	#FFBEC0C2$��0,0,0,0q����	#FFFAFAF9 	 �� RelativeSource��%�   '��       �� " ��.���  �@��
LineHeight �� *
FontSize+	 �� *Self+��  �@��TextAlignment$ Center=���  � ��$
 Wrap=���  �@��LineStackingStrategy$ BlockLineHeight=���  �@��Padding$ 5,0,5,0q�.@��� $��-1.75,0,1.75,0q�$��68������ $��20(�$��송장 재발행��.��� $	��996��$
��1920�� �� $	��15* ��� $��* ��� $��7* �
 � ColumnDefinitions
 �� $��2* ��� $��5* �.+��� $��10q�$��0��$��0��$��2q�  Brush.LightGray#��[ ._��� .+��� $	��0 5q�.+���  �� Orientation$ 
Horizontal=�$	��7 5q�.+��� $��20(�$��	박스 ID��$	��100�� �� TextAlignment$
 Left=�$��Center=�  Brush.DarkGray#��[ �� $��48��$��Center=�._��� $��20(�$��3q�$	��280��$��Center=����� *SearchedInvoice.BoxId+�� $��20(�$��피킹 완료��$��Center=�
���   ���   Image.LedOn#��[ $��20��$��Uniform=�$��5q����� " ��.���  � ��$ /Images/LedOff.png�� �� Triggers ��  �� Binding �� *SearchedInvoice.IsPicking+ �� ElementName$ 	ViewModel�� �� Value True.a���  � ��# [ �� $ 
Horizontal=�$	��7 5q�.+��� $��20(�$��송장 번호��$	��100��$
 Left=�$��Center=�#��[ �� $��20(�$��3q�$	��280��$��Center=����� *SearchedInvoice.InvoiceId+�� $��20(�$��주문 취소��$��Center=�
���   ��� #��[ $��20��$��Uniform=�$��5q����� " ��.���  � ��$ /Images/LedOff.png�� ��  �� *SearchedInvoice.IsCanceled+$ 	ViewModel�� True.a���  � ��# [ �� $��5 50 5 0q�$��2q�#��[ ._��� .+��� $	��0 5q�$��송장 발행��$
 Left=�
��� ���$��20(��� .+��� $ 
Horizontal=�$	��0 5q�.+��� $��20(�$��라인��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$��281.333������ *BcrInfo.Line+�� $ 
Horizontal=�$	��0 5q�.+��� $��20(�$��발행 일시��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$��278.667������ *BcrInfo.PrintedAt+ �� StringFormat$ yy-MM-dd HH:mm:ss���� $ 
Horizontal=�$	��0 5q�.+��� $��20(�$��발행 결과��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=����	#FFC3DCEA._��� $��20(�$��3q�$	��280������ *BcrInfo.PrintResult+�� $��20(�$��발행 여부��$��Center=�
���   ��� $��20��$��Uniform=�$��5q� �� ���� " ��.���  � ��$ /Images/LedOff.png�� ��  �� *BcrInfo.PrintResult+$ 	ViewModel��
 정상.a���  � ��# [ �� $��93���� $��5q�$��2q�#��[ ._��� .+��� $	��0 5q�$��송장 검증��$
 Left=�
��� ���$��20(��� .+��� $ 
Horizontal=�$	��0 5q�.+��� $��20(�$��검증 일시��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$	��390������ *BcrInfo.VerificatedAt+$ yy-MM-dd HH-mm:ss���� $ 
Horizontal=�$	��0 5q�.+��� $��20(�$��검증결과��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$	��390������ *BcrInfo.Verification+�� $��93���� $��10q�$��0��$��1��$��2q�#��[ ._�]� $��0��.+��� $	��Top=�.+��� $ 
Horizontal=�$��10q�.+��� $
��10 0q�.+��� $��	박스 ID��$��20(�$
 Left=�#��[ $��0,20,0,0q��� $��2q�$��50������ $ /Images/tab_underline.png�� j� Stretch$ UniformToFill=�._��� -    ��Name$ BoxIdTextBox�� �� CharacterCasing$ Upper=� �� 	MaxLength$ 10��  Style.TextBox.NoneBorder#��[  �� TextAlignment$ Center=�$��Center=�$��30(�$	��606��$
��0,-2q����	#FF004A74$��50������ *BoxId+��Z� �� $
��10 0q�$��Right=�.+��� $�� ��$��25(�$
 Left=��� $ 
Horizontal=�$��0,20,0,0q�.+��� -     Style.Button.Search#��[ $��검색��$��25(�$	��200��$��50������ $# /Images/btn_search_200x50.png��$ Uniform=��� -     Style.Button.Common#��[ $��	재발행��$��25(�$	��200��$��50��$��20,0,0,0q��� $��Right=�$��Center=�$
��Left=�.+��� -    	 Style.Button.Up#��[	 $��10q��� -    
 Style.Button.Down#��[
 $��10q�&   System.Windows.Controls.DataGrid    Style.DataGrid.Common#��[ $��35(�$��20,5,0,5q����� *InvoiceReprintList+   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 박스ID��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 250 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *BoxId+  $ 송장번호��$	 280  �� *	InvoiceId+  $ 리젝트 사유��$	 300  �� *Verification+  $ 재발행 일시��$	 435  �� *ReprintedAt+�� $��1��$��1��$��묶음 배송��$��25(�$
 Left=�$��Bottom=��� $��10q�$��2��$��0��$��2q�#��[ ._��� .+��� $	��7 5q�$��송장 검증 완료��$
 Left=�
��� ���$��20(��� .+��� $ 
Horizontal=�$	��7 5q�.+��� $��20(�$��	박스 ID��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$	��390������ *VerifiedInvoice.BoxId+�� $ 
Horizontal=�$	��7 5q�.+��� $��20(�$��송장 번호��$	��100��$
 Left=�$��Center=�#��[ �� $��2q�$��Center=�._��� $��20(�$��3q�$	��390������ *VerifiedInvoice.InvoiceId+�� $��93���� $��10q�$��2��$��1��$��2q�#��[ ._�]� .+��� $��Right=�$��Center=�$
��Left=�.+��� -   #��[	 $��10q��� -	   #��[
 $��10q�  #��[ $��30(�$��
20,20,0,15q����� *SameOrderInvoiceList+   $ 박스ID��$	 250  �� *BoxId+  $ 송장번호��$	 280  �� *	InvoiceId+  $ 오더번호��$	 335  �� *
CstOrdNo+  $ 피킹완료��$	 200  �� *	IsPicking+  $ 주문취소��$	 200  �� *
IsCanceled+�� $ 
Horizontal=�$��0��$��50��$��0,0,10,0q�$��Bottom=�$��Right=�$
��1770������ $3 -/ECS.Touch;component/Images/tab_underline.png��.+��� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$	��ECS��$��Center=�$��	940,0,0,0q��� $��20��$��Uniform=�$��5q����� " ��.���  � ��# [  ��  �� *ServerConnection+$ 	ViewModel��	 False.a���  � ��  Image.LedOff# [ �� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$��	프린터��$��Center=��� $��20��$��Uniform=�$��5q����� " ��.���  � ��# [  ��  �� *PrinterConnection+$ 	ViewModel��	 False.a���  � ��# [ � 
������ $��2��$y�Hidden=�.+��� #��[ $	��600�����	#FF777777$��1q�$��660,380,660,356q�._�]� $��0��
�������.+��� $	��Top=�#��[ $��56��$	��Top=�._�]� .+��� $
��Wrap=�$��수동 검증��$
��Left=�$��Center=�$��20,0,0,0q�$��20(�  Font.Common#��[ �� -
     Style.Button.Close#��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� � 
 �� $	��73* ��� $
��226* �.+��� $��25,13,25,38q�$��Center=�$��2��.+��� $��박스 아이디��$��20(�$
 Left=�#��[ $��0,20,0,0q�� 
 �� $��* ��� $	��130 �.+��� $��2q�$��0������ $ /Images/tab_underline.png��$ UniformToFill=�._��� -   $ ManualVerificationBoxId��$ 10��$ Upper=�#��[ $ Center=�$��Center=�$��30(�#��[ $
��0,-2q����	#FF004A74���� *VerificationBoxId+��Z� �� $��송장 번호��$��20(�$
 Left=�#��[ $��0,20,0,0q�� 
 �� $��* ��� $	��130 �.+��� $��2q�$��0������ $ /Images/tab_underline.png��$ UniformToFill=�._��� -   $! ManualVerificationInvoiceId��$ 12��#��[ $ Center=�$��Center=�$��30(�#��[ $
��0,-2q����	#FF004A74���� *VerificationInvoiceId+��Z� �� -   $��1��#��[ $��40��$��10,0,0,0q�$	��Top=�.���� $��20(�$ Center=�$��Center=�$��검증��$��3q�$	��100��� 
������ $��2��y��� *ShowErrorMessage+   BooleanToVisibilityConverter �� 	Converter# [  �� FallbackValue$ hidden��.+��� #��[ $	��800��$	��400�����	#FF777777$��1q�._�]� $��0��#��[ .+��� $	��Top=�#��[ $��56��$	��Top=�._�]� .+��� $
��Wrap=�$��알림��$
��Left=�$��Center=�$��20,0,0,0q�$��20(�#��[ �� -   #��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� �� $	��Top=�$ Center=�$��	0 122 0 0q�$��40(����� *	TextBrush+���� *ErrorMessage+