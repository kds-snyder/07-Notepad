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
        public static string fileName = ""; // file name
        public static string fileNameWithoutPath = ""; 

        public static bool textEditorChanged = false; // Keep track text changed
        public static string fileNameFilter = 
            "Text files (*.txt)|*.txt";
        public static string projectTitle = "NotePad Project";
        public static string projectTitleFilePrefix = " -- File: ";

        public MainWindow()
        {
            InitializeComponent();
            this.Title = projectTitle;         
        }

        // Open file 
        // If text has changed, offer chance to save it;
        //   do not open file if user clicked cancel
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            if (textEditorChanged)
            {
                MessageBoxResult fileAction =
                  querySaveFile("Text has changed; save file?",
                         "Save file");
                if (fileAction != MessageBoxResult.Cancel)
                {
                    openFile();
                }
           }
           else
           {
                openFile();
           }
        }

        // Save text 
        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            // If file is open, write the text editor contents to it                      
            if (fileName.Length > 0)
            {
                saveTextEditor();
            }
            // If there is no open file,
            //   offer the choice of saving a file 
            else
            {
               MessageBoxResult fileAction = 
                   querySaveFile("There is no open file; save a new file?",
                                        "Save new file");
            }
        }

        //Exit application
        // If text has changed, offer the chance to save it, and
        //   exit if user did not click cancel
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (textEditorChanged)
            {
                MessageBoxResult fileAction = 
                    querySaveFile("Text has changed; save file?", 
                                       "Save file");
                if (fileAction != MessageBoxResult.Cancel)
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        // Text in editor has changed: set text changed indicator
        private void textBoxEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            textEditorChanged = true;
        }

        // Prompt user whether to save file
        // input: message and action for message box
        // output: message box result
        private MessageBoxResult querySaveFile(string message, string caption)
        {
            MessageBoxResult saveFileAction =
                MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel);
            switch (saveFileAction)
            {
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    saveFile();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
            return saveFileAction;
        }

        // Open file using Windows dialog,
        //   place file name in fileName, and
        //   reset text changed indicator
        private void openFile()
        {
            try
            {
                // Use Windows dialog to get file name
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = fileNameFilter;
                bool? openFileResult = openDialog.ShowDialog();

                // If file name obtained, open the file and display its 
                //  contents in textbox editor
                if (openFileResult == true)
                {
                    fileName = openDialog.FileName;
                    fileNameWithoutPath = openDialog.SafeFileName;
                    textBoxEditor.Text =
                        File.ReadAllText(fileName);
                    textEditorChanged = false;
                    this.Title = projectTitle + 
                        projectTitleFilePrefix + fileNameWithoutPath;
                }
            }
            catch (Exception except)
            {
                MessageBox.Show("An error occurred: " + except.Message);
            }
        }

        // Save file 
        // If file name is empty, get name with Windows dialog
        private void saveFile()
        {
            try
            {
                if (fileName.Length == 0)
                {
                    // Use Windows save dialog to get file name
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = fileNameFilter;
                    bool? saveFileResult = saveDialog.ShowDialog();

                    // If file name was specified, 
                    //   write the text editor contents to it 
                    if (saveFileResult == true)
                    {
                        fileName = saveDialog.FileName;
                        fileNameWithoutPath = saveDialog.SafeFileName;
                    }
                }
                saveTextEditor();
            }
            catch (Exception except)
            {
                MessageBox.Show("An error occurred: " + except.Message);
            }
       }

        // Save text editor contents in file specified by fileName
        //  and reset text changed indicator
        private void saveTextEditor()
        {
            try
            {
                File.WriteAllText(fileName, textBoxEditor.Text);
                textEditorChanged = false;
                this.Title = projectTitle + 
                    projectTitleFilePrefix + fileNameWithoutPath;
                MessageBox.Show(fileName + " has been saved");
            }
            catch (Exception except)
            {
                MessageBox.Show("An error occurred: " + except.Message);
            }
        }

    }
}
