   M S B A M L   `   `   `  ����J  FDagorlad, Version=0.0.7846.16415, Culture=neutral, PublicKeyToken=null'clr-namespace:Dagorlad_7
Dagorlad_7  5   	   7 clr-namespace:Dagorlad_7.classesDagorlad_7.classes  5   	   3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  5   	       Dagorlad_7.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Dagorlad_7     5   	   0cls clr-namespace:Dagorlad_7.classes     5   	   .pgsclr-namespace:Dagorlad_7.Pages     5   	   -   5          � WindowStartupLocation$  CenterScreen=�5
   	      Background.Inside#���   5   	     
Foreground#���  5   	    � Title$ Dagorlad��5   	   $
��1000��5   	   $	��600��6   $	��600��5   	   $
��1000��6   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   � $��0��6   
 � Tag$
 Menu��6     Background.HalfOutside#���  6'   $��0q�6]    � ColumnDefinitions 5      �� 5      �� 5      $
��auto �6#   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0��62   .+�5      �� -   5         ��Name$ OrganizationButton��5   �   $��Organizations��6     ButtonStandart#��[ 6Y   $
 Menu��6�   .��5      �  5      �� 5      $
��auto �6/   �� 5       .+�5"      ��   Canvas_Organization#��[ 6+   $��25��6Z   $��25��6e   $��0,0,5,0q�6q   �� 5#      $'��!Поиск организаций��6$   $��1��6=   $
��Left=�6M   $��Center=�6h   5$      5%      �� 5&      -   5        $ 
ChatButton��5&   {   #��[ 6   $
��Chat��6D   $
 Menu��6�   .��5'      �  5(      �� 5)      $
��auto �6/   �� 5*      �� 5+      .+�5-      ��   Canvas_Chat#��[ 6+   $��25��6R   $��25��6]   $��0,0,5,0q�6i   �� 5.      $��Чат��6$   $��1��6/   $
��Left=�6?   $��Center=�6Z   �� 5/      $��2��6!    �� CornerRadius$ 50��61   $��16��6C   $��16��6N   
��� E��
�������$��5,0,0,0q�6�    �� Padding$ 1q�6�   ��50      �� 51   "   	 �� 
TargetType"	 ��6(   .�52   &   �� 
 �� Property
 � y� �� Value$ Visible=�6C    �� Triggers 53   &   �� 54   *    �� Binding 66   �� ,   &Dagorlad_7.Pages.AllCountUnreadedClass   Instance �� Source# Z  �� Path$ AllCountUnreaded� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ OneWay=� �� Value 06�   .a�55   .   �� 
 � y�$ 	Collapsed=�6K   56   +   58   #   ._�5:      �� $��0,0,0.5,1.4q�5;   (   $��Center=�5<   (   $��Center=�6E   ��5:   (   �� # Z $ AllCountUnreaded�$ PropertyChanged=�$ OneWay=�5<   E   5=      5>      5?      �� 5@      -   5        $��	Utilities��5@      #��[ 6U   $
 Menu��6�   ��5A      �� 5B      "	 ��6     �� BasedOn# [B��64   .�5C      �� 
 � y�$ 	Collapsed=�6;    5D      �� 5E   "    6.   �� #  Dagorlad_7.classes.MySettings   Settings# Z $ IsRegGlobalHook�$ OneWay=�$ PropertyChanged=� True6�   .a�5F   &   �� 
 � y�$ Visible=�6C   5G   #   5I      .��5K      �  5L      �� 5M      $
��auto �6/   �� 5N      .+�5P      ��   Canvas_Utilities#��[ 6+   $��25��6W   $��25��6b   $��0,0,5,0q�6n   �� 5Q      $��Утилиты��6$   $��1��63   $
��Left=�6C   $��Center=�6^   5R      5S      5T      �� 5U      $ 
Horizontal=�6   $��1��62   .+�5V      �� -   5        #��[ 5V      $��Settings��6c   $
 Menu��6   .��5W      �  5X      �� 5Y      $
��auto �6/   �� 5Z      .+�5\      ��   Canvas_Settings#��[ 6+   $��25��6V   $��25��6a   $��0,0,5,0q�6m   �� 5]      $��Настройки��6$   $��1��65   $
��Left=�6E   $��Center=�6`   5^      5_      �� 5`      -   5          
ButtonExit#��[ 5`      .��5a      � .+�5f      ��  	 Canvas_Exit#��[	 6+   $��25��6R   $��25��6]   $��0,0,0,0q�6i   5h      5i      5j      5k      � 5l   
   $��1��6   .+�5m       � -   5          �Name$ 	MainFrame��5m        � NavigationUIVisibility$ Hidden=�6'    K� ZIndex$ 1��6i   5n      5o      5p      