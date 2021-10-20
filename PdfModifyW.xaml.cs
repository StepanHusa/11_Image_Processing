using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Syncfusion.Pdf;
//using System.Windows.Shapes;
//using BitMiracle.Docotic.Pdf;
using Syncfusion.Windows.PdfViewer;


namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for PdfModifyW.xaml
    /// </summary>
    public partial class PdfModifyW : Window
    {
        string tempPdf;

        public PdfModifyW(string fileName)
        {
            InitializeComponent();
            //load (create if needed) pdf file
            {
                string dir = Path.GetTempPath() + "Stepan_Husa_Is_The_Genius\\";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                tempPdf = dir + "tmp" + Path.GetRandomFileName().Remove(8) + ".pdf";
                if (fileName == null)
                {
                    NewPdfDoc(tempPdf);

                    this.Title = "*untitled";
                }
                else if (File.Exists(fileName))
                {
                    File.Copy(fileName, tempPdf);

                    this.Title = Path.GetFileName(fileName);
                }

                pdfDocView.LoadAsync(tempPdf);

            }

            pdfDocView.PageClicked += PdfDocView_PageClicked;
        }

        private void PdfDocView_PageClicked(object sender, Syncfusion.Windows.PdfViewer.PageClickedEventArgs args)
        {


            var doc = pdfDocView.LoadedDocument;
            int pindex = args.PageIndex;


            int size = 20;
            System.Drawing.PointF point = new((float)(args.Position.X * 0.75), (float)(args.Position.Y * 0.75));


            doc.AddSquareAt(pindex, point, size);
            doc.Save(tempPdf);
            pdfDocView.Load(tempPdf);
        }

        private void NewPdfDoc(string path)
        {
            PdfDocument doc = new();
            PdfPage page = doc.Pages.Add();
            doc.Save(path);
        }

    }
}
