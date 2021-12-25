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
//using System.Windows.Media.Imaging;

namespace _11_Image_Processing
{
    public static class PdfExtensions
    {

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


        public static PdfLoadedDocument DrawRectangleBounds(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, bool SecondColor=false)
        {

            //TODO get rectangle relative
            var page = doc.Pages[pageint];
            var w = ST.baundWidth/2;
            PointF[] vertexes = { new PointF(rectangle.Left-2*w, rectangle.Bottom+w), new PointF(rectangle.Right+w, rectangle.Bottom+w), new PointF(rectangle.Right+w, rectangle.Top-w), new PointF(rectangle.Left-w, rectangle.Top-w), new PointF(rectangle.Left-w, rectangle.Bottom+2*w) };
            byte[] types = { 0, 1, 1, 1, 1 };
            PdfPath path = new(vertexes,types);
            path.Pen = ST.baundPen;
            if (SecondColor)
                path.Pen = ST.baundPenTwo;

            page.Graphics.DrawPath(path.Pen, path);

            return doc;
        }
        public static PdfLoadedDocument DrawIndexNextToRectangle(this PdfLoadedDocument doc, RectangleF rectangle, int pageint, string index)
        {
            var page = doc.Pages[pageint];
            var p = new PointF(rectangle.Right+ST.baundWidth, rectangle.Top);
            page.Graphics.DrawString(index,new PdfStandardFont(PdfFontFamily.Courier,ST.indexFontSize), new PdfPen(Color.Black),p);

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
        public static byte[] IntRectangleFBoolTupleListListToByteArray(this List<List<Tuple<int, RectangleF, bool>>> list)
        {
            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(list.Count);
                    foreach (var l in list)
                    {
                        bw.Write(l.Count);
                        foreach (var p in l)
                        {
                            bw.Write(p.Item1);//int32 (int)
                            bw.Write(p.Item2.X);//Single (float)
                            bw.Write(p.Item2.Y);//Single (float)
                            bw.Write(p.Item2.Width);//Single (float)
                            bw.Write(p.Item2.Height);//Single (float)
                            bw.Write(p.Item3);//Boolen (bool)

                        }
                    }
                }
                data = ms.ToArray();
            }

            return data;

        }
        public static List<List<Tuple<int,RectangleF,bool>>> ByteArrayToIntRectangleFBoolTupleListList(this byte[] ba)
        {
            List<List<Tuple<int, RectangleF, bool>>> lists;
            using (var ms = new MemoryStream(ba))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfList = r.ReadInt32();
                    lists = new();
                    for (int i = 0; i <lengthOfList; i++)
                    {
                        int Count = r.ReadInt32();
                        lists.Add(new());
                        for (int j = 0; j < Count; j++)
                        {
                            lists[i].Add(new Tuple<int, RectangleF, bool>(r.ReadInt32(),new RectangleF(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()),r.ReadBoolean()));
                        }
                    }
                }
            }
            return lists;

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
        public static string ToOFDFilter(this string[] extensionsWithoutDot)
        {
            string s = "All files(*.*) | *.*";
            foreach (var ext in extensionsWithoutDot)
            {
                s += $"|(*.{ext})|*.{ext}";
            
            }
            return s;
        }
        public static string ToOFDFilter(this string[] extensionsWithoutDot, string[] comments)
        {
            string s = "All files(*.*) | *.*";
            Array.Resize(ref comments, extensionsWithoutDot.Length);
            for (int i = 0; i < extensionsWithoutDot.Length; i++)
            {

               s += $"|{comments[i]}(*.{extensionsWithoutDot[i]})|*.{extensionsWithoutDot[i]}";

            }
            return s;
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
        public static List<List<List<bool>>> EvaluateWorks(this List<List<Bitmap>> works, List<List<Tuple<int, RectangleF, bool>>> questions, List<SizeF> sizesOfPages)
        {
            List<List<List<bool>>> resultsAll = new();

            foreach (var work in works)
            {
                resultsAll.Add(work.EvaluateOneWork(questions, sizesOfPages));
            }


            return resultsAll;
        }
        public static List<List<bool>> EvaluateOneWork(this List<Bitmap> work, List<List<Tuple<int, RectangleF, bool>>> questions, List<SizeF> sizesOfPages)
        {
            List<List<bool>> resultsOneWork = new();

            foreach (var question in questions)
            {
                List<bool> resultsQuestion = new();
                foreach (var box in question)
                {
                    int pageindex = box.Item1;
                    resultsQuestion.Add(box.IsDarkRocognize(work[pageindex], sizesOfPages[pageindex]));
                }
                resultsOneWork.Add(resultsQuestion);
            }
            return resultsOneWork;
        }

        public static bool IsDarkRocognize(this Tuple<int, RectangleF, bool> box, Bitmap image, SizeF sizeOfPage)
        {
            var rect = box.Item2;
            float racioX = image.Width / sizeOfPage.Width;
            float racioY = image.Height / sizeOfPage.Height;
            Size size = new((int)Math.Ceiling(box.Item2.Width * racioX), (int)Math.Ceiling(box.Item2.Height * racioY));
            Rectangle cropRect = new((int)(rect.X*racioX), (int)(rect.Y * racioY), (int)(rect.Width * racioX), (int)(rect.Height * racioY)) ;


            Bitmap crop = new Bitmap((int)size.Width,(int)size.Height);



            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(image, new Rectangle(0, 0, crop.Width, crop.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }

            //debug feature
            string f = @"C:\Users\stepa\source\repos\11_Image_Processing\debug files\s";
            int i = Directory.GetFiles(f).Length;
            f = f + "\\" + i + ".Bmp";
            crop.Save(f, ImageFormat.Bmp);


            if (ColorLevelOfBitmap(crop) < 0.5) return true;

            return false;
        }

        public static float ColorLevelOfBitmap(Bitmap I)
        {
            int c = I.Height * I.Width;
            float cc = 0;
            for (int i = 0; i < I.Width; i++)
                for (int j = 0; j < I.Height; j++)
                {
                    cc += I.GetPixel(i, j).GetBrightness();
                }
            return cc / c;
        }
    }

    static class ListExtensions
    {
        public static void Add(this List<Tuple<int, RectangleF,bool>> l, int i, RectangleF p,bool b)
        {
            l.Add(new Tuple<int, RectangleF,bool>(i, p,b));
        }
    }
    static class BitmapExtensions
    {
        public static System.Windows.Media.Imaging.BitmapImage BitmapToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapimage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static List<List<Bitmap>> DrowCorrect(this List<List<Bitmap>> works)
        {
            for (int i = 0; i < works.Count; i++)
            {
                foreach (var question in ST.boxesInQuestions)
                {
                    foreach (var box in question)
                    {
                        works[i][box.Item1]=works[i][box.Item1].DrowRectangle(box.Item2);//TODO get rectangle relative
                    }
                }
            }
            return works;
        }


        public static Bitmap DrowRectangle(this Bitmap b, RectangleF rect) //primarly debug feature
        {//TODO get rectangle relatice
            //float racioX = b.Width / sizeOfPage.Width;
            //float racioY = b.Height / sizeOfPage.Height;
            Rectangle cropRect = new((int)(rect.X * b.Width/ 595), (int)(rect.Y * b.Height / 842), (int)(rect.Width * b.Width/ 595), (int)(rect.Height * b.Height / 842));

            for (int i = 0; i <= cropRect.Width; i++)
            {
                b.SetPixel(cropRect.X+ i, cropRect.Y, Color.Yellow);
                b.SetPixel(cropRect.X+i, cropRect.Y+cropRect.Height, Color.Yellow);

            }

            for (int j = 0; j <= cropRect.Height; j++)
            {
                b.SetPixel(cropRect.X, cropRect.Y+j, Color.Yellow);
                b.SetPixel(cropRect.X + cropRect.Width, cropRect.Y+j, Color.Yellow);

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
    static class EventHandlerExtensions
    {
        //not working
        public static void RemoveAllEventHandlers(this Control b)
        {
            FieldInfo f1 = typeof(Control).GetField("EventPageClick", BindingFlags.Static | BindingFlags.NonPublic);

            object obj = f1.GetValue(b);
            PropertyInfo pi = b.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);

            EventHandlerList list = (EventHandlerList)pi.GetValue(b, null);
            list.RemoveHandler(obj, list[obj]);
        }
    }
}




