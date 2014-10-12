using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Event;
using MusicPlayer.Data;
using MusicPlayer.Utility;

namespace MusicPlayer.Controller
{
    public static class LibraryController
    {
        #region Public Variables

        public static LibraryEvent LibraryEvent = new LibraryEvent();

        #endregion

        #region Private Variables

        private static MainWindow _Player;
        private static MusicPlayerEntities dbObj = new MusicPlayerEntities();    // for singleton

        #endregion

        #region Public Properties

        public static tblMedia SelectedMedia { get; set; }

        #endregion

        #region Public Methods

        public static void Initalize(MainWindow player)
        {
            _Player = player;
        }

        /// <summary>
        /// Displays the File Dialog box to load the media file and adds
        /// it to the database. Raises the Library modified event.
        /// </summary>
        public static void AddMedia()
        {
            try
            {
                var objAdd = FileLoader.Load();

                if (objAdd == null)
                    throw new ArgumentNullException("File was not loaded.");

                AddMedia(objAdd);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, Logger.LogType.MessageBox, Logger.LogLevel.Warning);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static void AddMedia(tblMedia mediaFile)
        {
            AddMedia(new List<tblMedia>() { mediaFile });
        }

        public static void AddMedia(List<tblMedia> mediaFiles)
        {
            try
            {
                foreach (var file in mediaFiles)
                {
                    dbObj.tblMedias.Add(file);  //adding a song to the library
                }
                
                dbObj.SaveChanges();            // saving schanges in the db

                LibraryEvent.Modified();
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Deletes the media object from the database and raises the 
        /// Library modified event.
        /// </summary>
        /// <param name="media"></param>
        public static void DeleteMedia(tblMedia media)
        {
            try
            {
                dbObj.tblMedias.Remove(media);
                dbObj.SaveChanges();

                LibraryEvent.Modified();
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public static List<tblMedia> GetAllMedia()
        {
            return dbObj.tblMedias.Select(x => x).ToList();
        }

        public static tblMedia GetMedia()
        {
            if (SelectedMedia == null)
                throw new ArgumentNullException("No media is selected in the library.");

            return SelectedMedia;
        }
        
        public static tblMedia GetMedia(int MID)
        {
            var media = (tblMedia)dbObj.tblMedias.Select(o => o.MId == MID);

            if (media == null)
                throw new ArgumentNullException(string.Format("No media found in the library with MID {0}.", MID));

            return media;
        }

        #endregion
    }
}
