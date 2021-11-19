﻿using Syncfusion.Pdf;
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

namespace _11_Image_Processing
{
    public static class PdfExtensions
    {

        public static PdfLoadedDocument AddSquareAt(this PdfLoadedDocument doc, int pageint, PointF position)
        {
            //var page = doc.Pages[pageint];

            SizeF size = ST.sizeOfBox;
            //PdfSolidBrush brush = ST.borderBrush;
            RectangleF bounds = new RectangleF(position, size);
            //page.Graphics.DrawRectangle(brush, bounds);


            //float w = ST.boundWidth;
            //bounds.X += w;
            //bounds.Y += w;
            //bounds.Width -= 2 * w;
            //bounds.Height -= 2 * w;

            //page.Graphics.DrawRectangle(ST.insideBrush, bounds);
            doc.DrawRectangleBounds(bounds, pageint);

            return doc;
        }
        //public static PdfDocument AddSquareAt(this PdfDocument doc, int pageint, PointF position)
        //{
        //    var page = doc.Pages[pageint];

        //    SizeF size = ST.sizeOfBox;
        //    PdfSolidBrush brush = new PdfSolidBrush(Color.Red);
        //    RectangleF bounds = new RectangleF(position, size);
        //    //page.Graphics.DrawRectangle(brush, bounds);

        //    //float w = ST.boundWidth;
        //    //bounds.X += w;
        //    //bounds.Y += w;
        //    //bounds.Width -= 2 * w;
        //    //bounds.Height -= 2 * w;

        //    //page.Graphics.DrawRectangle(ST.insideBrush, bounds);

            

        //    return doc;
        //}
        public static PdfLoadedDocument AddRectangleAt(this PdfLoadedDocument doc, int pageint, RectangleF rect)
        {
            var page = doc.Pages[pageint];

            PdfSolidBrush brush =new(Color.White);
            RectangleF bounds = rect;


            page.Graphics.DrawRectangle(brush, bounds);

            //doc.DrawRectangleBounds(rect, pageint);

            return doc;

        }



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


                    b.Size -= new SizeF(w*2, w*2);
                    b.Location += new SizeF(w, w);
                    b.Size*= (float)r;
                    b.Location= b.Location.ScalePoint((float)r);


                    I=Rectangle.Round(b);



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
        public static List<bool[]> RecognizeTaggedBoxes(this PdfLoadedDocument doc,  List<PointF>[] lists, int sizeInt = 20, float w = 2, string fileName= @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\02.pdf")
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



        private static PointF[] TranslatePoints(this PointF[] points, float dx, float dy)
        {
            Matrix translateMatrix = new Matrix(1, 0, 0, 1, dx, dy);
            translateMatrix.TransformPoints(points); //this modifies myPointArray

            return points;
        }
        private static PointF[] ScalePoints(this PointF[] points, float scale) 
        { 
            Matrix scaleMatrix = new Matrix(scale, 0, 0, scale, 0, 0);
            scaleMatrix.TransformPoints(points);       //this modifies myPointArray

            return points;
        }
        private static PointF ScalePoint(this PointF point, float scale)
        {
            PointF[] points = { point };

            Matrix scaleMatrix = new Matrix(scale, 0, 0, scale, 0, 0);
            scaleMatrix.TransformPoints(points);       //this modifies myPointArray

            return points[0];
        }

        private static PdfLoadedDocument DrawRectangleBounds(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            PointF[] vertexes = { new PointF(rectangle.Left, rectangle.Bottom), new PointF(rectangle.Right, rectangle.Bottom), new PointF(rectangle.Right, rectangle.Top), new PointF(rectangle.Left, rectangle.Top), new PointF(rectangle.Left, rectangle.Bottom) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes,types);
            path.Pen = ST.boundPen;
            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
    }

    static class ByteExtensions
    {
        public static byte[] Combine(this byte[] first, byte[] second)
        {
            byte[] bytes = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);
            return bytes;
        }
        public static byte[] StringToByteArray(this string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] PointListArrayToByteArray(this List<PointF>[] array)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(array.Length);
                    foreach (var l in array)
                    {
                        bw.Write(l.Count);

                        foreach (var p in l)
                        {
                            bw.Write(p.X);
                            bw.Write(p.Y);
                        }
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static List<PointF>[] ByteArrayToPointFListArray(this byte[] bArray)
        {
            List<PointF>[] listsArray;
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfArray = r.ReadInt32();
                    listsArray = new List<PointF>[lengthOfArray];
                    for (int i = 0; i != lengthOfArray; i++)
                    {
                        int Count = r.ReadInt32();
                        listsArray[i] = new();
                        for (int j = 0; j != Count; j++)
                        {
                            listsArray[i].Add(new PointF(r.ReadSingle(), r.ReadSingle()));
                        }
                    }
                }
            }
            return listsArray;
        }
        public static byte[] RectangleListArrayToByteArray(this List<RectangleF>[] array)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(array.Length);
                    foreach (var l in array)
                    {
                        bw.Write(l.Count);

                        foreach (var p in l)
                        {
                            bw.Write(p.Location.X);
                            bw.Write(p.Location.Y);
                            bw.Write(p.Size.Width);
                            bw.Write(p.Size.Height);
                        }
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static List<RectangleF>[] ByteArrayToRectangleListArray(this byte[] bArray)
        {
            List<RectangleF>[] listsArray;
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfArray = r.ReadInt32();
                    listsArray = new List<RectangleF>[lengthOfArray];
                    for (int i = 0; i != lengthOfArray; i++)
                    {
                        int Count = r.ReadInt32();
                        listsArray[i] = new();
                        for (int j = 0; j != Count; j++)
                        {
                            listsArray[i].Add(new RectangleF(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()));
                        }
                    }
                }
            }
            return listsArray;
        }
        public static byte[] GetHashSHA1(this byte[] data)
        {
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
            {
                var hash = sha1.ComputeHash(data);
                return hash;
            }
        }
    }

    static class StringExtensions
    {
        public static string ToStringOfRegularFormat(this DateTime d)
        {
            using (var sw =new StringWriter())
            {
                sw.Write(d.Year);
                sw.Write("-");
                sw.Write(d.Month);
                sw.Write("-");
                sw.Write(d.Day);
                sw.Write(" ");
                sw.Write(d.Hour);
                sw.Write(":");
                sw.Write(d.Minute);
                sw.Write(":");
                sw.Write(d.Second);
                sw.Write(":");
                sw.Write(d.Millisecond);

                return sw.ToString();
            }
        }
    }

}



