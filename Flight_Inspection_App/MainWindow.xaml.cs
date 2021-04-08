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
        testViewModel tvm;
        Settings s;
        Connect c;
        public MainWindow()
        {
            InitializeComponent();
            //tvm = new testViewModel(new testModel(), s);
            s = new Settings("");
            c = new Connect("", s);
            tvm = new testViewModel(new testModel(30,40), c);
            DataContext = tvm;
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
                      //  BindingOperations.EnableCollectionSynchronization(tvm.vm_Chunks, this);
                        //          Connect c = new Connect(filePath, settings);
                        //          c.ExecuteClient(filePath);
                        c = new Connect(filePath, s);                       //TODO: is tvm still updated????
                        c.ExecuteClient(filePath);
                        tvm = new testViewModel(new testModel(60,70), c);
                        DataContext = tvm;
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
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                String filePath = openFileDialog.FileName;
                if (filePath.EndsWith("xml"))
                {
                    s = new Settings(filePath);                  //TODO: is tvm still updated????
                    s.UploadSettings();
                    tvm = new testViewModel(new testModel(80,90), new Connect("", s));
                    DataContext = tvm;
                    X.Visibility = Visibility.Hidden;
                    V.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Please choose a XML file");
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(tvm.vm_X);
            //tvm.vm_y = 20;
            //testBox.Text = Convert.ToString(tvm.vm_x);
        }

        private void testBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(tvm.vm_Y);
        }

        private void testBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Console.WriteLine(tvm.vm_Chunks["throttle"].values[tvm.vm_Chunks["throttle"].values.Count-1]);
            //     Console.WriteLine(tvm.vm_Chunks["throttle"].values[tvm.vm_CurrLine]);
            if (!tvm.vm_D.ContainsKey(15))
                tvm.vm_D.Add(15, 20);
            else
                Console.WriteLine(tvm.vm_Chunks["throttle"].Values[tvm.vm_CurrLine]);
        }

        private void ShowGraph1_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
