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
//using PdfSharp.Drawing;
using System.Diagnostics;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Interactive;

namespace _11_Image_Processing
{

    /// <summary>
    /// Interaction logic for PdfEditW.xaml
    /// </summary>
    public partial class PdfEditW : Window
    {

        double offset;
        private bool drawingRectangle;
        private PageMouseMoveEventArgs argsFirstVertex;
        private System.Windows.Point locFirstVertex;

        public PdfEditW()
        {
            InitializeComponent();


            if (ST.projectFileName != null)
                this.Title = ST.projectFileName;
            else this.Title = "*Untitled";
            //Debug.WriteLine(new FileInfo(fileName).Length);

            pdfViewControl.Load(ST.document);
            pdfViewControl.MaximumZoomPercentage = 6400;

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



            //pdfViewControl.MouseDoubleClick += Pdfwcontrol_MouseDoubleClick;
            pdfViewControl.MouseRightButtonUp += Pdfwcontrol_MouseRightButtonUp;
            pdfViewControl.ScrollChanged += Pdfwcontrol_ScrollChanged;

            //pdfViewControl.LoadedDocument.AddSquareAt(0, new System.Drawing.Point(10, 20), 10);
        }



        //TODO finish method and solve errors, debug changes

        private void Pdfwcontrol_ScrollChanged(object sender, ScrollChangedEventArgs args)
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

            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_B;
            else
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_B;

        }
        private void Menu_NewPage_Click(object sender, RoutedEventArgs e)
        {
            double zoom = pdfViewControl.ZoomPercentage / 100.0;
            var doc = pdfViewControl.LoadedDocument;

            doc.Pages.Add();

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            doc.Dispose();
            pdfViewControl.Load(stream);
            ST.document = pdfViewControl.LoadedDocument;
            ST.document.Save(ST.tempFile);

            //pdfViewControl.Save(tempPdf);

            pdfViewControl.ScrollTo(offset);
            pdfViewControl.Zoom = zoom * 100;
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
            {
                pdfViewControl.PageMouseMove += PdfViewControl_PageMouseMove_C;
                rectangleR.Visibility = Visibility.Visible;
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_A;
            }
            else
            {
                pdfViewControl.PageMouseMove -= PdfViewControl_PageMouseMove_C;
                rectangleR.Visibility = Visibility.Hidden;
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_A;
            }

        }
        private void D_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_D;
            else
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_D;

        }
        private void E_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as MenuItem).IsChecked)
                pdfViewControl.PageClicked += Pdfwcontrol_PageClicked_E;
            else
                pdfViewControl.PageClicked -= Pdfwcontrol_PageClicked_E;

        }


        private void Pdfwcontrol_PageClicked_A(object sender, PageClickedEventArgs args)
        {

            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage / 100.0;



            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));
            SizeF size = ST.sizeOfBox;

            RectangleF bounds = new RectangleF(point, size);
            doc.DrawRectangleBounds(bounds, pindex);


            ReloadDocument();

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

                    doc.DrawRectangleBounds(rect, pindex);


                    while (ST.pagesFields.Length <= pindex)
                        Array.Resize(ref ST.pagesFields, pindex + 1);
                    if (ST.pagesFields[pindex] == null)
                        ST.pagesFields[pindex] = new();
                    ST.pagesFields[pindex].Add(rect);

                    ReloadDocument();
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





            ////create space to list of questoins
            //while (ST.pagesQuestionsBoxes.Length <= pindex)
            //    Array.Resize(ref ST.pagesQuestionsBoxes, pindex + 1);
            //if (ST.pagesQuestionsBoxes[pindex] == null)
            //    ST.pagesQuestionsBoxes[pindex] = new();
            ////int iQ = ST.pagesQuestionsBoxes[pindex].Count; //index of question on page
            //ST.pagesQuestionsBoxes[pindex].Add(new PointF[n]);

 
            //get the index of new question
            int iQ = ST.boxesInQuestions.Count;
            //add list of answers in this question
            ST.boxesInQuestions.Add(new());


            var tb = new PdfTextBoxField(ST.document.Pages[args.PageIndex], "question");
            tb.Text = $"Question";
            tb.Bounds = new RectangleF(point.X, point.Y, ST.QS.widthOfQTBs, ST.QS.heightOfTB);
            doc.Form.Fields.Add(tb);



            for (int i = 0; i < ST.QS.n; i++)
            {
                //add answer i textbox field
                PdfTextBoxField textBoxField = new PdfTextBoxField(ST.document.Pages[args.PageIndex], "Enter your text");
                textBoxField.Text = $"Answer {i + 1}";
                textBoxField.Bounds = new RectangleF(point.X + ST.QS.tab, point.Y + ST.QS.heightOfTB + ST.QS.spaceUnderQ + i * (ST.QS.heightOfTB + ST.QS.spaceBtwAn), ST.QS.widthOfQTBs - ST.QS.tab, ST.QS.heightOfTB);
                doc.Form.Fields.Add(textBoxField);


                var pointb = new PointF(point.X + ST.QS.widthOfQTBs + ST.QS.spaceBeforeBox, point.Y + ST.QS.heightOfTB + ST.QS.spaceUnderQ + i * (ST.QS.heightOfTB + ST.QS.spaceBtwAn));
                SizeF size = ST.sizeOfBox;


                //square
                RectangleF bounds = new RectangleF(pointb, size);
                doc.DrawRectangleBounds(bounds, pindex);

                doc.DrawIndexNextToRectangle(bounds, pindex, /*pindex.ToString() +*/ (iQ+1).ToString() + Convert.ToChar(i + (int)'a'));

                //add square to 'The List'
                ST.boxesInQuestions[iQ].Add(pindex, bounds);

                ////add question to list of questoins
                //ST.pagesQuestionsBoxes[pindex][iQ][i] = pointb; 

                ////add to list of single boxes
                //while (ST.pagesPoints.Length <= pindex)
                //    Array.Resize(ref ST.pagesPoints, pindex + 1);
                //if (ST.pagesPoints[pindex] == null)
                //    ST.pagesPoints[pindex] = new();
                //ST.pagesPoints[pindex].Add(pointb);

            }

            ReloadDocument();
        }
        private void Pdfwcontrol_PageClicked_E(object sender, PageClickedEventArgs args)
        {

        }

        private void PdfViewControl_PageMouseMove(object sender, PageMouseMoveEventArgs args)
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
        private void PdfViewControl_PageMouseMove_C(object sender, PageMouseMoveEventArgs args)
        {
            Rectangle rect = new();


            rect.Location = new((int)(Mouse.GetPosition(this).X + 2 * rectangleR.StrokeThickness), (int)(Mouse.GetPosition(this).Y + 2 * rectangleR.StrokeThickness));
            rect.Size = ST.sizeOfBox.ToSize();

            //rect = rect.EvaluateInPositiveSize();

            Thickness thickness = new(rect.X, rect.Y, 0, 0);
            rectangleR.Margin = thickness;

            rectangleR.Width = rect.Width;
            rectangleR.Height = rect.Height;
        }

        private void ReloadDocument()
        {
            double zoom = pdfViewControl.ZoomPercentage / 100.0;
            var doc = pdfViewControl.LoadedDocument;

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            doc.Dispose();
            pdfViewControl.Load(stream);
            ST.document =(PdfLoadedDocument) pdfViewControl.LoadedDocument.Clone();
            ST.document.Save(ST.tempFile);


            pdfViewControl.ScrollTo(offset);
            pdfViewControl.Zoom = zoom * 100;

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

    }
}
