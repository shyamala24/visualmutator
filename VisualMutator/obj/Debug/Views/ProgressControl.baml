   M S B A M L   `   `   `  ����  VisualMutator)    #VisualMutator.Views.ProgressControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          >�Name$  ProgressCtr��5   <    � 	Resources 5	       �� 
TargetType   FadeAnimation �� Triggers �� Binding �� ElementName �� Path �� Value N� EnterActions	 �� TargetProperty
 W� From	 W� To g� Duration��%(  &	        �� 5
   
   " �6    5      �� 5       6   �� $ ProgressCtr��$ 
Visibility� Visible6[    5      �� 5      .��5      �� .��5   "   W� $	 Opacity�62   $	
 0.0X�5   2   $	 1.0X�6=   $ 0:0:0.5D�6F   5      5      5      5      5      .��5      � #��[  6   .+�5   
   �� ���	#C0FFFFFF._�5      �� $��Center=�6   $��Center=�66   .+�5      �� $
��14pt(�6   $��
Loading...��6,   � 5       � IsIndeterminate . $��16��65   $
��0, 4q�6A   5      5       5!      5"      