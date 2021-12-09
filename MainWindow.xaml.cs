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
                HotkeysManager.AddHotkey(ModifierKeys.Control, Key.E, () => { Menu_Edit_AddBoxex_Click(new object(),new RoutedEventArgs()); });


            }

            //events
            Closing += MainWindow_Closing;

            //debug
            {
                string debugFolder = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files";

                //this.WindowState = WindowState.Minimized;
                Menu_Load_New_Click(new object(), new RoutedEventArgs());
                //Menu_Edit_AddBoxex_Click(new object(), new RoutedEventArgs());

                //rec debug
                {

                    string d01 = debugFolder + @"\01.pdf";

                    List<PointF> pointsPage1 = new();
                    List<PointF> pointsPage2 = new();
                    List<Color> colors = new();

                    colors.Add(Color.Black);
                    colors.Add(Color.Gray);
                    colors.Add(Color.White);
                    for (int i = 0; i < 3; i++)
                    {
                        pointsPage1.Add(new PointF(20, 20 + 40 * i));
                    }
                    //d01.DebugCreateFile(pointsPage1,colors);

                    List<List<PointF>> listOfPages = new();
                    listOfPages.Add(pointsPage1);
                    listOfPages.Add(pointsPage2);

                    var doc = new PdfLoadedDocument(d01);
                    switch (doc.Pages.Count - listOfPages.Count)
                    {
                        case < 0:
                            while (doc.Pages.Count < listOfPages.Count)
                                doc.Pages.Add();
                            break;
                        case > 0:
                            while (doc.Pages.Count > listOfPages.Count)
                                listOfPages.Add(new List<PointF>());
                            break;
                    }

                    //ST.setOfToEvaluate[0].Add(new Bitmap(debugFolder + @"\01.png"));
                    //ST.setOfToEvaluate[0][0] = new Bitmap(debugFolder + @"\01.png");
                    //ST.document.EvaluateSet(); 

                    var value = doc.RecognizeTaggedBoxesDebug(d01, listOfPages);



                    var p = new Process();
                    p.StartInfo.UseShellExecute = true;

                }
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


        private void MyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Menu_Edit_AddBoxex_Click(sender, e);
        }

        //load
        private void Menu_Load_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog() { Filter = "PDF(*.pdf)|*.pdf", Title = "Open PDF" };
            if (open.ShowDialog() != true) return;
            var fileName = open.FileName;

            LoadDocument(fileName);
        }
        private void Menu_Load_New_Click(object sender, RoutedEventArgs e)
        {
            LoadDocument(null);
        }
        private void Menu_Load_Word_Click(object sender, RoutedEventArgs e)
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

        private void LoadDocument (string fileName)
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
                File.Copy(tempCopy, tempPdf);

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
            OpenFileDialog open =new() { Title = "Open Template", Filter = $"File Template(*{ST.ext})|*{ST.ext}" };
            if (open.ShowDialog() == false) return;

            LoadDataFromFile(open.FileName);

            Menu_Save_Project.IsEnabled = true;

        }
        //Edit
        private void Menu_Edit_AddBoxex_Click(object sender, RoutedEventArgs e)
        {

            PdfEditW m = new();
            m.Show();
        }
        private void Menu_Edit_NoTools_Click(object sender, RoutedEventArgs e)
        {
            PdfEditW m = new();
            m.Show();
            m.HideMenuTool();
            m.HideTools();
        }
        //Save
        private void Menu_Save_ProjectAs_Click(object sender, RoutedEventArgs e)
        {
            ST.versions.Add(DateTime.Now.ToStringOfRegularFormat());

            SaveFileDialog save = new() { Title = "Save Template", Filter = $"File Template(*{ST.ext})|*{ST.ext}", FileName=ST.projectName};
            if (save.ShowDialog() == false) return;

            SaveDataToFile(save.FileName);

            ST.projectFileName = save.FileName;
            ReloadWindowContent();

            Menu_Save_Project.IsEnabled = true;
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

            File.Copy(ST.tempFile,save.FileName);
            File.Copy(ST.tempFile, ST.tempFileCopy);
        }

        private void SaveDataToFile(string fileName)
        {
            byte[] FormatCode = ST.fileCode; //8 Byte identification code
            byte[] documentpdf = File.ReadAllBytes(ST.tempFile);
            byte[] listOfPointFsArray = ST.pagesPoints.PointListArrayToByteArray();
            byte[] listOfFieldsArray = ST.pagesFields.RectangleListArrayToByteArray();

            int listPLength = listOfPointFsArray.Length; //int 32
            int listFLength = listOfFieldsArray.Length;//int 32
            int docLength = documentpdf.Length; //int32


            using(MemoryStream ms = new())
            {
                using(BinaryWriter bw = new(ms))
                {
                    bw.Write(FormatCode);
                    bw.Write(listPLength);
                    bw.Write(listOfPointFsArray);
                    bw.Write(listFLength);
                    bw.Write(listOfFieldsArray);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    bw.Write(ms.ToArray().GetHashSHA1()); //closes file with hashcode to check if the file is the same and if we got to the end at the right time


                }
                File.WriteAllBytes(fileName,ms.ToArray());
            }



        }
        private void LoadDataFromFile(string filename)
        {
            //declare components
            byte[] FormatCode;
            byte[] documentpdf;
            byte[] listOfPointFsArray;
            byte[] listOfFieldsArray;
            byte[] hash;

            int listPLength;
            int listFLength;
            int docLength;

            //load components from file
            using (FileStream fs = new(filename, FileMode.Open))
            {
                using (BinaryReader br = new(fs))
                {
                    try
                    {
                        FormatCode = br.ReadBytes(8);
                        if (!FormatCode.SequenceEqual(ST.fileCode)) { MessageBox.Show("Open Template file wasn`t generated by this program"); return; }

                        listPLength = br.ReadInt32();
                        listOfPointFsArray = br.ReadBytes(listPLength);

                        listFLength = br.ReadInt32();
                        listOfFieldsArray = br.ReadBytes(listFLength);

                        docLength = br.ReadInt32();
                        documentpdf = br.ReadBytes(docLength);

                        hash = br.ReadBytes(20);
                    }
                    catch { MessageBox.Show("Not able to load thos file"); return; }
                }
            }
            //get hash of created files
            byte[] hashNew;
            using (MemoryStream ms = new()) 
            {
                using (BinaryWriter bw = new(ms))
                {
                    bw.Write(FormatCode);
                    bw.Write(listPLength);
                    bw.Write(listOfPointFsArray);
                    bw.Write(listFLength);
                    bw.Write(listOfFieldsArray);
                    bw.Write(docLength);
                    bw.Write(documentpdf);
                    hashNew = ms.ToArray().GetHashSHA1();
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
            string fn = Path.GetRandomFileName().Remove(8);
            string tempPdf = dir + "tmp" + fn + ".pdf";
            string tempCopy = dir + "tmp" + fn + "COPY" + ".pdf";


            File.WriteAllBytes(tempPdf, documentpdf); 
            File.WriteAllBytes(tempCopy, documentpdf);

            ST.tempFile = tempPdf;
            ST.tempFileCopy = tempCopy;
            ST.projectFileName = filename;
            ST.pagesPoints = listOfPointFsArray.ByteArrayToPointFListArray();

            ReloadWindowContent();
        }

        //Read
        private void Menu_Read_PNG_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "open PNG", Filter = "PNG(*.png)|*.png" };
            if (open.ShowDialog() != true) return;
            //open.FileName;

            ST.setOfToEvaluate = new();
            ST.setOfToEvaluate.Add(new());
            ST.setOfToEvaluate[0][0] = new Bitmap(open.FileName);
            
        }
        private void Menu_Read_ListOfScans_OnePage_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "Open list of scans", Filter = $"Pictures (all readable)|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF|All files (*.*)|*.*",Multiselect=true };//todo BMP, GIF, EXIF, JPG, PNG and TIFF (*.wav;*.mp3)|*.wav;*.mp3;*.txt" https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog.filter?view=windowsdesktop-6.0
            if (open.ShowDialog() == false) return;

            int l = ST.setOfToEvaluate.Count();//moves indexing if there are already bitmaps in the list
            for (int i = 0; i < open.FileNames.Length; i++)
            {
                ST.setOfToEvaluate.Add(new());
                ST.setOfToEvaluate[l + i].Add(new Bitmap(open.FileNames[i]));
            }




        }
        private void Menu_Read_ListOfScans_OnePdf_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog() { Title = "Open list of scans PDF", Filter = $"File Template(*.PDF)|*.PDF"};
            if (open.ShowDialog() == false) return;

            //TODO 03 get better dialog and finish loading of the file
            //and make text property to show its loaded
            var d = new dialogWindows.ChoiceDialogTemplate();
            d.ShowDialog();

            
        }
        //Print
        private void Menu_Print_ToJPEG_Click(object sender, RoutedEventArgs e)
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
        private void Menu_Print_ToPNG_Click(object sender, RoutedEventArgs e)
        {
            var save = new SaveFileDialog() { Title = "Save PNG", Filter = "PNG(*.png)|*.png" };
            if (save.ShowDialog() != true) return;
            PdfLoadedDocument doc = new(ST.tempFile);


            for (int i = 0; i < doc.Pages.Count; i++)
            {
                Bitmap image = doc.ExportAsImage(i, ST.dpiExport, ST.dpiExport);

                string fn = Path.GetFileNameWithoutExtension(save.FileName) + $"({i})" + Path.GetExtension(save.FileName);

                image.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
            }

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
        private void Window_Activated(object sender, EventArgs e)
        {
            ReloadWindowContent();
        }

        private void projecttext_LostFocus(object sender, RoutedEventArgs e)
        {
            ST.projectName = projecttext.Text;
        }

        private void reloadButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadWindowContent();
        }
        private void ReloadWindowContent()
        {
            if (ST.tempFile==null) throw new Exception("ReloadWindowContent whan no loaded document");
            var doc = new PdfLoadedDocument(ST.tempFile);

            Menu_Edit.IsEnabled = true;
            Menu_Save.IsEnabled = true;
            Menu_Print.IsEnabled = true;
            Menu_Read.IsEnabled = true;
            reloadButton.IsEnabled = true;
            this.Activated += Window_Activated;


            pdfDocumentView.Load(ST.tempFile);
            loadedPdfLabel.Content = Path.GetFileName(ST.fileName);
            pdfDocumentView.MinimumZoomPercentage = (int)Math.Ceiling(pdfDocumentView.MinimumZoomPercentage * 0.95);
            pdfDocumentView.ZoomTo(-1);

            Title = ST.appName + " -- " + ST.projectName;

            projecttext.Text = ST.projectName;
            projectfilenametext.Text = Path.GetFileName(ST.projectFileName);
            locationtext.Text = Path.GetDirectoryName(ST.projectFileName);
            pagecounttext.Text = doc.Pages.Count.ToString();
            int ii = 0;
            foreach (var pointFs in ST.pagesPoints)
                ii += pointFs.Count;
            boxcounttext.Text = ii.ToString();
            ii = 0;
            foreach (var rectangleFs in ST.pagesFields)
                ii += rectangleFs.Count;
            fieldcounttext.Text = ii.ToString();

            versionCombobox.Items.Clear();
            foreach (var item in ST.versions)
                versionCombobox.Items.Add(item);
            versionCombobox.SelectedIndex = versionCombobox.Items.Count - 1;
            //todo make odler versions and Ctrl+Z usable

            if (ST.versions.Count != 0)
                dateoflastsavetext.Text = ST.versions.Last();
            else dateoflastsavetext.Text = "not saved yet";

            //dateoflastsavetext.Text = ST.versions.Last().ToStringOfRegularFormat();

        }

    }
}

//TODO get the mic form connected (or maybe not) 	