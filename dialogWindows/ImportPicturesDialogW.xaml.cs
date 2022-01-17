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
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class ImportPicturesDialogW : Window
    {
        int loadedInt;
        public ImportPicturesDialogW(int loaded)
        {
            InitializeComponent();

            loadedInt = loaded;
            filesInfo.Content = loaded;
            sldAnswer.Maximum = loaded;
            sldAnswer.Value = ST.pagesOfDocument;
            CalculateRest();
            sldAnswer.ValueChanged += SldAnswer_ValueChanged;

        }

        private void SldAnswer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CalculateRest();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CalculateRest()
        {
            works.Content = (int)(loadedInt/sldAnswer.Value);
            int g = loadedInt - (int)works.Content * (int)sldAnswer.Value;
            left.Content = g;
            if (g != 0) left.Background = Brushes.Red;
            else left.Background = Brushes.Green;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            textAnswer.SelectAll();
            textAnswer.Focus();
        }

        public int[] Answer
        {
            get {
                
                var a = new int[(int)works.Content];
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = (int)sldAnswer.Value;
                }
                return a; 
            }
        }
        public bool? Invert
        {
            get { return invert.IsChecked; }
        }
    }
}
