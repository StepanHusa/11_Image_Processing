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

            //Hotkeys
            {
                HotkeysManager.SetupSystemHook();
                //add individual hotkyes
                //HotkeysManager.AddHotkey(ModifierKeys.Control, Key.E, () => { if (Menu_Edit.IsEnabled) Menu_Edit_NoTools_Click(null, null); });
                HotkeysManager.AddHotkey(ModifierKeys.Control, Key.S, () => { if (Menu_Project_Save.IsEnabled) Menu_Save_Project_Click(null, null); else if (Menu_Project_SaveAs.IsEnabled) Menu_Save_ProjectAs_Click(null, null); });
                HotkeysManager.AddHotkey((ModifierKeys.Control | ModifierKeys.Shift), Key.S, () => { if (Menu_Project_SaveAs.IsEnabled) Menu_Save_ProjectAs_Click(null, null); });

            }

            //events
            Closing += MainWindow_Closing;

            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files";
                //this.WindowState = WindowState.Minimized;
                //Menu_Load_New_Click(new object(), new RoutedEventArgs());
                //Menu_Edit_AddBoxex_Click(new object(), new RoutedEventArgs());

                //LoadDataFromFile(debugFolder + "\\01" + ST.projectExtension);
                LoadDataFromFile(debugFolder + "\\02" + ST.projectExtension);

                //Menu_Save_Project.IsEnabled = true;

                string s = debugFolder + "\\s\\";
                string f = debugFolder + "\\maxresdefault.jpg";
                string g = debugFolder + "\\02c.bmp";



                //ST.scansInPagesInWorks.Add(new());
                //ST.scansInPagesInWorks[0].Add(new Bitmap(debugFolder + "\\02(0).png"));

                //for (int i = 0; i < 5; i++)
                //{
                //    ST.scansInPagesInWorks.Add(new());
                //    for (int j = 0; j < 6; j++)
                //    {
                //        string h = s + (i * 6 + j) + ".bmp";
                //        ST.scansInPagesInWorks[i /*+ 1*/].Add(new Bitmap(h));

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

                //    //ST.scansInPagesInWorks[0].Add(new Bitmap(debugFolder + @"\01.png"));
                //    //ST.scansInPagesInWorks[0][0] = new Bitmap(debugFolder + @"\01.png");
                //    //ST.document.EvaluateSet(); 

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
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Need to shutdown the hook. idk what happens if
            // you dont, but it might cause a memory leak.
            HotkeysManager.ShutdownSystemHook();
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
            var open = new OpenFileDialog() { Title = "Open Word Doc", Filter = "Word Document(*.doc;*docx)|*.doc;*docx" };
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
            var dir = ST.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";

            if (fileName == null)
            {
                PdfExtensions.NewPdfDoc(tempPdf);
                PdfExtensions.NewPdfDoc(tempCopy);


                this.Title = "*untitled";
            }
            else if (File.Exists(fileName))
            {
                File.Copy(fileName, tempPdf);
                File.Copy(tempPdf, tempCopy);

                this.Title = Path.GetFileName(fileName);
            }


            ST.tempFile = tempPdf;
            ST.tempFileCopy = tempCopy;
            ST.fileName = fileName;

            ReloadWindowContent();
        }

        //open
        private void Menu_Open_Project_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new() { Title = "Open Template", Filter = $"File Template(*{ST.projectExtension})|*{ST.projectExtension}" };
            if (open.ShowDialog() == false) return;

            LoadDataFromFile(open.FileName);


        }
        //Edit
        private void CommandBinding_Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfEditW m = new();
            m.Show();
            m.pdfViewControl.ShowToolbar = false;
        }
        //Save
        private void Menu_Save_ProjectAs_Click(object sender, RoutedEventArgs e)
        {
            ST.versions.Add(DateTime.Now.ToStringOfRegularFormat());

            SaveFileDialog save = new() { Title = "Save Template", Filter = $"File Template(*{ST.projectExtension})|*{ST.projectExtension}", FileName = ST.projectName };
            if (save.ShowDialog() == false) return;

            SaveDataToFile(save.FileName);

            ST.projectFileName = save.FileName;
            ReloadWindowContent();

            Menu_Project_Save.IsEnabled = true;
        }
        private void Menu_Save_Project_Click(object sender, RoutedEventArgs e)
        {
            ST.versions.Add(DateTime.Now.ToStringOfRegularFormat());

            SaveDataToFile(ST.projectFileName);

            ReloadWindowContent();
        }
        private void Menu_Save_PDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new() { Title = "Save PDF", Filter = $"File Template(*.PDF)|*.PDF", FileName = ST.projectName };
            if (save.ShowDialog() == false) return;


            File.Copy(ST.tempFile, save.FileName, true);
            File.Copy(ST.tempFile, ST.tempFileCopy, true);
        }

        private void SaveDataToFile(string fileName)
        {
            byte[] FormatCode = ST.fileCode; //8 Byte identification code
            byte[] documentpdf = File.ReadAllBytes(ST.tempFile);
            //byte[] listOfPointFsArray = ST.pagesPoints.PointListArrayToByteArray();
            byte[] listBoxesInQuestions = ST.boxesInQuestions.IntRectangleFBoolTupleListListToByteArray();
            byte[] listOfFields = ST.pagesFields.RectangleListToByteArray();
            byte[] nameField = ST.nameField.RectangleTupleToByteArray();

            int docLength = documentpdf.Length; //int32
            //int listPLength = listOfPointFsArray.Length; //int 32
            int bInQLength = listBoxesInQuestions.Length;
            int listFLength = listOfFields.Length;//int 32
            int nameFieldLength = nameField.Length;//int 32


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
                    bw.Write(ms.ToArray().GetHashSHA1()); //closes file with hashcode to check if the file is the same and if we got to the end at the right time


                }
                File.WriteAllBytes(fileName, ms.ToArray());
            }



        }
        private void LoadDataFromFile(string filename)
        {
            //declare components
            byte[] FormatCode;
            byte[] documentpdf;
            //byte[] listOfPointFsArray;
            byte[] listBoxesInQuestions;
            byte[] listOfFields;
            byte[] nameField = ST.nameField.RectangleTupleToByteArray();

            byte[] hash;

            int docLength;
            //int listPLength;
            int bInQLength;
            int listFLength;
            int nameFieldLength = nameField.Length;//int 32


            //load components from file
            using (FileStream fs = new(filename, FileMode.Open))
            {
                using (BinaryReader br = new(fs))
                {
                    try
                    {
                        FormatCode = br.ReadBytes(8);
                        if (!FormatCode.SequenceEqual(ST.fileCode)) { MessageBox.Show("Open Template file wasn`t generated by this program"); return; }

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

                        hash = br.ReadBytes(20);
                    }
                    catch { MessageBox.Show("Not able to load this file"); return; }
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
                    hashNew = ms.ToArray().GetHashSHA1();
                }

            }
            //hash check
            if (!hash.SequenceEqual(hashNew))
            {
                MessageBoxResult dialogResult = MessageBox.Show("The File you opened was propably changed by other program \n Ignore by clicking OK, or Cancel", "Warning", MessageBoxButton.OKCancel);
                if (dialogResult == MessageBoxResult.Cancel)
                    return;
            }

            //initialize 
            var dir = ST.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";


            File.WriteAllBytes(tempPdf, documentpdf);
            File.WriteAllBytes(tempCopy, documentpdf);

            ST.tempFile = tempPdf;
            ST.tempFileCopy = tempCopy;
            ST.projectFileName = filename;
            //ST.pagesPoints = listOfPointFsArray.ByteArrayToPointFListArray();
            ST.boxesInQuestions = listBoxesInQuestions.ByteArrayToIntRectangleFBoolTupleListList();
            ST.pagesFields = listOfFields.ByteArrayToRectangleList();
            ST.nameField = nameField.ByteArrayToRectangleTuple();

            ReloadWindowContent();
            Menu_Project_Save.IsEnabled = true;
        }

        private void Menu_Print_Click(object sender, RoutedEventArgs e)
        {
            //PdfPrintingNet.PdfPrint printDoc = new(string.Empty,string.Empty);

            //printDoc.Print(ST.tempFile);
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



            PdfLoadedDocument doc = new(ST.tempFile);
            


            PrintDialog printDialog = new() { SelectedPagesEnabled = true, UserPageRangeEnabled=true, CurrentPageEnabled=true};
            if (printDialog.ShowDialog() != true) return;

            string xps = Path.GetTempFileName();
            var document = new Aspose.Pdf.Document(ST.tempFile);
            document.Save(xps, Aspose.Pdf.SaveFormat.Xps);

            // Open the selected document.
            XpsDocument xpsDocument = new(xps, FileAccess.Read);

            // Get a fixed document sequence for the selected document.
            FixedDocumentSequence fixedDocSeq = xpsDocument.GetFixedDocumentSequence();

            // Create a paginator for all pages in the selected document.
            DocumentPaginator docPaginator = fixedDocSeq.DocumentPaginator;

            // Print to a new file.
            printDialog.PrintDocument(docPaginator, $"Printing {ST.fileName}");


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
        //one
        //TODO eventualy remove fromm menu
        private void Menu_Read_PNG_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "open PNG", Filter = "PNG(*.png)|*.png" };
            if (open.ShowDialog() != true) return;
            //open.FileName;

            ST.scansInPagesInWorks = new();
            ST.scansInPagesInWorks.Add(new());
            ST.scansInPagesInWorks[0][0] = new Bitmap(open.FileName);
        }
        //one page files
        private void Menu_Read_ListOfScans_OnePage_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true };//todo add more filters 
            if (open.ShowDialog() == false) return;

            int l = ST.scansInPagesInWorks.Count();//moves indexing if there are already bitmaps in the list
            for (int i = 0; i < open.FileNames.Length; i++)
            {
                ST.scansInPagesInWorks.Add(new());
                ST.scansInPagesInWorks[l + i].Add(new Bitmap(open.FileNames[i]));
            }
        }
        //advanced read
        private void Menu_Read_ListOfScans_Dialog_Click(object sender, RoutedEventArgs e)
        {
            // var open = new OpenFileDialog() { Title = "Open list of scans PDF", Filter = $"Pictures (all readable)|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true }; //TODO make for images (maybe make method for strings)
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true };//todo add more filters 
            if (open.ShowDialog() == false) return;

            LoadNumberOfFiles(open.FileNames.Count(), open.FileNames);

        }
        private void Menu_Read_Scan_Click(object sender, RoutedEventArgs e)
        {
            var a = new ScanForm();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            LoadNumberOfFiles(a.tempScans.Count, a.tempScans.ToArray());
        }
        private void LoadNumberOfFiles(int Number, string[] ImageFiles)
        {
            var a = new ImportPicturesDialogW(Number);
            if (a.ShowDialog() != true)
            {
                if (MessageBox.Show("Realy return?", "caption", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    return;
                else a.ShowDialog();
            }

            var da = a.Answer;

            List<List<Bitmap>> works = new();
            int ii = 0;
            if (!a.Invert.Value)
                for (int i = 0; i < da.Item1; i++)
                {
                    works.Add(new());
                    for (int j = 0; j < da.Item2; j++)
                    {
                        works[i].Add(new Bitmap(ImageFiles[ii])); //BMP, GIF, EXIF, JPG, PNG and TIFF
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
                        works[i].Add(new Bitmap(ImageFiles[ii])); //BMP, GIF, EXIF, JPG, PNG and TIFF
                        ii++;
                    }
                }

            }

            ST.scansInPagesInWorks = works;
        }

        //Print
        private void Menu_Export_ToJPEG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save JPEG", Filter = "JPEG(*.jpeg)|*.jpeg" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(ST.tempFile);


            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i,ST.dpiExport,ST.dpiExport);

                string fn = Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn,System.Drawing.Imaging.ImageFormat.Jpeg);
            }

        }
        private void Menu_Export_ToPNG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save PNG", Filter = "PNG(*.png)|*.png" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(ST.tempFile);


            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i, ST.dpiExport, ST.dpiExport);

                string fn =Path.GetDirectoryName(save.FileName)+"\\"+ Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
            }

        }

        private void Menu_Eavluate_Click(object sender, RoutedEventArgs e)
        {
            //ST.resultsInQuestionsInWorks = ST.scansInPagesInWorks.EvaluateWorks(ST.boxesInQuestions, new PdfLoadedDocument(ST.tempFile).GetSizesOfPages());

            ST.resultsInQuestionsInWorks = ST.scansInPagesInWorks.EvaluateWorks(ST.boxesInQuestions);
            ST.namesScaned = ST.scansInPagesInWorks.GetCropedNames(ST.nameField);
            new ViewResultW().Show();

            //string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files";
            //string s = debugFolder + "\\s\\1.bmp";
            //string f = debugFolder + "\\maxresdefault.jpg";
            //string g = debugFolder + "\\02c.bmp";


            //byte[] photoBytes = File.ReadAllBytes(s);
            //// Format is automatically detected though can be changed.
            //System.Drawing.Size size = new System.Drawing.Size(500, 0);

            //using (MemoryStream inStream = new MemoryStream(photoBytes))
            //{
            //    using (MemoryStream outStream = new MemoryStream())
            //    {
            //        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
            //        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
            //        {
            //            // Load, resize, set the format and quality and save an image.
            //            imageFactory.Load(inStream)
            //                        .DetectEdges(new Laplacian3X3EdgeFilter())
            //                        .Resize(size)
            //                        .Save(g);
            //        }
            //    }
            //}


            //var Ocr = new IronTesseract();
            //using (var input = new OcrInput())
            //{
            //    //input.AddPdf("example.pdf", "password");
            //    //input.AddMultiFrameTiff("multi-frame.tiff");
            //    //input.AddImage("image1.png");
            //    input.AddImage(s);
            //    //... many more
            //    var Result = Ocr.Read(input);

            //    Debug.WriteLine(Result.Text);
            //    Debug.WriteLine(Result.Confidence);

            //}


        }
        //help and settings
        private void Menu_Help_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (ST.settingsWindow == null)
                ST.settingsWindow = new();
            ST.settingsWindow.Show();
        }


        //content
        private void projecttext_LostFocus(object sender, RoutedEventArgs e)
        {
            ST.projectName = projecttext.Text;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ReloadWindowContent();
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
            if (ST.tempFile==null) throw new Exception("ReloadWindowContent whan no loaded document");
            var doc = new PdfLoadedDocument(ST.tempFile);

            Menu_Edit.IsEnabled = true;
            Menu_Project_SaveAs.IsEnabled = true;
            Menu_Print.IsEnabled = true;
            Menu_Read.IsEnabled = true;
            Menu_Eavluate.IsEnabled = true;
            Menu_Export.IsEnabled = true;

            reloadButton.IsEnabled = true;
            this.Activated += Window_Activated;


            pdfDocumentView.UpdateLayout();
            pdfDocumentView.Load(doc);
            loadedPdfLabel.Content = Path.GetFileName(ST.fileName);
            //pdfDocumentView.MinimumZoomPercentage = pdfDocumentView.ZoomPercentage;
            pdfDocumentView.ZoomTo(-1);

            Title = ST.appName + " -- " + ST.projectName;

            projecttext.Text = ST.projectName;
            projectfilenametext.Text = Path.GetFileName(ST.projectFileName);
            locationtext.Text = Path.GetDirectoryName(ST.projectFileName);
            pagecounttext.Text = doc.Pages.Count.ToString();
            questioncounttext.Text = ST.boxesInQuestions.Count.ToString();
            int ii = 0;
            foreach (var question in ST.boxesInQuestions)
            {
                ii += question.Count;
            }
            boxcounttext.Text = ii.ToString();
            ii = 0;
            foreach (var tuples in ST.pagesFields)
                ii ++;
            fieldcounttext.Text = ii.ToString();

            versionCombobox.Items.Clear();
            foreach (var item in ST.versions)
                versionCombobox.Items.Add(item);
            versionCombobox.SelectedIndex = versionCombobox.Items.Count - 1;
            //todo make odler versions and Ctrl+Z usable

            if (ST.versions.Count != 0)
                dateoflastsavetext.Text = ST.versions.Last();
            else dateoflastsavetext.Text =Strings.notsavedyet;

            //dateoflastsavetext.Text = ST.versions.Last().ToStringOfRegularFormat();

        }
        private void Unload()
        {
            ST.nameField = null;
            ST.pagesFields = new();
            ST.boxesInQuestions = new();
            ST.resultsInQuestionsInWorks = null;
            ST.scansInPagesInWorks = new();

            reloadButton.IsEnabled = false;
            this.Activated -= Window_Activated;

            if (File.Exists(ST.tempFile))
                File.Delete(ST.tempFile);
            if (File.Exists(ST.tempFileCopy))
                File.Delete(ST.tempFileCopy);

            ST.tempFile = null;
            ST.tempFileCopy = null;
            ST.projectName = ST.templateProjectName;
            ST.projectFileName = null;
            ST.fileName = null;
            ST.versions = new();


            projecttext.Text = string.Empty;
            projectfilenametext.Text = string.Empty;
            locationtext.Text = string.Empty;
            pagecounttext.Text = string.Empty;
            questioncounttext.Text = string.Empty;
            boxcounttext.Text = string.Empty;
            fieldcounttext.Text = string.Empty;
            versionCombobox.Items.Clear();
            dateoflastsavetext.Text = string.Empty;

            Title = ST.appName + " -- #Unloaded";

            pdfDocumentView.Unload();
            loadedPdfLabel.Content = "";


            Menu_Edit.IsEnabled = false;
            Menu_Project_Save.IsEnabled = false;
            Menu_Project_SaveAs.IsEnabled = false;
            Menu_Print.IsEnabled = false;
            Menu_Export.IsEnabled = false;
            Menu_Read.IsEnabled = false;
            Menu_Eavluate.IsEnabled = false;
        }


        private void pdfDocumentView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double r = e.NewSize.Width / e.PreviousSize.Width;

            int newZoom =(int) (pdfDocumentView.ZoomPercentage*r);
                pdfDocumentView.ZoomTo(newZoom);
        }

        private void CommandBinding_CanExecuteTRUE(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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

//TODO get the mic form connected (or maybe not) 	
//TODO update the save project property 
//  (add saving preferences)

//TODO work on the recognasing 
//TODO add help and settings
//TODO add lock function
//TODO no green in export and print

