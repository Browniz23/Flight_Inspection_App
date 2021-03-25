﻿using System;
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

namespace Flight_Inspection_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*        [DllImport("user32.dll")]
               public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);*/
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // open file dialog so user can pick a CSV file.
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();
            if (response == true)
            {
                String filePath = openFileDialog.FileName;
                if (filePath.EndsWith("csv"))
                {
                    Connect.ExecuteClient(filePath);
                }
                else
                {
                    MessageBox.Show("Please choose a CSV file");
                }
            }
            /*System.Windows.Forms.SplitContainer sc = new System.Windows.Forms.SplitContainer();
            //sc.Handle   hWndOriginalParent = SetParent(hWndDocked, Panel1.Handle);
            fh1.Child.Controls.Add(sc); */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            button1.Background = Brushes.Aqua;
        }
    }
}
