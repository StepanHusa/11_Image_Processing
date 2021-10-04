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
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;

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

            InitializeComponent();

            //debug speedup
            //ButtonNew_Click(ButtonNew,new RoutedEventArgs());
            //this.WindowState = WindowState.Minimized;

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
