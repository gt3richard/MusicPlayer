   M S B A M L   `   `   `  ����  Mp3Trial    MusicPlayer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F     � Title$  Music Player��5   	   $	��350��6   $	��500��6.   $	��829��6=   $	��589��6I   .��5      � $��Stretch=�6   $��Stretch=�6)   .+�5   
   �� $��10,43,10,10q�6   $��Stretch=�6&   $��Stretch=�6D   .+�5	      r� -   5         r�Name$ MusicElement��5	       r� LoadedBehavior$ Manual=�61    r� UnloadedBehavior$
 Stop=�6I   $	��407��6a   $	��357��6n   
 �� Top$ 10��6z    �� Left$ 10��6�   �� 5
      $��Stretch=�6   $��63��63   $	 422��6?   $	��801��6P   .+�5      �� -   5         ��Name$ 	slideSeek��5      $��Stretch=�6-   $��26��6q   $��Bottom=�6}    �� TickFrequency$ 0.001Q�6�    � SmallChange$ 0.001Q�6�   	 � Maximum$		 100Q�6�   $ 10��6�   $	��791��6�   �� 5      -   5        $ slideVolume��5      $��Right=�5      $��32��65   $��Bottom=�6A   $	��206��6\   
 � Value$
 1Q�5   C   $	 595��6M   $ 26��6_   �� 5      $��Volume :��6   $��Right=�6+   $��32��6G   $��Bottom=�6S   $��56��6n   $	 534��6y   $ 26��6�   �� 5      -   5         ��Name$ btnPlayPause��5      $
��Play��6/   $��Center=�6>   $��Bottom=�6[   $��32��5      $��46��6   $	 143��68   $ 26��6J   
 � Tag$
 Play��6Z   5      �� 5      -   5        $ btnStop��5      $
��Stop��6*   $��Center=�69   $��Bottom=�6V   $��32��5      $��30��6   $	 244��63   $ 26��6E   �� 5      -   5        $ 
btnAddSong��5   b   $��Add Song��6   $
��Left=�6,   $��Bottom=�6G   $��60��5      $��32��6   $ 26��66   $	 279��6F   �� 5      -   5        $��btnNxt��5      $
��Next��6'   $��45��66   $	 194��6A   $ 26��6S   $��32��6c   6o   �� 5      -   5        $ btnPrev��5      $
��Prev��6*   $��45��69   $ 93��6D   $ 26��6U   $��32��6e   6q   �� 5      -	   5        $ btnDeleteSong��5   e   $��Delete Song��6   $
��Left=�6/   $��Bottom=�6J   $��72��5      $��32��6   $ 26��6E   $	 344��6U   5      &   System.Windows.Controls.DataGrid  5      -
   5          Name$ tblMediaDataGrid��5         AutoGenerateColumns .     EnableRowVirtualization . $	��397��6l   $	 382��5   /      RowDetailsVisibilityMode$ VisibleWhenSelected=�6A      
IsReadOnly . $ 20��5      $	��419��6'   $��Center=�63   $��Center=�6W    J� 	AllowDrop . ��5      ��    RowStyle 5!      �� 5"       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5#      -   5        5$         Columns 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      -   5        titleColumn��,  &System.Windows.Controls.DataGridColumn   Header$ Title��5'   X      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6g   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6>   �� *Title+6g     5(      -   5        albumColumn��$ Album��5(   X   $ * 6g    6>   �� *Album+6g     5)      -   5        artistColumn��$ Artist��5)   _   $ * 6o    6?   �� *FirstArtist+6o     5*      -   5        genreColumn��$ Genre��5*   ^   $ * 6m    6>   �� *FirstGenere+6m     5+      -   5        
yearColumn��$
 Year��5+   V   $ * 6d    6=   �� *Year+6d     5,      -   5        commentsColumn��$ Comments��5,   ^   $ * 6p    6A   �� *
Comments+6p    � ContextMenu 5.      �� 5/      .@�50      m� -   5        $��	Add Media��50   #   6R   m� 51      -   5        $��Remove Media��51   #   6X   52      54      56      o� 57   
   $
��Left=�6   $��22��6*   $	��Top=�66   $	��821��6N   .@�58      m� -   5         m�Name$
 Menu��58      $
��Menu��6%   .@�59      m� -   5        $ 	OpenMedia��59      $��
Open Media��6.   $
��Left=�6B   $	��180��6]   6i   m� 5:      -   5        $ AddMedia��5:      $��	Add Media��6-   $
��Left=�6@   $	��180��6[   6g   m� 5;      -   5        $ DelMedia��5;      $��Remove Media��6-   $
��Left=�6C   $	��180��6^   6j   �� 5<      m� 5=      -   5        $
 Exit��5=      $
��Exit��6)   $
��Left=�67   $	��180��6R   6^   5>      m� 5?      -   5        $ Playback��5?      $��Playback��6)   .@�5@      m� -   5        $
 Play��5@      $
��Play��6)   $
��Left=�67   $	��180��6R   6^   m� 5A      -   5        $
 Stop��5A      $
��Stop��6)   $
��Left=�67   $	��180��6R   6^   5B      5C      5D      5E      