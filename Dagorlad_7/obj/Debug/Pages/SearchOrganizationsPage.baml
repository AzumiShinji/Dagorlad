   M S B A M L   `   `   `  ����J  FDagorlad, Version=0.0.7846.16415, Culture=neutral, PublicKeyToken=null3clr-namespace:Dagorlad_7.PagesDagorlad_7.Pages  5      .    (Dagorlad_7.Pages.SearchOrganizationsPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Dagorlad_7.Pages     5      -   5          N� Title$  SearchOrganizationsPage��5
       � 	Resources 5      /   )Dagorlad_7.Pages.statusNameBrushConverter   statusNameBrushConverter��%   &	          5   
   6)   5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   � 
 � Tag$ PanelOrganizationsControl��6   $��0,0,0,0q�6/   $��0��6@    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5       5      �� 5      $
��auto �6    �� 5      .+�5      �� -   5         ��Name$, &DateUpdateDataBaseOfOrganizationsLabel��5        
Foreground#���  6D   $��0,0,0,5q�5      $��0��6%   $��0q�5      $��0��6    $��Medium"�6-   $��12(�6A    � Language$ RU�6O   $�� ��6]   �� 5      $��1��6   $��10,0,0,0q�6(   �� 5       -   5        $ ��http://webservice/org.aspx?�5    !     Foreground.Highlight#��[ 6v   .�6�   .�5!      �� �� 5"      $�Открыть��6#    �� 5#      $'�!поиск организаций��6#   $��Medium"�6<    �� 5$      $6�0в Веб-сервисе (Черная база)��6#   5%      5&      5'      � 5(      -   5         �Name$ 
SearchGrid��5(       $��1��6   $��0,0,0,5q�64   $��2��6E    5)      �� 5*      $
��auto �6'   �� 5+      �� 5,      $
��auto �6'   �� 5-      $
��auto �6'   .+�5/      �� $.��(Поиск среди найденных��6   #���  69   $��12(�6c   $��Center=�50      $��Medium"�67   $
��Wrap=�6K   $��80��6_   �� 51      -   5         ��Name$ SearchTextBox��51        Background.Highlight	 �� SelectionBrush#	 �  61     Background.Inside.Blob#���  52      $��Center=�6L   $��1��6n   �� 53      -   5        
 ��Name$*
 $SearchOrganizationFromListViewButton��53        ButtonStandart#��[ 6G   $��40��6r   $��40��6}   $��2��6�    � ToolTip$ 
Поиск��54   F   .��55      ��   Canvas_Search#��[ 6'   56      �� 57      -   5        $2
 ,Cancel_SearchOrganizationFromListView_Button��57      #��[ 6O   $��40��6z   $��40��6�   $��3��6�   $! Отменить поиск��58   M   .��59      ��   Canvas_Cancel#��[ 6'   5:      5;      5<      �� 5=   
   $��1��6   $
��Auto=�6$   .��5>      �� -   5         ��Name$ OrganizationsListView��5>        Background.Outside#���  6:   $��Disabled=�5?      
������  %� 
ScrollUnit$
 Item=�5@   0   ��5A      �� 5B      .R�5C      � 5D      ��5F      �� 5G      .R�5H      ��  �� Orientation$ Vertical=�6%   $	��460��6<   $��5q�6H   .+�5I      �  5J      �� 5K   "   $
��auto �60   �� 5L   "   �� 5M   "   �� 5N   "   .+�5P      �� $��0��6%    �� Padding$ 5q�6�   ��62   �� *
statusName+ �� 	Converter# [  ._�5Q   "   �  5R   &   �� 5S   *   �� 5T   *   $
��auto �6;   �� 5U   *   $
��auto �6;   .+�5W   &   �� #���  6L   $��Medium"�5X   0   $��14(�6D   $��Center=�5Y   0   ��5W   0   �� *
statusName+5Y   0   �� 5Z   &   $ CopiedLabel��6<   $��1��6,   #���  6Q   $��Medium"�6{   �� 5[   &   -	   5        $��2��5[   -    	 Canvas_Copy#��� 	 6=   #��[ 5\   J    5[   e   ��  �� ElementName$ CopiedLabel����5\   -   �� 5]   -   5^   #   5_      � 5`      $ Names��6#   $��1��6/    5a   "   �� 5b   &   $
��auto �64   �� 5c   &   $
��auto �64   .+�5e   "   �� $��0��6*    �� 
IsReadOnly .  �� TextWrapping$
 Wrap=�5f   Q   
������ $��Medium"�5g   *   $��14(�6>   #���  5h   *   #	 �  6T   ��5f   7   �� *
fullName+5h   T   �� 5i   "   $��1��6*    . $
 Wrap=�5j   y   
������ $��Normal"�5k   *   $��12(�6>    
 Foreground.History#��� 
 5l   *   #	 �  6\   ��5j   7   �� *	shortName+ �� StringFormat$- 'Сокр. наименование: {0}��5l   \   5m      � 5n      $��2��6#   $ Codes��60    5o   "   �� 5p   &   �� 5q   &   �� 5r   &    5t   "   �� 5u   &   �� 5v   &   .+�5x   "   �� $��0��6*   $��0��6:    . $
 Wrap=�6�   
������ #��� 
 5y   *   #	 �  6\   ��5x   Y   �� *inn+$ ИНН: {0}��5y   \   �� 5z   "   $��1��6*   $��0��6:    . $
 Wrap=�6�   
������ #��� 
 5{   *   #	 �  6\   ��5z   Y   �� *code+$6 0Код по сводному реестру: {0}��5{   \   �� 5|   "   $��2��6*   $��0��6:    . $
 Wrap=�6�   
������ #��� 
 5}   *   #	 �  6\   ��5|   Y   �� *ogrn+$ ОГРН: {0}��5}   \   �� 5~   "   $��0��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5   *   #	 �  6\   ��5~   Y   �� *kpp+$ КПП: {0}��5   \   �� 5�   "   $��1��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *
okpoCode+$ ОКПО: {0}��5�   \   �� 5�   "   $��2��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *pgmu+$ ПГМУ: {0}��5�   \   5�      � 5�      $��3��6#   $ AdditionalInformations��60    5�   "   �� 5�   &   �� 5�   &   �� 5�   &    5�   "   �� 5�   &   �� 5�   &   �� 5�   &   .+�5�   "   �� $��0��6*   $��0��6:   $��2��6G    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   m   �� *fio+$# Руководитель: {0}��5�   \   �� 5�   "   $��2��6*   $��0��6:   $��1��6G    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   m   �� *	recordNum+$4 .Регистрационный номер: {0}��5�   \   �� 5�   "   $��0��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *
cityName+$ Регион: {0}��5�   \   �� 5�   "   $��1��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *phone+$ Телефон: {0}��5�   \   �� 5�   "   $��2��6*   $��1��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *mail+$ Почта: {0}��5�   \   �� 5�   "   $��0��6*   $��2��6:    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   Y   �� *
orfkCode+$ Код УФК: {0}��5�   \   �� 5�   "   $��1��6*   $��2��6:   $��3��6G    . $
 Wrap=�6�   
������ #��� 
 5�   *   #	 �  6\   ��5�   m   �� *
orfkName+$ УФК: {0}��5�   \   5�      5�      5�      5�      5�      5�      5�      5�      