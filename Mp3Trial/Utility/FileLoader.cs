using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Data;
using System.Windows.Forms;

namespace MusicPlayer.Utility
{
    public static class FileLoader
    {
        public static tblMedia Load()
        {
            try
            {
                var openFD = new OpenFileDialog();
                openFD.AddExtension = true;
                openFD.DefaultExt = "*.*";
                openFD.Filter = "Media Files (*.*)|*.*";
                openFD.ShowDialog();

                var mediaList = Load(openFD.FileName);
                if(mediaList.Count > 0)
                    return mediaList[0];
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return null;
        }

        private static List<tblMedia> Load(string filename)
        {
            return Load(new string[] { filename });
        }

        public static List<tblMedia> Load(string[] filenames)
        {
            var mediaList = new List<tblMedia>();

            foreach (var filename in filenames)
            {
                try
                {
                    if (!String.IsNullOrEmpty(filename))
                    {
                        var media = new tblMedia();
                        var music = TagLib.File.Create(filename); // imp!
                        media.Title = music.Tag.Title;
                        media.Location = filename;
                        media.Year = music.Tag.Year.ToString();

                        mediaList.Add(media);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }

            return mediaList;
        }
    }
}
