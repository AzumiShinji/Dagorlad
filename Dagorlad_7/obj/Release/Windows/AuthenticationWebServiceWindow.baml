   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  7    1Dagorlad_7.Windows.AuthenticationWebServiceWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:Dagorlad_7.Windows     -     � 
ResizeMode$  NoResize=� � WindowStartupLocation$ CenterScreen=�$	��250�� � WindowStyle$
 None=� � SizeToContent$ Height=�   Background.Outside#���    � Title$ Авторизация��.���  � RowDefinitions �� �� $
��auto �.+�� $��0��$��
10,10,10,0q� � ColumnDefinitions �� $
��auto ���  �� �� �� �� .+��� $��2��$��Авторизация��  
Foreground#���  $��14(�$��Medium"�$��Center=��� $��
Логин��#���  $��1���� -    ��Name$ LoginTextBox��$��1��$��1���� $��Пароль��#���  $��2��$��0,5,0,0q�G� -    G�Name$ PasswordPasswordBox��#���    Background.Inside.Blob#���  $��Center=�$��0q�$��0,4,0,0q�  Background.Highlight	 G� SelectionBrush#	 �  $��5q�$��2��$��1���� -   
 ��Name$
 StatusLabel��$��3��$��2��$��5q�
��� E��$��Center=�$��Center=��� -    ��Name$ AcceptButton��  ButtonStandart#��[ $��1��$
��ОК��$��40�� �� 	IsDefault . 