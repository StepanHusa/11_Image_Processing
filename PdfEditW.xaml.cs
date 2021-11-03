using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Shapes;
using System.IO;
//using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for PdfEditW.xaml
    /// </summary>
    public partial class PdfEditW : Window
    {
        string tempPdf;
        double offset;

        public PdfEditW()
        {
            InitializeComponent();


            if (ST.fileName != null)
                this.Title = ST.fileName;
            else this.Title = "*Untitled";
            tempPdf = ST.tempFile;
            //Debug.WriteLine(new FileInfo(fileName).Length);

            pdfViewControl.Load(ST.document);

                

            
            {//PdfDocument doc = PdfSharp.Pdf.IO.PdfReader.Open(fileName);
             //PdfPage page = doc.AddPage();
             //XGraphics gfx = XGraphics.FromPdfPage(page);
             //XFont font = new("Arial", 20);
             //gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
             //doc.Save(fileName);

                //this.DataContext = new ViewModel(tempPdf);
                //PDFViewer.Visibility = Visibility.Visible;

                //Debug.WriteLine(new FileInfo(fileName).Length);
                //if (new FileInfo(fileName).Length < Math.Pow(10, 6))
                //{
                //    PdfLoadedDocument pdf = new PdfLoadedDocument(tempPdf);

                //    pdfViewControl.Load(pdf);
                //}
                //else
            }



            pdfViewControl.MouseDoubleClick += Pdfwcontrol_MouseDoubleClick;
            pdfViewControl.MouseRightButtonUp += Pdfwcontrol_MouseRightButtonUp;
            pdfViewControl.ScrollChanged += Pdfwcontrol_ScrollChanged1;

            //pdfViewControl.LoadedDocument.AddSquareAt(0, new System.Drawing.Point(10, 20), 10);
        }

        private void Pdfwcontrol_ScrollChanged1(object sender, ScrollChangedEventArgs args)
        {
            offset = args.VerticalOffset;
        }

        private void Pdfwcontrol_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {



            //a.Icon = Properties.Resources.checked_checkbox;
            ContextMenu cm = (ContextMenu)Resources["contextMenu"];
            cm.IsOpen = true;
            //cm=(ContextMenu)Resources["contextMenu"];
            


            //show
        }


        private void A_Click(object sender, RoutedEventArgs e)
        {
            if((sender as MenuItem).IsChecked) 
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;
            else
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;

        }
        private void B_Click(object sender, RoutedEventArgs e)
        {
            //var doc = pdfViewControl.LoadedDocument.AddSquareAt(0, new System.Drawing.Point(20, 20), 10);
            //doc.Save(tempPdf);
            //Debug.WriteLine(tempPdf);
            //pdfViewControl.Load(tempPdf);
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_C;
        }


        private void Pdfwcontrol_PageClicked_A(object sender, PageClickedEventArgs args)
        {

            var sen = sender as PdfViewerControl;
            var doc = ST.document;
            int pindex = args.PageIndex;
            double zoom = sen.ZoomPercentage / 100.0;


            int size = 20;
            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));
            doc.AddSquareAt(pindex, point, size);


            while (ST.pagesList.Length <= pindex)
                Array.Resize(ref ST.pagesList, pindex+1);
            if (ST.pagesList[pindex] == null)
                ST.pagesList[pindex] = new();
            ST.pagesList[pindex].Add(point);

            //doc.Save(tempPdf);
            //pdfViewControl.Load(ST.document);



            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            doc.Dispose();
            pdfViewControl.Load(stream);
            ST.document = pdfViewControl.LoadedDocument;

            //pdfViewControl.Save(tempPdf);

            pdfViewControl.ScrollTo(offset);
            pdfViewControl.Zoom = zoom * 100;


        }
        private void Pdfwcontrol_PageClicked_C(object sender, PageClickedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private double Pdfwcontrol_ScrollChanged(object sender, ScrollChangedEventArgs args)
        {
            var sen = sender as PdfViewerControl;
            return args.VerticalOffset;
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


        private void pdfwcontrol_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var sen = (PdfViewerControl)sender;
            var t=e.GetPosition(sen);
            HideTools();
            HideMenuTool();
        }


        //private void DeleteTemp()
        //{
        //    if(tempPdf!=null) { File.Delete(tempPdf);tempPdf = null; }
        //}7
        private void HideTools()
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
        private void HideMenuTool()
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

        private void b_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
