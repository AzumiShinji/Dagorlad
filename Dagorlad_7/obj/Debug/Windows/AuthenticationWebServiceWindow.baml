   M S B A M L   `   `   `  ����J  FDagorlad, Version=0.0.7846.16415, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  5   	   7    1Dagorlad_7.Windows.AuthenticationWebServiceWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Dagorlad_7.Windows     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   $	��250��5	   	    � WindowStyle$
 None=�6    � SizeToContent$ Height=�5
         Background.Outside#���   5   	    � Title$ Авторизация��5   	   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��auto �6   .+�5   
   � $��0��6   $��
10,10,10,0q�6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5       5      �� 5      �� 5      �� 5      �� 5      .+�5      �� $��2��6   $��Авторизация��6(     
Foreground#���  6>   $��14(�6h   $��Medium"�6v   $��Center=�6�   �� 5      $��
Логин��6   #���  6$   $��1��6N   �� 5      -   5         ��Name$ LoginTextBox��5      $��1��6-   $��1��6:   �� 5       $��Пароль��6   #���  6%   $��2��6O   $��0,5,0,0q�6\   G� 5!      -   5         G�Name$ PasswordPasswordBox��5$      #���  5!        Background.Inside.Blob#���  6D   $��Center=�5"      $��0q�68   $��0,4,0,0q�6L     Background.Highlight	 G� SelectionBrush#	 �  6]   $��5q�5#      $��2��5$   3   $��1��6@   �� 5%      -   5        
 ��Name$
 StatusLabel��5%      $��3��6)   $��2��66   $��5q�6J   
��� E��$��Center=�5'      $��Center=�68   5(      �� 5)      -   5         ��Name$ AcceptButton��5)        ButtonStandart#��[ 6(   $��1��5*      $
��ОК��6   $��40��6,    �� 	IsDefault . 6S   5+      5,      