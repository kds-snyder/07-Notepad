using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Notepad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string fileName = ""; // file name for all methods

        public MainWindow()
        {
            InitializeComponent();           
        }

        // Open file by using Windows open dialog box
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            // Use Windows dialog to get file name
            OpenFileDialog openDialog = new OpenFileDialog();
            bool? openFileResult = openDialog.ShowDialog();

            // If file name obtained, open the file and display its 
            //  contents in textbox editor
            if (openFileResult == true)
            {
                fileName = openDialog.FileName;
                textBoxEditor.Text =
                    File.ReadAllText(fileName);
            }           
        }

        // Save contents of text editor to the file 
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            // If file is open, write the text editor contents to it           
           
            if (fileName.Length > 0)
            {
                File.WriteAllText(fileName, textBoxEditor.Text);
                MessageBox.Show(fileName + " has been saved");
            }
            else
            {
                MessageBox.Show("There is no open file to save");
            }
        }

        //Exit notepad application
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
