   M S B A M L   `   `   `  ����H  DRemoteMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:RemoteMonitor.ViewRemoteMonitor.View  5      9!clr-namespace:RemoteMonitor.UtilsRemoteMonitor.Utils  5      %    RemoteMonitor.View.ConfigWindow   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:RemoteMonitor.View     5      8
converters!clr-namespace:RemoteMonitor.Utils     5        � 	Resources  5      3   -RemoteMonitor.Utils.BoolToVisibilityConverter   BoolToVisibilityConverter��%   &	          5   
   6/   5      .��5      � .+�5   
   �� $��Center=�6   $��Center=�62   .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� $ Vertical=�6   .+�5      �� $��
IP Address��6    
���   �$��20(�5      $
��Left=�5      $��0 10 0 5q�5      �� 5      -   5         ��Name$ ipAddressTextBox��5      $	��500��5      $��40��5      $��Center=�5      $
��Left=�5      $��0 0 0 0q�5      ��5      �� *	IpAddress+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5       �� 5"      $ Vertical=�6   .+�5#      �� $
��Port��6    
���   �$��20(�5%      $
��Left=�5&      $��	30 10 0 5q�5'      �� 5)      -   5        $ portTextBox��5)      $	��100��5*      $��40��5+      $��Center=�5,      $
��Left=�5-      $��30 0 0 0q�5/      ��5.      �� *Port+$ TwoWay=�$ PropertyChanged=�5/      51      52      �� 55      $��Connect��6   
�������$��14(�57      $��Medium"�58      $��0 50 0 5q�59      $��Center=�5:      ��5;      �� *ConnectCommand+��5<      �� 5=       �� 
TargetType" ��6   .�5>      ��  �� Property � �� �� Value �#35334067   �� 5?       �@�Cursor$
 Hand��63    �� Triggers 5@      P� 5A      	 P� Property	 �@J�IsMouseOver
 P� Value$

 True��6=   . �5B   "   ��  � �� �#FF4C706?   5C      5E      ��5H      �� 5I       �� 
TargetType" ��6&   .R�5J      �� $	��100��6!   $��40��5K   !    �� CornerRadius$ 10��5L   !   #��z��5M   !   ._�5N      �� $��Center=�6/   $��Center=�5O   -   5P      5Q      5S      �� 5U      $��Connexion réussie��6   
��� � �$��14(�5W      $��Center=�5X      $��0 10 0 0q�5Y      y�5Z      �� *HasConnectionSucceed+ �� 	Converter# [  5[      5\      5]      