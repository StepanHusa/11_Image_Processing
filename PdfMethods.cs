using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _11_Image_Processing
{
    public static class PdfMethods
    {
        public static PdfLoadedDocument AddSquareAt(this PdfLoadedDocument doc, int pageint, PointF position, int sizeint)
        {
            var page = doc.Pages[pageint];

            SizeF size = new(sizeint, sizeint);
            PdfSolidBrush brush = new PdfSolidBrush(Color.Red);
            RectangleF bounds = new RectangleF(position, size);
            page.Graphics.DrawRectangle(brush, bounds);

            brush.Color = Color.White;
            float w = 2;
            bounds.X += w;
            bounds.Y += w;
            bounds.Width -= 2 * w;
            bounds.Height -= 2 * w;

            page.Graphics.DrawRectangle(brush, bounds);

            return doc;
        }
        public static PdfDocument AddSquareAt(this PdfDocument doc, int pageint, PointF position, int sizeint)
        {
            var page = doc.Pages[pageint];

            SizeF size = new(sizeint, sizeint);
            PdfSolidBrush brush = new PdfSolidBrush(Color.Red);
            RectangleF bounds = new RectangleF(position, size);
            page.Graphics.DrawRectangle(brush, bounds);

            brush.Color = Color.White;
            float w = 2;
            bounds.X += w;
            bounds.Y += w;
            bounds.Width -= 2 * w;
            bounds.Height -= 2 * w;

            page.Graphics.DrawRectangle(brush, bounds);

            return doc;
        }
        public static PdfDocument DebugAddSquareAt(this PdfDocument doc, int pageint, PointF position, int sizeInt, Color innearColor, Color borderColor, float w = 2)
        {
            var page = doc.Pages[pageint];

            SizeF size = new(sizeInt, sizeInt);
            PdfSolidBrush brush = new PdfSolidBrush(borderColor);
            RectangleF bounds = new RectangleF(position, size);
            page.Graphics.DrawRectangle(brush, bounds);

            brush.Color = innearColor;
            
            bounds.X += w;
            bounds.Y += w;
            bounds.Width -= 2 * w;
            bounds.Height -= 2 * w;

            page.Graphics.DrawRectangle(brush, bounds);

            return doc;
        }



        public static void NewPdfDoc(string path)
        {
            PdfDocument doc = new();
            doc.Pages.Add();
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            //XFont font = new("Arial", 20);
            //gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            doc.Save(path);
        }

        public static void DebugCreateFile(this string path,List<PointF> positionList, List<Color> colors, int size = 20)
        {
            PdfDocument doc = new();
            doc.PageSettings.Margins.All = 0;
            PdfPage page = doc.Pages.Add();


            for (int i = 0; i < positionList.Count; i++)
                doc.DebugAddSquareAt(0, positionList[i], size,colors[i],Color.Black);


            doc.Save(path);
        }

        public static List<bool[]> RecognizeTaggedBoxes(this PdfLoadedDocument doc, string fileName, List<List<PointF>> list, int sizeInt = 20, float w = 2)
        {
            List<bool[]> output = new();
            Bitmap[] pageImages = doc.ExportAsImage(0, doc.Pages.Count - 1, 300, 300);
            SizeF size = new(sizeInt, sizeInt);

            if (list.Count > doc.Pages.Count) throw new Exception("not a correct sizes of lists (RecognizeTaggedBoxes)");


            int pgCount = -1;
            foreach (var points in list)
            {
                pgCount++;

                bool[] pageArray = new bool[points.Count];

                var s = doc.Pages[pgCount].Size; //595 842 size
                var sI = pageImages[pgCount].Size; //2479 3508 size image
                double r = sI.Height / s.Height; //4.16627 racio


                foreach (var point in points)
                {
                    pageImages[0].SetPixel((int)Math.Ceiling(point.X * r), (int)Math.Ceiling(point.Y * r), Color.Green);

                    RectangleF b = new RectangleF(point, size); //bounds
                    Rectangle I = new(); //Int Rectangle

                    b.X += w;
                    b.Y += w;
                    b.Width -= 2 * w;
                    b.Height -= 2 * w;

                    I.X = (int)Math.Round(r * b.X);
                    I.Y = (int)Math.Round(r * b.Y);
                    I.Width = (int)Math.Round(r * b.Width);
                    I.Height = (int)Math.Round(r * b.Height);


                    for (int i = I.X; i < I.X + I.Width; i++)
                        for (int j = I.Y; j < I.Y + I.Height; j++)
                        {
                            pageImages[0].SetPixel(i, j, Color.Blue);
                        }
                }

                output.Add(pageArray);
            }

            pageImages[0].Save(Path.ChangeExtension(fileName, "jpg"));

            return output;

        }
    }

}
