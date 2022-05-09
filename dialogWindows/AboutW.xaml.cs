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

namespace _11_Image_Processing.dialogWindows
{
    /// <summary>
    /// Interaction logic for AboutW.xaml
    /// </summary>
    public partial class AboutW : Window
    {
        public AboutW()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process p = new();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = "https://github.com/StepanHusa/11_Image_Processing.git";
                        p.Start();

        }
    }
}
