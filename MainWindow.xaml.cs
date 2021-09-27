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
//using System.Windows.Shapes;
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
        string filePath;

        public MainWindow()
        {
            //licencing PDFSharp and Syncfusion.PDFViewer
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc1MjU5QDMxMzkyZTMyMmUzMG5MSnFGODNPRngxVVVMcm9zRzVMRi9lZnRJc3JESzRtTEY4T2xMMi9USzg9");

            InitializeComponent();
        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {

            PdfEditW W = new(null);
            W.Show();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            filePath=openFileDialog.FileName;

            PdfEditW W = new(openFileDialog.FileName);
            W.Show();

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (filePath == null) { MessageBox.Show("no file to edit"); return; }
            PdfEditW W = new(filePath);
            W.Show();

        }

 


        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            HelloWorld();
        }

        private void HelloWorld()
        {
            PdfDocument doc = new();
            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new("Arial", 20);
            gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //string filename = "HelloWorld.pdf";
            filePath = Path.GetTempFileName();
            doc.Save(filePath);
            //Process.Start(filename);

        }

    }
}
