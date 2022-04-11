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
    /// Interaction logic for PdfEditWithoutToolsW.xaml
    /// </summary>
    public partial class PdfEditWithoutToolsW : Window
    {
        public PdfEditWithoutToolsW()
        {
            InitializeComponent();
            pdfViewControl.Load(ST.tempFileCopy);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pdfViewControl.LoadedDocument.Save(ST.tempFileCopy);
        }
    }
}
