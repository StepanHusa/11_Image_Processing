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
            BitMiracle.Docotic.LicenseManager.AddLicenseData("49YKU-QSUJS-1T3EP-28V4L-FIFQY");

            InitializeComponent();

            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files";

                //ButtonNew_Click(ButtonNew, new RoutedEventArgs());
                this.WindowState = WindowState.Minimized;

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
                

                var value = doc.RecognizeTaggedBoxes(d01,listOfPages);



                //var p = new Process();
                //p.StartInfo.UseShellExecute = true;
                //p.StartInfo.FileName = d01;
                //p.Start();

                //p.StartInfo.FileName = Path.ChangeExtension(d01, "jpg");
                //p.Start();

            }
        }
        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {

            PdfEditW W = new(null);
            W.Show();
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "JPEG(*.jpg)|*.jpg",Title="Open JPG" };
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
            throw new NotImplementedException();
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

        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new(1, 1,2,2);
            bool a = rectangle.Logical();
            
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            var m = new PdfModifyW(filePath);
            m.Show();
        }
    }
}

         
    

