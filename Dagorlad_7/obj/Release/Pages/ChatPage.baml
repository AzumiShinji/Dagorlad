   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  7 clr-namespace:Dagorlad_7.classesDagorlad_7.classes  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel     Dagorlad_7.Pages.ChatPage   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  *localclr-namespace:Dagorlad_7.Pages  +clss clr-namespace:Dagorlad_7.classes  McomponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase    N� Title$  ChatPage�� � 	Resources .   (Dagorlad_7.Pages.NullableContentToHidden   NullableContentToHidden1   +Dagorlad_7.Pages.BooleanVisibilityConverter  BooleanVisibilityConverter+   %Dagorlad_7.Pages.TransparentIfSticker  TransparentIfSticker'   !Dagorlad_7.Pages.FindHandlingLink  FindHandlingLink  src �� IsLiveSortingRequested �� Source �� SortDescriptions+  %System.ComponentModel.SortDescription   PropertyName   	Direction��%�   &	        &	      &	 
     &	      &	              ��  .  ��    $ user.CountUnreaded��$ 
Descending=�  $ 
user.Email��$ 	Ascending=�.��� -    �Name$ MainGrid�� � ColumnDefinitions �� $
��auto �$	��310��$	��450���� $
��auto ��� 	 � RowDefinitions	 �� $
��auto ��� $
��auto ��� �� $
��auto �.+�� 

 � Tag$
 InformationBlock��$��3��$��0��$��Stretch=�$
��auto��  Background.Highlight#���  .+�� $��Center=� �� $
��auto ��� $
��auto �.+��� -    ��Name$ InformationBlockLabel��$��Center=�$��Stretch=�$
��auto��$��13(�$��Medium"�  
Foreground#���  y���  �� RelativeSource �� *Self+ �� Path$ Content� �� 	Converter# [  �� -    ��Name$ HandeConnectButton��  ButtonStandart#��[ $y�	Collapsed=�$��1��#���  .���� $��Подключиться��$��	Underline{�� -   $ AdditionalBlock��$��2��$��1��  Background.Outside#���  $��Stretch=�$��42�� �� $
��auto ��� $
��auto ��� $
��auto �.+�� $��0��	 �� �� .+��� -   $ 	NameLabel��$��Medium"�$��0��$��Center=�$��10,0,0,0q�$��13(�#���  ���� *Name+�� -   $ DirectionLabel��$��Medium"�$��1��$y�Visible=�$��Center=�$��10,0,0,0q�$��10(� 	 Foreground.History#��� 	 ���� *	Direction+�� -   $ TypingLabel��$��Medium"�$��1��$y�Hidden=�$��печатает...��$��Center=�$��10,0,0,0q�$��10(�#���  � -   $ InformationClientGrid��$��1��$
��Left=� � DataContext �� *client_employee_info+.+��� -	   $$ ShowPopupInfoAboutClientButton�� 
 Canvas_Info#��[
 $��10,0,0,0q�#��[ ���� *ActualHeight+ �� *Self+� -
    �Name$ PopupInfoAboutClient�� � AllowsTransparency . $
��Fade=� � 	StaysOpen .   � PlacementTarget ��  �� ElementName$$ ShowPopupInfoAboutClientButton��.����   Background.HalfOutside#���   �� Padding$ 10q� �� CornerRadius$ 5��$��3q�  Background.Inside.Blob#���  ._��   �� 
TargetType �� BasedOn �� Property �� Value��%�   '��       7���� " ��8   .���  � ��# �  ��  � ��# �  ��  �@��SelectionBrush# �  ��  � ��$ 0q���  � ��$ 5q���  �@��
IsReadOnly . 	 �� $
��auto ��� �� �� �� �� �� �� �� �� .+��� -    ��Name$! InformationAboutClientImage��$
��None=��� $��1������ *Code+ �� StringFormat$ Код: {0}���� $��2������ *Email+$ 
Email: {0}���� $��3������ *Name+$ ФИО: {0}���� $��4������ *	Direction+$! Направление: {0}����  � Language$ RU�$��5������ *	BirthDate+$, &День рождения: {0:dd MMMM}���� $��6������ *Phone+$ Телефон: {0}���� $��7������ *
Position+$ Должность: {0}���� $��8������ *Rest+$ Отпуска: {0}���� -    ��Name$  InformationClientDateEnter��$��Medium"�$��12(�$ RU�$��9���� -     ��Name$  UsersListView��$��0��$��1��$��3��$��0,0,0,0q�#���  $��0q����� ! �� Source#! [ ���� " ��# [B��.�-   ���� .R��� " ��Name$" Border��$
��auto��$��5,2,10,2q�._�� $��Center=� �� $
��auto ��� �� $
��auto �	 �� $
��auto ��� $
��auto �.+��� $��0,0,5,0q�$��30��$��30��$��2��._��� $ UserLogoImage��# �� BitmapScalingMode$# HighQuality=����� *image+� $��1�� �� $
��auto ��� .+��� $��0��$��Medium"�
��� E��$��10(�$��Center=����� *user.Status+�� $ ��Name$$ NameTextBlock��$��1��$��Medium"�$��14(�#���  $��Center=����� *	user.Name+�� $$ LastMessageTextBlock��$��1��$��1��$��CharacterEllipsis=�$
��Left=�$	��220��$��16��$��11(�#��� 	 ���� *ActualWidth+$ Border������ *user.LastMessage+�� $" UnreadedCountBorder��$��2��$��2��$��Right=�$	 100��
��� E��$��20��$��20��$��5,0,0,0q�y��� *user.CountUnreaded+# [  ._��� $$ UnreadedCountTextBlock��#���  $��Medium"�$��Center=�$��Center=����� *user.CountUnreaded+�� $��1��$��1��$��Stretch=�$��1��|�.  $��4��#���  $��0q�� $
 DialogListViewGrid��$��2��$��2������ -   BackgroundChatDialog��% j� TileMode$
% None=�& j� Stretch$& UniformToFill=�' �� ImageSource' �� <   6Dagorlad_7.classes.BitmapImageClassBackgroundChatClass(   Instance#! Z( $ BitmapImage�) �� Mode$) OneWay=�* �� UpdateSourceTrigger$* PropertyChanged=�+ �� TargetNullValue+ Z� .+��� -   $  DialogListView��, %� 
ScrollUnit$, Pixel=�$��Disabled=�
������ $��0q����� *msgs+���� " ��  ListBoxItemDialog# [ .�-   ��  �@�FocusVisualStyle Z� ��  � ��$ Stretch=����� .R��� $ 5,5,0,5��$ 10,10,10,10q�$	��350������ *	IsSticker+# [ ._�� ���� " �- �� Triggers- :� . :� RoutedEvent$. Loaded��.[��� .���� .��W� / �� TargetProperty$/ Opacity�0 g� Duration$0 0:0:0.2D�1 W� From$	1 0.0X�	2 W� To$	2 1.0X�	 �� �� �� $
��auto ��� $
��auto �.+�� $��0�� �� �� $
��auto �.+��� $��0��#��� 	 $��0q�$��10(�$��Center=�$��Medium"����� *
SenderName+�� $��1��$ RU�$��8(�$��0q�
������ #��� 	 ���� *Time+$  ���� 3 �� 
IsReadOnly3 . $��1��$��0q�$��0q�4 �� IsDocumentEnabled4 . $��0,5,0,0q�
������ #���  5 �� SelectionBrush#5 �  y��� # [ 6 �� ConverterParameter$
6 text��
 ��  �� *Self+# [  7 �� 	Resources��%H   '��       �� " ��7 ���� '��       " ��.���  � ��# �  �� $��5q�$
��None=�$	��256��$	��192��"  WpfAnimatedGif.ImageBehavior8   RepeatBehavior$8 Forever��$��2��9   AnimatedSource9 �� *LinkSticker++ Z� y��� # [ $6 sticker���� $��3��
������ $��0q�: �� SelectionMode$: Single=����� *	FileLinks+y��� # [ $
6 file������ " ��.���  � �� �� ; �� 
TargetType"; ��.R��� $��Border��$ 2q�< J� SnapsToDevicePixels< . 
������ $��0q�._��� = �� Triggers= P� > P� Property> �@��
IsSelected? P� Value? . . ��� @ �� 
TargetName$@ Border�� � ��
 ���� ���� .R��� -   $��1��#��[ $��0q�#��� 	 $��Center=����� *Value+���� $ 5q�._��  �� $
��auto ��� .+���   Canvas_Attachments#��[ $��Center=��� $��1��A �� Padding$A 5,0,0,0q�$��Center=����� *Key+� -   $ MessageSendingGrid��$��3��$��2��#���   �� $
��auto ��� $
��auto ��� �� $
��auto �.+��� -   $ StickersPopupChooseButton��$��40��$��40��#��[ $��Bottom=�  Canvas_Stickers#��[ $��0��$��1���� -   $ AttachmentSendButton��$��40��$��40��#��[ $��Bottom=�#��[ $��1��$��1���� -   $ MessageTextBox��$��2��$
��auto��$	��300��$��Center=�B �� AcceptsReturnB . #5 �  $��1���� -   $ SendMessageButton��$��40��$��40��#��[ $��Bottom=�  Canvas_Send#��[ $��3��$��1���� .+�� -   $ StickersPopup�� .  $	��650��$	��400��$y�	Collapsed=� . $
��Fade=� �� $ MessageSendingGrid��.���� #���  $ 5q�$ 5��._��� -   C ��Name$C StickersTabControl��D �� TabStripPlacement$
D Left=�$��Visible=�#���  $��0q����� ���� " ��.���  � ��
 ���� ��  � �� �� *name+��  � �� �� "; ��.R�� .+��� $" border��$��1q�#���  $ 0q�._��� E ��Name$E ContentSite��$��Center=�$��Header��#���  $��0q���. = P� > �@��
IsSelected? . . ���   Background.Inside# �  $@ border�� � ������ "; ��.R��  �� $
��Auto ��� .+��� $
��Auto=�$��Hidden=�.���� F ��Name$F HeaderPanel��G K� ZIndex$G 1��-  'System.Windows.Input.KeyboardNavigationH   TabIndex$H 1��$��LeftToRight=�$��0��$��0��$��2,2,2,0q�I K� IsItemsHostI . �� $E PART_SelectedContentHost��#< z< #��z��$��SelectedContent��$��1��J �� ContentTemplateJ �� .R��� .���� ���� *items+���� .R�� ���� .R��� .+��� -   #��[ ���� *link+.���� $	��120��$	��120��$��Uniform=����� *image+