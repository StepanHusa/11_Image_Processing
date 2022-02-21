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



            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\";
                //this.WindowState = WindowState.Minimized;
                //Menu_Load_New_Click(new object(), new RoutedEventArgs());
                //Menu_Edit_AddBoxex_Click(new object(), new RoutedEventArgs());

                //LoadDataFromFile(debugFolder + "\\01" + Settings.projectExtension);
                LoadDataFromFile(debugFolder + "test\\01" + Settings.projectExtension);

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


            Settings.tempFile = tempPdf;
            Settings.tempFileCopy = tempCopy;
            Settings.fileName = fileName;

            ReloadWindowContent();
        }

        //open
        private void Menu_Open_Project_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new() { Title = "Open Template", Filter = $"File Template(*{Settings.projectExtension})|*{Settings.projectExtension}" };
            if (open.ShowDialog() == false) return;

            LoadDataFromFile(open.FileName);


        }
        //Edit
        private void Menu_Edit_Questions_Click(object sender, RoutedEventArgs e)
        {
            //TODO add second window for edit
        }
        private void CommandBinding_Edit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PdfEditW m = new();
            m.Show();
            m.pdfViewControl.ShowToolbar = false;
        }
        //Save
        private void Menu_Save_ProjectAs_Click(object sender, RoutedEventArgs e)
        {
            Settings.versions.Add(DateTime.Now.ToStringOfRegularFormat());

            SaveFileDialog save = new() { Title = "Save Template", Filter = $"File Template(*{Settings.projectExtension})|*{Settings.projectExtension}", FileName = Settings.projectName };
            if (save.ShowDialog() == false) return;

            SaveDataToFile(save.FileName);

            Settings.projectFileName = save.FileName;
            ReloadWindowContent();

            Menu_Project_Save.IsEnabled = true;
        }
        private void Menu_Save_Project_Click(object sender, RoutedEventArgs e)
        {
            Settings.versions.Add(DateTime.Now.ToStringOfRegularFormat());

            SaveDataToFile(Settings.projectFileName);

            ReloadWindowContent();
        }
        private void Menu_Save_PDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new() { Title = "Save PDF", Filter = $"File Template(*.PDF)|*.PDF", FileName = Settings.projectName };
            if (save.ShowDialog() == false) return;


            File.Copy(Settings.tempFile, save.FileName, true);
            File.Copy(Settings.tempFile, Settings.tempFileCopy, true);
        }

        private void SaveDataToFile(string fileName)
        {
            byte[] FormatCode = Settings.fileCode; //8 Byte identification code
            byte[] documentpdf = File.ReadAllBytes(Settings.tempFile);
            //byte[] listOfPointFsArray = Settings.pagesPoints.PointListArrayToByteArray();
            byte[] listBoxesInQuestions = Settings.boxesInQuestions.IntRectangleFBoolTupleListListToByteArray();
            byte[] listOfFields = Settings.pagesFields.RectangleListToByteArray();
            byte[] nameField = Settings.nameField.RectangleTupleToByteArray();

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
            byte[] nameField = Settings.nameField.RectangleTupleToByteArray();

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
            var dir = Settings.tempDirectoryName;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";


            File.WriteAllBytes(tempPdf, documentpdf);
            File.WriteAllBytes(tempCopy, documentpdf);

            Settings.tempFile = tempPdf;
            Settings.tempFileCopy = tempCopy;
            Settings.projectFileName = filename;
            //Settings.pagesPoints = listOfPointFsArray.ByteArrayToPointFListArray();
            Settings.boxesInQuestions = listBoxesInQuestions.ByteArrayToIntRectangleFBoolTupleListList();
            Settings.pagesFields = listOfFields.ByteArrayToRectangleList();
            Settings.nameField = nameField.ByteArrayToRectangleTuple();

            ReloadWindowContent();
            Menu_Project_Save.IsEnabled = true;
        }

        //Read
        //one page files
        private void Menu_Read_ListOfScans_OnePage_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true }; 
            if (open.ShowDialog() == false) return;

            int l = Settings.scansInPagesInWorks.Count();//moves indexing if there are already bitmaps in the list
            for (int i = 0; i < open.FileNames.Length; i++)
            {
                Settings.scansInPagesInWorks.Add(new());
                Settings.scansInPagesInWorks[l + i].Add(open.FileNames[i]);
            }
        }
        //advanced read
        private async void Menu_Read_ListOfScans_Dialog_Click(object sender, RoutedEventArgs e)
        {
            // var open = new OpenFileDialog() { Title = "Open list of scans PDF", Filter = $"Pictures (all readable)|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true }; //TODO make for images (maybe make method for strings)
            var open = new OpenFileDialog() { Title = Strings.Openlistofscans, Filter = Strings.Picturesallreadable + $"|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*", Multiselect = true };
            if (open.ShowDialog() == false) return;

            await LoadNumberOfFiles(open.FileNames.Count(), open.FileNames);

        }
        private async void Menu_Read_Scan_Click(object sender, RoutedEventArgs e)
        {
            var a = new ScanForm();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            await LoadNumberOfFiles(a.tempScans.Count, a.tempScans.ToArray());
        }
        private async Task LoadNumberOfFiles(int Number, string[] ImageFiles)
        {
            var a = new ImportPicturesDialogW(Number);
            if (a.ShowDialog() != true)
            {
                if (MessageBox.Show("Realy return?", "caption", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    return;
                else a.ShowDialog();
            }

            var da = a.Answer;
            var invert = a.Invert.Value;
            a.Close();

            Settings.scansInPagesInWorks = await Task.Run(() => { 

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

        //Print
        private void Menu_Export_ToJPEG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save JPEG", Filter = "JPEG(*.jpeg)|*.jpeg" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();

            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i,Settings.dpiExport,Settings.dpiExport);

                string fn = Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn,System.Drawing.Imaging.ImageFormat.Jpeg);
            }

        }
        private void Menu_Export_ToPNG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save PNG", Filter = "PNG(*.png)|*.png" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(Settings.tempFile);
            doc.RemakeBoxexOneColor();

            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i, Settings.dpiExport, Settings.dpiExport);

                string fn =Path.GetDirectoryName(save.FileName)+"\\"+ Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
            }

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


            PrintDialog printDialog = new() { SelectedPagesEnabled = true, UserPageRangeEnabled=true, CurrentPageEnabled=true};
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

        //evaluate
        private async void Menu_Eavluate_Click(object sender, RoutedEventArgs e)
        {
            int highestPageIndex=0;
            foreach (var question in Settings.boxesInQuestions)
                foreach (var box in question)
                    if (box.Item1 > highestPageIndex)
                        highestPageIndex = box.Item1;
            int lowestPagesInWork = int.MaxValue;
            foreach (var work in Settings.scansInPagesInWorks)
                if (work.Count < lowestPagesInWork)
                    lowestPagesInWork = work.Count;
            if (highestPageIndex > lowestPagesInWork) MessageBox.Show(Strings.warningScansDontHaveEnaughtPages, Strings.Warning);


            var pbw = new ProgressBarW();
            pbw.Show();
            await Task.Run(() => Settings.resultsInQuestionsInWorks = Settings.scansInPagesInWorks.EvaluateWorks(Settings.boxesInQuestions));

            Settings.namesScaned = Settings.scansInPagesInWorks.GetCropedNames(Settings.nameField);
            new ViewResultW().Show();
            pbw.Close();
        }

        //help and settings
        private void Menu_Help_HTML_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, "StudentTesterHelp.html"); //TODO make portable

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
            if (Settings.tempFile==null) throw new Exception("ReloadWindowContent whan no loaded document");
            var doc = new PdfLoadedDocument(Settings.tempFile);

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
            loadedPdfLabel.Content = Path.GetFileName(Settings.fileName);
            //pdfDocumentView.MinimumZoomPercentage = pdfDocumentView.ZoomPercentage;
            pdfDocumentView.ZoomTo(-1);

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
                ii ++;
            fieldcounttext.Text = ii.ToString();

            versionCombobox.Items.Clear();
            foreach (var item in Settings.versions)
                versionCombobox.Items.Add(item);
            versionCombobox.SelectedIndex = versionCombobox.Items.Count - 1;
            //todo make odler versions and Ctrl+Z usable

            if (Settings.versions.Count != 0)
                dateoflastsavetext.Text = Settings.versions.Last();
            else dateoflastsavetext.Text =Strings.notsavedyet;

            //dateoflastsavetext.Text = Settings.versions.Last().ToStringOfRegularFormat();

        }
        private void Unload()
        {
            Settings.nameField = null;
            Settings.pagesFields = new();
            Settings.boxesInQuestions = new();
            Settings.resultsInQuestionsInWorks = null;
            Settings.scansInPagesInWorks = new();

            reloadButton.IsEnabled = false;
            this.Activated -= Window_Activated;

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
            versionCombobox.Items.Clear();
            dateoflastsavetext.Text = string.Empty;

            Title = Settings.appName + " -- #Unloaded";

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

        private void CloceClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState != WindowState.Normal)
            {
                this.WindowState = WindowState.Normal;
            }

            this.DragMove();
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

	
//TODO update the save project property 
//  (add saving preferences)

//TODO add help and settings (help not finished) (settings not even designed)
//TODO add lock function

//TODO remake tuple to new variable, add bound width, mabe colors
//todo repair commands in pdfviewer