   M S B A M L   `   `   `  ���� D  @ECS.Touch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ECS.Touch.ViewsECS.Touch.Views  C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.CoreECS.Core.ViewModels.Touch +    %ECS.Touch.Views.WeightInspectorScreen   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:ECS.Touch.Views     Hvm9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.Core       N� Title$  WeightInspectorScreen�� � 	Resources  �� 
TargetType   Style.TextBlock.Font �� BasedOn  Style.TextBox.NoneBorder  Style.TabItem.CustomHead:  4System.Windows.Controls.Primitives.DatePickerTextBox �� Property �� Value  Stype.DatePicker.Calender&   System.Windows.Controls.Calendar��%�   '��       7   '��      7  '��      7  '  -     &	 Y     �� " ��8   �� " ��8   �� " ��8   �� "  .���  �@��
IsReadOnly . �� "   � DataContext 8  2ECS.Core.ViewModels.Touch.WeightInspectorViewModel  -   	ViewModel��.���  � RowDefinitions �� $��70 ��� $��* �.+�� ����  �� ImageSource$! /Images/MainBackground1.png���� $��2��$��0,-2,0,0q�$��2q�  Brush.White#��[ ��Z�  	 �� RelativeSource��%�   '��       �� " ��.���  �@��
LineHeight �� *
FontSize+	 �� *Self+��  �@��TextAlignment$ Center=���  � ��$
 Wrap=���  �@��LineStackingStrategy$ BlockLineHeight=���  �@��Padding$ 5,0,5,0q�.@��� $��0,2,0,-2q�$��68������ $��20(�$��중량검수��.��� $	��996��$
��1920�� �� $��4* ��� $��5* �.+��� $��0��$��10q�.+��� $��20(�$��	박스 ID��$	��100��
 �� TextAlignment$

 Left=�$
��Left=�  Brush.DarkGray#��[ ��  �� Orientation$ 
Horizontal=�.+��� $��2q�$��Center=�$	��190�����	#FFE2F0F8._��� $	��180(�$
 Center=�$��Center=�$
��1600��$
��Bold"�$	��175��$��0,8,0,3q����	#FF004A74���� *LastWeightCheckData.BoxId+�� -     Style.Button.RectRefresh#��[ $��40 0 0 0q�$��2q�$��Center=�$	��190����Z� ���� $% /Images/btn_reflash_190x190.png�� j� Stretch$ Uniform=�.���� $��40(�$
 Center=�$��Center=�$��새로고침��$��3,80,3,3q�$	��190���� $��2q�$
��Left=�$	��195��$
��1600��$��0 15 0 0q�._��� $ 
Horizontal=�$��Center=�$��Center=�$��3q�.+��� $	��160(�$
 Center=����� * LastWeightCheckData.MeasureWht+�� $	��160(�$
 Center=�$��kg���� $��10q�$��1��$��2q�  Brush.LightGray#���  ._�]� $��0��.+��� $��Right=�$��Center=�$
��Left=�.+��� -     Style.Button.Up#��[ $��10q��� -    	 Style.Button.Down#��[	 $��10q�&   System.Windows.Controls.DataGrid   
 Style.DataGrid.Common#��[
 $��28(�$��
20,18,0,15q����� *
WeightList+   RowStyle �� )  #System.Windows.Controls.DataGridRow"  .���  � ��  Brush.CjYellow# [  �� Triggers ��  �� Binding �� *Verification+ �� Value
 정상.a���  � �� Z�    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	박스 ID��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding �� *BoxId+  $ 박스 타입��$	 150  �� *	BoxType+  $ 투입 일시��$	 350  �� *CaseErectedAt+  $ 	하한값��$	 110  �� *	MinWeight+  $ 	기준값��$	 110  �� *StandardWht+  $ 	측정값��$	 110  �� *
MeasureWht+  $ 	상한값��$	 110  �� *	MaxWeight+  $ 측정 일시��$	 350  �� *	CheckedAt+  $ 결과��$	 153  �� *Verification+4
  .System.Windows.Controls.DataGridTemplateColumn
  $ 검증결과��$	 171  
  CellTemplate �� .R���  ��Name$ image��  Image.LedOff#��[ $��35�� �� Triggers �� $ 정상�� �� *Verification+.a���  �� 
TargetName$ image�� Z�  � ���� $��0,2,0,-2q�$��68������ $��20(�$��중량검수 검색��.��� $	��996��$
��1920�� � ColumnDefinitions �� $	��77* ��� $��1843* � �� $	��10* ��� $��5* �.+��� $��0��$��10q�$��2��.+��� $��20(�$��	박스 ID��$

 Left=�$
��Left=�#��[ �� $ 
Horizontal=�.+��� $��Center=�$��2q�$	��190������ $ /Images/tab_underline.png��$ Uniform=� �� RelativeTransform V� .~���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q���  �� CenterY$	 0.5Q�  �� CenterX$	  0.5Q�T� ! T� Y$
! 0.25Q�._��� -   " ��Name$" BoxId��# �� CharacterCasing$# Upper=�#��[ $	��180(�$ �� TextAlignment$$ Center=�
������ $��Center=�$��	0,-51,0,0q�$
��1600��$	��237�����	#FF004A74���� *BoxId+�� -     Style.Button.RectSearch#��[ $��40 0 0 0q�$��Center=�$	��190��.���� $��40(�$
 Center=�$��Center=�$��검색��$��3,80,3,3q�$	��190���� $��20(�$��고객주문번호��$

 Left=�$
��Left=�$��	 0 10 0 0q�#��[ �� $��2q�$
��Left=�$	��195��$��0 0 0 0q�._��� -   $" CstOrdNo��$# Upper=�#��[ $	��160(�$$ Center=�
������ $��Center=�$��	0,-51,0,0q�$
��1600��$	��237�����	#FF004A74���� *
CstOrdNo+�� $��2q�$
��Left=�$	��195��$��0 10 0 0q�$
��1600��._��� $ 
Horizontal=�$��Center=�$��Center=�$��3q�.+��� $	��160(�$
 Center=����� *SelectedBox.MeasureWht+�� $	��160(�$
 Center=�$��kg���� $��10q�$��1��$��2q�#���  $��2��._�]� $��0��.+��� $��Right=�$��Center=�$
��Left=�.+��� #��[ $��10q��� #��[	 $��10q�  #��[
 $��28(�$��
20,18,0,15q����� *SearchDisplayList+   $ 	박스 ID��$	 200  �� *BoxId+  $ 박스 타입��$	 150  �� *	BoxType+  $ 투입 일시��$	 350  �� *CaseErectedAt+  $ 	하한값��$	 110  �� *	MinWeight+  $ 	기준값��$	 110  �� *StandardWht+  $ 	측정값��$	 110  �� *
MeasureWht+  $ 	상한값��$	 110  �� *	MaxWeight+  $ 측정 일시��$	 350  �� *	CheckedAt+  $ 결과��$	 153  �� *Verification+
  $ 검증결과��$	 171  �� .R��� $ image��#��[ $��35�� �� $ 정상�� �� *Verification+.a��� $ image�� Z�  � ���� $ 
Horizontal=�$��0��$��50��$��282.5,0,662.74,0q�$��Bottom=����� $ /Images/tab_underline.png��.+��� $ 
Horizontal=�$
��auto��$��
800,0,10,0q�.+��� $
��Bold"�$��20(�$	��ECS��$��Center=��� $��20��$��Uniform=�$��5q����� " ��.���  � ��  Image.LedOn# [  ��  �� *ServerConnection+% �� ElementName$% 	ViewModel��	 False.a���  � ��# [ �� $ 
Horizontal=�$
��auto��$��0,0,10,0q�.+��� $
��Bold"�$��20(�$��	스캐너��$��Center=��� $��20��$��Uniform=�$��5q����� " ��.���  � ��# [  ��  �� *BcrConnection+$% 	ViewModel��	 False.a���  � ��# [ � 
������ $��2��y��� *	ShowPopup+   BooleanToVisibilityConverter& �� 	Converter#& [ ' �� FallbackValue$' Hidden��.+��� #��[ $
��1850��$	��900�����	#FF777777$��1q�._�]� $��0��
�������$��898.3��.+��� $	��Top=�$��76������ $5 //ECS.Touch;component/Images/MainBackground1.png��._��� $ 
Horizontal=�.+��� -   ( ��Name$( 
textBlock1��$
��Wrap=�$��중량검수 검색��$
��Left=�$	��Top=�$��	20,23,0,0q�$��20(�$7��1/ECS.Touch;component/Font/#Noto Sans CJK KR Black)�$
��1772���� -	     Style.Button.Close#��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� �� ���	#FFBEC0C2$��1q�$��50q�._��� $��2��.+�  -
   #��[
 $��25(�$��20q�$
��Left=�$	��682��$
��1605��)   SelectionMode$) Single=�*   SelectionUnit$* FullRow=����� *WeightSearchList+   $ 	박스 ID��$	 180  �� *BoxId+  $ 박스 타입��$	 150  �� *	BoxType+  $ 투입 일시��$	 280  �� *CaseErectedAt+  $ 	하한값��$	 110  �� *	MinWeight+  $ 	기준값��$	 110  �� *StandardWht+  $ 	측정값��$	 110  �� *
MeasureWht+  $ 	상한값��$	 110  �� *	MaxWeight+  $ 측정 일시��$	 280  �� *	CheckedAt+  $ 결과��$	 103  �� *Verification+
  $ 검증결과��$	 171  �� .R��� $ image��#��[ $��35�� �� $ 정상�� �� *Verification+.a��� $ image�� Z�  � ���� $��80��$��Right=�$��0,-652,0,20q�$	��682��.+��� -   #��[ $��10q�$	��260��$��60���� -   #��[	 $��10q�$	��260��$��60��� 
������ $��2��y��� *ShowErrorMessage+#& [ $' hidden��.+��� #��[ $	��800��$	��400�����	#FF777777$��1q�._�]� $��0��#��[ .+��� $	��Top=�#��[ $��56��$	��Top=�._�]� .+��� $
��Wrap=�$��알림��$
��Left=�$��Center=�$��20,0,0,0q�$��20(�  Font.Common#��[ �� -   #��[ $��0,0,20,0q�$��36��$��36��$��Right=�$��Center=���Z� �� $	��Top=�$
 Center=�$��	0 122 0 0q�$��40(����� *ErrorMessage+