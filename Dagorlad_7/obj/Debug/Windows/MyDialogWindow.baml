   M S B A M L   `   `   `  ����J  FDagorlad, Version=0.0.7846.16415, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  5   	   '    !Dagorlad_7.Windows.MyDialogWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Dagorlad_7.Windows     5   	   -   5          � WindowStyle$
  None=�5   	      Background.Outside#���   5	   	    � SizeToContent$ WidthAndHeight=�5
   	    � WindowStartupLocation$ CenterScreen=�6(    � ShowInTaskbar .  $	��300��5      $	��500��6.    � 
ResizeMode$ NoResize=�5   	    � Title$  ��5   	   .��5      ��  �� Padding$ 10q�6   ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      �� 5      $
��auto �6    .+�5      � $��1��6   $��25q�6    .+�5      �� -   5         ��Name$ TextTextBox��5      	 �� 
IsReadOnly	 . $��1��6A   $
��Auto=�6N   $��40��6~   $
��text��5      $��10q�6&   $��14(�62   $��Normal"�6@   $	��350��6T   #���   5      $��Center=�6C   $��0q�6e     Background.Highlight
 �� 
CaretBrush#
 �  6q     
Foreground#���  5      $��Center=�6;    �� TextWrapping$
 Wrap=�6V   5      � 5      -   5         �Name$ GridButtons��5      $��2��6(   $��Right=�65   ��5      �� 5       �� 
TargetType" �6    �� 	Resources 5       �� BasedOn �� Property �� Value��%�   '��       7���� 5      " ��6$   8   .�5    "   ��  � ��$ 10q�6<   �� 5!   "    � ��$	 100��6=   5"      5#      5$       � ColumnDefinitions 5&      �� 5'      $
��auto �6'   �� 5(      $
��auto �6'   �� 5)      $
��auto �6'   .+�5+      �� -   5         ��Name$ OkButton��5+      $��0��6+   $��40��6;   $��14(�6G     Foreground.Highlight#���  6U   $��Medium"�6�   $��ХОРОШО��5,      $��ok��65     ButtonStandart#��[ 6K   �� 5-      -   5        $ 	YesButton��5-      $��1��6,   $��40��6<   $��14(�6H   #���  6V   $��Medium"�6�   $
��ДА��5.      $	��yes��61   #��[ 6H   �� 5/      -   5        $ NoButton��5/      $��2��6+   $��40��6;   $��14(�6G   #���  6U   $��Medium"�6�   $��НЕТ��50      $��no��62   #��[ 6H   51      52      53      54      