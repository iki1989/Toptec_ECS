   M S B A M L   `   `   `  ���� D  @ECS.Touch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ECS.Touch.ViewsECS.Touch.Views  C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.CoreECS.Core.ViewModels.Touch (    "ECS.Touch.Views.SmartPackingScreen   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:ECS.Touch.Views     Hvm9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.Core       N� Title$  SmartPackingScreen�� � 	Resources  �� 
TargetType   Style.TextBlock.Font �� BasedOn  Style.TabItem.CustomHead=  7System.Windows.Controls.Primitives.DataGridColumnHeader �� Property �� Value:  4System.Windows.Controls.Primitives.DatePickerTextBox��%�   '��       7   '��      7  '       '  E     �� " ��8   �� " ��8   �� "  .���  � ��$ Center=��� "  .���  �@��
IsReadOnly .  � DataContext 5  /ECS.Core.ViewModels.Touch.SmartPackingViewModel  -   	ViewModel��.���  � RowDefinitions �� $��70 ��� $��* �.+�� ����  �� ImageSource$! /Images/MainBackground1.png��.+�(� $
��Left=�(� $��Right=��� $��2��$��0,0,0,0q�  Brush.LightGray#��[ $��0q�  Brush.White#��[  	 �� RelativeSource��%�   '��       �� " ��.���  �@��
LineHeight �� *
FontSize+	 �� *Self+��  �@��TextAlignment$ Center=���  � ��$
 Wrap=���  �@��LineStackingStrategy$ BlockLineHeight=���  �@��Padding$ 5,0,5,0q�.@��� $	��180��$��0q�$��68����Z� ���� $��20(�$
��Left=�
 �� TextAlignment$

 Left=� � UseLayoutRounding .  �� Padding$	 5,0q��� $�친환경완충재��.��� $	��996��$
��1920�� �� $��* ��� $��6* �.+��� $��2��.+���  �� Orientation$ 
Horizontal=�$��0,0,0,0q�.+��� $��20,0,8,0q�.+��� $��박스ID��$��20(�$

 Left=�$��0,30,0,0q�  Brush.DarkGray#��[ $��25���� $��2q�$��70������ $ /Images/tab_underline.png�� j� Stretch$ UniformToFill=�._��� -    ��Name$ BoxId�� �� CharacterCasing$ Upper=�  Style.TextBox.NoneBorder#��[  �� TextAlignment$ Center=�$��Center=�$��40(�$��0,10,0,0q�$
��Left=����	#FF004A74$	��296������ *BoxId+��Z� �� $��	10 0 10 0q�.+��� $�� ��$��25(�$

 Left=��� $ 
Horizontal=�$��27,0,0,0q�.+��� -     Style.Button.Search#��[ $��검색��$��30(�$	��200��$��50��$��0,40,0,0q�
���   ��� -     Style.Button.LongRefresh#��[ $��   새로고침��$��30(�$	��200��$��50��$��	20,40,0,0q�
���   ���Z� �� -     Style.Button.Common#��[ $��수동검증��$��30(�$	��200��$��50��$��	20,40,0,0q�
���   ���Z� �� $��10q�$��1��$��2q�#��[ ._�]� .+��� $��Right=�$��Center=�$
��Left=�.+��� -    	 Style.Button.Up#��[	 $��10q��� -    
 Style.Button.Down#��[
 $��10q�&   System.Windows.Controls.DataGrid    Style.DataGrid.Common#��[ $��25(�$��10,10,10,10q�   SelectionUnit$ FullRow=�   SelectionMode$ Single=� �� SelectedItem �� *SelectedData+���� *SmartPackingList+   RowStyle �� )  #System.Windows.Controls.DataGridRow"  .���  � ��  Brush.CjYellow# [  �� Triggers ��  �� Binding �� *RESULT+ �� Value
 정상.a���  � �� Z� ��  �� *RESULT+ 강제진행.a���  � �� Z�    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 박스타입��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *
BOX_TYPE+  $ 박스ID��$	 200  �� *BOX_ID+  $ 투입 일시��$	 300  �� *INSERT_TIME+ �� StringFormat$ yyyy-MM-dd HH:mm:ss��  $ 리젝트 사유��$	 400  �� *RESULT+  $ 체적 볼륨    (%)��$	 150  �� *VOLUME+  $ 체적 높이   (mm)��$	 150  �� *HEIGHT+4
  .System.Windows.Controls.DataGridTemplateColumn
  $ 수동처리��$ 60  
  CellTemplate �� .R���  ��Name$ image��$��40��$��Uniform=���Z�   �� Triggers  �� $
 True�� �� *	IS_MANUAL+.a��� ! �� 
TargetName$! image��  Image.LedOff# [  � ��
  $ 강제진행��$ 60  �� .R��� $ image��$��40��$��Uniform=���Z�   �� $ 강제진행�� �� *RESULT+.a��� $! image��# [  � ��  $ 완충재 수량��$ 80  �� *PACKING_AMOUNT+  $ 처리 일시��$	 300  �� *
OUT_TIME+$ yyyy-MM-dd HH:mm:ss���� $ 
Horizontal=�$��0��$��49��$��Bottom=�$	��994��$��Right=����� $ /Images/tab_underline.png��.+��� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$	��ECS��$��Center=�$��0,0,0,8q��� $��20��$��Uniform=�$��5,5,5,13q����� " ��.���  � ��  Image.LedOn# [  ��  �� *ServerConnection+" �� ElementName$" 	ViewModel��	 False.a���  � ��# [ �� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$��	충진PLC��$��Center=�$��0,0,0,8q��� $��20��$��Uniform=�$��5,5,5,13q����� " ��.���  � ��# [  ��  �� *PlcConnection+$" 	ViewModel��	 False.a���  � ��# [ �� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$	��BCR��$��Center=�$��0,0,0,8q��� $��20��$��Uniform=�$��5,5,5,13q����� " ��.���  � ��# [  ��  �� *BcrConnection+$" 	ViewModel��	 False.a���  � ��# [ � 
������ $��2��y��� *ShowManualPopup+   BooleanToVisibilityConverter# �� 	Converter## [ $ �� FallbackValue$$ hidden��.+��� #��[ $	��600�����	#FF777777$��1q�$��660,328,660,112q�._�]� $��0��
�������.+��� $	��Top=�#��[ $��56��$	��Top=�._�]� .+��� $
��Wrap=�$��수동 처리��$
��Left=�$��Center=�$��20,0,0,0q�$��20(�  Font.Common#��[ �� -     Style.Button.Close#��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� � % � ColumnDefinitions% �� $	��73* ��� $
��226* �.+��� $��25,13,25,38q�$��Center=�$��2��.+��� $��박스 아이디��$��20(�$

 Left=�#��[ $��0,20,0,0q�� .+��� $��2q�$��2������ $ /Images/tab_underline.png��$ UniformToFill=�._��� #��[  $
 Center=�$��Center=�$��30(�#��[ $
��0,-2q����	#FF004A74���� *SelectedData.BOX_ID+��Z� �� $��투입 일시��$��20(�$

 Left=�#��[ $��0,20,0,0q�� .+��� $��2q�$��2������ $ /Images/tab_underline.png��$ UniformToFill=�._��� #��[  $
 Center=�$��Center=�$��30(�#��[ $
��0,-2q����	#FF004A74���� *SelectedData.INSERT_TIME+$ yyyy-MM-dd HH:mm:ss����Z� �� $��리젝 사유��$��20(�$

 Left=�#��[ $��0,20,0,0q�� .+��� $��2q�$��2������ $ /Images/tab_underline.png��$ UniformToFill=�._��� #��[  $
 Center=�$��Center=�$��30(�#��[ $
��0,-2q����	#FF004A74���� *SelectedData.RESULT+��Z� �� $	��150��� $��0,50,0,0q�% �� $��* ��� $��* �.+��� -	   $��0��#��[ $��50��$��0,0,30,0q�$	��Top=�.���� $��20(�$
 Center=�$��Center=�$��수동 처리��$��3q�$	��100���� -
   $��1��#��[ $��50��$��0,0,30,0q�$	��Top=�.���� $��20(�$
 Center=�$��Center=�$��	재투입��$��3q�$	��100��� 
������ $��2��y��� *ShowErrorMessage+## [ $$ hidden��.+��� #��[ $	��800��$	��400�����	#FF777777$��1q�._�]� $��0��#��[ .+��� $	��Top=�#��[ $��56��$	��Top=�._�]� .+��� $
��Wrap=�$��알림��$
��Left=�$��Center=�$��20,0,0,0q�$��20(�#��[ �� -   #��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� �� $	��Top=�$
 Center=�$��	0 122 0 0q�$��40(����� *ErrorMessage+