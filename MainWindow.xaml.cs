using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using System.Drawing;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string tempPdf;
        string tempSetting;

        public MainWindow()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            InitializeComponent();
            tempPdf = System.IO.Path.GetTempFileName();
            HelloWorld();
        }

        private void ButtonOpenClearPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            var f=openFileDialog.FileName;

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = new ViewModel();
            PDFViewer.Visibility = Visibility.Visible;
        }

        private void HelloWorld()
        {
            PdfDocument doc = new();
            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new("Arial", 20);
            gfx.DrawString("Hello, World!", font, XBrushes.Black,new XRect(0, 0, page.Width, page.Height),XStringFormats.Center);
            string filename = "HelloWorld.pdf";
            doc.Save(filename);
            //Process.Start(filename);

        }
    }
}
