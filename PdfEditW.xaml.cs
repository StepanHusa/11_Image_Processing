﻿using System;
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

namespace _13_Testing_Software_PNGused
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
            else if (!File.Exists(fileName)) { MessageBox.Show("File not fount");this.Close(); }
  
            this.Title = Path.GetFileName(fileName);
            Debug.WriteLine(new FileInfo(fileName).Length);


            //BitmapImage bitmap = new();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri(fileName);
            //bitmap.EndInit();
            //ImageControl.Source = bitmap;
            
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

        private void pdfwcontrol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sen = (PdfViewerControl)sender;
            var t=e.GetPosition(sen);
        }

    }
}
