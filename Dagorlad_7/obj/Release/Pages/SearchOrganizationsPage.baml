   M S B A M L   `   `   `  ���� L  HDagorlad, Version=7.122.7839.29141, Culture=neutral, PublicKeyToken=null3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  .    (Dagorlad_7.Pages.SearchOrganizationsPage   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:Dagorlad_7.Pages     -     N� Title$  SearchOrganizationsPage�� � 	Resources /   )Dagorlad_7.Pages.statusNameBrushConverter   statusNameBrushConverter��%   &	          .��� $��10q� � RowDefinitions �� $
��auto ��� .+�� 
 � Tag$ PanelOrganizationsControl��$��0,0,0,0q�$��0�� � ColumnDefinitions �� $
��auto ���  �� $
��auto ��� .+��� -    ��Name$, &DateUpdateDataBaseOfOrganizationsLabel��  
Foreground#���  $��0,0,0,5q�$��0��$��0q�$��0��$��Medium"�$��12(� � Language$ RU�$�� ���� $��1��$��10,0,0,0q��� -   $ ��http://webservice/org.aspx?�  Foreground.Highlight#��[ .�.��� �� $�Открыть�� �� $'�!поиск организаций��$��Medium"� �� $6�0в Веб-сервисе (Черная база)��� -    �Name$ 
SearchGrid��$��1��$��0,0,0,5q�$��2�� �� $
��auto ��� �� $
��auto ��� $
��auto �.+��� $.��(Поиск среди найденных��#���  $��12(�$��Center=�$��Medium"�$
��Wrap=�$��80���� -    ��Name$ SearchTextBox��  Background.Highlight	 �� SelectionBrush#	 �    Background.Inside.Blob#���  $��Center=�$��1���� -   
 ��Name$*
 $SearchOrganizationFromListViewButton��  ButtonStandart#��[ $��40��$��40��$��2�� � ToolTip$ 
Поиск��.����   Canvas_Search#��[ �� -   $2
 ,Cancel_SearchOrganizationFromListView_Button��#��[ $��40��$��40��$��3��$! Отменить поиск��.����   Canvas_Cancel#��[ �� $��1��$
��Auto=�.���� -    ��Name$ OrganizationsListView��  Background.Outside#���  $��Disabled=�
������  %� 
ScrollUnit$
 Item=����� .R�� ���� .R���  �� Orientation$ Vertical=�$	��460��$��5q�.+��  �� $
��auto ��� �� �� .+��� $��0�� �� Padding$ 5q����� *
statusName+ �� 	Converter# [  ._��  �� �� $
��auto ��� $
��auto �.+��� #���  $��Medium"�$��14(�$��Center=����� *
statusName+�� $ CopiedLabel��$��1��#���  $��Medium"��� -	   $��2�� 	 Canvas_Copy#��� 	 #��[  ��  �� ElementName$ CopiedLabel������ � $ Names��$��1�� �� $
��auto ��� $
��auto �.+��� $��0�� �� 
IsReadOnly .  �� TextWrapping$
 Wrap=�
������ $��Medium"�$��14(�#���  #	 �  ���� *
fullName+�� $��1�� . $
 Wrap=�
������ $��Normal"�$��12(� 
 Foreground.History#��� 
 #	 �  ���� *	shortName+ �� StringFormat$- 'Сокр. наименование: {0}��� $��2��$ Codes�� �� �� ��  �� �� .+��� $��0��$��0�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *inn+$ ИНН: {0}���� $��1��$��0�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *code+$6 0Код по сводному реестру: {0}���� $��2��$��0�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *ogrn+$ ОГРН: {0}���� $��0��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *kpp+$ КПП: {0}���� $��1��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *
okpoCode+$ ОКПО: {0}���� $��2��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *pgmu+$ ПГМУ: {0}��� $��3��$ AdditionalInformations�� �� �� ��  �� �� �� .+��� $��0��$��0��$��2�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *fio+$# Руководитель: {0}���� $��2��$��0��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *	recordNum+$4 .Регистрационный номер: {0}���� $��0��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *
cityName+$ Регион: {0}���� $��1��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *phone+$ Телефон: {0}���� $��2��$��1�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *mail+$ Почта: {0}���� $��0��$��2�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *
orfkCode+$ Код УФК: {0}���� $��1��$��2��$��3�� . $
 Wrap=�
������ #��� 
 #	 �  ���� *
orfkName+$ УФК: {0}��