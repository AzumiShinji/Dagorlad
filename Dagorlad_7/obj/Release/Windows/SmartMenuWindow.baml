   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null7 clr-namespace:Dagorlad_7.WindowsDagorlad_7.Windows  3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  (    "Dagorlad_7.Windows.SmartMenuWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:Dagorlad_7.Windows     .pgsclr-namespace:Dagorlad_7.Pages     -     � WindowStyle$
  None=�   Background.Outside#���    � SizeToContent$ WidthAndHeight=�$	��300��$	��400�� � 
ResizeMode$ NoResize=� � Title$ Смарт-меню��$	��700��$	��900�� � 	Resources .   (Dagorlad_7.Pages.NullableContentToHidden  NullableContentToHidden/   )Dagorlad_7.Windows.ImageEditShowConverter  ImageEditShowConverter��%   &	       &	          .��� $��0q� � RowDefinitions �� $
��auto ��� �� $
��auto �.+��� -    ��Name$ StaysOpenCheckBox��$��Закрепить��  
Foreground#���  {�.  d� 	IsChecked$ FalseY�$��13(�$��Normal"�$��10,0,0,0q�$��0�� K� ZIndex$	 999��$��Center=�$
��Left=�$��Center=�� -   	 �Name$	 DragablePanel��$��0��$��1��$��Stretch=�#���   $��40��.+�� $��Center=�$��0��
 � ColumnDefinitions
 �� �� .+��� -    ��Name$ HeaderLabel��$��СМАРТ-МЕНЮ��$��0��$��Center=�$��14(�$��Medium"�$��Center=�#���  �� $��(Win+Shift+A)��$��0��$��Center=�$��8(�$��0q�$��	0,-25,0,0q�$��Center=�#���  �� -   $$ TemporaryNumberOfHandlingLabel��$��1�� �� ContentStringFormat$ Номер: {0}��$��Right=�$��Center=�$��14(�$��Medium"�$��Center=�  Foreground.Highlight#���  y��� *	Content+ �� RelativeSource �� *Self+ �� 	Converter# [ � -   $	 SmartAnswersGrid��  Background.Inside#���  $��1��$��2��
 �� $
��0.3* ���  �� $
��auto ��� �� $
��auto �.+�� 
 � Tag$ 
LeftColumn��$��1��#���    �� $
��auto ��� .+�� $��0��
 �� �� $
��auto �.+��� -    ��Name$ NewItemTextBox��$	��100��$��5q�$��36���� -    ��Name$ NewItemAddButton��$��1��$��36��$��36��$��0,5,5,5q�  ButtonStandart#��[   Canvas_Plus#��[ �� -	    ��Name$ SmartAnswersListBox�� %� 
ScrollUnit$ Pixel=�$��1��$	��200��$��Disabled=�
������ $��0q� �� SelectionMode$ Single=����� ����  �� 
TargetType" �� �� BasedOn# [B��.�-
   ��  �� Property � �� �� Value$ Stretch=����� .R�� $	 ControlMenuGrid�� � ToolTip �� *name+
 �� �� $
��auto ��� $
��auto ��� $
��auto �.+��� $
��auto��._�� 
 �� �� $
��auto �.+���  ��Name$ TextBlockName�� �� Padding$ 5q�$��Center=�$��CharacterEllipsis=�$��NoWrap=�#���  
������ ���� *name+�� $ TextBoxName��#���  $��5q�$��Center=� �� TextWrapping$ NoWrap=�$��0q�  Background.HalfOutside#���  $y�	Collapsed=�$��1q� 	 Background.Highlight �� SelectionBrush# � 	  �� 
CaretBrush# �  ���� *name+�� $ StateEditNameButton��-   $��1��#��[  ��  �� ElementName$ TextBlockName������ $ TextBoxName��.����   ��Name$  StateEditNameButtonImage��$��/images/edit_16.png��$��16��$��16��� $ MoveItem��$��1�� �� �� .+��� -   $��0��$��up��$' !Переместить вверх�� 
 Canvas_Arrow_Top#��[
 #��[ �� -   $��1��$
��down��$% Переместить вниз��  Canvas_Arrow_Down#��[ #��[ �� $ SwtichRemovePanelButton��-   $��2��#��[ $��30��$��30��$ Удалить�� �� $ ControlMenuGrid��.���� $��/images/remove_16.png��$��16��$��16��! �� BitmapScalingMode$! NearestNeighbor=�� $	 RemovePanelGrid��$��3��$y�	Collapsed=�
 �� �� .+��� -   $��0��#��[ $��30��$��30��$- 'Подтвердить удаление��$��remove��.���� $��/images/ok_16.png��$��16��$��16��$! NearestNeighbor=��� -   $��1��#��[ $��30��$��30��$' !Отменить удаление�� �� $ ControlMenuGrid��.���� $��/images/cancel_16.png��$��16��$��16��$! NearestNeighbor=�� $ RightColumn��$��1��$��1�� �� $
��auto ��� .+�� $��0��
 �� �� $
��auto �.+��� -   $ NewItemSubTextBox��$	��100��$��5q�$��36���� -   $ NewItemSubAddButton��$��1��$��36��$��36��$��0,5,0,5q�#��[ #��[ �� -   $  SmartAnswers_Items_ListBox��$ Pixel=�$	��400��$��Disabled=�$��1��#���  $��0q�$ Single=����� " ��# [B��.���  � ��$ Stretch=�-   ���� .R�� $	 ControlMenuGrid��$��70�� �� *title+
 �� �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$
��auto��._��� #���  $��Center=�$��NoWrap=�$��CharacterEllipsis=�$ 5q�
������ ���� *title+� $ MoveItem��$��1�� �� �� .+��� -   $��0��$��up��$' !Переместить вверх��#��[ #��[
 �� -   $��1��$
��down��$% Переместить вниз��#��[ #��[ �� $ CopiedLabel��$��2��#���  $��Medium"�$��Center=��� $  CopyContentSmartMenuButton��-   $��3��#��[ $/ )Копировать содержание��$��30��$��30��  Canvas_Copy#���  $
��copy�� �� $ CopiedLabel���� $ SmartAnswersShowPopup��-   $��4��#��[ $��30��$��30��$5 /Редактировать содержание��$
��show�� �� $" SmartAnswers_Items_ShowPopup��.���� $��16��$��16��$! NearestNeighbor=����� *text+# [ �� $ SwtichRemovePanelButton��-   $��5��#��[ $��30��$��30��$ Удалить��$��remove�� �� $ ControlMenuGrid��.���� $��/images/remove_16.png��$��16��$��16��$! NearestNeighbor=�� $	 RemovePanelGrid��$��6��$y�	Collapsed=�
 �� �� .+��� -   $��0��$- 'Подтвердить удаление��#��[ $��30��$��30��$��remove��.���� $��/images/ok_16.png��$��16��$��16��$! NearestNeighbor=��� -   $��1��#��[ $��30��$��30��$' !Отменить удаление�� �� $ ControlMenuGrid��.���� $��/images/cancel_16.png��$��16��$��16��$! NearestNeighbor=�� " �Name$"" SmartAnswers_Items_ShowPopup��# � 	StaysOpen# .  |�. $ � AllowsTransparency$ . $
��Fade=�% � PlacementTarget% �� $ SmartAnswersShowPopup��.���� $	��400��$	��500��#���  ._��  �� $
��auto ��� �� $
��auto ��� $
��auto �.+�� $��0��#���   $��40��
 �� �� .+��� #���    Background.Inside.Blob#���  $��30��$��Center=�$��14(�$��5q�# � 	 $	��200��$��Center=����� *title+�� $ IsEditCheckBox��$��1��$��Center=�$��Center=�$��10,0,0,0q�#���  $��14(�$��Medium"�$5��/Запретить редактирование��$
 TrueY��� $��1��& �� Padding$& 10q�' �� CornerRadius$' 5��$��10q�#���  ._��� $
 Wrap=�$	��700��$	��500��"( �� VerticalScrollBarVisibility$
( Auto=�#���  # � 	 $��Stretch=�) �� AcceptsReturn) . ���� *text+* �� 
IsReadOnly* �� *	IsChecked+$ IsEditCheckBox���� $��2��$& 10q�$' 5��$��
10,0,10,10q�#���  ._��� $ 0q�$��0q�$
��Wrap=�#���  ��Здесь можно указать номер запроса в формате {N}, тогда при копировании он автоматически подставиться.�� ECНапример: Консультация по запросу {N}.�� .,{S} - любой текст в буфере.�� -   $��Закрыть��$��3��#���   $��14(�$��Center=�$��10q�#��[ ���� $" SmartAnswers_Items_ShowPopup���� -   $ WindowHideButton��$��2��$��40��$��14(�$��2��  Foreground.History#���  $��Закрыть��#��[ 