   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  '    !Dagorlad_7.Windows.MyDialogWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:Dagorlad_7.Windows     -     � WindowStyle$
  None=�   Background.Outside#���    � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=� � ShowInTaskbar .  $	��300��$	��500�� � 
ResizeMode$ NoResize=� � Title$  ��.����  �� Padding$ 10q�._��  � RowDefinitions �� $
��auto ��� �� $
��auto �.+�� $��1��$��25q�.+��� -    ��Name$ TextTextBox��	 �� 
IsReadOnly	 . $��1��$
��Auto=�$��40��$
��text��$��10q�$��14(�$��Normal"�$	��350��#���   $��Center=�$��0q�  Background.Highlight
 �� 
CaretBrush#
 �    
Foreground#���  $��Center=� �� TextWrapping$
 Wrap=�� -    �Name$ GridButtons��$��2��$��Right=�����  �� 
TargetType" � �� 	Resources  �� BasedOn �� Property �� Value��%M   '��       7���� " ��8   .���  � ��$ 10q���  � ��$	 100�� � ColumnDefinitions �� $
��auto ��� $
��auto ��� $
��auto �.+��� -    ��Name$ OkButton��$��0��$��40��$��14(�  Foreground.Highlight#���  $��Medium"�$��ХОРОШО��$��ok��  ButtonStandart#��[ �� -   $ 	YesButton��$��1��$��40��$��14(�#���  $��Medium"�$
��ДА��$	��yes��#��[ �� -   $ NoButton��$��2��$��40��$��14(�#���  $��Medium"�$��НЕТ��$��no��#��[ 