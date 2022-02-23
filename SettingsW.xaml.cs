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

namespace _11_StudentTester
{
    /// <summary>
    /// Interaction logic for SettingsW.xaml
    /// </summary>
    public partial class SettingsW : Window
    {
        private System.Drawing.Color baundColor;
        private System.Drawing.Color baundColorTwo;



        public SettingsW()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            color1.Background = new SolidColorBrush(Settings.baundColor.ColorToDrawing());
            color2.Background = new SolidColorBrush(Settings.baundColorTwo.ColorToDrawing());

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.settingsWindow = null;
        }

        private void IfEnterMoveFocus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {
                (sender as TextBox).MoveFocus(new TraversalRequest(0)); e.Handled = true;
            }
        }

        private void ag_KeyDown(object sender, KeyEventArgs e)
        {
            IfEnterMoveFocus_KeyDown(sender,e);
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
                Settings.fileCode = ag.Text.StringToByteArray();
            }
            
        }

        private void btnDialogApply_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }


        private void color1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            baundColor = cd.Color;
            color1.Background= new SolidColorBrush(baundColor.ColorToDrawing());
        }

        private void color2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            baundColorTwo = cd.Color;
            color2.Background = new SolidColorBrush(baundColorTwo.ColorToDrawing());

        }
    }
}

