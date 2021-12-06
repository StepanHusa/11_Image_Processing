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

namespace _11_Image_Processing
{
    public static class PdfExtensions
    {

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

            //PdfSolidBrush brush =new(Color.White);
            //RectangleF bounds = rect;


            //page.Graphics.DrawRectangle(brush, bounds);

            doc.DrawRectangleBounds(rect, pageint);

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




        public static PdfLoadedDocument DrawRectangleBounds(this PdfLoadedDocument doc, RectangleF rectangle, int pageint)
        {
            var page = doc.Pages[pageint];
            PointF[] vertexes = { new PointF(rectangle.Left, rectangle.Bottom), new PointF(rectangle.Right, rectangle.Bottom), new PointF(rectangle.Right, rectangle.Top), new PointF(rectangle.Left, rectangle.Top), new PointF(rectangle.Left, rectangle.Bottom) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes,types);
            path.Pen = ST.boundPen;
            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
        public static PdfLoadedDocument DrawIndexNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, string index)
        {
            var page = doc.Pages[pageint];
            var p = new PointF(rectangle.Right+ST.boundWidth, rectangle.Top);
            page.Graphics.DrawString(index,new PdfStandardFont(PdfFontFamily.Courier,ST.indexFontSize), new PdfPen(Color.Black),p);

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

    static class PointFExtensions
    {
        public static PointF[] TranslatePoints(this PointF[] points, float dx, float dy)
        {
            Matrix translateMatrix = new Matrix(1, 0, 0, 1, dx, dy);
            translateMatrix.TransformPoints(points); //this modifies myPointArray

            return points;
        }
        public static PointF[] ScalePoints(this PointF[] points, float scale)
        {
            Matrix scaleMatrix = new Matrix(scale, 0, 0, scale, 0, 0);
            scaleMatrix.TransformPoints(points);       //this modifies myPointArray

            return points;
        }
        public static PointF ScalePoint(this PointF point, float scale)
        {
            PointF[] points = { point };

            Matrix scaleMatrix = new Matrix(scale, 0, 0, scale, 0, 0);
            scaleMatrix.TransformPoints(points);       //this modifies myPointArray

            return points[0];
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

    static class Conversions
    {
        public static Rectangle EvaluateInPositiveSize(this Rectangle rect)
        {
            int X, Y, W, H;
            if (rect.Size.Width < 0)
            {
                X = rect.X + rect.Size.Width;
                W = 0 - rect.Size.Width;
            }
            else
            {
                X = rect.X;
                W = rect.Size.Width;
            }

            if (rect.Size.Height < 0)
            {
                Y = rect.Y+ rect.Size.Height;
                H = 0 - rect.Size.Height;
            }
            else
            {
                Y = rect.Y;
                H = rect.Size.Height;
            }

            rect = new(X, Y, W, H);
            return rect;
        }
        public static RectangleF EvaluateInPositiveSize(this RectangleF rect)
        {
            float X, Y, W, H;
            if (rect.Size.Width < 0)
            {
                X = rect.X + rect.Size.Width;
                W = 0 - rect.Size.Width;
            }
            else
            {
                X = rect.X;
                W = rect.Size.Width;
            }

            if (rect.Size.Height < 0)
            {
                Y = rect.Y + rect.Size.Height;
                H = 0 - rect.Size.Height;
            }
            else
            {
                Y = rect.Y;
                H = rect.Size.Height;
            }

            rect = new(X, Y, W, H);
            return rect;
        }

    }    

    static class ObjectExtensions
    {
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }

    static class ImageProcessing
    {
        public static void Testing(this string filename)
        {
            
        }
        //public static void EvaluateSet( )
        //{
        //    List<PointF>[] lists;
        //    string fileName;
        //    PdfLoadedDocument doc = ST.document;
        //    float w = ST.boundWidth;
        //    List<bool[]> output = new();
        //    Bitmap[] pageImages = doc.ExportAsImage(0, doc.Pages.Count - 1, 300, 300);
        //    SizeF size = ST.sizeOfBox.Clone();

        //    double treshold = ST.treshold;

        //    if (lists.Length > doc.Pages.Count) throw new Exception("not a correct sizes of lists (RecognizeTaggedBoxes)");


        //    int pgCount = -1;
        //    foreach (var points in lists)
        //    {
        //        pgCount++;
        //        bool[] pageArray = new bool[points.Count];

        //        var s = doc.Pages[pgCount].Size; //595 842 size
        //        var sI = pageImages[pgCount].Size; //2479 3508 size image
        //        double r = sI.Height / s.Height; //4.16627 racio

        //        int pointCount = 0;
        //        foreach (var point in points)
        //        {
        //            RectangleF b = new RectangleF(point, size); //bounds
        //            Rectangle I = new(); //Int Rectangle


        //            b.Size -= new SizeF(w * 2, w * 2);
        //            b.Location += new SizeF(w, w);
        //            b.Size *= (float)r;
        //            b.Location = b.Location.ScalePoint((float)r);


        //            I = Rectangle.Round(b);



        //            int c = I.Width * I.Height;//count of pixels
        //            float cc = 0;
        //            for (int i = I.X; i < I.X + I.Width; i++)
        //                for (int j = I.Y; j < I.Y + I.Height; j++)
        //                {
        //                    pageImages[0].SetPixel(i, j, Color.Blue);
        //                    //var pix=pageImages[pgCount].GetPixel(i,j);
        //                    //var x = pix.GetBrightness();
        //                    cc += pageImages[pgCount].GetPixel(i, j).GetBrightness();
        //                }
        //            float av = cc / c;

        //            if (av < treshold) pageArray[pointCount] = true;
        //            pointCount++;
        //        }


        //        output.Add(pageArray);
        //    }

        //    pageImages[0].Save(Path.ChangeExtension(fileName, "jpg"));

        //}

    }

    static class ListExtensions
    {
        public static void Add(this List<Tuple<int, RectangleF>> l, int i, RectangleF p)
        {
            l.Add(new Tuple<int, RectangleF>(i, p));
        }
    }
}



