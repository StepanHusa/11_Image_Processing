using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _11_Image_Processing
{
    public static class ST //"Static Variables
    {
        internal static string fileName;
        internal static string tempFile;
        internal static PdfLoadedDocument document;
        internal static string tempDirectoryName = Path.GetTempPath() + "Stepan_Husa_Is_A_Genius\\";
        internal static List<PointF>[] pagesList = new List<PointF>[0]; //Pages array of lists containing points


    }
}
