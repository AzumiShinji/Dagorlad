   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null'clr-namespace:Dagorlad_7
Dagorlad_7  7 clr-namespace:Dagorlad_7.classesDagorlad_7.classes  3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages      Dagorlad_7.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:Dagorlad_7     0cls clr-namespace:Dagorlad_7.classes     .pgsclr-namespace:Dagorlad_7.Pages     -     � WindowStartupLocation$  CenterScreen=�   Background.Inside#���     
Foreground#���   � Title$ Dagorlad��$
��1000��$	��600��$	��600��$
��1000��.���  � RowDefinitions �� $
��auto ��� .+�� $��0��
 � Tag$
 Menu��  Background.HalfOutside#���  $��0q� � ColumnDefinitions �� �� $
��auto �.+���  �� Orientation$ 
Horizontal=�$��0��.+��� -    ��Name$ OrganizationButton��$��Organizations��  ButtonStandart#��[ $
 Menu��.���  �� $
��auto ��� .+���   Canvas_Organization#��[ $��25��$��25��$��0,0,5,0q��� $'��!Поиск организаций��$��1��$
��Left=�$��Center=��� -   $ 
ChatButton��#��[ $
��Chat��$
 Menu��.���  �� $
��auto ��� �� .+���   Canvas_Chat#��[ $��25��$��25��$��0,0,5,0q��� $��Чат��$��1��$
��Left=�$��Center=��� $��2�� �� CornerRadius$ 50��$��16��$��16��
��� E��
�������$��5,0,0,0q� �� Padding$ 1q����� 	 �� 
TargetType"	 ��.��� 
 �� Property
 � y� �� Value$ Visible=� �� Triggers ��  �� Binding �� ,   &Dagorlad_7.Pages.AllCountUnreadedClass   Instance �� Source# Z  �� Path$ AllCountUnreaded� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ OneWay=� �� Value 0.a��� 
 � y�$ 	Collapsed=�._��� $��0,0,0.5,1.4q�$��Center=�$��Center=����� # Z $ AllCountUnreaded�$ PropertyChanged=�$ OneWay=��� -   $��	Utilities��#��[ $
 Menu������ "	 �� �� BasedOn# [B��.��� 
 � y�$ 	Collapsed=� ��  �� #  Dagorlad_7.classes.MySettings   Settings# Z $ IsRegGlobalHook�$ OneWay=�$ PropertyChanged=� True.a��� 
 � y�$ Visible=�.���  �� $
��auto ��� .+���   Canvas_Utilities#��[ $��25��$��25��$��0,0,5,0q��� $��Утилиты��$��1��$
��Left=�$��Center=��� $ 
Horizontal=�$��1��.+��� -   #��[ $��Settings��$
 Menu��.���  �� $
��auto ��� .+���   Canvas_Settings#��[ $��25��$��25��$��0,0,5,0q��� $��Настройки��$��1��$
��Left=�$��Center=��� -     
ButtonExit#��[ .��� .+���  	 Canvas_Exit#��[	 $��25��$��25��$��0,0,0,0q�� $��1��.+� � -     �Name$ 	MainFrame��  � NavigationUIVisibility$ Hidden=� K� ZIndex$ 1��