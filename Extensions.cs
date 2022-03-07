using _11_Image_Processing.Resources.Strings;
using ImageProcessor.Imaging.Filters.EdgeDetection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _11_Image_Processing
{
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
        public static List<List<Tuple<int, RectangleF, bool>>> ByteArrayToIntRectangleFBoolTupleListList(this byte[] ba)
        {
            List<List<Tuple<int, RectangleF, bool>>> lists;
            using (var ms = new MemoryStream(ba))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfList = r.ReadInt32();
                    lists = new();
                    for (int i = 0; i < lengthOfList; i++)
                    {
                        int Count = r.ReadInt32();
                        lists.Add(new());
                        for (int j = 0; j < Count; j++)
                        {
                            lists[i].Add(new Tuple<int, RectangleF, bool>(r.ReadInt32(), new RectangleF(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()), r.ReadBoolean()));
                        }
                    }
                }
            }
            return lists;

        }
        public static byte[] RectangleListToByteArray(this List<Tuple<int, RectangleF>> list)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(list.Count);
                    foreach (var l in list)
                    {
                        bw.Write(l.Item1);
                        var p = l.Item2;
                        bw.Write(p.Location.X);
                        bw.Write(p.Location.Y);
                        bw.Write(p.Size.Width);
                        bw.Write(p.Size.Height);
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static List<Tuple<int, RectangleF>> ByteArrayToRectangleFTupleList(this byte[] bArray)
        {
            List<Tuple<int, RectangleF>> list = new();
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    int lengthOfList = r.ReadInt32();
                    for (int i = 0; i != lengthOfList; i++)
                    {
                        list.Add(new(r.ReadInt32(), new RectangleF(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle())));
                    }
                }
            }
            return list;
        }
        public static byte[] RectangleTupleToByteArray(this Tuple<int, RectangleF> tup)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    if (tup == null)
                        bw.Write(false);
                    else
                    {
                        bw.Write(true);
                        bw.Write(tup.Item1);
                        var p = tup.Item2;
                        bw.Write(p.Location.X);
                        bw.Write(p.Location.Y);
                        bw.Write(p.Size.Width);
                        bw.Write(p.Size.Height);
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static byte[] RectangleFListToByteArray(this List<RectangleF> list)
        {

            byte[] data;

            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    if (list == null)
                        bw.Write(true); //is null
                    else
                    {
                        bw.Write(false);
                        bw.Write(list.Count);
                        foreach (var rect in list)
                        {
                            bw.Write(rect.X);
                            bw.Write(rect.Y);
                            bw.Write(rect.Width);
                            bw.Write(rect.Height);

                        }
                    }
                }
                data = ms.ToArray();
            }

            return data;
        }
        public static List<RectangleF> ByteArrayToRectangleFList(this byte[] bArray)
        {
            List<RectangleF> list;
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    bool isNull = r.ReadBoolean();
                    if (isNull) return null;

                    int lengthOfList = r.ReadInt32();
                    list = new();
                    for (int i = 0; i != lengthOfList; i++)
                    {
                        int Count = r.ReadInt32();
                        list.Add(new(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()));
                    }
                }
            }
            return list;
        }

        public static Tuple<int, RectangleF> ByteArrayToRectangleFTuple(this byte[] bArray)
        {
            using (var ms = new MemoryStream(bArray))
            {
                using (var r = new BinaryReader(ms))
                {
                    if (r.ReadBoolean() == false)
                        return null;

                    else
                        return new(r.ReadInt32(), new RectangleF(r.ReadSingle(), r.ReadSingle(), r.ReadSingle(), r.ReadSingle()));
                }
            }
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
            using (var sw = new StringWriter())
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
            string s = Strings.Allfiles + "(*.*) | *.*";
            foreach (var ext in extensionsWithoutDot)
            {
                s += $"|(*.{ext})|*.{ext}";

            }
            return s;
        }
        public static string ToOFDFilter(this string[] extensionsWithoutDot, string[] comments)
        {
            string s = Strings.Allfiles + "(*.*) | *.*";
            Array.Resize(ref comments, extensionsWithoutDot.Length);
            for (int i = 0; i < extensionsWithoutDot.Length; i++)
            {

                s += $"|{comments[i]}(*.{extensionsWithoutDot[i]})|*.{extensionsWithoutDot[i]}";

            }
            return s;
        }
        public static char IntToAlphabet(this int i)
        {
            return Convert.ToChar(i + (int)'a');
        }

    }
    static class RectangleExtensions
    {
        public static Rectangle EvaluateInPositiveSizeOld(this Rectangle rect)
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
        public static Rectangle EvaluateInPositiveSize(this ref Rectangle rect)
        {
            if (rect.Size.Width < 0)
            {
                rect.X += rect.Size.Width;
                rect.Width = -rect.Size.Width;
            }

            if (rect.Size.Height < 0)
            {
                rect.Y += rect.Size.Height;
                rect.Height = -rect.Size.Height;
            }
            return rect;
        }
        public static RectangleF EvaluateInPositiveSizeOld(this RectangleF rect)
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
        public static RectangleF EvaluateInPositiveSize(this ref RectangleF rect)
        {
            if (rect.Size.Width < 0)
            {
                rect.X += rect.Size.Width;
                rect.Width = -rect.Size.Width;
            }

            if (rect.Size.Height < 0)
            {
                rect.Y += rect.Size.Height;
                rect.Height = -rect.Size.Height;
            }
            return rect;
        }
        public static Rectangle FromFourPoints(int x, int y, int xw, int yh)
        {
            return new(x, y, xw - x, yh - y);
        }
        public static RectangleF FromFourPoints(float x, float y, float xw, float yh)
        {
            return new(x, y, xw - x, yh - y);
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
    static class MathExtensions
    {
        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        public static Tuple<Line, double> LinearRegressionOld( this Point[] points)
        {
            int n = points.Length;
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < n; i++)
            {
                var x = points[i].X;
                var y = points[i].Y;
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            //var ssY = sumOfYSq - ((sumOfY * sumOfY) / n);

            var ssX = n * sumOfXSq - sumOfX * sumOfX;
            double a = (n * sumCodeviates - sumOfX * sumOfY) / ssX;
            double b = (sumOfXSq * sumOfY - sumOfX * sumCodeviates) / ssX;

            bool invertedXY=false;
            if (ssX==0)
            {
               a=double.PositiveInfinity;
                var ssY = n*sumOfYSq - sumOfY * sumOfY;
                b = points[0].X;
            }

            //var dblR = rNumerator / Math.Sqrt(rDenom);
            var rNumerator = (n * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (n * sumOfXSq - (sumOfX * sumOfX)) * (n * sumOfYSq - (sumOfY * sumOfY));
            double rSquared = rNumerator*rNumerator/rDenom;

            return new Tuple<Line,double>(new Line(a, b,0), rSquared);
        }

        public static PointF CrossectionOfTwoLines(this Line line1,Line line2)
        {
            double Det = line1.a * line2.b - line1.b * line2.a;

            double detA = line1.b * line2.c - line1.c * line2.b;
            double detB = line1.c * line2.a - line1.a * line2.c;

            return new PointF((float)(detA / Det),(float) (detB / Det));


        }

        public static Line LinearRegression(this List<Point> points)
        {
            //a*x+b*y+c=0
            double a; //coefficience of x
            double b; //coefficience of y
            double c; //constant

            int n = points.Count;
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double sumCodeviates = 0;

            for (var i = 0; i < n; i++)
            {
                var x = points[i].X;
                var y = points[i].Y;
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            //var ssY = sumOfYSq - ((sumOfY * sumOfY) / n);

            var ssX = n * sumOfXSq - sumOfX * sumOfX;
            var ssY = n * sumOfYSq - sumOfY * sumOfY;
            if (ssX == 0)
            {
                a = 1;
                b = 0;
                c = -points[0].X;

            }
            else if (ssY == 0)
            {
                a = 0;
                b = 1;
                c = -points[0].Y;
            }
            else
            {
                a = (n * sumCodeviates - sumOfX * sumOfY) / ssX;
                b = -1;
                c = (sumOfXSq * sumOfY - sumOfX * sumCodeviates) / ssX;

            }

            //var dblR = rNumerator / Math.Sqrt(rDenom);
            //var rNumerator = (n * sumCodeviates) - (sumOfX * sumOfY);
            //var rDenom = (n * sumOfXSq - (sumOfX * sumOfX)) * (n * sumOfYSq - (sumOfY * sumOfY));
            //double rSquared = rNumerator * rNumerator / rDenom;


            return new(a, b, c);
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

        public static Bitmap CorpRelativeToImage(this Bitmap orig, RectangleF relativeRect)
        {
            Rectangle cropRect = relativeRect.UnrelatitivizeToImage(orig);
            Size size = new(cropRect.Width, cropRect.Height);

            Bitmap crop = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(orig, new Rectangle(0, 0, crop.Width, crop.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            return crop;
        }
        public static Bitmap Corp(this Bitmap orig, Rectangle rect)
        {
            Size size = new(rect.Width, rect.Height);

            Bitmap crop = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(crop))
            {
                g.DrawImage(orig, new Rectangle(0, 0, crop.Width, crop.Height),
                                 rect,
                                 GraphicsUnit.Pixel);
            }
            return crop;
        }


    }
    static class WindowsMediaExtensions
    {
        public static System.Windows.Media.Color ColorToDrawing(this Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
    }
}
