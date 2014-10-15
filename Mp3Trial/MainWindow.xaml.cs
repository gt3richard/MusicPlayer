using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagLib.Id3v2;
using MusicPlayer.Data;
using MusicPlayer.Controller;
using MusicPlayer.Utility;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Variables

        private const string PLAYPAUSE_LABEL_PLAY = "Play";
        private const string PLAYPAUSE_LABEL_PAUSE = "Pause";

        #endregion

        #region Public Properties
        
        public double Duration
        {
            get
            {
                if (this.MusicElement.NaturalDuration.HasTimeSpan)
                    return this.MusicElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                return 0;
            }
        }

        public TimeSpan Position
        {
            get
            {
                return this.MusicElement.Position;
            }
            set
            {
                this.MusicElement.Position = value;
            }
        }

        #endregion

        #region Constructor
        
        public MainWindow()
        {
            InitializeComponent();

            //Load Library
            LibraryController.Initalize(this);
            UpdateGrid(LibraryController.GetAllMedia());
            LibraryController.LibraryEvent.MediaModified += LibraryEvent_MediaAdded;

            //Load MediaController
            MediaController.Initalize(this);
            MediaController.MediaEvent.MediaPlay += MediaEvent_MediaPlay;
            MediaController.MediaEvent.MediaPause += MediaEvent_MediaPause;
            MediaController.MediaEvent.MediaStop += MediaEvent_MediaStop;
            MediaController.MediaEvent.MediaPositionChanged += MediaEvent_MediaPositionChanged;
        }

        #endregion

        #region Public Method

        public void UpdateMusicSource(tblMedia media)
        {
            MusicElement.Source = new Uri(media.Location);
        }

        #endregion

        #region Event Handler

        private void MediaEvent_MediaPositionChanged(object sender, EventArgs e)
        {
            UpdateSeekBarValue();
        }

        private void MediaEvent_MediaPause(object sender, EventArgs e)
        {
            btnPlayPause.Content = PLAYPAUSE_LABEL_PLAY;
            MusicElement.Pause();
        }

        private void MediaEvent_MediaStop(object sender, EventArgs e)
        {
            btnPlayPause.Content = PLAYPAUSE_LABEL_PLAY;
            MusicElement.Stop();
            UpdateSeekBarValue();
        }

        private void MediaEvent_MediaPlay(object sender, EventArgs e)
        {
            btnPlayPause.Content = PLAYPAUSE_LABEL_PAUSE;   
            MusicElement.Play();
        }

        private void LibraryEvent_MediaAdded(object sender, EventArgs e)
        {
            UpdateGrid(LibraryController.GetAllMedia());
        }
        
        /// <summary>
        /// Updates the position of the Media file is someone drags the slider position manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slideSeek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaController.Position = new TimeSpan(0, 0, 0, 0, (int)slideSeek.Value);
        }

        private void slideVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MusicElement.Volume = slideVolume.Value;
        }

        private void btnAddSong_Click(object sender, RoutedEventArgs e)
        {
            LibraryController.AddMedia();
        }   

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            MediaController.PlayPause();
        }      

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            MediaController.Stop();
        }

        /// <summary>
        /// Play a media file without adding it to the library
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMedia_Click(object sender, RoutedEventArgs e)
        {
            MediaController.PlayDontSave();
        }

        /// <summary>
        /// A different position on the grid is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblMediaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tblMedia obj = tblMediaDataGrid.SelectedItem as tblMedia;
                LibraryController.LibraryEvent.Changed(obj);
            }
            catch(Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }   

        /// <summary>
        /// A row in the grid was double clicked which should start playing that song.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tblMediaDataGrid_rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow dgr = sender as DataGridRow;
                tblMedia clickObj = dgr.Item as tblMedia;

                LibraryController.LibraryEvent.Changed(clickObj);

                MediaController.Play();
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }   

        /// <summary>
        /// If you've reached the end of the library, wrap around to the start of the library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNxt_Click(object sender, RoutedEventArgs e)
        {
            if (tblMediaDataGrid.SelectedIndex + 1 >= tblMediaDataGrid.Items.Count)
            {
                var media = tblMediaDataGrid.Items[0] as tblMedia;
                MediaController.Next(media);

                UpdateGridSelection(0);
            }
            else
            {
                var media = tblMediaDataGrid.Items[tblMediaDataGrid.SelectedIndex + 1] as tblMedia;
                MediaController.Next(media);

                UpdateGridSelection(tblMediaDataGrid.SelectedIndex + 1);
            }
        }   

        /// <summary>
        /// If you've reach the first media file in the library, wrap around to the last media file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (tblMediaDataGrid.SelectedIndex == 0)
            {
                var media = tblMediaDataGrid.Items[tblMediaDataGrid.Items.Count - 1] as tblMedia;
                MediaController.Prev(media);

                UpdateGridSelection(tblMediaDataGrid.Items.Count - 1);
            }
            else
            {
                var media = tblMediaDataGrid.Items[tblMediaDataGrid.SelectedIndex - 1] as tblMedia;
                MediaController.Prev(media);

                UpdateGridSelection(tblMediaDataGrid.SelectedIndex - 1);
            }
        }   

        /// <summary>
        /// Remove the media from the library if it is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            if (tblMediaDataGrid.SelectedItem != null)
            {
                tblMedia delObj = tblMediaDataGrid.SelectedItem as tblMedia;
                LibraryController.DeleteMedia(delObj);
            }
        }

        private void AddMedia_Click(object sender, RoutedEventArgs e)
        {
            LibraryController.AddMedia();
        }

        private void DelMedia_Click(object sender, RoutedEventArgs e)
        {
            if (tblMediaDataGrid.SelectedItem != null)
            {
                tblMedia delObj = tblMediaDataGrid.SelectedItem as tblMedia;
                LibraryController.DeleteMedia(delObj);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MediaController.Play();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            MediaController.Stop();
        }

        private void tblMediaDataGrid_PreviewDragEnter(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true) == true) //Check if a File
                e.Effects = System.Windows.DragDropEffects.All;
            else
                e.Effects = System.Windows.DragDropEffects.None;

            e.Handled = true;
        }

        private void tblMediaDataGrid_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            var filesPaths = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop, true);

            var files = FileLoader.Load(filesPaths);

            LibraryController.AddMedia(files);
        }

        private void CM_AddMedia_Click(object sender, RoutedEventArgs e)
        {
            LibraryController.AddMedia();
        }

        private void CM_RemoveMedia_Click(object sender, RoutedEventArgs e)
        {
            if (tblMediaDataGrid.SelectedItem != null)
            {
                tblMedia delObj = tblMediaDataGrid.SelectedItem as tblMedia;
                LibraryController.DeleteMedia(delObj);
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Updates the seek bar's size and slider position
        /// </summary>
        private void UpdateSeekBarValue()
        {
            slideSeek.Maximum = MediaController.Duration;
            slideSeek.ValueChanged -= slideSeek_ValueChanged;
            slideSeek.Value = MediaController.Position.TotalMilliseconds;
            slideSeek.ValueChanged += slideSeek_ValueChanged;
        }

        /// <summary>
        /// Refreshes the library list
        /// </summary>
        /// <param name="medias"></param>
        private void UpdateGrid(List<tblMedia> medias)
        {
            tblMediaDataGrid.ItemsSource = null;
            tblMediaDataGrid.ItemsSource = medias;
        }

        /// <summary>
        /// Updates the selected media in the library grid.
        /// </summary>
        /// <param name="index"></param>
        private void UpdateGridSelection(int index)
        {
            tblMediaDataGrid.SelectionChanged -= tblMediaDataGrid_SelectionChanged;
            tblMediaDataGrid.SelectedIndex = index;
            tblMediaDataGrid.SelectionChanged += tblMediaDataGrid_SelectionChanged;
        }

        #endregion

       

        
     
        

        

    }
}
