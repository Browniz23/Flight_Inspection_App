using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

using OxyPlot;


namespace Flight_Inspection_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings s;
        
        public MainWindow()
        {
            InitializeComponent();
          //  Settings s = new Settings(" ");
            //vcvm = new videoControlsViewModel();
            //DataContext = vcvm;
        }
        /*        [DllImport("user32.dll")]
               public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (X.Visibility == Visibility.Hidden)   // needs to be only if settings are on
            {
                // open file dialog so user can pick a CSV file.
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                bool? response = openFileDialog.ShowDialog();
                if (response == true)
                {
                    String filePath = openFileDialog.FileName;
                    if (filePath.EndsWith("csv"))
                    {
           
                        Connect c = new Connect(filePath, s);                       //TODO: is tvm still updated????
                        c.ExecuteClient(filePath);

                        videoControl.setConnect(c);
                        ShowGraph1.setConnect(c);
                        //joystick.UpdateConnect(c);
                    }
                    else
                    {
                        MessageBox.Show("Please choose a CSV file");
                    }
                }
                /*System.Windows.Forms.SplitContainer sc = new System.Windows.Forms.SplitContainer();
                //sc.Handle   hWndOriginalParent = SetParent(hWndDocked, Panel1.Handle);
                fh1.Child.Controls.Add(sc); */
            } else
            {
                MessageBox.Show("Please insert XMl settings before uploading CSV file");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            button1.Background = Brushes.Aqua;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //     Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //     bool? response = openFileDialog.ShowDialog();
            //     if (response == true)
            //     {
            //         String filePath = openFileDialog.FileName;
            String filePath = "playback_small.xml";
                if (filePath.EndsWith("xml"))
                {
                    s = new Settings(filePath);                
                    s.UploadSettings();
                    X.Visibility = Visibility.Hidden;
                    V.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please choose a XML file");
                }
           // }
        }

        private void ShowGraph1_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
