   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel '    !Dagorlad_7.Windows.MyNotifyWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:Dagorlad_7.Windows     ScomponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase     -     � AllowsTransparency  .  � 
ResizeMode$ NoResize=� � WindowStyle$
 None=�
������  � ShowInTaskbar .  $��0q� � Topmost .  � SizeToContent$ Height=� � ShowActivated .   � Title$ MyNotifyWindow��$	��400�� � 	Resources 4   .Dagorlad_7.Windows.WidthFixedListViewConverter   WidthFixedListViewConverter  src	 �� Source
 �� SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName   	Direction��%J   &	        &	        �� 	 �� 
   $ dt��$ 
Descending=�.���� $��0q�$
��auto��._�� $
��auto�� � RowDefinitions �� $
��auto ��� $��* �.+�� -    �Name$ HideNotifiesListViewGrid��$��0��  Background.Inside#���  .+��� -   $��0��$��Скрыть все��$��0q�$��1,0,4,0q�$��40��  Foreground.History#���    ButtonStandart#��[ �� -    ��Name$ NotifiesListView��
������ $��1��$��0q� %� 
ScrollUnit$ Pixel=�$	��100��$	��310��$��Disabled=�$��Disabled=�����  �� Source# [ ����  �� 
TargetType" ��.���  �� Property � �� �� Value$ 0q���  � ��$ 0,3,3,0q���  � ��$ 0,0,0,0q�-   ��  � ��$ Stretch=����� .R��  � ColumnDefinitions �� �� $
��auto �.+�� $ MainGrid��-   #���  $	��100������ " � �� Triggers :�  :� RoutedEvent$ Loaded��.[��� .���� .��W�  �� TargetProperty$ Opacity� g� Duration$ 0:0:0.2D� W� From$	 0.0X�	 W� To$	 1.0X� �� �� $
��auto �.+�� $��0��$��5q� �� $
��auto ��� .+��� $��0��$��64��$��64��$��20,0,0,0q����� *image+� $��2��$��20,0,0,0q� �� $
��auto ��� .+���  ��Name$ TitleBoxText��$��0��$��5q�$��14(�$��Medium"�#���   �� Padding$ 0q�  
Foreground#���  $��Center=�$��NoWrap=����� *title+�� $ TextBoxText��$��1��$��5q�#���  $��Stretch=�#���  $
��Wrap=�$ 0q����� *text+� $��1��#���  .+��� -   $��1��  Canvas_Arrow_Right#��[ $��40��$��Center=�#��[ #���  ����  �� ElementName$ NotifiesListView��
 � Tag �� $ MainGrid��