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
using System.Windows.Shapes;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for SettingsW.xaml
    /// </summary>
    public partial class SettingsW : Window
    {
        public SettingsW()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ST.settingsWindow = null;
        }

        private void a_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                (sender as TextBox).MoveFocus(new TraversalRequest(0)); e.Handled = true;
            }
        }

        private void ag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (sender as TextBox).MoveFocus(new TraversalRequest(0));
                e.Handled = true;
                return;
            }
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            if (!(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = true;
            }



        }

        private void ag_LostFocus(object sender, RoutedEventArgs e)
        {
            while (ag.Text.Length < 8) ag.Text += "0";
            if (ag.Text.Length != 8) MessageBox.Show("invalid number");
            else
            {
                ST.fileCode = ag.Text.StringToByteArray();
            }
            
        }
    }
}

//TODO finish settings _ add all useful parameters _ add help section

//