   M S B A M L   `   `   `  ���� E  AECS.Server, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ECS.Server.ControlsECS.Server.Controls  C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.CoreECS.Core.ViewModels.Server 3clr-namespace:ECS.Server.ViewsECS.Server.Views  )    #ECS.Server.Views.WcsForcePostScreen   .Controls!clr-namespace:ECS.Server.Controls  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Ivm:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.Core     0localclr-namespace:ECS.Server.Views       N� Title$  WcsForcePostScreen�� � DataContext 6  0ECS.Core.ViewModels.Server.WcsForcePostViewModel  .���  � RowDefinitions �� $	��100 ��� $��* �.+��� $��박스아이디��$
��Left=�$��	10,21,0,0q�$	��Top=��� $��송장아이디��$
��Left=�$��	10,52,0,0q�$	��Top=��� -    ��Name$ BoxId��$��0��$
��Left=�$��23��$��	92,22,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��236���� -   $ 	InvoiceId��$��0��$
��Left=�$��23��$��	92,60,0,0q�$
 Wrap=�$	��Top=�$	��236���� -   $��0��$��찾기��$
��Left=�$��
339,22,0,0q�$	��Top=�$��75��$��23���� -   $��0��$��찾기��$
��Left=�$��
339,60,0,0q�$	��Top=�$��75��$��23��)   #ECS.Server.Controls.WpfPropertyGrid  -     Name$ propertyGrid��$��1��$��Stretch=�$��Stretch=�   HelpVisible .     ToolbarVisible .  $��10,10,168,19q��� -   $��중량검수 보고��$
��Left=�$��
475,18,0,0q�$	��Top=�$	��157��$��32���� -   $��송장검증 보고��$
��Left=�$��
475,58,0,0q�$	��Top=�$	��157��$��32��