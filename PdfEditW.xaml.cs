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

            //pdfViewControl.Load(ST.document); 
            pdfViewControl.Load(ST.tempFile);//this change prevented errors after closing this
            pdfViewControl.MaximumZoomPercentage = 6400;

            pdfViewControl.MouseRightButtonUp += Pdfwcontrol_MouseRightButtonUp;
            pdfViewControl.ScrollChanged += Pdfwcontrol_ScrollChanged;
        }

        private void Pdfwcontrol_ScrollChanged(object sender, ScrollChangedEventArgs args)
        {
            offset = args.VerticalOffset;
        }
        private void Pdfwcontrol_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ContextMenu cm = (ContextMenu)Resources["contextMenu"];
            cm.IsOpen = true;
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

            ReloadDocument();

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
 
            //get the index of new question
            int iQ = ST.boxesInQuestions.Count;
            //add list of answers in this question
            ST.boxesInQuestions.Add(new());


            var tb = new PdfTextBoxField(doc.Pages[args.PageIndex], "question");
            tb.Text = $"Question";
            tb.Bounds = new RectangleF(point.X, point.Y, ST.QS.widthOfQTBs, ST.QS.heightOfTB);
            doc.Form.Fields.Add(tb);



            for (int i = 0; i < ST.QS.n; i++)
            {
                //add answer i textbox field
                PdfTextBoxField textBoxField = new PdfTextBoxField(doc.Pages[args.PageIndex], "Enter your text");
                textBoxField.Text = $"Answer {i + 1}";
                textBoxField.Bounds = new RectangleF(point.X + ST.QS.tab, point.Y + ST.QS.heightOfTB + ST.QS.spaceUnderQ + i * (ST.QS.heightOfTB + ST.QS.spaceBtwAn), ST.QS.widthOfQTBs - ST.QS.tab, ST.QS.heightOfTB);
                doc.Form.Fields.Add(textBoxField);


                var pointb = new PointF(point.X + ST.QS.widthOfQTBs + ST.QS.spaceBeforeBox, point.Y + ST.QS.heightOfTB + ST.QS.spaceUnderQ + i * (ST.QS.heightOfTB + ST.QS.spaceBtwAn));
                SizeF size = ST.sizeOfBox;


                //square
                RectangleF bounds = new RectangleF(pointb, size);
                doc.DrawRectangleBounds(bounds, pindex);

                doc.DrawIndexNextToRectangle(bounds, pindex, /*pindex.ToString() +*/ (iQ + 1).ToString() + Convert.ToChar(i + (int)'a'));

                //add square to 'The List'
                ST.boxesInQuestions[iQ].Add(pindex, bounds);
            }

            ReloadDocument();
        }
        private void Pdfwcontrol_PageClicked_E(object sender, PageClickedEventArgs args)
        {
            var doc = pdfViewControl.LoadedDocument;
            int pindex = args.PageIndex;
            double zoom = pdfViewControl.ZoomPercentage / 100.0;

            PointF point = new((float)(args.Position.X * 0.75 / zoom), (float)(args.Position.Y * 0.75 / zoom));

            //get the index of new question
            int iQ = ST.boxesInQuestions.Count;
            //add list of answers in this question
            ST.boxesInQuestions.Add(new());


            for (int i = 0; i < ST.QS.n; i++)
            {
                var pointb = new PointF(point.X, point.Y+ i * (ST.sizeOfBox.Height + ST.spaceBetweenBoxes));
                SizeF size = ST.sizeOfBox;


                //square
                RectangleF bounds = new RectangleF(pointb, size);
                doc.DrawRectangleBounds(bounds, pindex);

                doc.DrawIndexNextToRectangle(bounds, pindex, /*pindex.ToString() +*/ (iQ + 1).ToString() + Convert.ToChar(i + (int)'a'));

                //add square to 'The List'
                ST.boxesInQuestions[iQ].Add(pindex, bounds);
            }

            ReloadDocument();

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

        private void UndoBox()
        {
            int i = ST.boxesInQuestions.Count;
            if (i == 0) return;
            int j=ST.boxesInQuestions[i-1].Count;
            if (j == 0) { ST.boxesInQuestions.RemoveAt(i-1);UndoBox(); }
            else
            {
                ST.boxesInQuestions[i-1].RemoveAt(j-1);
            }
            RemakeBoxex();
        }

        private void RemakeBoxex()
        {
            ST.document = new PdfLoadedDocument(ST.tempFileCopy);
            int i = 0;
            foreach (var question in ST.boxesInQuestions) { 
                int j = 0;
                foreach (var box in question)
                {
                    ST.document.DrawRectangleBounds(box.Item2, box.Item1);
                    ST.document.DrawIndexNextToRectangle(box.Item2, box.Item1, (i + 1).ToString() + Convert.ToChar(j + (int)'a'));
                }}
            pdfViewControl.Load(ST.document);
            ReloadDocument();

        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sizeSlider != null)
            {
                float s = (float)Math.Round(sizeSlider.Value, 3);
                ST.sizeOfBox = new SizeF(s, s);
                ST.indexFontSize = s/2;

            }
            if (widthSlider != null)
            {
                float f = (float)Math.Round(widthSlider.Value, 3);
                ST.baundWidth = f;
            }

            if (spaceSlider != null)
            {
                float p = (float)Math.Round(spaceSlider.Value, 3);
                ST.spaceBetweenBoxes = p;
            }

            if (countSlider != null)
            {
                ST.QS.n = (int)countSlider.Value;
            }
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

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            UndoBox();
        }
    }
}
