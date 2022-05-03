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
using System.Threading;
using System.Globalization;
using System.Resources;

namespace _11_Image_Processing
{
    public static class FileAndFolderExtensions
    {
        

        public static void DeleteTempFiles()
        {
            foreach (var file in ST.tempFilesToDelete)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }
        public static void CheckOrCreateLocalRoamingFolder()
        {
            if (!Directory.Exists(ST.roamingFolder)) Directory.CreateDirectory(ST.roamingFolder);

        }
        public static List<CultureInfo> FindAvalibleLanguages()
        {
            List<CultureInfo> cis = new();
            ResourceManager rm = new ResourceManager(typeof(Strings));

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    ResourceSet rs = rm.GetResourceSet(culture, true, false);
                    // or ResourceSet rs = rm.GetResourceSet(new CultureInfo(culture.TwoLetterISOLanguageName), true, false);
                    //string isSupported = (rs == null) ? " is not supported" : " is supported";
                    if (rs != null)
                        cis.Add(culture);
                }
                catch { }
            }

            return cis;
        }
    }
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

        public static PdfLoadedDocument DrawBox(this PdfLoadedDocument doc, Box box)
        {
            return doc.DrawRectangleBounds(box.Rectangle, box.Page, box.BoundWidth, box.IsCorrect);
        }
        public static PdfLoadedDocument DrawIndexNextToBox(this PdfLoadedDocument doc, Box box, string index)
        {
            return doc.DrawIndexNextToRectangle(box.Rectangle, box.Page, index);
        }


        public static PdfLoadedDocument DrawRectangleBounds(this PdfLoadedDocument doc, RectangleF rect, int pageint, float rWidth, bool SecondColor = false)
        {
            var page = doc.Pages[pageint];
            var w =/* rWidth * page.Size.Width*/ST.baundWidth/2;
            rect.UnrelatitivizeToPage(page);
            PointF[] vertexes = { new PointF(rect.Left - 2 * w, rect.Bottom + w), new PointF(rect.Right + w, rect.Bottom + w), new PointF(rect.Right + w, rect.Top - w), new PointF(rect.Left - w, rect.Top - w), new PointF(rect.Left - w, rect.Bottom + 2 * w) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes, types);
            path.Pen = ST.baundPen;
            //path.Pen=new PdfPen(Settings.baundColor, Settings.baundWidth);
            if (SecondColor)
                path.Pen = ST.baundPenTwo;

            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
        public static PdfLoadedDocument DrawIndexNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, string index)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelatitivizeToPage(page);
            var p = new PointF(rectangle.Right + ST.baundWidth, rectangle.Top);
            page.Graphics.DrawString(index, new PdfStandardFont(ST.stringFont, ST.QS.indexFontSize, ST.stringStyle), new PdfPen(Color.Black), p);

            return doc;
        }
        public static PdfLoadedDocument DrawNameNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelatitivizeToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * ST.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(ST.nameString, new PdfStandardFont(ST.stringFont, ST.QS.indexFontSize, ST.stringStyle), new PdfPen(Color.Black), p, format);

            return doc;
        }
        public static PdfLoadedDocument DrawStringNextToRectangle(this PdfLoadedDocument doc, string Text, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            rectangle.UnrelatitivizeToPage(page);
            rectangle.EvaluateInPositiveSize();
            var p = new PointF(rectangle.Left - 2 * ST.baundWidth, rectangle.Top);

            PdfStringFormat format = new() { Alignment = PdfTextAlignment.Right };
            page.Graphics.DrawString(Text, new PdfStandardFont(ST.stringFont, ST.QS.indexFontSize, ST.stringStyle), new PdfPen(Color.Black), p, format);

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
            foreach (var question in ST.boxesInQuestions)
            {
                int j = 0;
                foreach (var box in question)
                {
                    doc.DrawBox(box);
                    doc.DrawIndexNextToBox(box, (i + 1).ToString() + j.IntToAlphabet());
                    j++;
                }
                i++;
            }
            return doc;
        }
        public static PdfLoadedDocument RemakeFields(this PdfLoadedDocument doc)
        {
            int j = 0;
            foreach (var field in ST.Fields)
            {
                doc.DrawRectangleBounds(field.Item2, field.Item1, ST.baundWidth);
                doc.DrawStringNextToRectangle(Strings.text + (j + 1).ToString() + ":",field.Item2, field.Item1);
                j++;
            }

            return doc;
        }
        public static PdfLoadedDocument RemakeNameField(this PdfLoadedDocument doc)
        {
            if (ST.nameField == null) return doc;
            doc.DrawRectangleBounds(ST.nameField.Item2, ST.nameField.Item1, ST.baundWidth);
            doc.DrawNameNextToRectangle(ST.nameField.Item2, ST.nameField.Item1);
            return doc;
        }
        public static PdfLoadedDocument RemakeBoxexOneColor(this PdfLoadedDocument doc)
        {
            int i = 0;
            foreach (var question in ST.boxesInQuestions)
            {
                int j = 0;
                foreach (var box in question)
                {
                    doc.DrawRectangleBounds(box.Rectangle, box.Page, box.BoundWidth, false);
                    doc.DrawIndexNextToBox(box, (i + 1).ToString() + j.IntToAlphabet());
                    j++;
                }
                i++;
            }
            return doc;
        }
        public static PdfLoadedDocument MakeDocForExportOrPrint()
        {
            if (ST.tempFileCopy == null) return null;
            PdfLoadedDocument doc = new(ST.tempFileCopy);
            doc.RemakeBoxexOneColor().RemakeFields().RemakeNameField();
            return doc;
        }
        public static PdfLoadedDocument MakeDocForExportOrPrintWithAnswers()
        {
            if (ST.tempFileCopy == null) return null;
            PdfLoadedDocument doc = new(ST.tempFileCopy);
            doc.RemakeBoxex().RemakeFields().RemakeNameField();
            return doc;
        }


        //not used
        public static List<RectangleF> GetPositionerRectangles(this PdfLoadedDocument doc)
        {
            List<RectangleF> posis = new();

            foreach (PdfPageBase page in doc.Pages)
            {
                float marg = ST.positionersMargin * page.Size.Width;
                RectangleF marginRect = new RectangleF(marg, marg, page.Size.Width - 2 * marg, page.Size.Height - 2 * marg);
                posis.Add(marginRect.RelatitivizeToPage(page));
            }
            return posis;
        }

        public static PdfLoadedDocument AddPositioners(this PdfLoadedDocument doc)
        {
            List<RectangleF> save = new();
            foreach (PdfPageBase page in doc.Pages)
            {
                var w = ST.positionersWidth*page.Size.Width / 2;
                float ll = ST.positionersLegLength * page.Size.Width;//leglength
                float marg = ST.positionersMargin * page.Size.Width;
                byte[] types = { 0, 1, 1 };

                PointF VexTopLeft = new(marg + w, marg + w);//A1 in mm 594*841 blank page shows 595*842 random internet doc was in 595.32*842.04
                PointF[] vertexes = { new PointF(VexTopLeft.X + ll, VexTopLeft.Y), VexTopLeft, new PointF(VexTopLeft.X, VexTopLeft.Y + ll) };
                PdfPath path = new(vertexes, types);
                page.Graphics.DrawPath(new PdfPen(ST.positionersColor,2*w), path);

                PointF VexTopRight = new(page.Size.Width - marg - w, marg + w);
                vertexes = new PointF[] { new PointF(VexTopRight.X - ll, VexTopRight.Y), VexTopRight, new PointF(VexTopRight.X, VexTopRight.Y + ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(new PdfPen(ST.positionersColor,2*w), path);

                PointF VexBotRight = new(page.Size.Width - marg - w, page.Size.Height - marg - w);
                vertexes = new PointF[] { new PointF(VexBotRight.X - ll, VexBotRight.Y), VexBotRight, new PointF(VexBotRight.X, VexBotRight.Y - ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(new PdfPen(ST.positionersColor, 2 * w), path);

                PointF VexBotLeft = new(marg + w, page.Size.Height - marg - w);
                vertexes = new PointF[] { new PointF(VexBotLeft.X + ll, VexBotLeft.Y), VexBotLeft, new PointF(VexBotLeft.X, VexBotLeft.Y - ll) };
                path = new(vertexes, types);
                page.Graphics.DrawPath(new PdfPen(ST.positionersColor, 2 * w), path);

                //save for evaluation
                save.Add(new RectangleF((marg + w) / page.Size.Width, (marg + w) / page.Size.Height, 1 - 2*(marg + w) / page.Size.Width, 1- 2*(marg + w) / page.Size.Height));//set to the middle of lines
                //todo make relative to width only
            }
            ST.positioners = save;
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

            var I = J.ProcessFilter(ST.LaplacianOWNEdgeFilter);

            float c = 0;
            float cc = 0;
            for (int i = 0; i < I.Width; i++)
                for (int j = 0; j < I.Height; j++)
                {
                    float w = RelativeDistanceStar(i, j, I.Width, I.Height, (float)0.9).CalculateWeightBellShape(2, 5);
                    c += w;
                   cc += I.GetPixel(i, j).GetBrightness() * w;
                }

            float result = cc  * MathF.Sqrt(I.Height * I.Width) / (c* 8); //aroud 5 is a good treshold... meaning output is true if more than 1
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
        public static void SaveToDebugFolder(this Bitmap bit)
        {
            string g = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
            int k = Directory.GetFiles(g).Length;
            var h = g + "\\" + k + ".Bmp";
            bit.Save(h);
        }
        public static List<List<List<bool>>> EvaluateWorks(this List<List<string>> works, List<List<Box>> questions)
        {
            List<List<List<bool>>> resultsAll = new();
            //foreach (var work in works)
            //{
            //    try
            //    {
            //        resultsAll.Add(work.EvaluateOneWork(questions));
            //    }
            //    catch
            //    {
            //        System.Windows.MessageBox.Show(Strings.errorEvaluating + works.IndexOf(work), Strings.Error);
            //        for (int l = 0; l < questions.Count; l++)
            //        {
            //            List<bool> resultsQuestion = new();

            //            for (int j = 0; j < questions[l].Count; j++)
            //            {
            //                resultsQuestion.Add(false);
            //            }
            //            resultsAll[l].Add(resultsQuestion);
            //        }

            //    }
            //}
            ST.matrixPagesInWorks = new Matrix[works.Count][];
            for (int i = 0; i < works.Count; i++)
                resultsAll.Add(new());
            Parallel.For(0, works.Count, (i) =>
            {
                try
                {
                    resultsAll[i] = works[i].EvaluateOneWork(questions,i);
                }
                catch
                {
                    System.Windows.MessageBox.Show(Strings.errorEvaluating + (i + 1), Strings.Error);
                    for (int l = 0; l < questions.Count; l++)
                    {
                        List<bool> resultsQuestion = new();

                        for (int j = 0; j < questions[l].Count; j++)
                        {
                            resultsQuestion.Add(false);
                        }
                        resultsAll[i].Add(resultsQuestion);
                    }
                }
            });

            return resultsAll;
        }
        public static QuadrilateralF FindPositsFromSettings(this Bitmap bitmap)
        {
            int legLength = (int)(ST.positionersLegLength * bitmap.Width);
            int margin = (int)(ST.positionersMargin * bitmap.Width);


            return bitmap.FindPositionersInBitmap(legLength, margin);
        }//not used
        public static void AnalyzePositionersInBitmap(this Bitmap bitmap,int pageindex)//not used
        {
            float l = ST.positionersLegLength;
            int legLength = (int)(l * bitmap.Width);
            float m = ST.positionersMargin;
            int margin = (int)(m * bitmap.Width);
            var P = bitmap.FindPositionersInBitmap(legLength, margin); //positioners


            float rotation = (P.p1.X - P.p4.X) / (P.p1.Y - P.p4.Y);    //tan(alpha)


            //corners
            QuadrilateralF c = new();

            var dx = P.p2.X - P.p1.X;//distance between points still in page size
            var dy = P.p4.Y - P.p1.Y;//in the calculation only one difference is used, the other is neglected which is good until the rotation is more then 10deg (i guess)

            float widthNew = dx / (1 - 2 * m);
            var marNew = m * widthNew; //diminished margins still in page size
            float heightNew = dy +2*marNew;

            c.p1 = new(P.p1.X - marNew, P.p1.Y - marNew);
            c.p2 = new(P.p2.X + marNew, P.p2.Y - marNew);
            c.p3 = new(P.p3.X + marNew, P.p3.Y + marNew);
            c.p4 = new(P.p4.X - marNew, P.p4.Y + marNew);

            float sinAvr = (P.p1.X - P.p4.X) / (P.p1.Y - P.p4.Y);
            float cosAvr = 1 - sinAvr * sinAvr / 2;

            float sx = ST.positioners[pageindex].Width;
            float sy = ST.positioners[pageindex].Height;
            float px = P.p3.X - P.p1.X;
            float py = P.p3.Y - P.p1.Y;

            float invD = 1 / (sx * sx + sy * sy);
            float a1 =invD*( sx * px + sy * py);
            float a2 =invD*( sx * py - sy * px);

            Matrix matrix = new(ST.positioners[pageindex],new PointF[3] {P.p1,P.p2,P.p4 });

            var p3set = new PointF[1] { new(ST.positioners[pageindex].Right, ST.positioners[pageindex].Bottom) };
            matrix.TransformPoints(p3set);

            //Matrix matrix = new(cosAvr * widthNew, sinAvr * heightNew, -sinAvr * widthNew, cosAvr * heightNew, P.p1.X, P.p1.Y);

            //string f = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
            //int i = Directory.GetFiles(f).Length;
            //var g = f + "\\" + i + "s.Bmp";
            //f = f + "\\" + i + ".Bmp";
            //bitmap.SetPixel((int)P.p1.X, (int)P.p1.Y, Color.Red);
            //bitmap.SetPixel((int)P.p3.X, (int)P.p3.Y, Color.Red);
            //bitmap.Save(g);
            //bitmap.Crop(new((int)c.p1.X, (int)c.p1.Y, (int)widthNew, (int)heightNew)).Save(f);

            //todo work on finding best lines and the geometry of locating the rectangles
            //bitmap.SetPixel((int)(P.p1.X + widthNew - 2 * marNew), (int)(P.p1.Y + heightNew - 2 * marNew),Color.Red);
            //bitmap.SaveToDebugFolder();

        }
        public static Matrix MakeTransformationMatrixFromPositioners(this Bitmap bitmap,int pageindex)
        {
            float l = ST.positionersLegLength;
            int legLength = (int)(l * bitmap.Width);
            float m = ST.positionersMargin;
            int margin = (int)(m * bitmap.Width);
            float ww = ST.positionersWidth;
            int width = (int)(ww * bitmap.Width);
             //bitmap.FindPositionersInBitmap(legLength, margin);
            var P = bitmap.FindPositionersInBitmapShape(legLength, margin, width); //positioners

            //TODOd comment
            //bitmap.SetPixel((int)P.p1.X, (int)P.p1.Y, Color.Red);
            //bitmap.SetPixel((int)P.p2.X, (int)P.p2.Y, Color.Red);
            //bitmap.SetPixel((int)P.p3.X, (int)P.p3.Y, Color.Red);
            //bitmap.SetPixel((int)P.p4.X, (int)P.p4.Y, Color.Red);
            //bitmap.SaveToDebugFolder();



            if (ST.positioners == null)
            {
                PdfLoadedDocument ddoc = new(ST.tempFile);
                ddoc.AddPositioners();
                ddoc.Save(ST.tempFile);
            }
            Matrix matrix = new(ST.positioners[pageindex], new PointF[3] { P.p1, P.p2, P.p4 });

            return matrix;
        }
        public static QuadrilateralF FindPositionersInBitmapDiagonal(this Bitmap bitmap, int legLength, int margin)//not a good idea
        {
            Rectangle cropRect = new(0, 0, legLength + 2 * margin, legLength + 2 * margin);
            var crop = bitmap.CropRelativeToImage(cropRect);
            var converted = crop.ProcessFilter(ST.LaplFilterForPositionersBetter);
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
        public static float BitmapMean(this Bitmap bitmap)
        {
            int count = 0;
            int sum = 0;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    count += 3;
                    var c= bitmap.GetPixel(i, j);
                    sum += c.R + c.G + c.B;
                }
            }
            return (float)sum / (count * 255);

        }
        public static QuadrilateralF FindPositionersInBitmapOld(this Bitmap bitmap, int legLength, int margin)
        {
            int side = legLength + margin;
            int sidemarg = side + margin;
            float threshold = ST.positionersEdgenessThreshold;
            double[,] filter;
            if (bitmap.Width>2000)
                filter = ST.LaplFilterForPositionersBetterLarger;
            else filter = ST.LaplFilterForPositionersBetter;


            //things that can go wrong:
            //edge of the paper can be in the crop region
            //changing resolution can defuse edges of positioners
            //noise can confuse the algorythm
            //page gets smaller (instead of larger)




            Rectangle cropRect = new(margin, margin, side, side);
            var crop = bitmap.Crop(cropRect);
            var a = crop.GetPixel(241, 115);
            a = crop.GetPixel(284,121);
            a = crop.GetPixel(250, 123);
            //var converted = crop.ProcessFilter(Settings.LaplFilterForPositionersBetter);
            var converted = crop.ProcessFilter(filter);


            ////atodo comment
            //crop.SaveToDebugFolder();
            //converted.SaveToDebugFolder();
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
            var lineVert = linesPointsVer.LinearRegressionVerticalOutliers();


            //goes horizontaly and finds vertical line
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
            var lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            PointF p1 = lineHor.CrossectionOfTwoLines(lineVert);
            //add margin
            p1 += new Size(margin, margin);

            //top right
            cropRect = new(bitmap.Width - sidemarg, margin, side, side);
            crop = bitmap.Crop(cropRect);
            //crop.RotateFlip(RotateFlipType.Rotate90FlipNone);
            converted = crop.ProcessFilter(ST.LaplFilterForPositionersBetter);

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
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

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
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            PointF p2 = lineHor.CrossectionOfTwoLines(lineVert);
            p2 += new Size(bitmap.Width - sidemarg, margin);
            //bottom right
            cropRect = new(bitmap.Width - sidemarg, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Crop(cropRect);
            converted = crop.ProcessFilter(ST.LaplFilterForPositionersBetter);

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
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

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
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            PointF p3 = lineHor.CrossectionOfTwoLines(lineVert);
            p3 += new Size(bitmap.Width - sidemarg, bitmap.Height - sidemarg);

            //TODOd comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //ii = Directory.GetFiles(f).Length;
            //g = f + "\\" + ii + ".Bmp";
            //crop.Save(g);
            //bottom left
            cropRect = new(margin, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Crop(cropRect);
            converted = crop.ProcessFilter(ST.LaplFilterForPositionersBetter);

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
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

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
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();

            ////TODOd comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Blue);
            //}
            //crop.SaveToDebugFolder();
            //converted.SaveToDebugFolder();


            PointF p4 = lineHor.CrossectionOfTwoLines(lineVert);
            p4 += new Size(margin, bitmap.Height - sidemarg);

            //TODOd comment
            //bitmap.SetPixel((int)p1.X, (int)p1.Y, Color.Red);
            //bitmap.SetPixel((int)p2.X, (int)p2.Y, Color.Red);
            //bitmap.SetPixel((int)p3.X, (int)p3.Y, Color.Red);
            //bitmap.SetPixel((int)p4.X, (int)p4.Y, Color.Red);
            //bitmap.SaveToDebugFolder();

            //crop.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\crop.bmp");
            //converted.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\converted.bmp");

            //bitmap.SetPixel((int)p1.X,(int) p1.Y, Color.Yellow);
            //bitmap.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\bit.bmp");




            return new(p1, p2, p3, p4);
        }
        public static QuadrilateralF FindPositionersInBitmap(this Bitmap bitmap, int legLength, int margin)
        {
            int side = legLength + 2*margin;
            int sidemarg = side /*+ margin*/;
            float threshold = ST.positionersEdgenessThreshold;

            //things that can go wrong:
            //edge of the paper can be in the crop region
            //changing resolution can defuse edges of positioners
            //noise can confuse the algorythm
            //page gets smaller (instead of larger)

            //Rectangle cropRect = new(margin, margin, side, side);
            Rectangle cropRect = new(0, 0, side, side);

            var crop = bitmap.Crop(cropRect);
            var a = crop.GetPixel(241, 115);
            a = crop.GetPixel(284, 121);
            a = crop.GetPixel(250, 123);
            //bitmap.SaveToDebugFolder();
            //var converted = crop.ProcessFilter(Settings.LaplFilterForPositionersBetter);
            //var converted = crop.ProcessFilter(filter);


            //atodo comment
            //crop.SaveToDebugFolder();
            //converted.SaveToDebugFolder();
            //goes through square where the angle is expected in lines left to right and finds first point of edge

            List<Point> linesPointsVer = new();
            for (int i = 0; i < side; i++)
            {
                //Point starting = new(margin,margin+ i);

                //float[] line = new float[2 * margin];
                for (int j = 0; j < side; j++)
                {
                    if (crop.GetPixel(i, j).GetBrightness() < threshold)
                    {
                        linesPointsVer.Add(new(i, j));
                        break;
                    }
                }
            }
            var lineVert = linesPointsVer.LinearRegressionVerticalOutliers();


            //goes horizontaly and finds vertical line
            List<Point> linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (crop.GetPixel(i, j).GetBrightness() < threshold)
                    {
                        linesPointsHor.Add(new(i, j));
                        break;
                    }
                }
            var lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            ////aTODO comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Blue);
            //}
            //crop.SaveToDebugFolder();

            PointF p1 = lineHor.CrossectionOfTwoLines(lineVert);
            //add margin
            p1 += new Size(margin, margin);

            //top right
            cropRect = new(bitmap.Width - sidemarg, margin, side, side);
            crop = bitmap.Crop(cropRect);
            //crop.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //converted = crop.ProcessFilter(Settings.LaplFilterForPositionersBetter);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (crop.GetPixel(side - i - 1, j).GetBrightness() < threshold)
                    {
                        linesPointsVer.Add(new(side - i - 1, j));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (crop.GetPixel(side - i - 1, j).GetBrightness() < threshold)
                    {
                        linesPointsHor.Add(new(side - i - 1, j));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            //////aTODO comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Blue);
            //}
            //crop.SaveToDebugFolder();

            PointF p2 = lineHor.CrossectionOfTwoLines(lineVert);
            p2 += new Size(bitmap.Width - sidemarg, margin);
            //bottom right
            cropRect = new(bitmap.Width - sidemarg, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Crop(cropRect);
            //converted = crop.ProcessFilter(Settings.LaplFilterForPositionersBetter);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (crop.GetPixel(side - i - 1, side - j - 1).GetBrightness() < threshold)
                    {
                        linesPointsVer.Add(new(side - i - 1, side - j - 1));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (crop.GetPixel(side - i - 1, side - j - 1).GetBrightness() < threshold)
                    {
                        linesPointsHor.Add(new(side - i - 1, side - j - 1));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();


            PointF p3 = lineHor.CrossectionOfTwoLines(lineVert);
            p3 += new Size(bitmap.Width - sidemarg, bitmap.Height - sidemarg);

            ////aTODO comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Blue);
            //}
            //crop.SaveToDebugFolder();



            //bottom left
            cropRect = new(margin, bitmap.Height - sidemarg, side, side);
            crop = bitmap.Crop(cropRect);
            //converted = crop.ProcessFilter(Settings.LaplFilterForPositionersBetter);

            linesPointsVer = new();
            for (int i = 0; i < side; i++)
                for (int j = 0; j < side; j++)
                {
                    if (crop.GetPixel(i, side - j - 1).GetBrightness() < threshold)
                    {
                        linesPointsVer.Add(new(i, side - j - 1));
                        break;
                    }
                }
            lineVert = linesPointsVer.LinearRegressionVerticalOutliers();

            linesPointsHor = new();
            for (int j = 0; j < side; j++)
                for (int i = 0; i < side; i++)
                {
                    if (crop.GetPixel(i, side - j - 1).GetBrightness() < threshold)
                    {
                        linesPointsHor.Add(new(i, side - j - 1));
                        break;
                    }
                }
            lineHor = linesPointsHor.LinearRegressionHorizontalOutliers();

            ////aTODO comment
            //foreach (var point in linesPointsHor)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Red);
            //}
            //foreach (var point in linesPointsVer)
            //{
            //    crop.SetPixel((int)point.X, (int)point.Y, Color.Blue);
            //}
            //crop.SaveToDebugFolder();
            //converted.SaveToDebugFolder();


            PointF p4 = lineHor.CrossectionOfTwoLines(lineVert);
            p4 += new Size(margin, bitmap.Height - sidemarg);

            ////aTODO comment
            //bitmap.SetPixel((int)p1.X, (int)p1.Y, Color.Red);
            //bitmap.SetPixel((int)p2.X, (int)p2.Y, Color.Red);
            //bitmap.SetPixel((int)p3.X, (int)p3.Y, Color.Red);
            //bitmap.SetPixel((int)p4.X, (int)p4.Y, Color.Red);
            ////bitmap.SaveToDebugFolder();

            //crop.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\crop.bmp");
            //converted.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\converted.bmp");

            //bitmap.SetPixel((int)p1.X,(int) p1.Y, Color.Yellow);
            //bitmap.Save(@"C:\Users\stepa\source\repos\11_Image_Processing\debug files\test\posits\bit.bmp");




            return new(p1, p2, p3, p4);
        }
        public static QuadrilateralF FindPositionersInBitmapShape(this Bitmap bitmap, int legLength, int margin,int width)
        {
            var bmg = new BitmapInGrayscale255(bitmap);
            int w = bitmap.Width;
            int h = bitmap.Height;

            int min = int.MaxValue;
            Point minP = new();

            for (int i = 0; i < 2 * margin + legLength; i++)
                for (int j = 0; j < 2 * margin + legLength; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < width; k++)
                        for (int l = 0; l < legLength; l++)
                            sum += bmg[i + k, j + l];

                    for (int k = 0; k < legLength; k++)
                        for (int l = 0; l < width; l++)
                            sum += bmg[i + k, j + l];

                    if (sum < min)
                    {
                        min = sum;
                        minP = new(i + width / 2, j + width / 2);
                    }
                }
            var p1 = minP;

            min = int.MaxValue;

            for (int i = 0; i < 2 * margin + legLength; i++)
                for (int j = 0; j < 2 * margin + legLength; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < width; k++)
                        for (int l = 0; l < legLength; l++)
                            sum += bmg[w - i - k - 1, j + l];

                    for (int k = 0; k < legLength; k++)
                        for (int l = 0; l < width; l++)
                            sum += bmg[w - i - k - 1, j + l];

                    if (sum < min)
                    {
                        min = sum;
                        minP = new(w - i - width / 2 - 1, j + width / 2);
                    }
                }

            var p2 = minP;

            min = int.MaxValue;

            for (int i = 0; i < 2 * margin + legLength; i++)
                for (int j = 0; j < 2 * margin + legLength; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < width; k++)
                        for (int l = 0; l < legLength; l++)
                            sum += bmg[w - i - k - 1, h - j - l - 1];

                    for (int k = 0; k < legLength; k++)
                        for (int l = 0; l < width; l++)
                            sum += bmg[w - i - k - 1, h - j - l - 1];

                    if (sum < min)
                    {
                        min = sum;
                        minP = new(w - i - width / 2 - 1, h - j - width / 2 - 1);
                    }
                }

            var p3 = minP;

            min = int.MaxValue;

            for (int i = 0; i < 2 * margin + legLength; i++)
                for (int j = 0; j < 2 * margin + legLength; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < width; k++)
                        for (int l = 0; l < legLength; l++)
                            sum += bmg[i + k, h - j - l - 1];

                    for (int k = 0; k < legLength; k++)
                        for (int l = 0; l < width; l++)
                            sum += bmg[i + k, h - j - l - 1];

                    if (sum < min)
                    {
                        min = sum;
                        minP = new(i + width / 2, h - j - width / 2 - 1);
                    }
                }

            var p4 = minP;









            //atodo comment
            //for (int k = 0; k < width; k++)
            //    for (int l = 0; l < legLength; l++)
            //        bitmap.SetPixel(minP.X + k, minP.Y + l, Color.Green);

            //for (int k = 0; k < legLength; k++)
            //    for (int l = 0; l < width; l++)
            //        bitmap.SetPixel(minP.X + k, minP.Y + l, Color.Green);

            return new(p1,p2,p3,p4);
        }


        public static List<List<bool>> EvaluateOneWork(this List<string> work, List<List<Box>> questions,int index)
        {
            List<List<bool>> resultsOneWork = new();
            Bitmap[] pages = new Bitmap[work.Count];
            var matrixes = new Matrix[work.Count];
            float marginST = ST.positionersMargin;

            for (int i = 0; i < work.Count; i++)
            {
                pages[i] = new Bitmap(work[i]);
                matrixes[i] = pages[i].MakeTransformationMatrixFromPositioners(i);
            }
            ST.matrixPagesInWorks[index] = matrixes;

            //pages[0] = new Bitmap(work[0]);
            //matrixes[0] = pages[0].MakeTransformationMatrixFromPositioners(0);

            foreach (var question in questions)
            {
                List<bool> resultsQuestion = new();
                foreach (var box in question)
                {
                    int pageindex = box.Page;

                    var rect = box.Rectangle;
                    RectangleF newRect = new(rect.Location.ApplyMatrix(matrixes[pageindex]), rect.Size.ApplyMatrix(matrixes[pageindex]));
                    Bitmap crop = pages[pageindex].Crop(Rectangle.Round(newRect));

                    //debug feature
                    //TODO comment
                    pages[pageindex].DrowRectangle(newRect);
                    if (questions.IndexOf(question) == 3)
                        crop.SaveToDebugFolder();

                    bool IsCross = crop.IsEdgyInTheCenterRecognize();

                    resultsQuestion.Add(IsCross);
                    crop.Dispose();


                }
                resultsOneWork.Add(resultsQuestion);
            }

            pages[0].SaveToDebugFolder();
            pages[1].SaveToDebugFolder();
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].Dispose();
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
                var rect = nameField.Item2;
                var bitmap = new Bitmap(work[nameField.Item1]);
                var matrix = bitmap.MakeTransformationMatrixFromPositioners(nameField.Item1);
                l.Add(bitmap.Crop( Rectangle.Round(new(rect.Location.ApplyMatrix(matrix), rect.Size.ApplyMatrix(matrix)))));
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
        public static void Add(this List<Box> list, int page, RectangleF rectangle,float width, bool isCorrect)
        {
            list.Add(new Box(page, rectangle,width, isCorrect));
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
                        works[i][box.Page] = works[i][box.Page].DrowRectangle(box.Rectangle);
                    }
                }
            }
            return works;
        }

        public static Bitmap DrowRectangle(this Bitmap b, RectangleF rect) //primarly debug feature
        {
            //float racioX = b.Width / sizeOfPage.Width;
            //float racioY = b.Height / sizeOfPage.Height;
            Rectangle cropRect = Rectangle.Round(rect);
            for (int i = 0; i <= cropRect.Width; i++)
            {
                b.SetPixel(cropRect.X + i, cropRect.Y, Color.Yellow);
                b.SetPixel(cropRect.X + i, cropRect.Y + cropRect.Height, Color.DarkBlue);

            }

            for (int j = 0; j <= cropRect.Height; j++)
            {
                b.SetPixel(cropRect.X, cropRect.Y + j, Color.Yellow);
                b.SetPixel(cropRect.X + cropRect.Width, cropRect.Y + j, Color.DarkBlue);

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
        public static Rectangle UnrelatitivizeToWidthofImage(this RectangleF relativeRect, Bitmap image)
        {
            Rectangle cropRect = new((int)Math.Ceiling(relativeRect.X * image.Width), (int)Math.Ceiling(relativeRect.Y * image.Width), (int)Math.Floor(relativeRect.Width * image.Width), (int)Math.Floor(relativeRect.Height * image.Width));

            return cropRect;

        }

    }

}





