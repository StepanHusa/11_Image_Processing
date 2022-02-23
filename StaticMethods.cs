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
using _11_StudentTester.Resources.Strings;
using ImageProcessor.Common.Extensions;

namespace _11_StudentTester
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
            var w = Settings.baundWidth / 2;
            rect.UnrelativateToPage(page);
            PointF[] vertexes = { new PointF(rect.Left - 2 * w, rect.Bottom + w), new PointF(rect.Right + w, rect.Bottom + w), new PointF(rect.Right + w, rect.Top - w), new PointF(rect.Left - w, rect.Top - w), new PointF(rect.Left - w, rect.Bottom + 2 * w) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes, types);
            path.Pen = Settings.baundPen;
            if (SecondColor)
                path.Pen = Settings.baundPenTwo;

            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
        public static PdfLoadedDocument DrawIndexNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, string index)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            var p = new PointF(rectangle.Right + Settings.baundWidth, rectangle.Top);
            page.Graphics.DrawString(index, new PdfStandardFont(Settings.stringFont, Settings.QS.indexFontSize, Settings.stringStyle), new PdfPen(Color.Black), p);

            return doc;
        }
        public static PdfLoadedDocument DrawNameNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * Settings.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(Settings.nameString, new PdfStandardFont(Settings.stringFont, Settings.QS.indexFontSize, Settings.stringStyle), new PdfPen(Color.Black), p, format);

            return doc;
        }
        public static PdfLoadedDocument DrawStringNextToRectangle(this PdfLoadedDocument doc, string Text, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelativateToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * Settings.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(Text, new PdfStandardFont(Settings.stringFont, Settings.QS.indexFontSize, Settings.stringStyle), new PdfPen(Color.Black), p, format);

            return doc;
        }

        public static List<SizeF> GetSizesOfPages(this PdfLoadedDocument doc)
        {
            List<SizeF> sizeFs = new();
            for (int i = 0; i < doc.Pages.Count; i++)
                sizeFs.Add(doc.Pages[i].Size);
            return sizeFs;
        }


        public static PdfLoadedDocument RemakeBoxex(this PdfLoadedDocument doc)
        {
            int i = 0;
            foreach (var question in Settings.boxesInQuestions)
            {
                int j = 0;
                foreach (var box in question)
                {
                    doc.DrawRectangleBounds(box.Item2, box.Item1,box.Item3);
                    doc.DrawIndexNextToRectangle(box.Item2, box.Item1, (i + 1).ToString() + j.IntToAlphabet());
                    j++;
                }
                i++;
            }
            return doc;
        }
        public static PdfLoadedDocument RemakeFields(this PdfLoadedDocument doc)
        {
            int j = 0;
            foreach (var field in Settings.pagesFields)
            {
                doc.DrawRectangleBounds(field.Item2, field.Item1);
                doc.DrawIndexNextToRectangle(field.Item2, field.Item1, Strings.text + (j + 1).ToString() + ":");
                j++;
            }

            return doc;
        }
        public static PdfLoadedDocument RemakeNameField(this PdfLoadedDocument doc)
        {
            if (Settings.nameField == null) return doc;
            doc.DrawRectangleBounds(Settings.nameField.Item2, Settings.nameField.Item1);
            doc.DrawNameNextToRectangle(Settings.nameField.Item2, Settings.nameField.Item1);
            return doc;
        }

        public static PdfLoadedDocument RemakeBoxexOneColor(this PdfLoadedDocument doc)
        {
            int i = 0;
            foreach (var question in Settings.boxesInQuestions)
            {
                int j = 0;
                foreach (var box in question)
                {
                    doc.DrawRectangleBounds(box.Item2, box.Item1);
                    doc.DrawIndexNextToRectangle(box.Item2, box.Item1, (i + 1).ToString() + j.IntToAlphabet());
                    j++;
                }
                i++;
            }
            return doc;
        }

        //TODODone add methods to remake fields and name field
        //TODO saparate element and text editing

    }


    static class MineEdgeProcess
    {
        public static Bitmap ProcessFilter(this Bitmap source, double[,] horizontalFilter)
        {
            int width = source.Width;
            int height = source.Height;
            int maxWidth = width + 1;
            int maxHeight = height + 1;
            int bufferedWidth = width + 2;
            int bufferedHeight = height + 2;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var input = new Bitmap(bufferedWidth, bufferedHeight, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            input.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(input))
            {
                // Fixes an issue with transparency not converting properly.
                graphics.Clear(Color.Transparent);

                var destinationRectangle = new Rectangle(0, 0, bufferedWidth, bufferedHeight);
                var rectangle = new Rectangle(0, 0, width, height);

                // If it's greyscale apply a colormatrix to the image.
                using (var attributes = new ImageAttributes())
                {
                    // We use a trick here to detect right to the edges of the image.
                    // flip/tile the image with a pixel in excess in each direction to duplicate pixels.
                    // Later on we draw pixels without that excess.
                    using (var tb = new TextureBrush(source, rectangle, attributes))
                    {
                        tb.WrapMode = WrapMode.TileFlipXY;
                        tb.TranslateTransform(1, 1);

                        graphics.FillRectangle(tb, destinationRectangle);
                    }
                }
            }

            try
            {

                int kernelLength = horizontalFilter.GetLength(0);
                int radius = kernelLength >> 1;

                using (var sourceBitmap = new FastBitmap(input))
                {
                    using (var destinationBitmap = new FastBitmap(destination))
                    {
                        // Loop through the pixels.
                        Parallel.For(
                            0,
                            bufferedHeight,
                            y =>
                            {
                                for (int x = 0; x < bufferedWidth; x++)
                                {
                                    double rX = 0;
                                    double gX = 0;
                                    double bX = 0;

                                    // Apply each matrix multiplier to the color components for each pixel.
                                    for (int fy = 0; fy < kernelLength; fy++)
                                    {
                                        int fyr = fy - radius;
                                        int offsetY = y + fyr;

                                        // Skip the current row
                                        if (offsetY < 0)
                                        {
                                            continue;
                                        }

                                        // Outwith the current bounds so break.
                                        if (offsetY >= bufferedHeight)
                                        {
                                            break;
                                        }

                                        for (int fx = 0; fx < kernelLength; fx++)
                                        {
                                            int fxr = fx - radius;
                                            int offsetX = x + fxr;

                                            // Skip the column
                                            if (offsetX < 0)
                                            {
                                                continue;
                                            }

                                            if (offsetX < bufferedWidth)
                                            {
                                                // ReSharper disable once AccessToDisposedClosure
                                                Color currentColor = sourceBitmap.GetPixel(offsetX, offsetY);
                                                double r = currentColor.R;
                                                double g = currentColor.G;
                                                double b = currentColor.B;

                                                rX += horizontalFilter[fy, fx] * r;

                                                gX += horizontalFilter[fy, fx] * g;

                                                bX += horizontalFilter[fy, fx] * b;
                                            }
                                        }
                                    }


                                    // Apply the equation and sanitize.
                                    byte red = rX.ToByte();
                                    byte green = gX.ToByte();
                                    byte blue = bX.ToByte();


                                    var newColor = Color.FromArgb(red, green, blue);
                                    if (y > 0 && x > 0 && y < maxHeight && x < maxWidth)
                                    {
                                        // ReSharper disable once AccessToDisposedClosure
                                        destinationBitmap.SetPixel(x - 1, y - 1, newColor);
                                    }
                                }
                            });
                    }
                }
            }
            finally
            {
                // We created a new image. Cleanup.
                input.Dispose();
            }

            return destination;
        }
        public static Bitmap ProcessFilterAbs(this Bitmap source, double[,] horizontalFilter)
        {
            int width = source.Width;
            int height = source.Height;
            int maxWidth = width + 1;
            int maxHeight = height + 1;
            int bufferedWidth = width + 2;
            int bufferedHeight = height + 2;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var input = new Bitmap(bufferedWidth, bufferedHeight, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            input.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(input))
            {
                // Fixes an issue with transparency not converting properly.
                graphics.Clear(Color.Transparent);

                var destinationRectangle = new Rectangle(0, 0, bufferedWidth, bufferedHeight);
                var rectangle = new Rectangle(0, 0, width, height);

                // If it's greyscale apply a colormatrix to the image.
                using (var attributes = new ImageAttributes())
                {
                    // We use a trick here to detect right to the edges of the image.
                    // flip/tile the image with a pixel in excess in each direction to duplicate pixels.
                    // Later on we draw pixels without that excess.
                    using (var tb = new TextureBrush(source, rectangle, attributes))
                    {
                        tb.WrapMode = WrapMode.TileFlipXY;
                        tb.TranslateTransform(1, 1);

                        graphics.FillRectangle(tb, destinationRectangle);
                    }
                }
            }

            try
            {

                int kernelLength = horizontalFilter.GetLength(0);
                int radius = kernelLength >> 1;

                using (var sourceBitmap = new FastBitmap(input))
                {
                    using (var destinationBitmap = new FastBitmap(destination))
                    {
                        // Loop through the pixels.
                        Parallel.For(
                            0,
                            bufferedHeight,
                            y =>
                            {
                                for (int x = 0; x < bufferedWidth; x++)
                                {
                                    double rX = 0;
                                    double gX = 0;
                                    double bX = 0;

                                    // Apply each matrix multiplier to the color components for each pixel.
                                    for (int fy = 0; fy < kernelLength; fy++)
                                    {
                                        int fyr = fy - radius;
                                        int offsetY = y + fyr;

                                        // Skip the current row
                                        if (offsetY < 0)
                                        {
                                            continue;
                                        }

                                        // Outwith the current bounds so break.
                                        if (offsetY >= bufferedHeight)
                                        {
                                            break;
                                        }

                                        for (int fx = 0; fx < kernelLength; fx++)
                                        {
                                            int fxr = fx - radius;
                                            int offsetX = x + fxr;

                                            // Skip the column
                                            if (offsetX < 0)
                                            {
                                                continue;
                                            }

                                            if (offsetX < bufferedWidth)
                                            {
                                                // ReSharper disable once AccessToDisposedClosure
                                                Color currentColor = sourceBitmap.GetPixel(offsetX, offsetY);
                                                double r = currentColor.R;
                                                double g = currentColor.G;
                                                double b = currentColor.B;

                                                rX += horizontalFilter[fy, fx] * r;

                                                gX += horizontalFilter[fy, fx] * g;

                                                bX += horizontalFilter[fy, fx] * b;
                                            }
                                        }
                                    }


                                    //Absolute value added by StepanHusa
                                    if (rX < 0) rX = -rX;
                                    if (gX < 0) gX = -gX;
                                    if (bX < 0) bX = -bX;

                                    // Apply the equation and sanitize.
                                    byte red = rX.ToByte();
                                    byte green = gX.ToByte();
                                    byte blue = bX.ToByte();


                                    var newColor = Color.FromArgb(red, green, blue);
                                    if (y > 0 && x > 0 && y < maxHeight && x < maxWidth)
                                    {
                                        // ReSharper disable once AccessToDisposedClosure
                                        destinationBitmap.SetPixel(x - 1, y - 1, newColor);
                                    }
                                }
                            });
                    }
                }
            }
            finally
            {
                // We created a new image. Cleanup.
                input.Dispose();
            }

            return destination;
        }
        public static Bitmap ProcessFilterAbsEdgeFix(this Bitmap source, double[,] horizontalFilter)
        {
            int width = source.Width;
            int height = source.Height;
            int maxWidth = width + 1;
            int maxHeight = height + 1;
            int bufferedWidth = width + 2;
            int bufferedHeight = height + 2;

            var destination = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
            var input = new Bitmap(bufferedWidth, bufferedHeight, PixelFormat.Format32bppPArgb);
            destination.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            input.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(input))
            {
                // Fixes an issue with transparency not converting properly.
                graphics.Clear(Color.Transparent);

                var destinationRectangle = new Rectangle(0, 0, bufferedWidth, bufferedHeight);
                var rectangle = new Rectangle(0, 0, width, height);

                // If it's greyscale apply a colormatrix to the image.
                using (var attributes = new ImageAttributes())
                {
                    // We use a trick here to detect right to the edges of the image.
                    // flip/tile the image with a pixel in excess in each direction to duplicate pixels.
                    // Later on we draw pixels without that excess.
                    using (var tb = new TextureBrush(source, rectangle, attributes))
                    {
                        tb.WrapMode = WrapMode.TileFlipXY;
                        tb.TranslateTransform(1, 1);

                        graphics.FillRectangle(tb, destinationRectangle);
                    }
                }
            }

            try
            {

                int kernelLength = horizontalFilter.GetLength(0);
                int radius = kernelLength >> 1;

                using (var sourceBitmap = new FastBitmap(input))
                {
                    using (var destinationBitmap = new FastBitmap(destination))
                    {
                        // Loop through the pixels.
                        Parallel.For(
                            0,
                            bufferedHeight,
                            y =>
                            {
                                for (int x = 0; x < bufferedWidth; x++)
                                {
                                    double rX = 0;
                                    double gX = 0;
                                    double bX = 0;

                                    // Apply each matrix multiplier to the color components for each pixel.
                                    for (int fy = 0; fy < kernelLength; fy++)
                                    {
                                        int fyr = fy - radius;
                                        int offsetY = y + fyr;

                                        // Skip the current row
                                        if (offsetY < 0)
                                        {
                                            offsetY = 0;
                                        }

                                        // Outwith the current bounds so break.
                                        if (offsetY > maxHeight)
                                        {
                                            offsetY = maxHeight;
                                        }

                                        for (int fx = 0; fx < kernelLength; fx++)
                                        {
                                            int fxr = fx - radius;
                                            int offsetX = x + fxr;

                                            // Skip the column
                                            if (offsetX < 0)
                                            {
                                                offsetX = 0;
                                            }
                                            if (offsetX > maxWidth)
                                            {
                                                offsetX = maxWidth;
                                            }
                                            // ReSharper disable once AccessToDisposedClosure
                                            Color currentColor = sourceBitmap.GetPixel(offsetX, offsetY);
                                            double r = currentColor.R;
                                            double g = currentColor.G;
                                            double b = currentColor.B;

                                            rX += horizontalFilter[fy, fx] * r;

                                            gX += horizontalFilter[fy, fx] * g;

                                            bX += horizontalFilter[fy, fx] * b;
                                        }
                                    }


                                    //Absolute value added by StepanHusa
                                    if (rX < 0) rX = -rX;
                                    if (gX < 0) gX = -gX;
                                    if (bX < 0) bX = -bX;

                                    // Apply the equation and sanitize.
                                    byte red = rX.ToByte();
                                    byte green = gX.ToByte();
                                    byte blue = bX.ToByte();


                                    var newColor = Color.FromArgb(red, green, blue);
                                    if (y > 0 && x > 0 && y < maxHeight && x < maxWidth)
                                    {
                                        // ReSharper disable once AccessToDisposedClosure
                                        destinationBitmap.SetPixel(x - 1, y - 1, newColor);
                                    }
                                }
                            });
                    }
                }
            }
            finally
            {
                // We created a new image. Cleanup.
                input.Dispose();
            }

            return destination;
        }

    }
    static class FinalCrossRecognition
    {
        public static float CenterEdgesNum11Im(this Bitmap J, float centralization = 2, float threshold = 1)
        {
            //correct size
            bool resize = false;
            int size = Math.Max(J.Width, J.Height);
            if (J.Width != J.Height) resize = true;
            if (size < 20) { size = 50; resize = true; }
            else if (size > 100) { size = 50; resize = true; }
            if (resize) J = J.Resize(size);

            var I = J.ProcessFilter(Settings.LaplacianOWNEdgeFilter);

            float c = 0;
            float cc = 0;
            for (int i = 0; i < I.Width; i++)
                for (int j = 0; j < I.Height; j++)
                {
                    float w = RelativeDistanceStar(i, j, I.Width, I.Height, (float)0.9).CalculateWeightBellShape(2, 5);
                    c += w;
                   cc += I.GetPixel(i, j).GetBrightness() * w;
                }

            float result = cc / c * MathF.Sqrt(I.Height * I.Width) / 5; //aroud 5 is a good treshold... meaning output is true if more than 1


            return result;
        }
        public static float CalculateWeightBellShape(this float distRel, float k, float l)
        {
            return MathF.Pow(1 - MathF.Pow(distRel, k * MathF.Log(l)), l);
        }
        public static float RelativeDistanceStar(int x, int y, int Width, int Height, float p)
        {
            float dx = (float)1 / 2 - (float)x / Width;
            float dy =(float) 1 / 2 - (float)y / Height;

            return MathF.Pow(MathF.Pow(MathF.Abs(dx + dy), p) + MathF.Pow(MathF.Abs(dx - dy), p), 1 / p);
        }
        public static float RelativeDistanceMaxNorm(int x, int y, int Width, int Height)
        {
            float dx = MathF.Abs(1 / 2 - x / Width);
            float dy = MathF.Abs(1 / 2 - y / Height);

            return MathF.Max(dx, dy);
        }

        public static Bitmap Resize(this Bitmap J, int size)
        {
            var I = new Bitmap(J, new Size(size, size));
            return I;
        }
    }




    static class ImageProcessing
    {
        public static List<List<List<bool>>> EvaluateWorks(this List<List<string>> works, List<List<Tuple<int, RectangleF, bool>>> questions, ProgressBar bar=null)
        {
            List<List<List<bool>>> resultsAll = new();

            foreach (var work in works)
            {
                resultsAll.Add(work.EvaluateOneWork(questions));
                if(bar!=null)
                    bar.Value = (double)works.IndexOf(work)/works.Count;
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
                    bool IsCross = crop.IsEdgyInTheCenterRecognize();

                    resultsQuestion.Add(IsCross);

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
        public static bool IsEdgyInTheCenterRecognize(this Bitmap crop)
        {
            return crop.CenterEdgesNum11Im()>1;
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


        public static float CalculateWeightLinear(int x, int y, int Width, int Height)
        {
            float distRel = MathF.Max(MathF.Abs(x - Width / 2), MathF.Abs(y - Height / 2)) / MathF.Max(Width / 2, Height / 2);
            return 1 - distRel ;
        }
        public static float CalculateWeightQuadratic(int x,int y,int Width,int Height,float power)
        {
            float distRel = MathF.Max(MathF.Abs(x - Width / 2), MathF.Abs(y - Height / 2)) / MathF.Max(Width / 2, Height / 2);
            return MathF.Pow(1 - distRel, power);
        }
        /// <summary>
        /// calculates weight for points of square
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="k">parameter for function of distance telling the central width (good is 2)</param>
        /// <param name="l">parameter of steepness (works above 2, good is 5)</param>
        /// <returns></returns>
        //public static float CalculateWeightBellShape(this float distRel, float k, float l)
        //{
        //    return MathF.Pow(1 - MathF.Pow(distRel, k * MathF.Log(l)), l);
        //}


        public static float RelativeDistanceEuclidus(int x, int y, int Width, int Height)
        {
            float dx = MathF.Abs(1 / 2 - x / Width);
            float dy = MathF.Abs(1 / 2 - y / Height);
            return MathF.Sqrt(MathF.Pow(dx, 2) + MathF.Pow(dy, 2));
        }
        public static float RelativeDistancePNorm(int x, int y, int Width, int Height, float p)
        {
            float dx = MathF.Abs(1 / 2 - x / Width);
            float dy = MathF.Abs(1 / 2 - y / Height);

            return MathF.Pow(MathF.Pow(dx, p) + MathF.Pow(dy, p),1/p);
        }
        public static float RelativeDistanceStar(int x, int y, int Width, int Height, float p)
        {
            float dx = 1 / 2 - x / Width;
            float dy = 1 / 2 - y / Height;

            return MathF.Pow(MathF.Pow(MathF.Abs(dx +dy), p) + MathF.Pow(MathF.Abs(dx -dy), p), 1 / p);
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
                foreach (var question in Settings.boxesInQuestions)
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





