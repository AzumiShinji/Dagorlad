   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  #    Dagorlad_7.Pages.SettingsPage   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Dagorlad_7.Pages     -      
Foreground  N� 
Foreground#  �    N� Title$ SettingsPage��.��� $��Center=�$��Center=� � RowDefinitions �� $
��auto ��� �� $
��auto �.+�� 
 � Tag$
 Head�� � ColumnDefinitions �� �� .+�� $��Center=� �� �� .+�� $��10q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  -   
dropEffect��   
BlurRadius$ 15Q�   ShadowDepth$ 5Q�   Opacity$ 1Q�  Background.Highlight.Color	   Color#	 [ .+���   Canvas_Logo#��[ $��36��$��36��� $��1��$��Center=� �� �� �� $
��auto �.+��� -   
 ��Name$
 AppNameLabel��$��AppName��  Foreground.History#���  $��Medium"�$��14(�$��Center=��� -   $
 AppVersionLabel��$��
AppVersion��$��1��#���  $��Medium"�$��10(�$��Center=��� -    ��Name$ HandUpdateButton��$.��(Доступна новая версия��$y�Visible=�  Background.Highlight#���  $��2��  ButtonStandart#��[ � $��10q�$ Middle��$��1�� �� �� .+��  �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+�� $��0�� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� -    ��Name$ IsAutorunCheckBox��$��0�� d� 	IsChecked$ FalseY�$/��)Автозапуск приложения��#���   $��Center=�$��Medium"�$��14(��� -   $# IsSearchOrganizationsCheckBox��$��1��$ FalseY�$)��#Искать организации��#���   $��Center=�$��Medium"�$��14(��� -   $  IsEnabledSmartMenuCheckBox��$��2��$ FalseY�$��Смарт-меню��#���   $��Center=�$��Medium"�$��14(��� -	   $ IsRegGlobalEventCheckBox��$ FalseY�$��3��$��Medium"�$��14(�$F��@Регистрировать глобальные события��#���   � $��1��$��Center=�$��5q� �� ��  �� $
��auto ��� �� .+��� $!��Цветовая схема��#���   $��Center=�$��14(�$��Medium"�$��2��	� -
    	�Name$  DarkColorSchemeRadioButton��$��0��$��Темная�� 	� 	GroupName$ 
TypeScheme��$��1��#���   $��14(�$��Medium"�	� -   $! LightColorSchemeRadioButton��$��1��$��Светлая��$ 
TypeScheme��$��10,0,0,0q�$��1��#���   $��14(�$��Medium"��� -   $7 1IsTransparentBackgroundDialogOfChatWindowCheckBox��$��2��$��2��$ FalseY�$+��%Диалог чата без фона��#���   $��Center=�� $��2�� �� $
��auto ��� �� $
��auto �.+��� $.��(Адреc локальной почты:��#���   �� -    ��Name$ EmailTextBox��$��1�� �� 
IsReadOnly . #���   $	��150�� �� SelectionBrush# �  �� -   $ AllowingEditEmailButton��$��2��#��[ .���� $��/images/pencil_16.png��$��16��$��16��� $��3�� �� ��  �� �� �� �� -   $ OpenLogsFolderButton��$��4��#��[ $y�	Collapsed=�$E��?Открыть локальную папку программы��$��40��� $ Bottom��$��2��.+��� -   #��[ $��Сохранить��$��40��