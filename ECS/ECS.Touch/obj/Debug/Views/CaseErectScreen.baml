   M S B A M L   `   `   `  ����D  @ECS.Touch, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ECS.Touch.ViewsECS.Touch.Views  5      C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.CoreECS.Core.ViewModels.Touch 5      %    ECS.Touch.Views.CaseErectScreen   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:ECS.Touch.Views     5      Hvm9clr-namespace:ECS.Core.ViewModels.Touch;assembly=ECS.Core     5        N� Title$  CaseErectScreen��5
       � 	Resources 5       �� 
TargetType   Style.TextBlock.Font �� BasedOn  Style.TabItem.CustomHead:  4System.Windows.Controls.Primitives.DatePickerTextBox �� Property �� Value  Stype.DatePicker.Calender&   System.Windows.Controls.Calendar��%�   '��       7   '�� "     7  '  D     &	 �     �� 5   
   " ��6   8   6'   �� 5   
   " ��6   8   6%   �� 5   
   "  6   .�5      ��  �@��
IsReadOnly . 6+   5      �� 5   
   "  62   5      5       � DataContext 5      2  ,ECS.Core.ViewModels.Touch.CaseErectViewModel  5   
   -   5        	ViewModel��5       .��5      �  � RowDefinitions 5   
   �� 5      $��70 �6   �� 5      $��* �6   .+�5   
   � ��5      �� 5       �� ImageSource$! /Images/MainBackground1.png��6   .+�5       (� $
��Left=�6   (� 5!      $��Right=�6   5"      �� 5$   
   -   5        $��2��5$      $��0,0,0,0q�6&     Brush.LightGray#��[ 67   $��0q�6f     Brush.White#��[ 6z    5%      	 �� RelativeSource��%O  '��       �� 5&      " ��6   .�5'      ��  �@��
LineHeight 63   �� *
FontSize+	 �� *Self+�� 5(       �@��TextAlignment$ Center=�66   �� 5)       � ��$
 Wrap=�65   �� 5*       �@��LineStackingStrategy$ BlockLineHeight=�6=   �� 5+       �@��Padding$ 5,0,5,0q�60   5,      5-      .@�5.      �� $	��140��6   $��0q�6"   $��68��6D   ��6-   Z� ��5/      �� 50      $��20(�6    �� 6/   $�투입��63   6@   .��52      � $	��996��6   $
��1920��6$   
 � ColumnDefinitions
 53      �� 54      $��* �6+   �� 55      $��* �6+   .+�57      � $��0��6    58      �� 59      $��* �6,   �� 5:      $��3* �6,   .+�5<      �� $��0��6%   .+�5=      ��  �� Orientation$ 
Horizontal=�6)   $
��10 5q�6B   .+�5>   "   �� $��0q�6)   
���   �$	��600��6Q   $
��Left=�6]   ._�5?   &   �� $��50(�60   $��0q�6>    �� Padding$ 0,10,0,0q�6d   $��Center=�6w   $
��Left=�6�   $	��600��6�   ��6I   Z� ��5@   *   �� 5A   .   " ��64   .�5B   2   ��  �@��TextAlignment$ Center=�6R   �� 5C   2    � ��$ 검증 실패��6I   �� 5D   2    � ��  Brush.CjRed# [ 6O   �� 5E   2    � ��  Brush.Black# [ 6O    �� Triggers 5F   2   �� 5G   6    �� Binding 6B   �� *RecentErectData1.Verification+ �� Value OK6t   .a�5H   :   ��  � ��$ 검증 성공��6Q   �� 5I   :    � ��  Brush.YellowGreen# [ 6a   �� 5J   :    � ��# [ 6W   5K   7   5M   /   5O   '   5P   #   �� 5T   "   $��	40,-5,0,0q�6P   $��Center=�6c   $��16��6~   ���	#FF007DC3 d� 	IsChecked 6+   �� *IgnoreInkLeak1+ � LayoutTransform 5U   &   �� 5V   *    �� ScaleX$ 3Q�69    �� ScaleY$ 3Q�6D   5X   #   �� 5Y   "   $��30(�6-    �� TextAlignment$ Center=�6;   $��Center=�6R   $��잉크 부족 무시��6m   $��3q�6}   $	��200��6�   5Z      �� 5[      $ 
Horizontal=�6)   $
��10 5q�6B   .+�5\   "   �� $��박스 호수��6,   $��30(�69   $��Center=�6G   $	��170��6b   $
 Left=�6n   
���   �6�   �� 5]   "   $	��430��6@   ��6)   Z� ._�5^   &   �� $ 
Horizontal=�61   $��Center=�6J   $��Center=�6g   .+�5_   *   �� $��40(�6^   $ Center=�6m   ��64   �� *RecentErectData1.BoxType+6m   �� 5`   *   $	�� / ��64   $��40(�6?   $ Center=�6N   �� 5a   *   $��40(�6^   $ Center=�6m   ��64   �� *RecentErectData1.BoxName+6m   5b   '   5c   #   �� 5d   "   $��70,0,0,0q�6)   $��2q�6;   $��Center=�6O   $	��200��6j   $��70��6v   ._�5e   &   �� $ 
Horizontal=�61   $��Center=�6J   $��4q�6g   .+�5f   *   �� $��30(�64   $ Center=�6B   $��Center=�6Y   ��6t   �� *	InkRatio1+�� 5g   *   $��30(�64   $ Center=�6B   $��Center=�6Y   $��%��6t   5h   '   5i   #   5j      �� 5k      $ 
Horizontal=�6)   $
��10 5q�6B   .+�5l   "   �� $��	박스 ID��6,   $��30(�69   $��Center=�6G   $	��170��6b   $
 Left=�6n   
���   �6�   �� 5m   "   $��40(�6T   $��Center=�6b   $	��430��6}   $ Center=�6�   ��6,   �� *RecentErectData1.BoxId+6�   �� 5n   "   -   5          Style.Button.Common#��[ 5n   )   $��70��6k   $��70,0,0,0q�6w   $��Center=�6�   .��5o   &   �� $��30(�61   $ Center=�6?   $��Center=�6V   $��	재가동��6q   $��3q�6|   $	��200��6�   5p   #   5q      5r      �� 5s      $��10q�6!   $��1��6-   $��2q�6:   #��[ 6N   ._�5t      ]� .+�5u   "   �� $	��Top=�6-   $ 
Horizontal=�6B   $��20,0,0,0q�6[   $	��916��6n   .+�5v   &   �� $��투입1��60   $8��230                                                (�6;   $��Bottom=�5w   2    	 Brush.CjBlue#��[	 6M   �� 5x   &    
 Style.Button.Refresh#��[
 6-   $��783 10 0 10q�6[   $��40��6p   $��40��6{   #��[	 6�   5y   #   �� 5z   "   $��Right=�6-   $��Center=�6D   $
��Left=�6_   .+�5{   &   �� -   5          Style.Button.Top#��[ 5{   -   $��10q�6r   �� 5|   &   -   5          Style.Button.Up#��[ 5|   -   $��10q�6p   �� 5}   &   -   5          Style.Button.Down#��[ 5}   -   $��10q�6t   �� 5~   &   -   5          Style.Button.Bottom#��[ 5~   -   $��10q�6x   5   #   &   System.Windows.Controls.DataGrid  5�   "     Style.DataGrid.Common#��[ 6+   $��28(�6Z   $��
20,14,0,20q�6h   ��6|   �� *CaseErectList1+   RowStyle 5�   &   �� 5�   *   )  #System.Windows.Controls.DataGridRow"  60   .�5�   .   ��  � ��  Brush.CjYellow# [ 6K    5�   .   �� 5�   2    6>   �� *Verification+ OK6_   .a�5�   6   ��  � �� 6S   Z� 5�   3   5�   +      Columns 5�   &   0  *System.Windows.Controls.DataGridTextColumn  5�   *   ,  &System.Windows.Controls.DataGridColumn   Header$ 박스타입��6=      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6g   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 6K   �� *	BoxType+6g     5�   *   $ 박스ID��6=   $	 200 6f    6K   �� *BoxId+6f     5�   *   $ 투입 일시��6=   $	 354 6�    6L   �� *	ErectedAt+ �� StringFormat$ yy-MM-dd HH:mm:ss��6�   4
  .System.Windows.Controls.DataGridTemplateColumn
  5�   *   $ 검증결과��6A   $	 150 6O    
  CellTemplate 5�   .   �� 5�   2   .R�5�   6   ��  ��Name$ image��6<   $��33��6K    �� Triggers 5�   6   �� 5�   :   $ NOREAD��6g    6F   �� *Verification+.a�5�   >   ��  �� 
TargetName$ image��6E    � ��$ /Images/LedOff.png��6j   5�   ;   �� 5�   :   $ 	DUPLICATE��6g    6F   �� *Verification+.a�5�   >   �� $ image��6E    � ��$ /Images/LedOff.png��6j   5�   ;   5�   3   5�   +   5�   #   5�      5�      5�      � 5�      $��1��6    5�      �� 5�      $��* �6,   �� 5�      $��3* �6,   .+�5�      �� $��0��6%   .+�5�      �� $ 
Horizontal=�6)   $
��10 5q�6B   .+�5�   "   �� $��0q�6)   
���   �$	��600��6Q   $
��Left=�6]   ._�5�   &   �� $��50(�60   $��0q�6>   $ 0,10,0,0q�6d   $��Center=�6w   $
��Left=�6�   $	��600��6�   ��6I   Z� ��5�   *   �� 5�   .   " ��64   .�5�   2   ��  �@��TextAlignment$ Center=�6R   �� 5�   2    � ��$ 검증 실패��6I   �� 5�   2    � ��# [ 6O   �� 5�   2    � ��# [ 6O    5�   2   �� 5�   6    6B   �� *RecentErectData2.Verification+ OK6t   .a�5�   :   ��  � ��$ 검증 성공��6Q   �� 5�   :    � ��# [ 6a   �� 5�   :    � ��# [ 6W   5�   7   5�   /   5�   '   5�   #   �� 5�   "   $��	12,-5,0,0q�6P   $��Center=�6c   $��16��6~   ���	#FF007DC3 6+   �� *IgnoreInkLeak2+ 5�   &   �� 5�   *   $ 3Q�69   $ 3Q�6D   5�   #   �� 5�   "   $��30(�6-   $ Center=�6;   $��Center=�6R   $��잉크 부족 무시��6m   $��3q�6}   $	��190��6�   �� 5�   "   -   5          Style.Button.PopupBtn#��[ 5�   I   $��5,0,25,0q�6x   $��70��6�   $��70��6�   $��0q�6�   ��6�   Z� ��5�   &   �� 5�   *   $  /Images/btn_line_70x70.png��65    j� Stretch$ Uniform=�6^   .��5�   &   �� $��50(�61   $ Center=�6?   $��Center=�6V   $	��※��6q   $��67��6z   $��40��6�   ���	#FF007DC36�   5�   #   5�      �� 5�      $ 
Horizontal=�6)   $
��10 5q�6B   .+�5�   "   �� $��박스 호수��6,   $��30(�69   $��Center=�6G   $	��170��6b   $
 Left=�6n   
���   �6�   �� 5�   "   $	��430��6@   ��6)   Z� ._�5�   &   �� $ 
Horizontal=�61   $��Center=�6J   $��Center=�6g   .+�5�   *   �� $��40(�6^   $ Center=�6m   ��64   �� *RecentErectData2.BoxType+6m   �� 5�   *   $	�� / ��64   $��40(�6?   $ Center=�6N   �� 5�   *   $��40(�6^   $ Center=�6m   ��64   �� *RecentErectData2.BoxName+6m   5�   '   5�   #   �� 5�   "   $��35,0,0,0q�6)   $��2q�6;   $��Center=�6O   $��70��6j   $	��200��6v   ._�5�   &   �� $ 
Horizontal=�61   $��Center=�6J   $��4q�6g   .+�5�   *   �� $��30(�64   $ Center=�6B   $��Center=�6Y   ��6t   �� *	InkRatio2+�� 5�   *   $��30(�64   $ Center=�6B   $��Center=�6Y   $��%��6t   5�   '   5�   #   5�      �� 5�      $ 
Horizontal=�6)   $
��10 5q�6B   .+�5�   "   �� $��	박스 ID��6,   $��30(�69   $��Center=�6G   $	��170��6b   $
 Left=�6n   
���   �6�   �� 5�   "   $��40(�6T   $��Center=�6b   $	��430��6}   $ Center=�6�   ��6,   �� *RecentErectData2.BoxId+6�   �� 5�   "   -	   5        #��[ 5�   )   $��35,0,0,0q�6k   $��Center=�6}   $��70��6�   .��5�   &   �� $��30(�61   $ Center=�6?   $��Center=�6V   $��	재가동��6q   $��3q�6|   $	��200��6�   5�   #   5�      5�      �� 5�      $��10q�6!   $��1��6-   $��1��6:   $��2q�6J   #��[ 6^   ._�5�      ]� .+�5�   "   �� $	��Top=�6-   $ 
Horizontal=�6B   $��20,0,0,0q�6[   $	��916��6n   .+�5�   &   �� $��투입2��60   $8��230                                                (�6;   $��Bottom=�5�   2   #��[	 6M   �� 5�   &   #��[
 6-   $��783 10 0 10q�6[   $��40��6p   $��40��6{   5�   #   �� 5�   "   $��Right=�6-   $��Center=�6D   $
��Left=�6_   .+�5�   &   �� -
   5        #��[ 5�   -   $��10q�6r   �� 5�   &   -   5        #��[ 5�   -   $��10q�6p   �� 5�   &   -   5        #��[ 5�   -   $��10q�6t   �� 5�   &   -   5        #��[ 5�   -   $��10q�6x   5�   #     5�   "   #��[ 6+   $��28(�6Z   $��
20,14,0,20q�6h   ��6|   �� *CaseErectList2+ 5�   &   �� 5�   *   "  60   .�5�   .   ��  � ��# [ 6K    5�   .   �� 5�   2    6>   �� *Verification+ OK6_   .a�5�   6   ��  � �� 6S   Z� 5�   3   5�   +    5�   &     5�   *   $ 박스타입��6=   $	 150 6g    6K   �� *	BoxType+6g     5   *   $ 박스ID��6=   $	 200 6f    6K   �� *BoxId+6f     5  *   $ 투입 일시��6=   $	 354 6�    6L   �� *	ErectedAt+$ yy-MM-dd HH:mm:ss��6�   
  5  *   $ 검증결과��6A   $	 150 6O    5  .   �� 5  2   .R�5  6   �� $ image��6<   $��35��6K    5  6   �� 5  :   $ NOREAD��6g    6F   �� *Verification+.a�5  >   �� $ image��6E    � ��$ /Images/LedOff.png��6j   5	  ;   �� 5
  :   $ 	DUPLICATE��6g    6F   �� *Verification+.a�5  >   �� $ image��6E    � ��$ /Images/LedOff.png��6j   5  ;   5  3   5  +   5  #   5     5     5     5     5     �� 5     $	��140��6   $��0q�6"   $��68��6D   ��6-   Z� ��5     �� 5     $��20(�6    $��Right=�6.   �� 6K   $�	재발행��6O   6]   .��5     � $	��996��6   $
��1920��6$    5     �� 5     $��2* �6(   �� 5     $��8* �6(   .+�5!     �� $��2��6!   .+�5"     �� $��	10 0 10 0q�6%   .+�5#     �� $��20(�6(   $
 Left=�66   $��	-5,30,0,0q�6K     Brush.DarkGray#��[ 6^   $��25��6�   �� 6�   $�생성��6�   �� 6�   $� ��6�   �� 6�   $�일시��6�   6�   �� 5$     $ 
Horizontal=�6)   $��70��6B   .+�5%  "   (  "System.Windows.Controls.DatePicker  -   5        $��CaseErectBegin��5%  -   $��30(�6�       DisplayDate$  
2019-09-13��6�   ! J� RenderTransformOrigin$! 0.5,1&�6�     DatePickerIconResizingStyle#��[ 6�   $	��342��6  $��45��6  $
��0,12q�6&  "   SelectedDate" 6n   �� *SearchBegin+ 5&  &   # �� 
TargetType��%�   '��       �� 5'  *   " ��60   .�5(  .   ��  � �� 5)  2   �� 5*  6   "# ��6F   .R�5+  :   (� .��5,  >   �� 5-  ;   5.  7   50  /   51  +   52  '   53  #   �� 54  "   $��~��6,   $��25(�65   $��Center=�6C     55  "   -   5        $��CaseErectEnd��55  -   $��30(�6�   $  
2019-09-13��6�   $! 0.5,1&�6�   #��[ 6�   $	��342��6
  $��45��6  $
��0,12q�6"  " 6l   �� *	SearchEnd+ 56  &   ��%�   '��       �� 57  *   " ��60   .�58  .   ��  � �� 59  2   �� 5:  6   "# ��6F   .R�5;  :   (� .��5<  >   �� 5=  ;   5>  7   5@  /   5A  +   5B  '   5C  #   5D     5E     �� 5F     $ 
Horizontal=�6%   $��0��6>   $��2��6K   $��70��6_   .+�5G     �� -   5        $��CaseErectBoxType��5G  '   $	��200��6?   $��
10,10,10,0q�6K   $ �� SelectedIndex$$ 0��6_   $��30(�6�   $��Center=�6�   $��Center=�6�   $��50��6�   % �� Text% 6q   �� *SelectedBoxType+��6�   �� *BoxTypeList+6  �� 5H     -   5        & ��Name$& CaseErectCount��5H  >   ' �� CharacterCasing$' Upper=�6&   ( �� 	MaxLength$( 10��6V   
) � Tag$) 박스 아이디��6�     Style.WatermarkedTextBox#��[ 6�   $��30(�6�   $	��281��6�   * �� TextAlignment$* Center=�6�   $��
10,10,10,0q�6�   $��50��6  ��6e   �� *	BoxNumber+��6  Z� .��6r  ��5I  "   �� 5J  &   $ /Images/tab_underline.png��61   $ Uniform=�6Y   + �� RelativeTransform+ 5K  *   V� 5L  .   .~�5M  2   �� , �� CenterY$	, 0.5Q�6A   - �� CenterX$	- 0.5Q�6O   �� 5N  2   . �� CenterY$	. 0.5Q�6@   / �� CenterX$	/ 0.5Q�6N   �� 5O  2   0 �� CenterY$	0 0.5Q�6B   1 �� CenterX$	1 0.5Q�6P   T� 5P  2   2 T� Y$	2 0.3Q�6E   5Q  /   5S  '   .��5T  6   5U     �� 5V     -   5          Style.Button.Search#��[ 5V  %   $��검색��6g   $��30(�6t   $	��200��6�   $��60��6�   $��0,10,0,0q�6�   
���   �6�   �� 5W     $��	20,10,0,0q�6K   $��Center=�6^    6'   �� *DisplayReject+ 5X  "   �� 5Y  &   $ 3Q�65   $ 3Q�6@   5[     �� 5\     $��20(�6(   $	��231��66   $��Center=�6B   $
 Left=�6]   $��미검증만 표시��6r   $��10,29.5,10,10q�6�   
���   �$
��Left=�6�   $
��30.5��6�   �� 5]     $ 
Horizontal=�6)   $��60��6B   .+�5^  "   �� -   5        #��[ 5^  )   $��새로고침��6k   $��30(�6z   $	��200��6�   $
��10 0q�6�   �� 5_  "   -   5        #��[ 5_  )   $��신규 발행��6k   $��30(�6{   $	��200��6�   $
��10 0q�6�   �� 5`  "   -   5        #��[ 5`  )   $��	재발행��6k   $��30(�6y   $	��200��6�   $
��10 0q�6�   �� 5a  "   -   5        #��[ 5a  )   $��수동 검증��6z   $��30(�6�   $	��200��6�   $
��10 0q�6�   5b     5c     5d     �� 5e     $��10q�6   $��1��6)   $��2q�66   #���  6J   ._�5f     ]� .+�5g     �� $��Right=�6)   $��Center=�6@   $
��Left=�6[   .+�5h  "   �� -   5        #��[ 5h  )   $��10q�6s   �� 5i  "   -   5        #��[ 5i  )   $��10q�6q   �� 5j  "   -   5        #��[ 5j  )   $��10q�6u   �� 5k  "   -   5        #��[ 5k  )   $��10q�6y   5l       5m     -   5        3  Name$3 
SearchGrid��5m  '   #��[ 6;   $��
20,20,0,13q�6j   $��43(�6~   ��6�   �� *CaseErectSearchList+ 5n  "     5o  &   $ 호기��69   $	 300 6e    6E   �� *ErectorType+6e     5p  &   $ 	박스 ID��69   $	 318 6c    6H   �� *BoxId+6c     5q  &   $ 박스 타입��69   $	 200 6e    6H   �� *	BoxType+6e     5r  &   $ 출고 일시��69   $	 500 6�    6H   �� *	ErectedAt+$ yy-MM-dd HH:mm:ss��6�     5s  &   $ 인쇄 수량��69   $	 200 6g    6H   �� *
PrintCount+6g   
  5t  &   $ 검증결과��6=   $	 296 6K    5u  *   �� 5v  .   .R�5w  2   �� $ image��68   $��40��6G     Image.LedOn#��[ 6R   $��Uniform=�6x    5x  2   �� 5y  6   $ N��6c    6B   �� *Verification+.a�5z  :   �� $ image��6A    � ��$ /Images/LedOff.png��6f   5{  7   5}  /   5  '   5�     5�     5�     5�     5�     5�     �� 5�  
   $ 
Horizontal=�6   $��0��6/   $��50��6<   $��Bottom=�6H   $
��1638��6c   $��Right=�6p   ��5�     �� 5�     $ /Images/tab_underline.png��6   .+�5�     �� $ 
Horizontal=�6   $
��auto��62   $��
100,0,10,0q�6?   $��Right=�6S   .+�5�     �� $
��Bold"�6   $��20(�6.   $	��ECS��6<   $��Center=�6G   $��0,0,0,8q�6b   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *ServerConnection+4 �� ElementName$4 	ViewModel��	 False6j   .a�5�  &   ��  � ��  Image.LedOff# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��제함기#1��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Erector1Connection+$4 	ViewModel��	 False6l   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��제함기#2��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Erector2Connection+$4 	ViewModel��	 False6l   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��잉크젯#1��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Inkjet1Connection+$4 	ViewModel��	 False6k   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��잉크젯#2��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   $��Right=�6G   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Inkjet2Connection+$4 	ViewModel��	 False6k   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��BCR#1��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Bcr1Connection+$4 	ViewModel��	 False6h   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��BCR#2��6<   $��Center=�6I   $��0,0,0,8q�6d   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   $��Right=�6G   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *Bcr2Connection+$4 	ViewModel��	 False6h   .a�5�  &   ��  � ��# [ 6?   5�  #   5�     5�     5�     �� 5�     $ 
Horizontal=�6   $
��auto��62   $��0,0,10,0q�6?   .+�5�     �� $
��Bold"�6   $��20(�6.   $��	프린터��6<   $��Center=�6G   $��0,0,0,8q�6b   �� 5�     $��20��6   $��Uniform=�6#   $��5,5,5,13q�65   $��Right=�6G   ��5�     �� 5�     " ��6    .�5�     ��  � ��# [ 67    5�     �� 5�  "    6.   �� *PrinterConnection+$4 	ViewModel��	 False6k   .a�5�  &   ��  � ��# [ 6?   5�  #   5      5     5     5     � 5     
������ $��2��6,   y�6=   �� *ShowNumberingPopup+   BooleanToVisibilityConverter5 �� 	Converter#5 [ 6 �� FallbackValue$6 hidden��.+�5     �� #��[ 6   $
��1850��6C   $	��900��6P   ���	#FF777777$��1q�6u   ._�5     ]� $��0��6   
�������.+�5	     �� $	��Top=�6   $��76��62   $	��Top=�6>   ��5
     �� 5     $5 //ECS.Touch;component/Images/MainBackground1.png��6)   ._�5     �� $ 
Horizontal=�6%   .+�5     �� $
��Wrap=�6(   $��채번 설정��6<   $
��Left=�6I   $	��Top=�6d   $��	20,23,0,0q�6|   $��20(�6�   $7��1/ECS.Touch;component/Font/#Noto Sans CJK KR Black)�6�   $
��1772��6�   �� 5     -   5          Style.Button.Close#��[ 5  F   $��0,0,20,0q�6r   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6�   Z� 6�   5     5     ]� 5     $	��Top=�6    $��90��65   .+�5     �� -   5        $��ErectorType��5  #   $	��200��66   $$ 0��6B   $��30(�6T     Font.Common#��[ 6b   $
��Left=�6�   $��Center=�6�   $��20q�6�   .@�5     �� $��1호기��6+   �� 5     $��2호기��6+   5     �� 5     $ 
Horizontal=�6%   $��Right=�6>   $��RightToLeft=�6U   .+�5     �� -   5        #��[ 5  H   $��70��6u   $��10,0,0,0q�6�   $��Bottom=�6�   .��5  "   �� $��30(�6-   $ Center=�6;   $��Center=�6R   $��새로고침��6m   $��3q�6y   $	��200��6�   5     �� 5     -   5        #��[ 5  B   $��70��6o   $��10,0,0,0q�6{   $��Bottom=�6�   .��5  "   �� $��30(�6-   $ Center=�6;   $��Center=�6R   $��삭제��6m   $��3q�6w   $	��200��6�   5     �� 5     -    5        #��[ 5  B   $��70��6o   $��10,0,0,0q�6{   $��Bottom=�6�   .��5   "   �� $��30(�6-   $ Center=�6;   $��Center=�6R   $��수정��6m   $��3q�6w   $	��200��6�   5!     �� 5"     -!   5        #��[ 5"  B   $��70��6o   $��10,0,0,0q�6{   $��Bottom=�6�   .��5#  "   �� $��30(�6-   $ Center=�6;   $��Center=�6R   $��추가��6m   $��3q�6w   $	��200��6�   5$     5%     5&     �� 5'     $
��Left=�6!   $	��398��67   .+�5(     �� $)��#/ECS.Touch;component/Images/box.png��6    $��Uniform=�6M   $��	20 20 0 0q�6_   �� 5)     #��[ 6!   $	��396��6O   $
��25 0q�6\   ._�5*     �� $��	15 20 0 0q�6)   .+�5+  "   �� $ 
Horizontal=�6-   .+�5,  &   �� $��No : ��60   $��35(�6=   $	��150��6K   $ Right=�6W   �� 5-  &   $��35(�6T   ��60   �� *SelectedBoxInfo.No+6T   5.  #   �� 5/  "   $ 
Horizontal=�6-   .+�50  &   �� $��Name : ��60   $��35(�6?   $	��150��6M   $ Right=�6Y   �� 51  &   $��35(�6V   ��60   �� *SelectedBoxInfo.Name+6V   52  #   �� 53  "   $ 
Horizontal=�6-   .+�54  &   �� $��	Weight : ��60   $��35(�6A   $	��150��6O   $ Right=�6[   �� 55  &   $��35(�6X   ��60   �� *SelectedBoxInfo.Weight+6X   56  #   �� 57  "   $ 
Horizontal=�6-   .+�58  &   �� $��Type : ��60   $��35(�6?   $	��150��6M   $ Right=�6Y   �� 59  &   $��35(�6[   ��60   �� *SelectedBoxInfo.BoxTypeCd+6[   5:  #   �� 5;  "   $ 
Horizontal=�6-   .+�5<  &   �� $
��L : ��60   $��35(�6<   $	��150��6J   $ Right=�6V   �� 5=  &   $��35(�6X   ��60   �� *SelectedBoxInfo.Length+6X   5>  #   �� 5?  "   $ 
Horizontal=�6-   .+�5@  &   �� $
��W : ��60   $��35(�6<   $	��150��6J   $ Right=�6V   �� 5A  &   $��35(�6W   ��60   �� *SelectedBoxInfo.Width+6W   5B  #   �� 5C  "   $ 
Horizontal=�6-   .+�5D  &   �� $
��H : ��60   $��35(�6<   $	��150��6J   $ Right=�6V   �� 5E  &   $��35(�6X   ��60   �� *SelectedBoxInfo.Height+6X   5F  #   5G     5H     5I     �� 5J     ���	#FFBEC0C2$��1q�65   $��20q�6I   $
��1416��6U   ._�5K     ]� $��2��6$   .+�5L     �� $ Vertical=�6)   $��Center=�6@   $��0 0 10 0q�6[   $��Right=�6m   .+�5M  "   �� -"   5        #��[ 5M  )   6S   �� 5N  "   -#   5        #��[ 5N  )   6R   �� 5O  "   -$   5        #��[ 5O  )   6T   �� 5P  "   -%   5        #��[ 5P  )   6V   5Q       5R     -&   5        $3 BoxSettingGrid��5R  '   $
��Left=�6�   #��[ 6�   $��20(�6�   $��20q�6�   $��Right=�6  $
��1318��6H  ��6$  �� *BoxInfoList+ 5S  "     5T  &   $ No��69   $ 60 6]    6E   �� *No+6]     5U  &   $
 Name��69   $	 130 6a    6G   �� *Name+6a     5V  &   $ Weight��69   $ 90 6e    6I   �� *Weight+6e     5W  &   $ L��69   $ 60 6`    6D   �� *Length+6`     5X  &   $ W��69   $ 60 6_    6D   �� *Width+6_     5Y  &   $ H��69   $ 60 6`    6D   �� *Height+6`     5Z  &   $ H_Sensor��69   $	 100 6m    6K   �� *HeightSensor+6m     5[  &   $
 Type��69   $ 64 6f    6G   �� *	BoxTypeCd+6f     5\  &   $ 정상 From��69   $	 115 6J    5]  *   `� 5^  .     SwitchMultiConverter7 `� 	Converter#7 [ 6;   8 `� ConverterParameter$
8 from��6m   .1�5_  2   �� $4 ErectorType��6:   9 �� Path$9 SelectedIndex�6T   �� 5`  2   5a  /   5c  '     5d  &   $ 	정상 To��69   $	 100 6H    5e  *   `� 5f  .   #7 [ 6;   $8 to��6m   .1�5g  2   �� $4 ErectorType��6:   $9 SelectedIndex�6T   �� 5h  2   5i  /   5k  '     5l  &   $ 정상 현재번호��69   $	 130 6J    5m  *   `� 5n  .   #7 [ 6;   $8 current��6m   .1�5o  2   �� $4 ErectorType��6:   $9 SelectedIndex�6T   �� 5p  2   5q  /   5s  '     5t  &   $ 수기 From��69   $	 115 6j    6J   �� *
ManualFrom+6j     5u  &   $ 	수기 To��69   $	 100 6f    6H   �� *
ManualTo+6f     5v  &   $ 수기 현재번호��69   $	 130 6m    6J   �� *ManualCurrent+6m   5x     5y     5z     5{     5|     � 5~     -'   5        $��ManualVerificationBoxId��5~     
������ $��2��6K   y�6\   �� *ShowNumberingMessageBox+#5 [ $6 hidden��.+�5     �� #��[ 6   $
��1250��6G   $	��550��6T   ���	#FF777777$��1q�6y   ._�5�     ]� $��0��6    
�������.+�5�     �� $	��Top=�6!   #��[ 66   $��56��6d   $	��Top=�6p   ._�5�     ]� .+�5�  "   �� $
��Wrap=�6,   $
��Left=�6@   $��Center=�6[   $��20,0,0,0q�6v   $��20(�6�   #��[ 6�   ��5�  &   �� 5�  *   " ��60   .�5�  .   ��  � ��$ 추가��6E    5�  .   �� 5�  2    6>   �� *
IsInsert+	 False6[   .a�5�  6   ��  � ��$ 수정��6M   5�  3   5�  +   5�  #   �� 5�  "   -(   5        #��[ 5�  O   $��0,0,20,0q�6{   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6�   Z� 6�   5�     5�     � 5�      5�     �� 5�  "   $��* �60   �� 5�  "   $��4* �60   
 5�     �� 5�  "   $��* �63   �� 5�  "   $��* �63   .+�5�     �� $ 
Horizontal=�6)   $��RightToLeft=�6B   $��0��6^   $��2��6k   .+�5�  "   �� -)   5        #��[ 5�  P   $��70��6}   $��	25 0 0 12q�6�   $��Bottom=�6�   .��5�  &   �� $��30(�60   $ Center=�6>   $��Center=�6U   $��확인��6p   $��3q�6z   $	��200��6�   5�  #   5�     �� 5�     $��0��6%   $��1��65   ._�5�  "   �� $��	15 20 0 0q�6-   .+�5�  &   �� $ 
Horizontal=�61   .+�5�  *   �� $��Name : ��64   $��40(�6C   $	��220��6Q   $ Right=�6]   �� 5�  *     Style.TextBox.NoneBorder#��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *
TargetName+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��	Weight : ��64   $��40(�6E   $	��220��6S   $ Right=�6_   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetWeight+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��Width : ��64   $��40(�6D   $	��220��6R   $ Right=�6^   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetWidth+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��40(�65   $	��220��6C   $ Right=�6O   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��정상 From : ��64   $��40(�6F   $	��220��6T   $ Right=�6`   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetNormalFrom+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��수기 From : ��64   $��40(�6F   $	��220��6T   $ Right=�6`   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetManualFrom+6�   5�  '   5�  #   5�     �� 5�     $��1��6%   $��1��65   ._�5�  "   �� $��	15 20 0 0q�6-   .+�5�  &   �� $ 
Horizontal=�61   .+�5�  *   �� $��Type : ��64   $��40(�6C   $	��220��6Q   $ Right=�6]   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   $��40(�6�   
������ ��6�   �� *
TargetType+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��	Length : ��64   $��40(�6E   $	��220��6S   $ Right=�6_   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetLength+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��	Height : ��64   $��40(�6E   $	��220��6S   $ Right=�6_   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetHeight+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��H_Sensor : ��64   $��40(�6G   $	��220��6U   $ Right=�6a   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetHeightSensor+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��정상 To : ��64   $��40(�6D   $	��220��6R   $ Right=�6^   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetNormalTo+6�   5�  '   �� 5�  &   $ 
Horizontal=�61   .+�5�  *   �� $��수기 To : ��64   $��40(�6D   $	��220��6R   $ Right=�6^   �� 5�  *   #��[ 62   #��[ 6d   $	��300��6�   
������ $��40(�6�   ��6�   �� *TargetManualTo+6�   5�  '   5�  #   5�     5�     5�     5�     5�     5�     � 5�  
   
������ $��2��6(   y�69   �� *ShowNewPrint+#5 [ $6 hidden��.+�5�     �� #��[ 6   $	��600��6C   $	��200��6O   ���	#FF777777$��1q�6t   ._�5�     ]� $��0��6   
�������.+�5�     �� $	��Top=�6   #��[ 62   $��56��6`   $	��Top=�6l   ._�5�     ]� .+�5�     �� $
��Wrap=�6(   $��신규발행��6<   $
��Left=�6H   $��Center=�6c   $��20,0,0,0q�6~   $��20(�6�   #��[ 6�   �� 5�     -*   5        #��[ 5�  @   $��0,0,20,0q�6l   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6~   Z� 6�   5�     5�     � 5�     
 5�     �� 5�     $��* �6/   �� 5�     $��* �6/   �� 5�     $��* �6/   .+�5�     �� $��0��6$   $��수량��64   $��40(�6>   $ Center=�6L   $��0,30,0,0q�6c   #��[ 6u   �� 5�     -+   5        $��newPrintCount��5�  2   $��1��6"   $( 4��6G   : �� TextWrapping$
: Wrap=�6U   $
��Left=�6�   $��Center=�6�   $��0,0,0,0q�6�   $��60(�6�   #��[ 6�   $	��180��6  ��6i   �� *NewPrintCount+6  �� 5�     -,   5        $��2��5�  !   #��[ 61   $��80��6t   $��0,30,0,0q�6�   $	��Top=�6�   .��5�     �� $��30(�6)   $ Center=�67   $��Center=�6N   $��인쇄��6i   $	��100��6s   5�     5�     5�     5�     5�     � 5�  
   
������ $��2��6(   y�69   �� *ShowManualVerification+#5 [ $6 hidden��.+�5�     �� #��[ 6   $	��600��6C   $	��505��6O   ���	#FF777777$��1q�6t   ._�5�     ]� $��0��6   
�������.+�5      �� $	��Top=�6   #��[ 62   $��56��6`   $	��Top=�6l   ._�5     ]� .+�5     �� $
��Wrap=�6(   $��수동 검증��6<   $
��Left=�6I   $��Center=�6d   $��20,0,0,0q�6   $��20(�6�   #��[ 6�   ��5  "   �� 5  &   " ��6,    5  *   �� 5  .    6:   �� *
IsInsert+	 False6W   .a�5  2   ��  � ��$ 수정��6I   5  /   5
  '   5     �� 5     --   5        #��[ 5  J   $��0,0,20,0q�6v   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6�   Z� 6�   5     5     � 5      5     �� 5     $��* �6,   �� 5     $��4* �6,   �� 5     $��50 �6,   
 5     �� 5     $��6* �6/   �� 5     $��50 �6/   .+�5     �� $��0��6%   $��0��62   $��	25 0 25 0q�6B   $��Bottom=�6U   .+�5     �� $��	박스 ID��6(   $��20(�65   $
 Left=�6C   #��[ 6X   $��0,20,0,0q�6�   � 5     
 5  "   �� 5  &   $
��700* �67   �� 5  &   $	��130 �67   .+�5!  "   �� $��2q�6)   $��0��6=   ��5"  &   �� 5#  *   $ /Images/tab_underline.png��65   $ UniformToFill=�6]   ._�5%  &   �� -.   5        $& BoxId��5%  F   $' Upper=�6.   $( 10��6U   #��[ 6�   $* Center=�6�   $��Center=�6�   $��30(�6  #��[ 6  $
��0,-2q�6D  ���	#FF004A74��6i  Z� 5&  #   �� 5'  "   -/   5        $��1��5'  )   #��[ 69   $��40��6f   $��0,0,10,0q�6r   $	��Top=�6�   .��5(  &   �� $��20(�60   $ Center=�6>   $��Center=�6U   $��확인��6p   $��3q�6z   $	��100��6�   5)  #   5*     5+     �� 5,     $��1��6!   $��0��6.   ._�5-       -0   5        $3 NonVerifiedBoxGird��5-  '   #��[ 6C   $	��502��6r   $	��291��6~   $��30(�6�   ��6�   �� *NonVerifiedBoxList+ 5.  "     5/  &   $ 호기��69   $	 130 6f    6E   �� *ErectorType+6f     50  &   $ 박스ID��69   $	 220 6b    6G   �� *BoxId+6b     51  &   $ 박스 타입��69   $	 150 6e    6H   �� *	BoxType+6e   53     54     �� 55     $��1��6%   $��1��62   $��Center=�6B   $
��Left=�6]   $��40��6x   .+�56     �� -1   5        #��[ 56  %   $	��100��6p   �� 57     -2   5        #��[ 57  %   $	��100��6t   58     �� 59     $��2��6%   $��0��62   $��Center=�6B   $ 
Horizontal=�6_   $	��Top=�6x   $��40��6�   .+�5:     �� -3   5        #��[ 5:  %   $��40��6r   $��0,0,10,0q�6~   $	��Top=�6�   .��5;  "   �� $��20(�6-   $ Center=�6;   $��Center=�6R   $��검증��6m   $��3q�6w   $	��100��6�   5<     �� 5=     -4   5        #��[ 5=  %   $��40��6w   $��10,0,0,0q�6�   $	��Top=�6�   .��5>  "   �� $��20(�6-   $ Center=�6;   $��Center=�6R   $��취소��6m   $��3q�6w   $	��100��6�   5?     5@     5A     5B     5D     5E     � 5H  
   
������ $��2��6(   y�69   �� *ShowPasswordBox+#5 [ $6 hidden��.+�5I     �� #��[ 6   $
��1000��6C   $	��500��6P   ���	#FF777777$��1q�6u   ._�5J     ]� $��0��6   #��[ 6)   .+�5K     �� $	��Top=�6   #��[ 62   $��56��6_   $	��Top=�6k   ._�5L     ]� .+�5M     �� $
��Wrap=�6(   $��	로그인��6<   $
��Left=�6G   $��Center=�6b   $��20,0,0,0q�6}   $��20(�6�   #��[ 6�   �� 5N     -5   5        #��[ 5N  C   $��0,0,20,0q�6o   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6�   Z� 6�   5O     5P     �� 5Q     $��Center=�6!   $��Center=�6<   $ 
Horizontal=�6Y   .+�5S     �� $ Center=�6$   $��비밀 번호 : ��6;   $��40(�6K   G� 5T     -6   5        ; G�Name$; Password��5T  &   $��0 5 0 0q�68   $	��250��6I   $��30(�6V   �� 5U     -7   5        $��1��5U  !   #��[ 61   $��55��6^   $��	10,5,10,0q�6j   $	��Top=�6}   .��5V     �� $��20(�6(   $ Center=�66   $��Center=�6M   $��확인��6h   $��3q�6r   $	��100��6}   5W     5X     5Y     5Z     5[     � 5^  
   
������ $��2��6(   y�69   �� *ShowErrorMessage+#5 [ $6 hidden��.+�5_     �� #��[ 6   $	��800��6C   $	��400��6O   ���	#FF777777$��1q�6t   ._�5`     ]� $��0��6   #��[ 6)   .+�5a     �� $	��Top=�6   #��[ 62   $��56��6_   $	��Top=�6k   ._�5b     ]� .+�5c     �� $
��Wrap=�6(   $��알림��6<   $
��Left=�6F   $��Center=�6a   $��20,0,0,0q�6|   $��20(�6�   #��[ 6�   �� 5d     -8   5        #��[ 5d  D   $��0,0,20,0q�6p   $��36��6�   $��36��6�   $��Right=�6�   $��Center=�6�   ��6�   Z� 6�   5e     5f     �� 5g     $	��Top=�6    $ Center=�65   $��	0 122 0 0q�6L   $��40(�6}   ��6_   �� *ErrorMessage+6}   5h     5i     5j     5k     5l     