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
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
using System.Diagnostics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using WordToPDF;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //licencing PDFSharp and Syncfusion.PDFViewer
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc1MjU5QDMxMzkyZTMyMmUzMG5MSnFGODNPRngxVVVMcm9zRzVMRi9lZnRJc3JESzRtTEY4T2xMMi9USzg9");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI2OTM3QDMxMzkyZTMzMmUzMGZrd0Izb241N05UeDB4Nk5PZUJweldpaG5CQUxkdDlMdnVuZXVWeG9SVXM9");

            BitMiracle.Docotic.LicenseManager.AddLicenseData("49YKU-QSUJS-1T3EP-28V4L-FIFQY");

            InitializeComponent();

            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files";

                this.WindowState = WindowState.Minimized;
                New_Click(new object(), new RoutedEventArgs());
                MenuEditOptions_AddBoxex_Click(new object(), new RoutedEventArgs());


                //PdfModifyW m = new(null);
                //m.Show();

                string d01 = debugFolder + @"\01.pdf";

                List<PointF> pointsPage1=new();
                List<PointF> pointsPage2 = new();
                List<Color> colors = new();

                colors.Add(Color.Black);
                colors.Add(Color.Gray);
                colors.Add(Color.White);
                for (int i = 0; i < 3; i++)
                {
                    pointsPage1.Add(new PointF(20, 20 + 40 * i));
                }
                d01.DebugCreateFile(pointsPage1,colors);

                List < List < PointF > >listOfPages = new();
                listOfPages.Add(pointsPage1);
                listOfPages.Add(pointsPage2);

                var doc = new PdfLoadedDocument(d01);
                switch (doc.Pages.Count - listOfPages.Count)
                {
                    case < 0:
                        while(doc.Pages.Count < listOfPages.Count)
                            doc.Pages.Add();
                        break;
                    case > 0:
                        while (doc.Pages.Count > listOfPages.Count)
                            listOfPages.Add(new List<PointF>());
                        break;
                }
                

                var value = doc.RecognizeTaggedBoxesDebug(d01,listOfPages);



                var p = new Process();
                p.StartInfo.UseShellExecute = true;

                //p.StartInfo.FileName = d01;
                //p.Start();

                //p.StartInfo.FileName = Path.ChangeExtension(d01, "jpg");
                //p.Start();

            }
        }


        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new(1, 1,2,2);
            bool a = rectangle.Logical();

        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog() { Filter = "PDF(*.pdf)|*.pdf", Title = "Open PDF" };
            if (open.ShowDialog() != true) return;
            var fileName = open.FileName;

            LoadDocument(fileName);
        }
        private void New_Click(object sender, RoutedEventArgs e)
        {
            LoadDocument(null);
        }



        //private void HelloWorld()
        //{
        //    PdfDocument doc = new();
        //    PdfPage page = doc.AddPage();
        //    XGraphics gfx = XGraphics.FromPdfPage(page);
        //    XFont font = new("Arial", 20);
        //    gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
        //    //string filename = "HelloWorld.pdf";
        //    filePath = Path.GetTempFileName();
        //    doc.Save(filePath);
        //    //Process.Start(filename);

        //}

        private void LoadDocument (string fileName)
        {
            var dir = ST.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string tempPdf = dir + "tmp" + Path.GetRandomFileName().Remove(8) + ".pdf";
            if (fileName == null)
            {
                PdfMethods.NewPdfDoc(tempPdf);

                this.Title = "*untitled";
            }
            else if (File.Exists(fileName))
            {
                File.Copy(fileName, tempPdf);

                this.Title = Path.GetFileName(fileName);
            }


            ST.tempFile = tempPdf;
            ST.fileName = fileName;
            ST.document = new(tempPdf);

            MenuEditOptions.IsEnabled = true;
            MenuSaveOptions.IsEnabled = true;
            MenuPrintOptions.IsEnabled = true;

        }


        private void MenuEditOptions_AddBoxex_Click(object sender, RoutedEventArgs e)
        {
            PdfEditW m = new();
            m.Show();
        }

        private void Word_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "Open Word Doc",Filter= "Word Document(*.doc;*docx)|*.doc;*docx" };
            if (!(bool)open.ShowDialog()) return;

            Word2Pdf objWorPdf = new Word2Pdf();

            string FromLocation = open.FileName;
            string FileExtension = Path.GetExtension(FromLocation);
            string ToLocation = Path.GetDirectoryName(FromLocation) + "\\" + Path.GetFileNameWithoutExtension(FromLocation) + "_(ConvertedFromWord)" + ".pdf";


            if (FileExtension == ".doc" || FileExtension == ".docx")
            {
                objWorPdf.InputLocation = FromLocation;
                objWorPdf.OutputLocation = ToLocation;
                objWorPdf.Word2PdfCOnversion();
            }
            else { MessageBox.Show("Invalid Input");return; }

            LoadDocument(ToLocation);
        }


        private void MenuPrintOptions_ToJPEG_Click(object sender, RoutedEventArgs e)
        {
            var b =ST.document.RecognizeTaggedBoxes(ST.pagesList);
        }

        private void MenuSaveOptions_Template_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

         
    

