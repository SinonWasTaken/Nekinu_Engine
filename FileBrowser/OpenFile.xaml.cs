using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FileBrowser
{
    /// <summary>
    /// Interaction logic for OpenFile.xaml
    /// </summary>
    public partial class OpenFile : Window
    {
        public OpenFile()
        {
            InitializeComponent();
        }

        public static Dictionary<Stream, string> getFile(string title, string file_extension, string starting_directory)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Scene files (*" + file_extension + ") | *" + file_extension + "*";
            openFileDialog.InitialDirectory = starting_directory;
            openFileDialog.ShowDialog();
            try
            {
                Dictionary<Stream, string> a = new Dictionary<Stream, string>();
                a.Add(openFileDialog.OpenFile(), openFileDialog.FileName);
                return a;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error getting file! {0}", (object)ex));
                return null;
            }
        }

        public static string saveFile(string title, string save_extension, string default_directory)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = title;
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = save_extension;
            saveFileDialog.Filter = "Scene (*" + save_extension + ") | *" + save_extension + "*";
            saveFileDialog.ShowDialog();
            return saveFileDialog.FileName;
        }
    }
}
