   M S B A M L   `   `   `  ����J  FDagorlad, Version=0.0.7846.16415, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  5   	   '    !Dagorlad_7.Windows.MiniMenuWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Dagorlad_7.Windows     5   	   -   5          � WindowStyle$
  None=�5   	      Background.Outside#���   5	   	    � 
ResizeMode$ NoResize=�5
   	    � ShowActivated .   � Topmost .  � SizeToContent$ Height=�5   	    � ShowInTaskbar .   � Title$ Мини-меню��5   	   $��35��6   .��5      � $��0q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   �� -   5        $��0��5      $��12��6K     Background.Inside.Blob#��[ 5      ��5      �� 5       �� 
TargetType" ��6   	 �� Triggers	 5      :� 5      
 :� RoutedEvent$
 StackPanel.MouseEnter��6'   .[�5      �� .��5   "   �� .��5   &   ��   Background.Highlight.Color	 �� To# [ 65    �� TargetProperty$5 /(StackPanel.Background).(SolidColorBrush.Color)�5   %    g� Duration$ 0:0:0.15D�5   %   5    #   5!      5"      :� 5#      $
 StackPanel.MouseLeave��6'   .[�5$      �� .��5%   "   �� .��5&   &   �� $5 /(StackPanel.Background).(SolidColorBrush.Color)�5'   %   $ 0:0:0.15D�5(   %   5)   #   5*      5+      5-      5/      �� 50   
   -   5        $��1��50        ButtonStandart#��[ 6   $
��auto��6I   $��5q�6W   .��51      � .+�52      �� $��16��6   $��16��6#   $��/smartmenu.ico��6/   53      54      55      56      