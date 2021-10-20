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

namespace _13_Testing_Software_PNGused
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



                var p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = d01;
                p.Start();

                p.StartInfo.FileName = Path.ChangeExtension(d01, "jpg");
                p.Start();

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
            var m = new PdfModifyW(null);
            m.Show();

        }
    }
}
        private void ButtonRun_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new(1, 1,2,2);
            bool a = rectangle.Logical();
            
        }

         
    }
    static class PDFExtensions
    {
        static void ToPNG(string filename)
        {
            PdfDocument document = PdfReader.Open(filename);

            int imageCount = 0;
            // Iterate pages
            foreach (PdfPage page in document.Pages)
            {
                // Get resources dictionary
                PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                if (resources != null)
                {
                    // Get external objects dictionary
                    PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
                    if (xObjects != null)
                    {
                        ICollection<PdfItem> items = xObjects.Elements.Values;
                        // Iterate references to external objects
                        foreach (PdfItem item in items)
                        {
                            PdfReference reference = item as PdfReference;
                            if (reference != null)
                            {
                                PdfDictionary xObject = reference.Value as PdfDictionary;
                                // Is external object an image?
                                if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                                {
                                    ExportImage(xObject, ref imageCount);
                                }
                            }
                        }
                    }
                }
            }
        }
        static void ExportImage(PdfDictionary image, ref int count)
        {
            string filter = image.Elements.GetName("/Filter");
            switch (filter)
            {
                case "/DCTDecode":
                    ExportJpegImage(image, ref count);
                    break;

                case "/FlateDecode":
                    ExportAsPngImage(image, ref count);
                    break;
            }
        }
        static void ExportJpegImage(PdfDictionary image, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            byte[] stream = image.Stream.Value;
            FileStream fs = new FileStream(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();
        }
        static void ExportAsPngImage(PdfDictionary image, ref int count)
        {
            int width = image.Elements.GetInteger(PdfImage.Keys.Width);
            int height = image.Elements.GetInteger(PdfImage.Keys.Height);
            int bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);

            // TODO: You can put the code here that converts vom PDF internal image format to a Windows bitmap
            // and use GDI+ to save it in PNG format.
            // It is the work of a day or two for the most important formats. Take a look at the file
            // PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
            // We don't need that feature at the moment and therefore will not implement it.
            // If you write the code for exporting images I would be pleased to publish it in a future release
            // of PDFsharp.
        }
    }
}
