   M S B A M L   `   `   `  ����F  BECS.Server, Version=2.10.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ECS.Server.ControlsECS.Server.Controls  5      C ?ECS.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.CoreECS.Core.ViewModels.Server 5      3clr-namespace:ECS.Server.ViewsECS.Server.Views  5      )    #ECS.Server.Views.WcsForcePostScreen   5      .Controls!clr-namespace:ECS.Server.Controls  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ivm:clr-namespace:ECS.Core.ViewModels.Server;assembly=ECS.Core     5      0localclr-namespace:ECS.Server.Views     5        N� Title$  WcsForcePostScreen��5       � DataContext 5      6  0ECS.Core.ViewModels.Server.WcsForcePostViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��박스아이디��6   $
��Left=�6-   $��	10,21,0,0q�6H   $	��Top=�6[   �� 5   
   $��0��6   $��송장아이디��6   $
��Left=�6-   $��	10,52,0,0q�6H   $	��Top=�6[   �� 5   
   -   5         ��Name$ BoxId��5      $��0��6   $
��Left=�6.   $��23��6I   $��	92,22,0,0q�6U    �� TextWrapping$
 Wrap=�6h   $	��Top=�6}   $	��236��6�   �� 5   
   -   5        $ 	InvoiceId��5      $��0��6   $
��Left=�62   $��23��6M   $��	92,60,0,0q�6Y   $
 Wrap=�6l   $	��Top=�6�   $	��236��6�   �� 5   
   -   5        $��0��5      $��찾기��6   $
��Left=�6+   $��
339,22,0,0q�6F   $	��Top=�6Z   $��75��6r   $��23��6}   6�   �� 5   
   -   5        $��0��5      $��찾기��6   $
��Left=�6+   $��
339,60,0,0q�6F   $	��Top=�6Z   $��75��6r   $��23��6}   6�   �� 5   
   -   5        $��0��5      $��중량검수 보고��6   $
��Left=�60   $��
475,18,0,0q�6K   $	��Top=�6_   $	��157��6w   $��32��6�   6�   �� 5   
   -   5        $��0��5      $��송장검증 보고��6   $
��Left=�60   $��
475,58,0,0q�6K   $	��Top=�6_   $	��157��6w   $��32��6�   6�   �� 5   
   $��0��6   $S��M경고 : 강제보고이므로, ECS Database에는 적용되지 않습니다.��6!   
���  ��$
��Left=�6a   $��	640,0,0,0q�6|   $��Center=�6�   $
��Wrap=�6�   )   #ECS.Server.Controls.WpfPropertyGrid  5   
   -   5          Name$ propertyGrid��5   1   $��1��6$   $��Stretch=�6G   $��Stretch=�6e      HelpVisible .     ToolbarVisible .  $��10,10,168,19q�6�   5      5       