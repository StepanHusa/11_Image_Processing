using Syncfusion.Pdf.Graphics;
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
    public class ST //"Static Variables (or maybe more like settings)
    {

        //unchanging
        internal static string appName = "Application Name";

        //settings
        internal static string templateProjectName = "*Untitled";

        internal static float boundWidth = 2;
        internal static PdfPen boundPen = new(Color.Red, boundWidth);
        internal static SizeF sizeOfBox = new(20, 20);
        internal static float dpiExport = 600;
        internal static double treshold = 0.7;

        internal static string tempDirectoryName = Path.GetTempPath() + "Stepan_Husa_Is_A_Genius\\";
        internal static string ext = ".st0r"; //templateExtension
        internal static byte[] fileCode = "008800ff001100aa".StringToByteArray(); //8 bytes file format conformation

        //file specified info
        internal static string projectName = templateProjectName;
        internal static string projectFileName;
        internal static string fileName;
        internal static string tempFile;

        internal static List<string> versions = new();


        //changing variables
        //internal static bool documentIsLoaded = false;
        internal static PdfLoadedDocument document;
        internal static List<PointF>[] pagesPoints = new List<PointF>[0]; //Pages array of lists containing points
        internal static List<RectangleF>[] pagesFields = new List<RectangleF>[0]; //Pages array of lists containing fields
        internal static List<PointF[]>[] pagesQuestionsBoxes = new List<PointF[]>[0];



        internal static List<List<Tuple<int, RectangleF>>> boxesInQuestions = new();


        internal static List<Bitmap[]> setOfToEvaluate = new List<Bitmap[]>();

        //output
        //internal static List


        //additional var
        internal static int indexFontSize = 10;

        internal static class QS
        {
            internal static int n = 4; //number of answers
            internal static float heightOfTB = 20;
            internal static float widthOfQTBs = 250;
            internal static float tab = 20;
            internal static float spaceUnderQ = 10;
            internal static float spaceBtwAn = 10;
            internal static float spaceBeforeBox = 50;
        }


    }
}
