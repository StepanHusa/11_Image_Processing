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

                //this.WindowState = WindowState.Minimized;
                //New_Click(new object(), new RoutedEventArgs());
                //MenuEditOptions_AddBoxex_Click(new object(), new RoutedEventArgs());
                //MenuSaveOptions_Template_Click(new object(),new RoutedEventArgs());

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

        private void ShowLoadedDocumentInfoAndEditView()
        {
            MenuEditOptions.IsEnabled = true;
            MenuSaveOptions.IsEnabled = true;
            MenuPrintOptions.IsEnabled = true;
            pdfDocumentView.Load(ST.document);
            pdfDocumentView.MinimumZoomPercentage -= 20;
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

            ShowLoadedDocumentInfoAndEditView();
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
            var b = ST.document.RecognizeTaggedBoxes(ST.pagesList);
        }

        private void MenuSaveOptions_Template_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new() { Title = "Save Template", Filter = $"File Template(*{ST.ext})|*{ST.ext}" };
            if (save.ShowDialog() == false) return;

            ST.document.Save(ST.tempFile);

            SaveDataToFile(save.FileName);


        }
        private void MenuLoadOptions_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open =new() { Title = "Open Template", Filter = $"File Template(*{ST.ext})|*{ST.ext}" };
            if (open.ShowDialog() == false) return;

            LoadDataFromFile(open.FileName);

        }

        void SaveDataToFile(string fileName)
        {
            byte[] FormatCode = ST.fileInfoCode.StringToByteArray(); //8 Byte identification code
            byte[] documentpdf = File.ReadAllBytes(ST.tempFile);
            byte[] listOfPointFsArray = ST.pagesList.PointListArrayToByteArray();

            int listLength = listOfPointFsArray.Length; //int 32
            int docLength = documentpdf.Length; //int32


            using(MemoryStream ms = new())
            {
                using(BinaryWriter bw = new(ms))
                {
                    bw.Write(FormatCode);
                    bw.Write(listLength);
                    bw.Write(listOfPointFsArray);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    bw.Write(ms.ToArray().GetHashSHA1()); //closes file with hashcode to check if the file is the same and if we got to the end at the right time


                }
                File.WriteAllBytes(fileName,ms.ToArray());
            }



        }

        void LoadDataFromFile(string filename)
        {
            //declare components
            byte[] FormatCode;
            byte[] documentpdf;
            byte[] listOfPointFsArray;
            byte[] hash;

            int listLength;
            int docLength;

            //load components from file
            using (FileStream fs = new(filename, FileMode.Open))
            {
                using (BinaryReader br = new(fs))
                {
                    FormatCode = br.ReadBytes(8);
                    if (!FormatCode.SequenceEqual(ST.fileInfoCode.StringToByteArray())) { MessageBox.Show("Open Template file wasn`t generated by this program"); return; }

                    listLength = br.ReadInt32();
                    listOfPointFsArray = br.ReadBytes(listLength);

                    docLength = br.ReadInt32();
                    documentpdf = br.ReadBytes(docLength);

                    hash = br.ReadBytes(20);
                }
            }
            //get hash of created files
            byte[] hashNew;
            using (MemoryStream ms = new()) 
            {
                using (BinaryWriter bw = new(ms))
                {
                    bw.Write(FormatCode);
                    bw.Write(listLength);
                    bw.Write(listOfPointFsArray);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    hashNew=ms.ToArray().GetHashSHA1();
                }

            }
            //hash check
            if (!hash.SequenceEqual(hashNew))
            {
                MessageBoxResult dialogResult = MessageBox.Show("The File you opened was propably changed by other program \n Ignore by clicking OK, of Cancel", "Warning", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.Cancel)
                    return;
            }

            //initialize 
            var dir = ST.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string tempPdf = dir + "tmp" + Path.GetRandomFileName().Remove(8) + ".pdf";


            File.WriteAllBytes(tempPdf, documentpdf);

            ST.tempFile = tempPdf;
            ST.fileName = filename;
            ST.document = new(tempPdf);
            ST.pagesList = listOfPointFsArray.ByteArrayToPointFListArray();



            ShowLoadedDocumentInfoAndEditView();
        }

    }
    static class ByteExtensions
    {
        public static byte[] Combine(this byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
        public static byte[] StringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] PointListArrayToByteArray(this List<PointF>[] array)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(array.Length);
                    foreach (var l in array)
                    {
                        bw.Write(l.Count);

                        foreach (var p in l)
                        {
                            bw.Write(p.X);
                            bw.Write(p.Y);
                        }
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static List<PointF>[] ByteArrayToPointFListArray(this byte[] bArray)
        {
            List<PointF>[] listsArray;
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfArray = r.ReadInt32();
                    listsArray = new List<PointF>[lengthOfArray];
                    for (int i = 0; i != lengthOfArray; i++)
                    {
                        int Count = r.ReadInt32();
                        listsArray[i] = new();
                        for (int j = 0; j != Count; j++)
                        {
                            listsArray[i].Add(new PointF(r.ReadSingle(), r.ReadSingle()));
                        }
                    }
                }
            }
            return listsArray;
        }
        public static byte[] GetHashSHA1(this byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                var hash= sha1.ComputeHash(data);
                return hash;
            }
        }
    }
}