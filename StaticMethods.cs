using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
//using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ImageProcessor;
using ImageProcessor.Imaging;
using System.Drawing.Imaging;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.InteropServices;
//using System.Windows.Media.Imaging;
using _11_Image_Processing.Resources.Strings;


namespace _11_Image_Processing
{
    public static class Pdf
    {

        //public static PdfLoadedDocument AddRectangleAt(this PdfLoadedDocument doc, int pageint, RectangleF rect)
        //{
        //    var page = doc.Pages[pageint];

        //    //PdfSolidBrush brush =new(Color.White);
        //    //RectangleF bounds = rect;


        //    //page.Graphics.DrawRectangle(brush, bounds);

        //    //doc.DrawRectangleBounds(rect, pageint);

        //    return doc;

        //}



        public static void NewPdfDoc(string path)
        {
            PdfDocument doc = new();
            doc.Pages.Add();
            doc.Save(path);
        }

        public static List<bool[]> RecognizeTaggedBoxesDebug(this PdfLoadedDocument doc, string fileName, List<List<PointF>> list, int sizeInt = 20, float w = 2)
        {
            List<bool[]> output = new();
            Bitmap[] pageImages = doc.ExportAsImage(0, doc.Pages.Count - 1, 300, 300);
            SizeF size = new(sizeInt, sizeInt);
            double treshold = 0.7;

            if (list.Count > doc.Pages.Count) throw new Exception("not a correct sizes of lists (RecognizeTaggedBoxesDebug)");


            int pgCount = -1;
            foreach (var points in list)
            {
                pgCount++;
                bool[] pageArray = new bool[points.Count];

                var s = doc.Pages[pgCount].Size; //595 842 size
                var sI = pageImages[pgCount].Size; //2479 3508 size image
                double r = sI.Height / s.Height; //4.16627 racio

                int pointCount = 0;
                foreach (var point in points)
                {
                    RectangleF b = new RectangleF(point, size); //bounds
                    Rectangle I = new(); //Int Rectangle


                    b.Size -= new SizeF(w * 2, w * 2);
                    b.Location += new SizeF(w, w);
                    b.Size *= (float)r;
                    b.Location = b.Location.ScalePoint((float)r);


                    I = Rectangle.Round(b);



                    int c = I.Width * I.Height;//count of pixels
                    float cc = 0;
                    for (int i = I.X; i < I.X + I.Width; i++)
                        for (int j = I.Y; j < I.Y + I.Height; j++)
                        {
                            pageImages[0].SetPixel(i, j, Color.Blue);
                            //var pix=pageImages[pgCount].GetPixel(i,j);
                            //var x = pix.GetBrightness();
                            cc += pageImages[pgCount].GetPixel(i, j).GetBrightness();
                        }
                    float av = cc / c;

                    if (av < treshold) pageArray[pointCount] = true;
                    pointCount++;
                }


                output.Add(pageArray);
            }

            pageImages[0].Save(Path.ChangeExtension(fileName, "jpg"));

            return output;

        }
        public static List<bool[]> RecognizeTaggedBoxes(this PdfLoadedDocument doc, List<PointF>[] lists, int sizeInt = 20, float w = 2, string fileName = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\02.pdf")
        {
            List<bool[]> output = new();
            Bitmap[] pageImages = doc.ExportAsImage(0, doc.Pages.Count - 1, 300, 300);
            SizeF size = new(sizeInt, sizeInt);
            double treshold = 0.7;

            if (lists.Length > doc.Pages.Count) throw new Exception("not a correct sizes of lists (RecognizeTaggedBoxes)");


            int pgCount = -1;
            foreach (var points in lists)
            {
                pgCount++;
                bool[] pageArray = new bool[points.Count];

                var s = doc.Pages[pgCount].Size; //595 842 size
                var sI = pageImages[pgCount].Size; //2479 3508 size image
                double r = sI.Height / s.Height; //4.16627 racio

                int pointCount = 0;
                foreach (var point in points)
                {
                    RectangleF b = new RectangleF(point, size); //bounds
                    Rectangle I = new(); //Int Rectangle


                    b.Size -= new SizeF(w * 2, w * 2);
                    b.Location += new SizeF(w, w);
                    b.Size *= (float)r;
                    b.Location = b.Location.ScalePoint((float)r);


                    I = Rectangle.Round(b);



                    int c = I.Width * I.Height;//count of pixels
                    float cc = 0;
                    for (int i = I.X; i < I.X + I.Width; i++)
                        for (int j = I.Y; j < I.Y + I.Height; j++)
                        {
                            pageImages[0].SetPixel(i, j, Color.Blue);
                            //var pix=pageImages[pgCount].GetPixel(i,j);
                            //var x = pix.GetBrightness();
                            cc += pageImages[pgCount].GetPixel(i, j).GetBrightness();
                        }
                    float av = cc / c;

                    if (av < treshold) pageArray[pointCount] = true;
                    pointCount++;
                }


                output.Add(pageArray);
            }

            pageImages[0].Save(Path.ChangeExtension(fileName, "jpg"));

            return output;

        }


        public static PdfLoadedDocument DrawRectangleBounds(this PdfLoadedDocument doc, RectangleF rect, int pageint, bool SecondColor = false)
        {
            var page = doc.Pages[pageint];
            var w = ST.baundWidth / 2;
            rect.UnrelativateToPage(page);
            PointF[] vertexes = { new PointF(rect.Left - 2 * w, rect.Bottom + w), new PointF(rect.Right + w, rect.Bottom + w), new PointF(rect.Right + w, rect.Top - w), new PointF(rect.Left - w, rect.Top - w), new PointF(rect.Left - w, rect.Bottom + 2 * w) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes, types);
            path.Pen = ST.baundPen;
            if (SecondColor)
                path.Pen = ST.baundPenTwo;

            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
        public static PdfLoadedDocument DrawIndexNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, string index)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            var p = new PointF(rectangle.Right + ST.baundWidth, rectangle.Top);
            page.Graphics.DrawString(index, new PdfStandardFont(PdfFontFamily.Courier, ST.QS.indexFontSize), new PdfPen(Color.Black), p);

            return doc;
        }
        public static PdfLoadedDocument DrawNameNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * ST.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(ST.nameString, new PdfStandardFont(PdfFontFamily.Courier, ST.QS.indexFontSize), new PdfPen(Color.Black), p, format);

            return doc;
        }
        public static PdfLoadedDocument DrawStringNextToRectangle(this PdfLoadedDocument doc, string Text, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * ST.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(Text, new PdfStandardFont(PdfFontFamily.Courier, ST.QS.indexFontSize), new PdfPen(Color.Black), p, format);

            return doc;
        }

        public static List<SizeF> GetSizesOfPages(this PdfLoadedDocument doc)
        {
            List<SizeF> sizeFs = new();
            for (int i = 0; i < doc.Pages.Count; i++)
                sizeFs.Add(doc.Pages[i].Size);
            return sizeFs;
        }

    }







    static class ImageProcessing
    {
        public static List<List<List<bool>>> EvaluateWorks(this List<List<string>> works, List<List<Tuple<int, RectangleF, bool>>> questions)
        {
            List<List<List<bool>>> resultsAll = new();

            foreach (var work in works)
            {
                resultsAll.Add(work.EvaluateOneWork(questions));
            }


            return resultsAll;
        }
        public static List<List<bool>> EvaluateOneWork(this List<string> work, List<List<Tuple<int, RectangleF, bool>>> questions)
        {
            List<List<bool>> resultsOneWork = new();

            foreach (var question in questions)
            {
                List<bool> resultsQuestion = new();
                foreach (var box in question)
                {
                    int pageindex = box.Item1;
                    var workBitmap = new Bitmap(work[pageindex]);
                    Bitmap crop = workBitmap.Corp(box.Item2);
                    bool IsDark = crop.IsDarkRocognize();
                    bool IsEdgy = crop.IsEdgyRecognize();
                    bool IsCross = crop.IsEdgyInTheCenterRecognize();
                    resultsQuestion.Add(IsDark);

                    workBitmap.Dispose(); //otherwise the memory explodes
                }
                resultsOneWork.Add(resultsQuestion);
            }
            return resultsOneWork;
        }

        public static bool IsDarkRocognize(this Bitmap crop)
        {
            //debug feature
            //string f = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
            //int i = Directory.GetFiles(f).Length;
            //f = f + "\\" + i + ".Bmp";
            //crop.Save(f, ImageFormat.Bmp);

            if (BrightnessLevelOfBitmap(crop) < 0.5) return true;
            else return false;
        }
        public static bool IsEdgyRecognize(this Bitmap crop)
        {
            return false;
        }
        public static bool IsEdgyInTheCenterRecognize(this Bitmap crop)
        {
            return crop.CenterEdgesNum()>1;
        }


        public static float BrightnessLevelOfBitmap(Bitmap Image)
        {
            int c = Image.Height * Image.Width;
            float cc = 0;
            for (int i = 0; i < Image.Width; i++)
                for (int j = 0; j < Image.Height; j++)
                {
                    cc += Image.GetPixel(i, j).GetBrightness();
                }
            return cc / c;
        }
        public static float EdgesNum(this Bitmap J, float threshold = 1)
        {
            var I = J.LaplasTransform1();

            float c = I.Height*I.Width;
            float cc = 0;
            for (int i = 0; i < I.Width; i++)
                for (int j = 0; j < I.Height; j++)
                {
                    cc += I.GetPixel(i, j).GetBrightness();
                }
            float result = cc / c / threshold; //aroud 1 is a good treshold... meaning output is true if more than
                                                                                //TODO measure the good th
            return result;
        }
        public static float CenterEdgesNum(this Bitmap J, float centralization = 2,float threshold=1)
        {
            var I = J.LaplasTransform1();

            float c = 0;
            float cc = 0;
            for (int i = 0; i < I.Width; i++)
                for (int j = 0; j < I.Height; j++)
                {
                    float w = MathF.Pow(1 - MathF.Max(MathF.Abs(i - I.Width / 2), MathF.Abs(j - I.Height / 2)) / MathF.Max(I.Width / 2, I.Height / 2), centralization);

                    c += w;
                    cc += I.GetPixel(i, j).GetBrightness() * w;
                }
            float result = cc / c * MathF.Sqrt(I.Height * I.Width) / threshold; //aroud 1 is a good treshold... meaning output is true if more than
                                                                                //TODO measure the good th
            return result;
        }

        public static List<Bitmap> GetCropedNames(this List<List<string>> works, Tuple<int, RectangleF> nameField)
        {
            if (nameField == null) { System.Windows.MessageBox.Show(Strings.WarningnoNameFieldadded); return null; }

            List<Bitmap> l = new();
            foreach (var work in works)
            {
                l.Add(new Bitmap(work[nameField.Item1]).Corp(nameField.Item2));
            }
            return l;
        }
    }

    static class ListExtensions
    {
        public static void Add(this List<Tuple<int, RectangleF, bool>> l, int i, RectangleF p, bool b)
        {
            l.Add(new Tuple<int, RectangleF, bool>(i, p, b));
        }
    }
    static class Bitmaps
    {
        public static List<List<Bitmap>> DrowCorrect(this List<List<Bitmap>> works)
        {
            for (int i = 0; i < works.Count; i++)
            {
                foreach (var question in ST.boxesInQuestions)
                {
                    foreach (var box in question)
                    {
                        works[i][box.Item1] = works[i][box.Item1].DrowRectangle(box.Item2);
                    }
                }
            }
            return works;
        }

        public static Bitmap DrowRectangle(this Bitmap b, RectangleF rect) //primarly debug feature
        {
            //float racioX = b.Width / sizeOfPage.Width;
            //float racioY = b.Height / sizeOfPage.Height;
            Rectangle cropRect = new((int)(rect.X * b.Width), (int)(rect.Y * b.Height), (int)(rect.Width * b.Width), (int)(rect.Height * b.Height));

            for (int i = 0; i <= cropRect.Width; i++)
            {
                b.SetPixel(cropRect.X + i, cropRect.Y, Color.Yellow);
                b.SetPixel(cropRect.X + i, cropRect.Y + cropRect.Height, Color.Yellow);

            }

            for (int j = 0; j <= cropRect.Height; j++)
            {
                b.SetPixel(cropRect.X, cropRect.Y + j, Color.Yellow);
                b.SetPixel(cropRect.X + cropRect.Width, cropRect.Y + j, Color.Yellow);

            }

            //for (int i = 0; i < b.Height; i++)
            //{
            //    for (int j = 0; j < b.Width; j++)
            //    {
            //        b.SetPixel(i, j, Color.Black);
            //    }
            //}

            return b;
        }
    }
    static class Rectangles
    {
        public static RectangleF UnrelativateToPage(this ref RectangleF rect, PdfPageBase page)
        {
            rect.X *= page.Size.Width;
            rect.Y *= page.Size.Height;
            rect.Width *= page.Size.Width;
            rect.Height *= page.Size.Height;
            return rect;
        }
        public static void RelativateToPage(this ref RectangleF rect, PdfPageBase page)
        {
            rect.X /= page.Size.Width;
            rect.Y /= page.Size.Height;
            rect.Width /= page.Size.Width;
            rect.Height /= page.Size.Height;


        }

        public static Rectangle UnrelativateToImage(this RectangleF relativeRect, Bitmap image)
        {
            Rectangle cropRect = new((int)Math.Ceiling(relativeRect.X * image.Width), (int)Math.Ceiling(relativeRect.Y * image.Height), (int)Math.Floor(relativeRect.Width * image.Width), (int)Math.Floor(relativeRect.Height * image.Height));

            return cropRect;

        }
    }

}





