using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading.Tasks;
using MusicPlayer.Event;
using MusicPlayer.Data;
using MusicPlayer.Utility;

namespace MusicPlayer.Controller
{
    public static class MediaController
    {
        #region Public Variables

        public static MediaEvent MediaEvent = new MediaEvent();

        #endregion

        #region Private Variables

        private static DispatcherTimer _MediaTimer;
        private static MainWindow _MPWindow;

        #endregion

        #region Public Properties

        public static bool IsPlaying { get; set; }
        public static double Duration
        {
            get
            {
                return _MPWindow.Duration;
            }
        }
        public static TimeSpan Position
        {
            get
            {
                return _MPWindow.Position;
            }
            set
            {
                _MPWindow.Position = value;
            }
        }

        #endregion

        #region Private Properties

        private static bool IsLoaded { get; set; }

        #endregion

        #region Public Methods

        public static void Initalize(MainWindow app)
        {
            _MPWindow = app;
            LibraryController.LibraryEvent.MediaChanged += LibraryEvent_MediaChanged;
            IsLoaded = false;
        }

        /// <summary>
        /// Allows for one button that acts as a Play Pause toggle.
        /// </summary>
        public static void PlayPause()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        /// <summary>
        /// Plays the currently selected song in the library
        /// </summary>
        public static void Play()
        {
            Play(LibraryController.GetMedia());
        }

        /// <summary>
        /// Loads a new media file and then attempts to play it
        /// </summary>
        public static void PlayDontSave()
        {
            var media = FileLoader.Load();
            LibraryController.LibraryEvent.Changed(media);
            Play(media);
            
        }

        /// <summary>
        /// Plays the specified media. Raises the Play event
        /// </summary>
        /// <param name="media"></param>
        public static void Play(tblMedia media)
        {
            try
            {
                if (media == null || String.IsNullOrEmpty(media.Location))
                    throw new ArgumentNullException("Specified media to play is invalid.");

                if (!IsLoaded)
                {
                    _MPWindow.UpdateMusicSource(media);
                    IsLoaded = true;

                    _MediaTimer = new DispatcherTimer();
                    _MediaTimer.Interval = TimeSpan.FromMilliseconds(500);
                    _MediaTimer.Tick += new EventHandler(SeekBar_Slider_UpdateValue);
                    _MediaTimer.Start();
                }

                MediaEvent.Play();
                IsPlaying = true;
            }
            catch (ArgumentNullException ex)
            {
                return; //Don't play anything
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void Stop()
        {
            try
            {
                MediaEvent.Stop();
                Position = new TimeSpan(0, 0, 0, 0, 0);
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void Pause()
        {
            try
            {
                MediaEvent.Pause();
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void Prev(tblMedia media)
        {
            if (IsPlaying)
                Stop();

            IsLoaded = false;
            //Load previous song
            //Play
            Play(media);
        }

        public static void Next(tblMedia media)
        {
            if (IsPlaying)
                Stop();

            IsLoaded = false;
            //Load next song
            //Play
            Play(media);
        }

        #endregion

        #region Private Methods

        private static void LibraryEvent_MediaChanged(object sender, MediaChangedEventArgs e)
        {
            IsLoaded = false;
            LibraryController.SelectedMedia = e.Media;
        }

        private static void SeekBar_Slider_UpdateValue(object sender, EventArgs e)
        {
            //If you reach the end of the stop, raise the stop event.
            if (Duration > 0 && Duration == Position.TotalMilliseconds)
                Stop();

            MediaEvent.PositionChanged();
        }

        #endregion
    }
}
