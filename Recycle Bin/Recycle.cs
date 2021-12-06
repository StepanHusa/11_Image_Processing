//using PdfSharp.Pdf;
//using PdfSharp.Pdf.Advanced;
//using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Image_ProcessingR
{
    static class PDFExtensions
    {
        public static PdfLoadedDocument AddSquareAt(this PdfLoadedDocument doc, int pageint, PointF position)
        {
            SizeF size = ST.sizeOfBox;

            RectangleF bounds = new RectangleF(position, size);

            doc.DrawRectangleBounds(bounds, pageint);

            return doc;
        }

        //static void ToPNG(string filename)
        //{
        //    PdfDocument document = PdfReader.Open(filename);

        //    int imageCount = 0;
        //    // Iterate pages
        //    foreach (PdfPage page in document.Pages)
        //    {
        //        // Get resources dictionary
        //        PdfDictionary resources = page.Elements.GetDictionary("/Resources");
        //        if (resources != null)
        //        {
        //            // Get external objects dictionary
        //            PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
        //            if (xObjects != null)
        //            {
        //                ICollection<PdfItem> items = xObjects.Elements.Values;
        //                // Iterate references to external objects
        //                foreach (PdfItem item in items)
        //                {
        //                    if (item is PdfReference reference)
        //                    {
        //                        // Is external object an image?
        //                        if (reference.Value is PdfDictionary xObject && xObject.Elements.GetString("/Subtype") == "/Image")
        //                        {
        //                            ExportImage(xObject, ref imageCount);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        static void ExportImage(PdfDictionary image, ref int count)
        {
            string filter = image.Elements.GetName("/Filter");
            switch (filter)
            {
                case "/DCTDecode":
                    ExportJpegImage(image, ref count);
                    break;

                case "/FlateDecode":
                    ExportAsPngImage(image, ref count);
                    break;
            }
        }
        static void ExportJpegImage(PdfDictionary image, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            byte[] stream = image.Stream.Value;
            FileStream fs = new FileStream(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();
        }
        static void ExportAsPngImage(PdfDictionary image, ref int count)
        {
            int width = image.Elements.GetInteger(PdfImage.Keys.Width);
            int height = image.Elements.GetInteger(PdfImage.Keys.Height);
            int bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);

            // TOD You can put the code here that converts vom PDF internal image format to a Windows bitmap
            // and use GDI+ to save it in PNG format.
            // It is the work of a day or two for the most important formats. Take a look at the file
            // PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
            // We don't need that feature at the moment and therefore will not implement it.
            // If you write the code for exporting images I would be pleased to publish it in a future release
            // of PDFsharp.
        }
    }

}
