   M S B A M L   `   `   `  ���� E  AECS.Viewer, Version=1.6.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ECS.Viewer
ECS.Viewer      ECS.Viewer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:ECS.Viewer       � WindowState$  	Maximized=� � WindowStyle$
 None=�$
��1280��$
��1024�� � Topmost .  .���  � RowDefinitions �� $��30 ��� $��* �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Right=�.+��� $ 
Horizontal=�$��60��$��Center=�.+��� $
��Bold"�$��16(�$��DB��$��Center=��� $��Images/LedOn.ico��$��Uniform=�$��5q��� -   $��Uniform=�$��60������  �� 
TargetType" ��.���  �� Property � �� �� Value$ Images/close.ico�� �� Triggers P� 	 P� Property	 �@J�IsMouseOver
 P� Value
 . . ���  � ��$ Images/close2.png��� $��1�� � ColumnDefinitions �� $��1045* ��� $
��227* �.+� �   � Source$! Views/Pages/MainScreen.xaml?�  � NavigationUIVisibility$ Hidden=�$��2��