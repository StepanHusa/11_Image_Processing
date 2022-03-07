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
using ImageProcessor.Common.Extensions;
using System.Diagnostics;

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
            var w = Settings.baundWidth / 2;
            rect.UnrelatitivizeToPage(page);
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
            rectangle.UnrelatitivizeToPage(page);
            var p = new PointF(rectangle.Right + Settings.baundWidth, rectangle.Top);
            page.Graphics.DrawString(index, new PdfStandardFont(Settings.stringFont, Settings.QS.indexFontSize, Settings.stringStyle), new PdfPen(Color.Black), p);

            return doc;
        }
        public static PdfLoadedDocument DrawNameNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelatitivizeToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * Settings.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(Settings.nameString, new PdfStandardFont(Settings.stringFont, Settings.QS.indexFontSize, Settings.stringStyle), new PdfPen(Color.Black), p, format);

            return doc;
        }
        public static PdfLoadedDocument DrawStringNextToRectangle(this PdfLoadedDocument doc, string Text, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelatitivizeToPage(page);
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

        public static List<RectangleF> GetPositionerRectangles(this PdfLoadedDocument doc)
        {
            List<RectangleF> posis = new();

            foreach (PdfPageBase page in doc.Pages)
            {
                float marg = Settings.positionersMargin * page.Size.Width;
                RectangleF marginRect = new RectangleF(marg, marg, page.Size.Width - 2 * marg, page.Size.Height - 2 * marg);
                posis.Add(marginRect.RelatitivizeToPage(page));
            }
            return posis;
        }

        public static PdfLoadedDocument AddPositioners(this PdfLoadedDocument doc)
        {
            foreach (PdfPageBase page in doc.Pages)
            {
                var w = Settings.positionersWidth / 2;
                float ll = Settings.positionersLegLength * page.Size.Width;//leglength
                float marg = Settings.positionersMargin * page.Size.Width;
                byte[] types = { 0, 1, 1 };

                PointF VexTopLeft = new(marg + w, marg + w);//A1 in mm 594*841 blank page shows 595*842 random internet doc was in 595.32*842.04
                PointF[] vertexes = { new PointF(VexTopLeft.X + ll, VexTopLeft.Y), VexTopLeft, new PointF(VexTopLeft.X, VexTopLeft.Y + ll) };
                PdfPath path = new(vertexes, types);
                page.Graphics.DrawPath(Settings.positionersPen, path);

                VexTopLeft = new(page.Size.Width - marg - w, marg + w);
                vertexes = new PointF[] { new PointF(VexTopLeft.X - ll, VexTopLeft.Y), VexTopLeft, new PointF(VexTopLeft.X, VexTopLeft.Y + ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(Settings.positionersPen, path);

                VexTopLeft = new(page.Size.Width - marg - w, page.Size.Height - marg - w);
                vertexes = new PointF[] { new PointF(VexTopLeft.X - ll, VexTopLeft.Y), VexTopLeft, new PointF(VexTopLeft.X, VexTopLeft.Y - ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(Settings.positionersPen, path);

                VexTopLeft = new(marg + w, page.Size.Height - marg - w);
                vertexes = new PointF[] { new PointF(VexTopLeft.X + ll, VexTopLeft.Y), VexTopLeft, new PointF(VexTopLeft.X, VexTopLeft.Y - ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(Settings.positionersPen, path);

                //save for evaluation
            }
            return doc;
        }



        //TODODone add methods to remake fields and name field
        //TODODone saparate element and text editing

    }


    static class MineEdgeProcess
    {
        public static Bitmap ProcessFilterGray(this Bitmap source, double[,] horizontalFilter)
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
        public static List<List<List<bool>>> EvaluateWorks(this List<List<string>> works, List<List<Tuple<int, RectangleF, bool>>> questions)
        {
            List<List<List<bool>>> resultsAll = new();

            foreach (var work in works)
            {
                resultsAll.Add(work.EvaluateOneWork(questions));
            }


            return resultsAll;
        }
        public static QuadrilateralF FindPositsFromSettings(this Bitmap bitmap)
        {
            int legLength = (int)(Settings.positionersLegLength * bitmap.Width);
            int Margin = (int)(Settings.positionersMargin * bitmap.Width);

            return bitmap.FindPositionersInBitmap(legLength, Margin);
        }

        public static Tuple<PointF, float, float, float> AnalyzePositionersInBitmap(this Bitmap bitmap)
        {
            float l = Settings.positionersLegLength;
            int legLength = (int)(l * bitmap.Width);
            float m = Settings.positionersMargin;
            int margin = (int)(m * bitmap.Width);
            var P = bitmap.FindPositionersInBitmap(legLength, margin); //positioners

            float rotation = (P.p1.X - P.p2.X) / (P.p1.Y - P.p2.Y);    //tan(alpha)


            //corners
            QuadrilateralF c = new();

            var dx = P.p2.X - P.p1.X;//distance between points still in page size
            var dy = P.p4.Y - P.p1.Y;//in the calculation only one difference is used, the other is neglected which is good until the rotation is more then 10deg (i guess)

            float widthNew = dx / (1 - 2 * m);
            float heightNew = dy / (1 - 2 * m);
            float ratio = heightNew/bitmap.Height;

            var mNew = m * dx / (1 - 2 * m); //diminished margins still in page size
            c.p1 = new(P.p1.X - mNew, P.p1.Y - mNew);
            c.p2 = new(P.p2.X + mNew, P.p2.Y - mNew);
            c.p3 = new(P.p3.X + mNew, P.p3.Y + mNew);
            c.p4 = new(P.p4.X - mNew, P.p4.Y + mNew);

            string f = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
            int i = Directory.GetFiles(f).Length;
            var g = f + "\\" + i + "s.Bmp";
            f = f + "\\" + i + ".Bmp";
            bitmap.SetPixel((int)P.p1.X, (int)P.p1.Y, Color.Red);
            bitmap.SetPixel((int)P.p3.X, (int)P.p3.Y, Color.Red);
            bitmap.Save(g);
            bitmap.Corp(new((int)c.p1.X, (int)c.p1.Y, (int)widthNew, (int)heightNew)).Save(f);

            //todo work on finding best lines and the geometry of locating the rectangles

            return new Tuple<PointF,float,float,float>(P.p1, widthNew, dy, mNew);
        }
        public static QuadrilateralF FindPositionersInBitmapDiagonal(this Bitmap bitmap, int legLength, int margin)//not a good idea
        {
            Rectangle cropRect = new(0, 0, legLength + 2 * margin, legLength + 2 * margin);
            var crop = bitmap.CorpRelativeToImage(cropRect);
            var converted = crop.ProcessFilter(Settings.LaplFilterForPositioners);
            float threshold = (float)0.5;


            Point[] linesPoints = new Point[2 * legLength - 1];
            for (int i = 0; i < legLength; i++)
            {
                //Point starting = new(0, i);

                //float[] line = new float[2 * margin];
                for (int j = 0; j < 2 * margin; j++)
                {
                    if (converted.GetPixel(j, j + i).GetBrightness() > threshold)
                    {
                        linesPoints[legLength - 1 - i] = new(j, j + i);
                        break;
                    }
                }
                //linesPoints[legLength - 1 - i] = new(margin, margin);//todo correct
            }
            for (int i = 1; i < legLength; i++)
            {
                for (int j = 0; j < 2 * margin; j++)
                {
                    if (converted.GetPixel(j + i, j).GetBrightness() > threshold)
                    {
                        linesPoints[legLength - 1 + i] = new(j + i, j);
                        break;
                    }
                }
            }

            //var line1= linesPoints.LinearRegression();
            //var line2 = linesPoints.LinearRegression();
            //PointF p1 = line1.Item1.CrossectionOfTwoLines(line2.Item1);



            return null;
        }
        public static QuadrilateralF FindPositionersInBitmap(this Bitmap bitmap, int legLength, int margin)
        {
            int side = legLength + margin;
            int sidemarg = side + margin;
            float threshold = Settings.positionersEdgenessThreshold;

            Rectangle cropRect = new(margin, margin, side, side);
            var crop = bitmap.Corp(cropRect);
            var converted = crop.ProcessFilter(Settings.LaplFilterForPositioners);



            //goes through square where the angle is expected in lines left to right and finds first point of edge
            List<Point> linesPointsVer = new();
            for (int i = 0; i < side; i++)
            {
                //Point starting = new(margin,margin+ i);

                //float[] line = new float[2 * margin];
                for (int j = 0; j < side; j++)
                {
                    if (converted.GetPixel(i, j).GetBrightness() > threshold)
                    {
                        linesPointsVer.Add(new(i, j));
                        break;
                    }
                }
            }
            var lineVert = linesPointsVer.LinearRegression();//todo remove debuging

            //goes verticaly
            List<Point> linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (converted.GetPixel(i, j).GetBrightness() > threshold)
                    {
                        linesPointsHor.Add(new(i, j));
                        break;
                    }
                }
            var lineHor = linesPointsHor.LinearRegression();


            PointF p1 = lineHor.CrossectionOfTwoLines(lineVert);
            //add margin
            p1 += new Size(margin, margin);

            //top right
            cropRect = new(bitmap.Width - sidemarg, margin, side, side);
            crop = bitmap.Corp(cropRect);
            //crop.RotateFlip(RotateFlipType.Rotate90FlipNone);
            converted = crop.ProcessFilter(Settings.LaplFilterForPositioners);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (converted.GetPixel(side - i - 1, j).GetBrightness() > threshold)
                    {
                        linesPointsVer.Add(new(side - i - 1, j));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegression();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (converted.GetPixel(side - i - 1, j).GetBrightness() > threshold)
                    {
                        linesPointsHor.Add(new(side - i - 1, j));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegression();


            PointF p2 = lineHor.CrossectionOfTwoLines(lineVert);
            p2 += new Size(bitmap.Width - sidemarg, margin);

            //bottom right
            cropRect = new(bitmap.Width - sidemarg, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Corp(cropRect);
            converted = crop.ProcessFilter(Settings.LaplFilterForPositioners);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (converted.GetPixel(side - i - 1, side - j - 1).GetBrightness() > threshold)
                    {
                        linesPointsVer.Add(new(side - i - 1, side - j - 1));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegression();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (converted.GetPixel(side - i - 1, side - j - 1).GetBrightness() > threshold)
                    {
                        linesPointsHor.Add(new(side - i - 1, side - j - 1));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegression();


            PointF p3 = lineHor.CrossectionOfTwoLines(lineVert);
            p3 += new Size(bitmap.Width - sidemarg, bitmap.Height - sidemarg);

            //bottom left
            cropRect = new(margin, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Corp(cropRect);
            converted = crop.ProcessFilter(Settings.LaplFilterForPositioners);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (converted.GetPixel(i, side - j - 1).GetBrightness() > threshold)
                    {
                        linesPointsVer.Add(new(i, side - j - 1));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegression();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (converted.GetPixel(i, side - j - 1).GetBrightness() > threshold)
                    {
                        linesPointsHor.Add(new(i, side - j - 1));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegression();


            PointF p4 = lineHor.CrossectionOfTwoLines(lineVert);
            p4 += new Size(margin, bitmap.Height - sidemarg);

            //todo comment
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //crop.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\crop.bmp");
            //converted.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\converted.bmp");


            //bitmap.SetPixel((int)p1.X,(int) p1.Y, Color.Yellow);
            //bitmap.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\bit.bmp");




            return new(p1, p2, p3, p4);
        }
        public static QuadrilateralF FindCornersOfScanWithPositionersFromSettings(this Bitmap bitmap)
        {
            return bitmap.FindPositionersInBitmap((int)(Settings.positionersLegLength * bitmap.Width), (int)(Settings.positionersMargin * bitmap.Width)); //positioners
        }

        public static List<List<bool>> EvaluateOneWork(this List<string> work, List<List<Tuple<int, RectangleF, bool>>> questions)
        {
            List<List<bool>> resultsOneWork = new();
            Bitmap[] pages = new Bitmap[work.Count];
            var positioners = new Tuple<PointF, float, float, float>[work.Count];
            
            for (int i = 0; i < work.Count; i++)
            {
                pages[i] = new Bitmap(work[i]);
                positioners[i] = pages[i].AnalyzePositionersInBitmap();
            }



            foreach (var question in questions)
            {
                List<bool> resultsQuestion = new();
                foreach (var box in question)
                {
                    int pageindex = box.Item1;
                    //var rect = box.Item2;
                    //var ps = positioners[pageindex];
                    //float r = ps.Item2;
                    //float rx = rect.X*r - ps.Item4 + ps.Item1.X;
                    //float ry= rect.Y*r - ps.Item4 + ps.Item1.Y;
                    //float rw = rect.Width*r;
                    //float rh = rect.Width * r;//todo not the correct way of doing so
                    //var rNew = Rectangle.Round(new(rx, ry, rw, rh));

                    //Bitmap crop = pages[pageindex].Corp(rNew); 

                    Bitmap crop = pages[pageindex].CorpRelativeToImage(box.Item2); 


                    //debug feature
                    //TODO comment
                    string f = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
                    int i = Directory.GetFiles(f).Length;
                    f = f + "\\" + i + ".Bmp";
                    crop.Save(f);

                    //add to settings
                    //bool IsDark = crop.IsDarkRocognize();
                    bool IsCross = crop.IsEdgyInTheCenterRecognize();

                    resultsQuestion.Add(IsCross);
                    crop.Dispose();
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
                l.Add(new Bitmap(work[nameField.Item1]).CorpRelativeToImage(nameField.Item2));
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
        public static RectangleF UnrelatitivizeToPage(this ref RectangleF rect, PdfPageBase page)
        {
            rect.X *= page.Size.Width;
            rect.Y *= page.Size.Height;
            rect.Width *= page.Size.Width;
            rect.Height *= page.Size.Height;
            return rect;
        }
        public static RectangleF RelatitivizeToPage(this ref RectangleF rect, PdfPageBase page)
        {
            rect.X /= page.Size.Width;
            rect.Y /= page.Size.Height;
            rect.Width /= page.Size.Width;
            rect.Height /= page.Size.Height;
            return rect;

        }

        public static Rectangle UnrelatitivizeToImage(this RectangleF relativeRect, Bitmap image)
        {
            Rectangle cropRect = new((int)Math.Ceiling(relativeRect.X * image.Width), (int)Math.Ceiling(relativeRect.Y * image.Height), (int)Math.Floor(relativeRect.Width * image.Width), (int)Math.Floor(relativeRect.Height * image.Height));

            return cropRect;

        }

    }

}





