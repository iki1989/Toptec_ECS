   M S B A M L   `   `   `  ����F  BECS.Viewer, Version=1.11.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ECS.Viewer
ECS.Viewer  5   	       ECS.Viewer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ECS.Viewer     5   	     � WindowState$  	Maximized=�5   	    � WindowStyle$
 None=�6!   $
��1280��64   $
��1024��6A    � Topmost .  .��5	      �  � RowDefinitions 5
   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      �� $ 
Horizontal=�6   $��60��62   $��Center=�6=   .+�5      �� $
��Bold"�6   $��16(�6.   $��DB��6<   $��Center=�6F   �� 5      $��Images/LedOn.ico��6   $��Uniform=�62   $��5q�6D   5      �� 5      -   5        $��Uniform=�5      $��60��6>   ��5      �� 5       �� 
TargetType" ��6   .�5      ��  �� Property � �� �� Value$ Images/close.ico��63    �� Triggers 5      P� 5      	 P� Property	 �@J�IsMouseOver
 P� Value
 . . �5   "   ��  � ��$ Images/close2.png��6A   5      5       5"      5#      � 5$   
   $��1��6    � ColumnDefinitions 5%      �� 5&      $��1045* �6#   �� 5'      $
��227* �6#   .+�5)       �   � Source$! Views/Pages/MainScreen.xaml?�6     � NavigationUIVisibility$ Hidden=�69   $��2��6Y   5+      5,      5-      