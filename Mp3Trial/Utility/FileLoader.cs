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
            tblMedia objAdd = new tblMedia();

            try
            {
                var openFD = new OpenFileDialog();
                openFD.AddExtension = true;
                openFD.DefaultExt = "*.*";
                openFD.Filter = "Media Files (*.*)|*.*";
                openFD.ShowDialog();
            
                if(!String.IsNullOrEmpty(openFD.FileName))
                {
                    var music = TagLib.File.Create(openFD.FileName); // imp!
                    objAdd.Title = music.Tag.Title;
                    objAdd.Location = openFD.FileName;
                    objAdd.Year = music.Tag.Year.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }

            return objAdd;
        }
    }
}
