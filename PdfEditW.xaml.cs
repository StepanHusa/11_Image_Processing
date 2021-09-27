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
//using System.Windows.Shapes;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for PdfEditW.xaml
    /// </summary>
    public partial class PdfEditW : Window
    {
        string tempPdf;

        public PdfEditW(string fileName)
        {
            InitializeComponent();

            if (fileName == null)
            {
                tempPdf = Path.ChangeExtension(Path.GetTempFileName(), "pdf");
                NewPdfDoc(tempPdf);
                fileName = tempPdf;
            }

            InitializeComponent();


            //PdfDocument doc = PdfSharp.Pdf.IO.PdfReader.Open(fileName);
            //PdfPage page = doc.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            //XFont font = new("Arial", 20);
            //gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            //doc.Save(fileName);

            //this.DataContext = new ViewModel(tempPdf);
            //PDFViewer.Visibility = Visibility.Visible;

            this.Title = Path.GetFileName(fileName);
            Debug.WriteLine(new FileInfo(fileName).Length);
            if (new FileInfo(fileName).Length < Math.Pow(10, 6))
            {
                PdfLoadedDocument pdf = new PdfLoadedDocument(fileName);
                
                pdfwcontrol.Load(pdf);
            }
            else pdfwcontrol.Load(fileName);
        }
        private void NewPdfDoc(string path)
        {
            PdfDocument doc = new();
            PdfPage page = doc.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            //XFont font = new("Arial", 20);
            //gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            doc.Save(path);
        }

        //private void DeleteTemp()
        //{
        //    if(tempPdf!=null) { File.Delete(tempPdf);tempPdf = null; }
        //}

    }
}
