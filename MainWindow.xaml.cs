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
//using PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
using System.Diagnostics;
using System.Drawing;
using Syncfusion.Pdf.Parsing;
using WordToPDF;
using Syncfusion.Pdf;
using ImageProcessor;
using ImageProcessor.Imaging.Filters.EdgeDetection;
using IronOcr;
using System.Windows.Xps.Packaging;
//using System.Drawing.Printing;
//using System.Printing;
//using Aspose.Pdf;
using PdfPrintingNet;
using Syncfusion.Windows.PdfViewer;
using _11_Image_Processing.Resources.Strings;
using System.Threading;
using System.Drawing.Drawing2D;
using Syncfusion.Pdf.Interactive;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {

            //licencing
            {
                //licencing PDFSharp and Syncfusion.PDFViewer
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDc1MjU5QDMxMzkyZTMyMmUzMG5MSnFGODNPRngxVVVMcm9zRzVMRi9lZnRJc3JESzRtTEY4T2xMMi9USzg9");
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTI2OTM3QDMxMzkyZTMzMmUzMGZrd0Izb241N05UeDB4Nk5PZUJweldpaG5CQUxkdDlMdnVuZXVWeG9SVXM9");

                //BitMiracle.Docotic.LicenseManager.AddLicenseData("49YKU-QSUJS-1T3EP-28V4L-FIFQY"); 
            }

            InitializeComponent();

            CheckArgsForFiles();


            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\";
                //LoadDataFromFile(debugFolder + "test\\01" + Settings.projectExtension);
                LoadDataFromFile(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\val\val2" + Settings.projectExtension);

                //PdfLoadedDocument ddoc = new(Settings.tempFile);
                //ddoc.AddPositioners();
                //ddoc.Save(Settings.tempFile);
                ////load from pdf
                //{
                //    List<string> tempScans = new();
                //    string path;

                //    var doc = new PdfLoadedDocument(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\val\Stepan_sken.pdf");
                //    for (int j = 0; j < doc.Pages.Count; j++)
                //    {
                //        Bitmap bitmap = doc.ExportAsImage(j, Settings.dpiEvaluatePdf, Settings.dpiEvaluatePdf)/*.CropAddMarginFromSettings()*/;

                //        do
                //        {
                //            path = Path.GetTempFileName();
                //            if (File.Exists(path))
                //            {
                //                File.Delete(path);
                //            }
                //            path = Path.ChangeExtension(path, ".bmp");

                //        } while (File.Exists(path));
                //        bitmap.Save(path);
                //        bitmap.Dispose();
                //        tempScans.Add(path);
                //    }
                //    var ImageFiles = tempScans.ToArray();

                //    List<List<string>> works = new();
                //    int ii = 0;
                //    for (int i = 0; i < 1; i++)
                //    {
                //        works.Add(new());
                //        for (int j = 0; j < 2; j++)
                //        {
                //            works[i].Add(ImageFiles[ii]); //BMP, GIF, EXIF, JPG, PNG and TIFF
                //            ii++;
                //        }
                //    }
                //    Settings.scanPagesInWorks = works;
                //}
                //Bitmap bm = new(Settings.scanPagesInWorks[0][0]);
                //bm.MakeTransformationMatrixFromPositioners(0);
                //bm.SaveToDebugFolder();


                //Settings.resultsInQuestionsInWorks = Settings.scanPagesInWorks.EvaluateWorks(Settings.boxesInQuestions);
                //Menu_View_Result.IsEnabled = true;
                //ShowResultView();
                //var f = new ViewResultW();
                //f.Show();

                //TODod Comment

                //b.ProcessFilter(Settings.LaplFilterForPositioners).Save(debugFolder + "test\\posits\\b.bmp");

                //Matrix matrix = new(new(0,0,1,1), new PointF[3] { new(0,0),new(1,1),new(1,-1) });

                //var p3set = new PointF[1] { new PointF(1,1) };
                //matrix.TransformPoints( p3set );



                //var f=  b.FindPositsFromSettings();

                //var point = new Line(1, -1, 0).CrossectionOfTwoLines(new Line(1, -1, 1));
                //PdfLoadedDocument doc = new(Settings.tempFile);
                //doc.AddPositioners();


                //var ls = new List<List<string>>();
                //var ff = debugFolder + "test\\posits\\";
                //ls.Add(new() { ff + "a.bmp", ff + "b.bmp" });
                //ls.EvaluateWorks(Settings.boxesInQuestions);


                //var bs = new System.Drawing.Point[] { new (0, 1), new (0,1 ) ,new(0,2),new(4,4) };
                //_ = bs.LinearRegression();

                // Bitmap bitm = new(debugFolder + "test\\posits\\a.png");
                //var q= bitm.FindPositionersInBitmap((int)(Settings.positionersLegLength * bitm.Width), (int)(Settings.positionersMargin * bitm.Width));
                // var r = RectangleExtensions.FromFourPoints((int)((q.p1.X + q.p4.X) / 2), (int)((q.p1.Y + q.p2.Y) / 2), (int)((q.p2.X + q.p3.X) / 2), (int)((q.p3.Y + q.p4.Y) / 2));


                //bitm.Crop(r).Save(debugFolder + "test\\posits\\crop.png");

                //for (int i = 0; i < b.Width; i++)
                //    for (int j = 0; j < b.Height; j++)
                //    {
                //        var s = new System.Drawing.Point(i, j).GetEdgeValue(b, Settings.LaplFilterForPositioners);
                //        b.SetPixel(i, j, Color.FromArgb(s,s,s));
                //    }
                //b.Save(debugFolder + "test\\posits\\b.bmp");
                //this.WindowState = WindowState.Minimized;
                //Menu_Load_New_Click(new object(), new RoutedEventArgs());
                //Menu_Edit_AddBoxex_Click(new object(), new RoutedEventArgs());

                //LoadDataFromFile(debugFolder + "\\01" + Settings.projectExtension);

                //Menu_Save_Project.IsEnabled = true;

                //string s = debugFolder + "\\s\\";
                //string f = debugFolder + "\\maxresdefault.jpg";
                //string g = debugFolder + "\\02c.bmp";



                //Settings.scansInPagesInWorks.Add(new());
                //Settings.scansInPagesInWorks[0].Add(new Bitmap(debugFolder + "\\02(0).png"));

                //for (int i = 0; i < 5; i++)
                //{
                //    Settings.scansInPagesInWorks.Add(new());
                //    for (int j = 0; j < 6; j++)
                //    {
                //        string h = s + (i * 6 + j) + ".bmp";
                //        Settings.scansInPagesInWorks[i /*+ 1*/].Add(new Bitmap(h));

                //    }
                //}




                //new ImportPicturesDialogW(11).ShowDialog();

                //rec debug
                //{

                //    string d01 = debugFolder + @"\01.pdf";

                //    List<PointF> pointsPage1 = new();
                //    List<PointF> pointsPage2 = new();
                //    List<Color> colors = new();

                //    colors.Add(Color.Black);
                //    colors.Add(Color.Gray);
                //    colors.Add(Color.White);
                //    for (int i = 0; i < 3; i++)
                //    {
                //        pointsPage1.Add(new PointF(20, 20 + 40 * i));
                //    }
                //    //d01.DebugCreateFile(pointsPage1,colors);

                //    List<List<PointF>> listOfPages = new();
                //    listOfPages.Add(pointsPage1);
                //    listOfPages.Add(pointsPage2);

                //    var doc = new PdfLoadedDocument(d01);
                //    switch (doc.Pages.Count - listOfPages.Count)
                //    {
                //        case < 0:
                //            while (doc.Pages.Count < listOfPages.Count)
                //                doc.Pages.Add();
                //            break;
                //        case > 0:
                //            while (doc.Pages.Count > listOfPages.Count)
                //                listOfPages.Add(new List<PointF>());
                //            break;
                //    }

                //    //Settings.scansInPagesInWorks[0].Add(new Bitmap(debugFolder + @"\01.png"));
                //    //Settings.scansInPagesInWorks[0][0] = new Bitmap(debugFolder + @"\01.png");
                //    //Settings.document.EvaluateSet(); 

                //    var value = doc.RecognizeTaggedBoxesDebug(d01, listOfPages);



                //    var p = new Process();
                //    p.StartInfo.UseShellExecute = true;

                //}
                //p.StartInfo.FileName = d01;
                //p.Start();

                //p.StartInfo.FileName = Path.ChangeExtension(d01, "jpg");
                //p.Start();

            }
        }

        private void CheckArgsForFiles()
        {
            String[] arguments = Environment.GetCommandLineArgs();

            if (arguments.GetLength(0) > 1)
            {
                string filePathFormMainArgs = arguments[1];
                if (File.Exists(arguments[1]))
                {
                    string FileExtension = Path.GetExtension(filePathFormMainArgs);
                    if (FileExtension == ".pdf")
                        LoadDocument(filePathFormMainArgs);
                    else if (FileExtension == ".doc" | FileExtension == ".docx")
                    {
                        Word2Pdf objWorPdf = new Word2Pdf();
                        string FromLocation = filePathFormMainArgs;
                        string ToLocation = Path.GetDirectoryName(FromLocation) + "\\" + Path.GetFileNameWithoutExtension(FromLocation) + Strings.ConvertedFromWord + ".pdf";


                        objWorPdf.InputLocation = FromLocation;
                        objWorPdf.OutputLocation = ToLocation;
                        objWorPdf.Word2PdfCOnversion();
                        LoadDocument(ToLocation);
                    }
                    else if (FileExtension == Settings.projectExtension)
                    {
                        LoadDataFromFile(filePathFormMainArgs);
                    }
                }

            }
        }
        //load
        private void Menu_Load_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog() { Filter = "PDF(*.pdf)|*.pdf", Title = "Open PDF" };
            if (open.ShowDialog() != true) return;
            var fileName = open.FileName;

            Unload();
            LoadDocument(fileName);
        }
        private void Menu_Load_New_Click(object sender, RoutedEventArgs e)
        {
            Unload();
            LoadDocument(null);
        }
        private void Menu_Load_Word_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "Open Word Doc", Filter = "Word Document(*.doc;*.docx)|*.doc;*.docx" };
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
            else { MessageBox.Show("Invalid Input"); return; }

            Unload();
            LoadDocument(ToLocation);
        }

        private void LoadDocument(string fileName)
        {
            var dir = Settings.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";

            if (fileName == null)
            {
                Pdf.NewPdfDoc(tempPdf);
                Pdf.NewPdfDoc(tempCopy);

                this.Title = "*untitled";
            }
            else if (File.Exists(fileName))
            {
                File.Copy(fileName, tempPdf);
                File.Copy(tempPdf, tempCopy);

                this.Title = Path.GetFileName(fileName);
            }

            var doc = new PdfLoadedDocument(tempPdf);
            doc.AddPositioners();
            doc.Save();
            doc.Dispose();

            Settings.tempFile = tempPdf;
            Settings.tempFilesToDelete.Add(tempPdf);
            Settings.tempFileCopy = tempCopy;
            Settings.tempFilesToDelete.Add(tempCopy);
            Settings.fileName = fileName;
            Settings.projectName = Settings.templateProjectName;

            ReloadWindowContent();
            windowHeader.Content = Settings.projectName + "*";
        }

        //open
        private void Menu_Open_Project_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new() { Title = Strings.OpenProject, Filter = Strings.StudentTesterProjects + $"(*{Settings.projectExtension})|*{Settings.projectExtension}" };
            if (open.ShowDialog() == false) return;

            LoadDataFromFile(open.FileName);


        }
        //Edit
        private void Menu_Edit_Questions_Click(object sender, RoutedEventArgs e)
        {
            PdfEditWithoutToolsW m = new();
            m.Show();
        }
        private void CommandBinding_Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Menu_View_Edit_Click(null,null);
        }
        //Save
        private void Menu_Save_ProjectAs_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog save = new() { Title = "Save Template", Filter = $"File Template(*{Settings.projectExtension})|*{Settings.projectExtension}", FileName = Settings.projectName };
            if (save.ShowDialog() == false) return;

            SaveDataToFile(save.FileName);
            SavedInfo();


            Settings.projectFileName = save.FileName;
            ReloadWindowContent();

            Menu_Project_Save.IsEnabled = true;
        }
        private void Menu_Save_Project_Click(object sender, RoutedEventArgs e)
        {

            SaveDataToFile(Settings.projectFileName);
            SavedInfo();

            ReloadWindowContent();
        }

        private void SaveDataToFile(string fileName)
        {
            byte[] FormatCode = Settings.fileCode; //8 Byte identification code
            byte[] documentpdf = File.ReadAllBytes(Settings.tempFile);
            //byte[] listOfPointFsArray = Settings.pagesPoints.PointListArrayToByteArray();
            byte[] listBoxesInQuestions = Settings.boxesInQuestions.BoxListListToByteArray();
            byte[] listOfFields = Settings.pagesFields.RectangleListToByteArray();
            byte[] nameField = Settings.nameField.RectangleTupleToByteArray();
            byte[] listPositionres = Settings.positioners.PositionersListToByteArray();

            int docLength = documentpdf.Length; //int32
            //int listPLength = listOfPointFsArray.Length; //int 32
            int bInQLength = listBoxesInQuestions.Length;
            int listFLength = listOfFields.Length;//int 32
            int nameFieldLength = nameField.Length;//int 32
            int listPositionresLength = listPositionres.Length;//int 32


            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    bw.Write(FormatCode);
                    //bw.Write(listPLength);
                    //bw.Write(listOfPointFsArray);
                    bw.Write(bInQLength);
                    bw.Write(listBoxesInQuestions);
                    bw.Write(listFLength);
                    bw.Write(listOfFields);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    bw.Write(nameFieldLength);
                    bw.Write(nameField);
                    bw.Write(listPositionresLength);
                    bw.Write(listPositionres);

                    bw.Write(ms.ToArray().GetHashSHA1()); //closes file with hashcode to check if the file is the same and if we got to the end at the right time


                }
                File.WriteAllBytes(fileName, ms.ToArray());
            }

            Settings.versions.Add(DateTime.Now.ToStringOfRegularFormat());


        }
        private void SavedInfo()
        {
            saved = true;
            windowHeader.Content = Settings.projectName + " • " + Strings.Saved;
        }
        private void LoadDataFromFile(string filename)
        {
            //declare components
            byte[] FormatCode;
            byte[] documentpdf;
            //byte[] listOfPointFsArray;
            byte[] listBoxesInQuestions;
            byte[] listOfFields;
            byte[] nameField;
            byte[] listPositionres;

            byte[] hash;

            int docLength;
            //int listPLength;
            int bInQLength;
            int listFLength;
            int nameFieldLength;//int 32
            int listPositionresLength;


            //load components from file
            //try
            //{
                using (FileStream fs = new(filename, FileMode.Open))
                {
                    using (BinaryReader br = new(fs))
                    {
                            FormatCode = br.ReadBytes(8);
                            if (!FormatCode.SequenceEqual(Settings.fileCode)) { MessageBox.Show("Open Template file wasn`t generated by this program"); return; }

                            //listPLength = br.ReadInt32();
                            //listOfPointFsArray = br.ReadBytes(listPLength);

                            bInQLength = br.ReadInt32();
                            listBoxesInQuestions = br.ReadBytes(bInQLength);

                            listFLength = br.ReadInt32();
                            listOfFields = br.ReadBytes(listFLength);

                            docLength = br.ReadInt32();
                            documentpdf = br.ReadBytes(docLength);

                            nameFieldLength = br.ReadInt32();
                            nameField = br.ReadBytes(nameFieldLength);

                            listPositionresLength = br.ReadInt32();
                            listPositionres = br.ReadBytes(listPositionresLength);

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
                    //bw.Write(listPLength);
                    //bw.Write(listOfPointFsArray);
                    bw.Write(bInQLength);
                    bw.Write(listBoxesInQuestions);
                    bw.Write(listFLength);
                    bw.Write(listOfFields);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    bw.Write(nameFieldLength);
                    bw.Write(nameField);
                    bw.Write(listPositionresLength);
                    bw.Write(listPositionres);

                    hashNew = ms.ToArray().GetHashSHA1();
                }

            }
            //hash check
            if (!hash.SequenceEqual(hashNew))
            {
                MessageBoxResult dialogResult = MessageBox.Show("The File you opened was propably changed by other program \n Ignore by clicking OK, or Cancel", Strings.Warning, MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.Cancel)
                    return;
            }

            //initialize 
            var dir = Settings.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";


            File.WriteAllBytes(tempPdf, documentpdf);
            File.WriteAllBytes(tempCopy, documentpdf);

            Settings.tempFile = tempPdf;
            Settings.tempFilesToDelete.Add(tempPdf);
            Settings.tempFileCopy = tempCopy;
            Settings.tempFilesToDelete.Add(tempCopy);

            Settings.projectFileName = filename;
            //Settings.pagesPoints = listOfPointFsArray.ByteArrayToPointFListArray();
            Settings.boxesInQuestions = listBoxesInQuestions.ByteArrayToBoxListList();
            Settings.pagesFields = listOfFields.ByteArrayToRectangleFTupleList();
            Settings.nameField = nameField.ByteArrayToRectangleFTuple();
            Settings.positioners = listPositionres.ByteArrayToPositionersList();
            Settings.versions = new();
            Settings.versions.Add(DateTime.Now.ToStringOfRegularFormat());
            //}
            //catch { MessageBox.Show(Strings.notabletoread); return; }


            ReloadWindowContent();
            Menu_Project_Save.IsEnabled = true;
            windowHeader.Content = Settings.projectName;
        }
        //Print
        private void Menu_Export_ToJPEG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save JPEG", Filter = "JPEG(*.jpeg)|*.jpeg" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();

            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i, Settings.dpiExport, Settings.dpiExport)/*.CropAddMarginFromSettings()*/;
                image.SaveToDebugFolder();

                string fn = Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

        }
        private void Menu_Export_ToPNG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save PNG", Filter = "PNG(*.png)|*.png" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();
            //doc.AddPositioners();

            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i, Settings.dpiExport, Settings.dpiExport);

                //debug
                //TODOd comment
                //var rect = Settings.positioners[i].UnrelatitivizeToImage(image);
                //image.SetPixel(rect.X + rect.Width, rect.Y, Color.Yellow);
                //image.SaveToDebugFolder();

                string fn = Path.GetDirectoryName(save.FileName) + "\\" + Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
            }

        }
        private void Menu_Export_PDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new() { Title = "Save PDF", Filter = $"File Template(*.PDF)|*.PDF", FileName = Settings.projectName };
            if (save.ShowDialog() == false) return;
            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();
            doc.Save(save.FileName);
        }
        private void Menu_Print_Click(object sender, RoutedEventArgs e)
        {
            //PdfPrintingNet.PdfPrint printDoc = new(string.Empty,string.Empty);

            //printDoc.Print(Settings.tempFile);
            //printDoc.





            //PdfDocument doc = new PdfDocument();
            //doc.LoadFromFile(FilePathandFileName);

            ////Use the default printer to print all the pages 
            ////doc.PrintDocument.Print(); 

            ////Set the printer and select the pages you want to print 

            //PrintDialog dialogPrint = new PrintDialog();
            //dialogPrint.AllowPrintToFile = true;
            //dialogPrint.AllowSomePages = true;
            //dialogPrint.PrinterSettings.MinimumPage = 1;
            //dialogPrint.PrinterSettings.MaximumPage = doc.Pages.Count;
            //dialogPrint.PrinterSettings.FromPage = 1;
            //dialogPrint.PrinterSettings.ToPage = doc.Pages.Count;

            //if (dialogPrint.ShowDialog() == DialogResult.OK)
            //{
            //    //Set the pagenumber which you choose as the start page to print 
            //    doc.PrintFromPage = dialogPrint.PrinterSettings.FromPage;
            //    //Set the pagenumber which you choose as the final page to print 
            //    doc.PrintToPage = dialogPrint.PrinterSettings.ToPage;
            //    //Set the name of the printer which is to print the PDF 
            //    doc.PrinterName = dialogPrint.PrinterSettings.PrinterName;

            //    PrintDocument printDoc = doc.PrintDocument;
            //    dialogPrint.Document = printDoc;
            //    printDoc.Print();
            //}



            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();
            var stream = new MemoryStream();
            doc.Save(stream);


            PrintDialog printDialog = new() { SelectedPagesEnabled = true, UserPageRangeEnabled = true, CurrentPageEnabled = true };
            if (printDialog.ShowDialog() != true) return;

            string xps = Path.GetTempFileName();
            var document = new Aspose.Pdf.Document(stream);
            stream.Dispose();
            document.Save(xps, Aspose.Pdf.SaveFormat.Xps);
            document.Dispose();

            // Open the selected document.
            XpsDocument xpsDocument = new(xps, FileAccess.Read);

            // Get a fixed document sequence for the selected document.
            FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();

            // Create a paginator for all pages in the selected document.
            DocumentPaginator docPaginator = fixedDocSeq.DocumentPaginator;

            // Print to a new file.
            printDialog.PrintDocument(docPaginator, Strings.Printing + Settings.fileName);


            File.Delete(xps);
            //string xpsFileName = Path.GetFileName(xpsFilePath);

            //prin

            //try
            //{
            //    // The AddJob method adds a new print job for an XPS
            //    // document into the print queue, and assigns a job name.
            //    // Use fastCopy to skip XPS validation and progress notifications.
            //    // If fastCopy is false, the thread that calls PrintQueue.AddJob
            //    // must have a single-threaded apartment state.
            //    PrintSystemJobInfo xpsPrintJob =
            //            defaultPrintQueue.AddJob(jobName: xpsFileName, documentPath: xpsFilePath, fastCopy);

            //    // If the queue is not paused and the printer is working, then jobs will automatically begin printing.
            //    Debug.WriteLine($"Added {xpsFileName} to the print queue.");
            //}
            //catch (PrintJobException e)
            //{
            //    allAdded = false;
            //    Debug.WriteLine($"Failed to add {xpsFileName} to the print queue: {e.Message}\r\n{e.InnerException}");
            //}

        }

        //Read
        //one page files
        private void Menu_Read_ListOfScans_OnePage_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true };
            if (open.ShowDialog() == false) return;

            int l = Settings.scanPagesInWorks.Count();//moves indexing if there are already bitmaps in the list
            for (int i = 0; i < open.FileNames.Length; i++)
            {
                Settings.scanPagesInWorks.Add(new());
                Settings.scanPagesInWorks[l + i].Add(open.FileNames[i]);
            }
        }
        private async void Menu_Read_PDF_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.PDF + $"|*.PDF|" + Strings.Allfiles + " (*.*)|*.*", Multiselect = true };
            if (open.ShowDialog() == false) return;

            List<string> tempScans = new();
            string path;




            foreach (var item in open.FileNames)
            {
                var doc = new PdfLoadedDocument(item);
                var child = doc.DocumentInformation.XmpMetadata.XmlData.FirstChild;
                for (int j = 0; j < doc.Pages.Count; j++)
                {
                    Bitmap bitmap = doc.ExportAsImage(j, Settings.dpiEvaluatePdf, Settings.dpiEvaluatePdf);
                    do
                    {
                        path = Path.GetTempFileName();
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        path = Path.ChangeExtension(path, ".bmp");

                    } while (File.Exists(path));
                    bitmap.Save(path);
                    bitmap.Dispose();
                    tempScans.Add(path);
                }
            }
            await LoadNumberOfFiles(tempScans.ToArray());
        }
        //advanced read
        private async void Menu_Read_ListOfScans_Dialog_Click(object sender, RoutedEventArgs e)
        {
            // var open = new OpenFileDialog() { Title = "Open list of scans PDF", Filter = $"Pictures (all readable)|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true }; //TODO make for images (maybe make method for strings)
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|" + Strings.Allfiles + " (*.*)|*.*", Multiselect = true };
            if (open.ShowDialog() == false) return;

            await LoadNumberOfFiles(open.FileNames);

        }
        private async void Menu_Read_Scan_Click(object sender, RoutedEventArgs e)
        {
            var a = new ScanForm();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            await LoadNumberOfFiles(a.tempScans.ToArray());
        }
        private async Task LoadNumberOfFiles(string[] ImageFiles)
        {
            var a = new ImportPicturesDialogW(ImageFiles.Length);
            if (a.ShowDialog() != true)
            {
                if (MessageBox.Show("Realy return?", "caption", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    return;
                else a.ShowDialog();
            }

            var da = a.Answer;
            var invert = a.Invert.Value;
            a.Close();

            Settings.scanPagesInWorks = await Task.Run(() =>
            {

                List<List<string>> works = new();
                int ii = 0;
                if (!invert)
                    for (int i = 0; i < da.Item1; i++)
                    {
                        works.Add(new());
                        for (int j = 0; j < da.Item2; j++)
                        {
                            works[i].Add(ImageFiles[ii]); //BMP, GIF, EXIF, JPG, PNG and TIFF
                            ii++;
                        }
                    }
                else
                {
                    for (int i = 0; i < da.Item1; i++)
                        works.Add(new());
                    for (int j = 0; j < da.Item2; j++)
                    {
                        for (int i = 0; i < da.Item1; i++)
                        {
                            works[i].Add(ImageFiles[ii]); //BMP, GIF, EXIF, JPG, PNG and TIFF
                            ii++;
                        }
                    }
                }
                return works;
            });

        }


        //evaluate
        private async void Menu_Eavluate_Click(object sender, RoutedEventArgs e)
        {
            new ControlBeforeEvaluatonW().ShowDialog();


            int highestPageIndex = 0;
            foreach (var question in Settings.boxesInQuestions)
                foreach (var box in question)
                    if (box.Page > highestPageIndex)
                        highestPageIndex = box.Page;
            int lowestPagesInWork = int.MaxValue;
            foreach (var work in Settings.scanPagesInWorks)
                if (work.Count < lowestPagesInWork)
                    lowestPagesInWork = work.Count;
            if (highestPageIndex > lowestPagesInWork) MessageBox.Show(Strings.warningScansDontHaveEnaughtPages, Strings.Warning);




            var pbw = new ProgressBarW();
            pbw.Show();
            await Task.Run(() => Settings.resultsInQuestionsInWorks = Settings.scanPagesInWorks.EvaluateWorks(Settings.boxesInQuestions));

            //Settings.namesScaned = Settings.scansInPagesInWorks.GetCropedNames(Settings.nameField);
            //new ViewResultW().Show();
            ShowResultView();
            Menu_View_Edit.IsEnabled = true;
            pbw.Close();
        }

        //view
        private void Menu_View_Main_Click(object sender, RoutedEventArgs e)
        {
            projectInfo.Visibility = Visibility.Visible;
            results.Visibility = Visibility.Hidden;
            editView.Visibility = Visibility.Hidden;

            PreviewKeyDown -= Window_KeyDown;

        }
        private void Menu_View_Result_Click(object sender, RoutedEventArgs e)
        {
            projectInfo.Visibility = Visibility.Hidden;
            results.Visibility = Visibility.Visible;
            editView.Visibility = Visibility.Hidden;

            PreviewKeyDown -= Window_KeyDown;

        }
        private void Menu_View_Edit_Click(object sender, RoutedEventArgs e)
        {
            projectInfo.Visibility = Visibility.Hidden;
            results.Visibility = Visibility.Hidden;
            editView.Visibility = Visibility.Visible;
            pdfViewControl.ShowToolbar = false;
            HideTools();

            pdfViewControl.Load(Settings.tempFile);
            pdfViewControl.MaximumZoomPercentage = 6400;
            pdfViewControl.ScrollChanged -= PdfViewControl_ScrollChanged;
            pdfViewControl.ScrollChanged += PdfViewControl_ScrollChanged;
            pdfViewControl.ZoomChanged -= PdfViewControl_ZoomChanged;
            pdfViewControl.ZoomChanged += PdfViewControl_ZoomChanged;

            PreviewKeyDown -= Window_KeyDown;
            PreviewKeyDown += Window_KeyDown;
        }

        private void PdfViewControl_ScrollChanged(object sender, ScrollChangedEventArgs args)
        {
            offset = args.VerticalOffset;
        }

        //help and settings
        private void Menu_Help_HTML_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "StudentTesterHelp.html");

        }
        private void Menu_Help_CHM_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "StudentTesterHelp.chm");

        }

        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsW().ShowDialog();
        }


        //content
        private void projecttext_LostFocus(object sender, RoutedEventArgs e)
        {
            Settings.projectName = projecttext.Text;
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadWindowContent();
        }
        private void Menu_Project_Unload_Click(object sender, RoutedEventArgs e)
        {
            Unload();
        }

        private void ReloadWindowContent()
        {
            if (Settings.tempFile == null)
            {
                Unload();

            }
            else
            {
                var doc = new PdfLoadedDocument(Settings.tempFile);

                Menu_Edit.IsEnabled = true;
                Menu_View_Edit.IsEnabled = true;
                Menu_Project_SaveAs.IsEnabled = true;
                Menu_Print.IsEnabled = true;
                Menu_Read.IsEnabled = true;
                Menu_Eavluate.IsEnabled = true;
                Menu_Export.IsEnabled = true;

                reloadButton.IsEnabled = true;


                //pdfDocumentView.UpdateLayout();
                //pdfDocumentView.Load(doc);
                loadedPdfLabel.Content = Path.GetFileName(Settings.fileName);
                //pdfDocumentView.ZoomTo(-1);

                Title = Settings.appName + " -- " + Settings.projectName;

                projecttext.Text = Settings.projectName;
                projectfilenametext.Text = Path.GetFileName(Settings.projectFileName);
                locationtext.Text = Path.GetDirectoryName(Settings.projectFileName);
                pagecounttext.Text = doc.Pages.Count.ToString();
                questioncounttext.Text = Settings.boxesInQuestions.Count.ToString();
                int ii = 0;
                foreach (var question in Settings.boxesInQuestions)
                {
                    ii += question.Count;
                }
                boxcounttext.Text = ii.ToString();
                ii = 0;
                foreach (var tuples in Settings.pagesFields)
                    ii++;
                fieldcounttext.Text = ii.ToString();

                if (Settings.versions.Count != 0)
                    dateoflastsavetext.Text = Settings.versions.Last();
                else dateoflastsavetext.Text = Strings.notsavedyet;

                //dateoflastsavetext.Text = Settings.versions.Last().ToStringOfRegularFormat();
            }
        }
        private void Unload()
        {
            Settings.nameField = null;
            Settings.pagesFields = new();
            Settings.boxesInQuestions = new();
            Settings.resultsInQuestionsInWorks = null;
            Settings.scanPagesInWorks = new();

            reloadButton.IsEnabled = false;

            if (File.Exists(Settings.tempFile))
                File.Delete(Settings.tempFile);
            if (File.Exists(Settings.tempFileCopy))
                File.Delete(Settings.tempFileCopy);

            Settings.tempFile = null;
            Settings.tempFileCopy = null;
            Settings.projectName = Settings.templateProjectName;
            Settings.projectFileName = null;
            Settings.fileName = null;
            Settings.versions = new();


            projecttext.Text = string.Empty;
            projectfilenametext.Text = string.Empty;
            locationtext.Text = string.Empty;
            pagecounttext.Text = string.Empty;
            questioncounttext.Text = string.Empty;
            boxcounttext.Text = string.Empty;
            fieldcounttext.Text = string.Empty;
            dateoflastsavetext.Text = string.Empty;

            Title = Settings.appName + " - " + Strings.unloaded;

            //pdfDocumentView.Unload();
            loadedPdfLabel.Content = "";


            Menu_Edit.IsEnabled = false;
            Menu_Project_Save.IsEnabled = false;
            Menu_Project_SaveAs.IsEnabled = false;
            Menu_Print.IsEnabled = false;
            Menu_Export.IsEnabled = false;
            Menu_Read.IsEnabled = false;
            Menu_Eavluate.IsEnabled = false;

            Menu_View_Edit.IsEnabled = false;
            Menu_View_Result.IsEnabled = false;
            Menu_View_Main_Click(null, null);
        }


        //private void pdfDocumentView_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    double r = e.NewSize.Width / e.PreviousSize.Width;

        //    //int newZoom = (int)(pdfDocumentView.ZoomPercentage * r);
        //    //pdfDocumentView.ZoomTo(newZoom);
        //}

        //resultView
        private void ShowResultView()
        {
            projectInfo.Visibility = Visibility.Hidden;
            results.Visibility = Visibility.Visible;

            if (namesScaned == null & Settings.nameField != null)
            {
                namesScaned = new();
                foreach (var item in Settings.scanPagesInWorks)
                {
                    var rect = Settings.nameField.Item2;
                    var b = new System.Drawing.Bitmap(item[Settings.nameField.Item1]);
                    var mat = b.MakeTransformationMatrixFromPositioners(Settings.nameField.Item1);
                    var newRect = new System.Drawing.RectangleF(rect.Location.ApplyMatrix(mat), rect.Size.ApplyMatrix(mat));
                    namesScaned.Add(b.Crop(System.Drawing.Rectangle.Round(newRect)));
                }
            }

            //Settings.scansInPagesInWorks.DrowCorrect();
            SetupTabsOfView();
            SetUpAllResults();

        }
        internal static List<System.Drawing.Bitmap> namesScaned;
        public void SetupTabsOfView()
        {
            for (int i = 0; i < Settings.scanPagesInWorks.Count; i++)
            {
                TabItem z = new();
                if (namesScaned != null)
                {
                    System.Windows.Controls.Image wImage = new();
                    wImage.Source = namesScaned[i].BitmapToImageSource();
                    wImage.Height = 32;
                    StackPanel sp = new();
                    sp.Children.Add(wImage);
                    z.Header = sp;
                }
                else z.Header = i + 1;

                z.Content = GenerateLeftGrid(i);

                tabsHorizontal.Items.Add(z);
                //_TODO make memory suitable
            }
            try { tabsHorizontal.SelectedIndex = 0; } catch { }
        }
        private Grid GenerateLeftGrid(int i)
        {
            //var imagesTabs = ViewWorks(i); 
            var imagesTabsButton = (Button)Resources["butView"];
            imagesTabsButton.Content = Strings.ViewScan;
            imagesTabsButton.Tag = i;
            imagesTabsButton.Click += ImagesTabsButton_Click;

            var table = ResultsList(i);
            var result = new StackPanel();
            var tupleTextCheckbox = new StackPanel();
            tupleTextCheckbox.Orientation = Orientation.Horizontal;
            tupleTextCheckbox.Children.Add(new System.Windows.Controls.Image());//TODO add evaluation of fields
            tupleTextCheckbox.Children.Add(new CheckBox());




            result.Children.Add(tupleTextCheckbox);


            //TODO finish this section and add statistics

            Grid grid = new();
            grid.RowDefinitions.Add(new() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new());
            grid.RowDefinitions.Add(new() { Height = new GridLength(40) });
            grid.Children.Add(imagesTabsButton);
            grid.Children.Add(table);
            grid.Children.Add(result);
            Grid.SetRow(imagesTabsButton, 3);
            Grid.SetRow(table, 0);
            Grid.SetRow(result, 1);

            return grid;
        }
        private void ImagesTabsButton_Click(object sender, RoutedEventArgs e)
        {
            int i = (int)(sender as Button).Tag;
            new WorkImagesVeiwWindow(i).Show();
        }
        private ListView ResultsList(int workindex)
        {
            var lv = (ListView)Resources["lview"];
            var list = GetListToDisplay(workindex);
            lv.ItemsSource = list;

            return lv;
        }
        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sen = sender as TabControl;
            int ii = sen.SelectedIndex;

            for (int i = 0; i < tabsHorizontal.Items.Count; i++)
            {
                var tabs = (tabsHorizontal.Items[i] as TabItem).Content as TabControl;
                if (tabs.Items.Count > ii)
                    tabs.SelectedIndex = ii;
                else tabs.SelectedIndex = tabs.Items.Count - 1;
            }




        }
        private List<ResultOfQuestion> GetListToDisplay(int workindex)
        {
            var list = new List<ResultOfQuestion>();
            int ii = 0;
            foreach (var question in Settings.boxesInQuestions)
            {
                ii++;
                int q = Settings.boxesInQuestions.IndexOf(question);
                string correct = string.Empty;
                string checkedd = string.Empty;

                foreach (var box in question)
                    if (box.IsCorrect) correct += $"{question.IndexOf(box).IntToAlphabet()}, ";

                for (int i = 0; i < Settings.resultsInQuestionsInWorks[workindex][q].Count; i++)
                    if (Settings.resultsInQuestionsInWorks[workindex][q][i]) checkedd += $"{i.IntToAlphabet()}, ";

                list.Add(new(checkedd, correct, ii));
            }
            return list;
        }
        private void SetUpAllResults()
        {

            var s = GetListOfAll();
            allResults.ItemsSource = s;
            bool isWithNames = false;
            foreach (var item in s)
                if (item.Name != null)
                    isWithNames = true;

            int height = 40; //same as allResults listview height of row

            if (isWithNames)
                foreach (var item in s)
                {
                    System.Windows.Controls.Image x = item.Name;
                    x.Height = height;
                    namesPanel.Children.Add(x);
                }
            else namesPanel.Width = 0;
        }
        private List<ResultOfAllOne> GetListOfAll()
        {
            var list = new List<ResultOfAllOne>();
            for (int i = 0; i < Settings.resultsInQuestionsInWorks.Count; i++)
            {
                var li = GetListToDisplay(i);

                int right = 0;
                foreach (var item in li)
                    if (item.CorrectBool)
                        right++;
                ResultOfAllOne r;
                if (namesScaned != null)
                    if (namesScaned.Count > i)
                        r = new(i + 1, right, li.Count, namesScaned[i]);
                    else r = new(i + 1, right, li.Count);
                else r = new(i + 1, right, li.Count);

                list.Add(r);
            }


            return list;
        }



        private void CloseClick(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            this.Close();
        }
        private void MinimazeClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaximazeClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }
        private void CommandBinding_CanExecuteTRUE(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
            }

            this.DragMove();
        }

        //editview
        private double offset;
        private static bool saved=false;
        private bool drawingRectangle;
        private PageMouseMoveEventArgs argsFirstVertex;
        private System.Windows.Point locFirstVertex;




        private void Menu_NewPage_Click(object sender, RoutedEventArgs e)
        {
            var doc = pdfViewControl.LoadedDocument;

            doc.Pages.Add();

            ReloadDocument();
            File.Copy(Settings.tempFile, Settings.tempFileCopy, true);
        }
        private void deletePage_Click(object sender, RoutedEventArgs e)
        {
            var doc = pdfViewControl.LoadedDocument;
            if (doc.Pages.Count == 1) { MessageBox.Show(Strings.cannotDeleteLastPage); return; }

            doc.Pages.RemoveAt(pdfViewControl.CurrentPage - 1);

            var l = Settings.boxesInQuestions;
            for (int i = l.Count - 1; i > -1; i--)
            {
                if (l[i][0].Page == pdfViewControl.CurrentPage - 1)
                {
                    l.RemoveAt(i);
                }
                else if (l[i][0].Page > pdfViewControl.CurrentPage - 1)
                    for (int j = 0; j < l[i].Count; j++)
                    {
                        l[i][j] = new(l[i][j].Page - 1, l[i][j].Rectangle, l[i][j].BoundWidth, l[i][j].IsCorrect);
                    } //deosn't acount for the case where question can be on more than one page
            }

            ReloadDocument();
            File.Copy(Settings.tempFile, Settings.tempFileCopy, true);


        }
        //clik methods not used
        private void A_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);
            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;
            //else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;

        }
        private void B_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_B;
            //else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_B;

        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
            {
                pdfViewControl.PageMouseMove += PdfViewControl_PageMouseMove_C;
                rectangleR.Visibility = Visibility.Visible;
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;
            }
            //else
            //{
            //    pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove_C;
            //    rectangleR.Visibility = Visibility.Hidden;
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;
            //}

        }
        private void D_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_D;
            //else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_D;

        }
        private void E_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_E;
            //else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_E;

        }
        private void toogleAn_Click(object sender, RoutedEventArgs e)
        {
            //UncheckAllOthers(sender);

            //if ((sender as MenuItem).IsChecked)
            //    pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_Tog;
            ////else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;

        }
        private void nameField_Click(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_name;
            //else
            //    pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_name;
        }

        private MenuItem chacked = null;
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            {
                foreach (MenuItem item in toolMenu.Children)
                    if (item != toogleAn)
                        if (item.IsChecked)
                        {
                            chacked = item;
                            item.IsChecked = false;
                        }

                toogleAn.IsChecked = true;
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_Tog;
                this.PreviewKeyUp += Window_KeyUp;
            }


        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers != ModifierKeys.Alt)
            {
                if (chacked != null)
                {
                    chacked.IsChecked = true;
                    chacked = null;
                }

                toogleAn.IsChecked = false;
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;
                this.PreviewKeyUp -= Window_KeyUp;
            }
        }

        private void CheckMenuItem(object sender, RoutedEventArgs e)
        {
            (sender as MenuItem).IsChecked ^= true;
        }
        private void a_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);
            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;

        }
        private void a_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;

        }
        private void b_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_B;

        }
        private void b_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_B;
        }
        private void c_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
            {
                pdfViewControl.PageMouseMove += PdfViewControl_PageMouseMove_C;
                rectangleR.Visibility = Visibility.Visible;
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;
            }

        }
        private void c_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove_C;
            rectangleR.Visibility = Visibility.Hidden;
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;

        }
        private void d_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_D;

        }
        private void d_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_D;
        }
        private void e_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_E;

        }
        private void e_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_E;

        }
        private void toogleAn_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
            {
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_Tog;
            }

        }
        private void toogleAn_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;

        }
        private void nameField_Checked(object sender, RoutedEventArgs e)
        {
            UncheckAllOthers(sender);

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_name;

        }
        private void nameField_Unchecked(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_name;

        }

        private void UncheckAll(ContextMenu contextmenu)
        {
            foreach (MenuItem item in contextmenu.Items)
            {
                item.IsChecked = false;
            }
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_B;
            //pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove_C;
            //rectangleR.Visibility = Visibility.Hidden;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_D;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_E;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_name;


        }
        private void UncheckAllOthers(object contextMenuItem)
        {
            foreach (MenuItem item in toolMenu.Children)
            {
                if (item != contextMenuItem)
                    item.IsChecked = false;
            }

            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_B;
            //pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove_C;
            //rectangleR.Visibility = Visibility.Hidden;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_D;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_E;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_Tog;
            //pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_name;

        }




        private void Pdfwcontrol_PageClicked_A(object sender, PageClickedEventArgs args)
        {

            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage / 100.0;



            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));
            SizeF size = Settings.sizeOfBox;

            RectangleF bounds = new RectangleF(point, size);
            bounds.RelatitivizeToPage(doc.Pages[pindex]);

            doc.DrawBox(new(pindex,bounds, Settings.baundWidth/doc.Pages[pindex].Size.Width,false));


            ReloadDocument();
            pdfViewControl.Zoom = zoom*100;

        }
        private void Pdfwcontrol_PageClicked_B(object sender, PageClickedEventArgs args)
        {
            drawingRectangle ^= true;
            if (drawingRectangle)
            {
                argsFirstVertex = args;
                locFirstVertex = Mouse.GetPosition(this);
                //this.MouseMove += PdfEditW_MouseMove;
                pdfViewControl.PageMouseMove += PdfViewControl_PageMouseMove;
                PdfViewControl_PageMouseMove(sender, args);//just to prevent bug displaying rectangle elsewhere
                rectangleR.Visibility = Visibility.Visible;
            }
            else
            {
                //add rectangle
                {
                    var doc = pdfViewControl.LoadedDocument;
                    int pindex = args.PageIndex;
                    double zoom = pdfViewControl.ZoomPercentage / 100.0;


                    RectangleF rect = new();
                    rect.Location = new((float)(argsFirstVertex.Position.X * 0.75 / zoom), (float)(argsFirstVertex.Position.Y * 0.75 / zoom));
                    rect.Size = new((float)((args.Position.X - argsFirstVertex.Position.X) * 0.75 / zoom), (float)((args.Position.Y - argsFirstVertex.Position.Y) * 0.75 / zoom));
                    rect.RelatitivizeToPage(doc.Pages[pindex]);

                    doc.DrawRectangleBounds(rect, pindex,Settings.baundWidth);
                    doc.DrawStringNextToRectangle(Strings.text + (Settings.pagesFields.Count + 1) + ":", rect, pindex);

                    Settings.pagesFields.Add(new(pindex, rect));

                    ReloadDocument();
                    pdfViewControl.Zoom = zoom*100;
                }
                //this.MouseMove -= PdfEditW_MouseMove;
                pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove;
                rectangleR.Visibility = Visibility.Hidden;

            }
        }
        private void Pdfwcontrol_PageClicked_D(object sender, PageClickedEventArgs args)
        {
            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage / 100.0;

            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));

            //get the index of new question
            int iQ = Settings.boxesInQuestions.Count;
            //add list of answers in this question
            Settings.boxesInQuestions.Add(new());


            var tb = new PdfTextBoxField(doc.Pages[args.PageIndex], Strings.question);
            tb.Text = $"Question";
            tb.Bounds = new RectangleF(point.X, point.Y, Settings.QS.widthOfQTBs, Settings.QS.heightOfTB);
            doc.Form.Fields.Add(tb);



            for (int i = 0; i < Settings.QS.n; i++)
            {
                //add answer i textbox field
                PdfTextBoxField textBoxField = new PdfTextBoxField(doc.Pages[args.PageIndex], Strings.Enteryourtext);
                textBoxField.Text = Strings.Answer + (i + 1);
                textBoxField.Bounds = new RectangleF(point.X + Settings.QS.tab, point.Y + Settings.QS.heightOfTB + Settings.QS.spaceUnderQ + i * (Settings.QS.heightOfTB + Settings.QS.spaceBtwAn), Settings.QS.widthOfQTBs - Settings.QS.tab, Settings.QS.heightOfTB);
                doc.Form.Fields.Add(textBoxField);


                var pointb = new PointF(point.X + Settings.QS.widthOfQTBs + Settings.QS.spaceBeforeBox, point.Y + Settings.QS.heightOfTB + Settings.QS.spaceUnderQ + i * (Settings.QS.heightOfTB + Settings.QS.spaceBtwAn));
                SizeF size = Settings.sizeOfBox;


                //square
                RectangleF bounds = new RectangleF(pointb, size);
                bounds.RelatitivizeToPage(doc.Pages[pindex]);

                doc.DrawRectangleBounds(bounds, pindex, Settings.baundWidth);
                doc.DrawIndexNextToRectangle(bounds, pindex, /*pindex.ToString() +*/ (iQ + 1).ToString() + i.IntToAlphabet());

                bounds.RelatitivizeToPage(doc.Pages[pindex]);
                //add square to 'The List'
                Settings.boxesInQuestions[iQ].Add(pindex, bounds, Settings.baundWidth, false);
            }

            ReloadDocument();
            pdfViewControl.Zoom = zoom*100;

        }
        private void Pdfwcontrol_PageClicked_E(object sender, PageClickedEventArgs args)
        {
            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage/100.0;

            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));

            //get the index of new question
            int iQ = Settings.boxesInQuestions.Count;
            //add list of answers in this question
            Settings.boxesInQuestions.Add(new());


            for (int i = 0; i < Settings.QS.n; i++)
            {
                var pointb = new PointF(point.X, point.Y + i * (Settings.sizeOfBox.Height + Settings.QS.spaceBtwAn));
                SizeF size = Settings.sizeOfBox;

                //square
                RectangleF bounds = new RectangleF(pointb, size);
                bounds.RelatitivizeToPage(doc.Pages[pindex]);

                Box box = new(pindex, bounds, Settings.baundWidth / doc.Pages[pindex].Size.Width, false);
                doc.DrawBox(box);

                doc.DrawIndexNextToRectangle(bounds, pindex, /*pindex.ToString() +*/ (iQ + 1).ToString() + i.IntToAlphabet());

                //add square to 'The List'
                Settings.boxesInQuestions[iQ].Add(box);
            }

            ReloadDocument();
            pdfViewControl.Zoom = zoom*100;


        }
        private void Pdfwcontrol_PageClicked_Tog(object sender, PageClickedEventArgs args)
        {
            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage / 100.0;
            var b = Settings.boxesInQuestions;

            bool hit = false;

            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));

            for (int i = 0; i < b.Count; i++)
                for (int j = 0; j < b[i].Count; j++)
                    if (b[i][j].Page == pindex)
                    {
                        var r = b[i][j].Rectangle;
                        if (r.UnrelatitivizeToPage(doc.Pages[pindex]).Contains(point))
                        {
                            b[i][j] = new(b[i][j].Page, b[i][j].Rectangle, b[i][j].BoundWidth, !b[i][j].IsCorrect);
                            doc.DrawRectangleBounds(b[i][j].Rectangle, b[i][j].Page, b[i][j].BoundWidth, b[i][j].IsCorrect);
                            hit = true;
                        }
                    }
            if (hit) ReloadDocument();
        }
        private void Pdfwcontrol_PageClicked_name(object sender, PageClickedEventArgs args)
        {
            if (Settings.nameField != null)
                if (MessageBox.Show(Strings.WarningMoreThanOneNamefield, Strings.Warning, MessageBoxButton.YesNoCancel) != MessageBoxResult.Yes)
                    return;

            drawingRectangle ^= true;
            if (drawingRectangle)
            {
                argsFirstVertex = args;
                locFirstVertex = Mouse.GetPosition(this);
                //this.MouseMove += PdfEditW_MouseMove;
                pdfViewControl.PageMouseMove += PdfViewControl_PageMouseMove;
                rectangleR.Visibility = Visibility.Visible;
            }
            else
            {
                //add rectangle to name
                {
                    var doc = pdfViewControl.LoadedDocument;
                    int pindex = args.PageIndex;
                    double zoom = pdfViewControl.ZoomPercentage / 100.0;


                    RectangleF rect = new();
                    rect.Location = new((float)(argsFirstVertex.Position.X * 0.75 / zoom), (float)(argsFirstVertex.Position.Y * 0.75 / zoom));
                    rect.Size = new((float)((args.Position.X - argsFirstVertex.Position.X) * 0.75 / zoom), (float)((args.Position.Y - argsFirstVertex.Position.Y) * 0.75 / zoom));

                    rect.RelatitivizeToPage(doc.Pages[pindex]);

                    doc.DrawRectangleBounds(rect, pindex, Settings.baundWidth);
                    doc.DrawNameNextToRectangle(rect, pindex);


                    Settings.nameField = new(pindex, rect);
                    ReloadDocument();
                    pdfViewControl.Zoom = zoom*100;

                }
                //this.MouseMove -= PdfEditW_MouseMove;
                pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove;
                rectangleR.Visibility = Visibility.Hidden;

            }
        }

        private void PdfViewControl_PageMouseMove(object sender, PageMouseMoveEventArgs args)
        {

            Rectangle rect = new();


            rect.Location = new((int)locFirstVertex.X, (int)locFirstVertex.Y);
            rect.Size = new((int)(Mouse.GetPosition(this).X - locFirstVertex.X /*- 2 * rectangleR.StrokeThickness*/), (int)(Mouse.GetPosition(this).Y - locFirstVertex.Y /*- 2 * rectangleR.StrokeThickness*/));

            rect = rect.EvaluateInPositiveSize();

            Thickness thickness = new(rect.X, rect.Y, 0, 0);
            rectangleR.Margin = thickness;

            rectangleR.Width = rect.Width;
            rectangleR.Height = rect.Height;




        }
        private void PdfViewControl_PageMouseMove_C(object sender, PageMouseMoveEventArgs args)
        {
            Rectangle rect = new();


            rect.Location = new((int)(Mouse.GetPosition(this).X + 2 * rectangleR.StrokeThickness), (int)(Mouse.GetPosition(this).Y + 2 * rectangleR.StrokeThickness));
            rect.Size = Settings.sizeOfBox.ToSize();

            Thickness thickness = new(rect.X, rect.Y, 0, 0);
            rectangleR.Margin = thickness;

            rectangleR.Width = rect.Width;
            rectangleR.Height = rect.Height;
        }

        private void ReloadDocument()
        {
            saved = false;
            windowHeader.Content = Settings.projectName+"*";


            double zoom = pdfViewControl.ZoomPercentage / 100.0;
            var doc = pdfViewControl.LoadedDocument;
            doc.Save(Settings.tempFile);

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            doc.Dispose();
            pdfViewControl.Load(stream);

            pdfViewControl.ScrollTo(offset);
            pdfViewControl.Zoom = zoom * 100;
        }
        private void PdfViewControl_ZoomChanged(object sender, ZoomEventArgs args)
        {
            CalculateAndShowPreviewBoxes();
        }
        private void CalculateAndShowPreviewBoxes()
        {
            if (!pdfViewControl.IsLoaded) { return; }
            double ratio = 1.0 *pdfViewControl.ZoomPercentage / 75.0;

            int nNew = Settings.QS.n; 
            if (nNew > 4) nNew = 4;

            while (preview.Children.Count > nNew)
                preview.Children.RemoveAt(nNew);
            while (preview.Children.Count < nNew)
            {
                var r = new System.Windows.Shapes.Rectangle();

                preview.Children.Add(r);
            }
            for (int i = 0; i<preview.Children.Count; i++)
            {
                var r = preview.Children[i] as System.Windows.Shapes.Rectangle;
                Canvas.SetTop(r, 5 +ratio* i * (Settings.sizeOfBox.Height + Settings.QS.spaceBtwAn));
                Canvas.SetLeft(r, 5);
                r.Height = (Settings.sizeOfBox.Height+ 2*Settings.baundWidth )* ratio;
                r.Width = (Settings.sizeOfBox.Width+ 2*Settings.baundWidth) *ratio;
                r.Stroke= new System.Windows.Media.SolidColorBrush(Settings.baundColor.ColorFromDrawing());
                r.StrokeThickness = Settings.baundWidth*ratio;
            }

        }

        //undo
        private void UndoBox()
        {
            int i = Settings.boxesInQuestions.Count;
            if (i == 0) return;
            int j = Settings.boxesInQuestions[i - 1].Count;
            if (j == 0) { Settings.boxesInQuestions.RemoveAt(i - 1); UndoBox(); }
            else
            {
                Settings.boxesInQuestions[i - 1].RemoveAt(j - 1);
            }
            RemakeBoxexAndFieldsN();
        }
        private void UndoQuestion()
        {
            int i = Settings.boxesInQuestions.Count;
            if (i == 0) return;
            Settings.boxesInQuestions.RemoveAt(i - 1);
            RemakeBoxexAndFieldsN();

        }
        private void undoBButton_Click(object sender, RoutedEventArgs e)
        {
            UndoBox();
        }
        private void undoQButton_Click(object sender, RoutedEventArgs e)
        {
            UndoQuestion();
        }
        private void undoFButton_Click(object sender, RoutedEventArgs e)
        {
            int i = Settings.pagesFields.Count;
            if (i == 0) return;
            Settings.pagesFields.RemoveAt(i - 1);
            RemakeBoxexAndFieldsN();

        }

        private void undoNFButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.nameField == null) return;
            Settings.nameField = null;
            RemakeBoxexAndFieldsN();
        }

        private void RemakeBoxexAndFieldsN()
        {
            var doc = new PdfLoadedDocument(Settings.tempFileCopy);
            doc.RemakeBoxex();
            doc.RemakeFields();
            doc.RemakeNameField();

            pdfViewControl.Load(doc);
            ReloadDocument();
        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sizeSlider != null)
            {
                float s = (float)Math.Round(sizeSlider.Value, 3);
                Settings.sizeOfBox = new SizeF(s, s);
                Settings.QS.indexFontSize = s / 2;

            }
            if (widthSlider != null)
            {
                float f = (float)Math.Round(widthSlider.Value, 3);
                Settings.baundWidth = f;
            }

            if (spaceSlider != null)
            {
                float p = (float)Math.Round(spaceSlider.Value, 3);
                Settings.QS.spaceBtwAn = p;
            }
            if (countSlider != null)
            {
                Settings.QS.n = (int)countSlider.Value;
            }
            CalculateAndShowPreviewBoxes();

        }

        //unused
        private void PdfEditW_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rect = new();


            rect.Location = new((int)locFirstVertex.X, (int)locFirstVertex.Y);
            rect.Size = new((int)(Mouse.GetPosition(this).X - locFirstVertex.X - 2 * rectangleR.StrokeThickness), (int)(Mouse.GetPosition(this).Y - locFirstVertex.Y - 2 * rectangleR.StrokeThickness));

            rect = rect.EvaluateInPositiveSize();

            Thickness thickness = new(rect.X, rect.Y, 0, 0);
            rectangleR.Margin = thickness;

            rectangleR.Width = rect.Width;
            rectangleR.Height = rect.Height;

        }
        private void Pdfwcontrol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var pointToWindow = Mouse.GetPosition(this);

            //BitmapImage bitmap = new();
            //bitmap.BeginInit();
            //bitmap.UriSource = new Uri(fileName);
            //bitmap.EndInit();
            //ImageControl.Source = bitmap;

        }
        public void HideTools()
        {
            //Get the instance of the toolbar using its template name.
            DocumentToolbar toolbar = pdfViewControl.Template.FindName("PART_Toolbar", pdfViewControl) as DocumentToolbar;

            //Get the instance of the open file button using its template name.

            var Filetoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_FileToggleButton", toolbar);
            var Navigationtoolsseparatorkl = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("Part_NavigationToolsSeparator", toolbar);
            var Firstpagetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonGoToFirstPage", toolbar);
            var Previouspagetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonGoToPreviousPage", toolbar);
            var Currentpagenumbertoolkl = (System.Windows.Controls.TextBox)toolbar.Template.FindName("PART_TextCurrentPageIndex", toolbar);
            var Pagecounttoolkl = (System.Windows.Controls.TextBlock)toolbar.Template.FindName("PART_LabelTotalPageCount", toolbar);
            var Nextpagetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonGoToNextPage", toolbar);
            var Lastpagetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonGoToLastPage", toolbar);
            var Zoomtoolsseparatorkl = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("Part_ZoomToolsSeparator_0", toolbar);
            var Currentzoomleveltoolkl00 = (System.Windows.Controls.ComboBox)toolbar.Template.FindName("PART_ComboBoxCurrentZoomLevel", toolbar);
            var Zoomintoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonZoomIn", toolbar);
            var Zoomouttoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonZoomOut", toolbar);
            var Zoomtoolsseparatorkl01 = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("PART_ZoomToolsSeparator_1", toolbar);
            var Fitwidthtoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonFitWidth", toolbar);
            var Fitpagetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonFitPage", toolbar);
            var Annotationtoolsseparatorkl = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("PART_AnnotationToolsSeparator", toolbar);
            var Inktoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Ink", toolbar);
            var Highlighttoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Highlight", toolbar);
            var Underlinetoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Underline", toolbar);
            var Strikethroughtoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Strikethrough", toolbar);
            var Shapestoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Shapes", toolbar);
            var Filltoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Fill", toolbar);
            var Addtextboxtoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_FreeText", toolbar);
            var Textpropertiestoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonTextBoxFont", toolbar);
            var Separatorbetweentheannotationandcursortoolskl = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("PART_AnnotationsSeparator", toolbar);
            var Stamptoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_Stamp", toolbar);
            var Handwrittensignaturetoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonSignature", toolbar);
            var Selecttoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_SelectTool", toolbar);
            var Handtoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_HandTool", toolbar);
            var Marqueezoomtoolkl = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_MarqueeZoom", toolbar);
            var Separatorbetweenthecursortoolsandtextsearchbuttonkl = (System.Windows.Shapes.Rectangle)toolbar.Template.FindName("Part_CursorTools", toolbar);
            var Textsearchtoolkl = (System.Windows.Controls.Button)toolbar.Template.FindName("PART_ButtonTextSearch", toolbar);



            //Set the visibility of the button to collapsed.
            //Filetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Navigationtoolsseparatorkl.Visibility = System.Windows.Visibility.Collapsed;
            Firstpagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Previouspagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Currentpagenumbertoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Currentpagenumbertoolkl.Visibility = System.Windows.Visibility.Collapsed;
            //Pagecounttoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Nextpagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Lastpagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Lastpagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Zoomtoolsseparatorkl.Visibility = System.Windows.Visibility.Collapsed;
            Currentzoomleveltoolkl00.Visibility = System.Windows.Visibility.Collapsed;
            Zoomintoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Zoomouttoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Zoomtoolsseparatorkl01.Visibility = System.Windows.Visibility.Collapsed;
            Fitwidthtoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Fitpagetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Annotationtoolsseparatorkl.Visibility = System.Windows.Visibility.Collapsed;
            Inktoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Highlighttoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Underlinetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Strikethroughtoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Shapestoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Filltoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Addtextboxtoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Textpropertiestoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Separatorbetweentheannotationandcursortoolskl.Visibility = System.Windows.Visibility.Collapsed;
            Stamptoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Handwrittensignaturetoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Selecttoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Handtoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Marqueezoomtoolkl.Visibility = System.Windows.Visibility.Collapsed;
            Separatorbetweenthecursortoolsandtextsearchbuttonkl.Visibility = System.Windows.Visibility.Collapsed;
            Textsearchtoolkl.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void HideMenuTool()
        {
            //Get the instance of the toolbar using its template name.
            DocumentToolbar toolbar = pdfViewControl.Template.FindName("PART_Toolbar", pdfViewControl) as DocumentToolbar;

            //Get the instance of the file menu button using its template name.
            var MenuItem = (System.Windows.Controls.Primitives.ToggleButton)toolbar.Template.FindName("PART_FileToggleButton", toolbar);

            //Get the instance of the file menu button context menu and the item collection.
            ContextMenu FileContextMenu = MenuItem.ContextMenu;
            foreach (MenuItem FileMenuItem in FileContextMenu.Items)
            {
                //Get the instance of the open menu item using its template name and disable its visibility.
                if (FileMenuItem.Name == "PART_OpenMenuItem")
                    FileMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                if (FileMenuItem.Name == "PART_SaveMenuItem")
                    FileMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                if (FileMenuItem.Name == "PART_SaveAsMenuItem")
                    FileMenuItem.Visibility = System.Windows.Visibility.Collapsed;
                if (FileMenuItem.Name == "PART_PrintMenuItem")
                    FileMenuItem.Visibility = System.Windows.Visibility.Collapsed;

            }
        }


        private void CloseCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Close();
        }
        private void CommandBindingNameField_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            nameField.IsChecked ^= true;
        }
        private void CommandBindingBoxes_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            boxex.IsChecked ^= true;
        }
        private void CommandBindingFields_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            b.IsChecked ^= true;
        }
        private void CommandBindingUndoQ_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UndoQuestion();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (saved) return;
            var f = MessageBox.Show(Strings.savequestion, Strings.closing, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (f == MessageBoxResult.Cancel) e.Cancel = true;
            if (f != MessageBoxResult.Yes) return;
            else if (Menu_Project_Save.IsEnabled) Menu_Save_Project_Click(null, null);
            else if (Menu_Project_SaveAs.IsEnabled) Menu_Save_ProjectAs_Click(null, null);
            else MessageBox.Show(Strings.NoProjectLoaded, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
           StaticMethods.DeleteTempFiles();
        }


        ///// <summary>
        ///// Print all pages of an XPS document.
        ///// Optionally, hide the print dialog window.
        ///// </summary>
        ///// <param name="xpsFilePath">Path to source XPS file</param>
        ///// <param name="hidePrintDialog">Whether to hide the print dialog window (shown by default)</param>
        ///// <returns>Whether the document printed</returns>
        //public static bool PrintWholeDocument(string xpsFilePath, bool hidePrintDialog = false)
        //{
        //    // Create the print dialog object and set options.
        //    PrintDialog printDialog = new();

        //    if (!hidePrintDialog)
        //    {
        //        // Display the dialog. This returns true if the user presses the Print button.
        //        bool? isPrinted = printDialog.ShowDialog();
        //        if (isPrinted != true)
        //            return false;
        //    }

        //    // Print the whole document.
        //    try
        //    {
        //        // Open the selected document.
        //        XpsDocument xpsDocument = new(xpsFilePath, FileAccess.Read);

        //        // Get a fixed document sequence for the selected document.
        //        FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();

        //        // Create a paginator for all pages in the selected document.
        //        DocumentPaginator docPaginator = fixedDocSeq.DocumentPaginator;

        //        // Print to a new file.
        //        printDialog.PrintDocument(docPaginator, $"Printing {Path.GetFileName(xpsFilePath)}");

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);

        //        return false;
        //    }
        //}
        ///// <summary>
        ///// Asyncronously, add a batch of XPS documents to the print queue using a PrintQueue.AddJob method.
        ///// Handle the thread apartment state required by the PrintQueue.AddJob method.
        ///// </summary>
        ///// <param name="xpsFilePaths">A collection of XPS documents.</param>
        ///// <param name="fastCopy">Whether to validate the XPS documents.</param>
        ///// <returns>Whether all documents were added to the print queue.</returns>
        //public static async Task<bool> BatchAddToPrintQueueAsync(IEnumerable<string> xpsFilePaths, bool fastCopy = false)
        //{
        //    bool allAdded = true;

        //    // Queue some work to run on the ThreadPool.
        //    // Wait for completion without blocking the calling thread.
        //    await Task.Run(() =>
        //    {
        //        if (fastCopy)
        //            allAdded = BatchAddToPrintQueue(xpsFilePaths, fastCopy);
        //        else
        //        {
        //            // Create a thread to call the PrintQueue.AddJob method.
        //            Thread newThread = new(() =>
        //            {
        //                allAdded = BatchAddToPrintQueue(xpsFilePaths, fastCopy);
        //            });

        //            // Set the thread to single-threaded apartment state.
        //            newThread.SetApartmentState(ApartmentState.STA);

        //            // Start the thread.
        //            newThread.Start();

        //            // Wait for thread completion. Blocks the calling thread,
        //            // which is a ThreadPool thread.
        //            newThread.Join();
        //        }
        //    });

        //    return allAdded;
        //}

        ///// <summary>
        ///// Add a batch of XPS documents to the print queue using a PrintQueue.AddJob method.
        ///// </summary>
        ///// <param name="xpsFilePaths">A collection of XPS documents.</param>
        ///// <param name="fastCopy">Whether to validate the XPS documents.</param>
        ///// <returns>Whether all documents were added to the print queue.</returns>
        //public static bool BatchAddToPrintQueue(IEnumerable<string> xpsFilePaths, bool fastCopy)
        //{
        //    bool allAdded = true;

        //    // To print without getting the "Save Output File As" dialog, ensure
        //    // that your default printer is not the Microsoft XPS Document Writer,
        //    // Microsoft Print to PDF, or other print-to-file option.

        //    // Get a reference to the default print queue.
        //    PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

        //    // Iterate through the document collection.
        //    foreach (string xpsFilePath in xpsFilePaths)
        //    {
        //        // Get document name.
        //        string xpsFileName = Path.GetFileName(xpsFilePath);

        //        try
        //        {
        //            // The AddJob method adds a new print job for an XPS
        //            // document into the print queue, and assigns a job name.
        //            // Use fastCopy to skip XPS validation and progress notifications.
        //            // If fastCopy is false, the thread that calls PrintQueue.AddJob
        //            // must have a single-threaded apartment state.
        //            PrintSystemJobInfo xpsPrintJob =
        //                    defaultPrintQueue.AddJob(jobName: xpsFileName, documentPath: xpsFilePath, fastCopy);

        //            // If the queue is not paused and the printer is working, then jobs will automatically begin printing.
        //            Debug.WriteLine($"Added {xpsFileName} to the print queue.");
        //        }
        //        catch (PrintJobException e)
        //        {
        //            allAdded = false;
        //            Debug.WriteLine($"Failed to add {xpsFileName} to the print queue: {e.Message}\r\n{e.InnerException}");
        //        }
        //    }

        //    return allAdded;
        //}

    }
}


//TODO update the save project property (settings save)
//  (add saving preferences)

//TODO add help and settings (help not finished) (settings not even designed)
//TODO add lock function

//TODO remake tuple to new variable, add bound width, mabe colors (or at least add bound width to tuple)
//todoDone repair commands in pdfviewer
//TODOdone positioning in evaluation
//make edge detection direction dependent
//add non linar transformation
//todo topbar bug when maximazed
//tododone debug starting from file
//todo find the right moment to add mositioners
//TODO all in one window
//TODO design using 2 files
