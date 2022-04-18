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
using _11_Image_Processing.Resources.Strings;
using System.Drawing.Drawing2D;

namespace _11_Image_Processing
{
    public class ST 
    {

        //unchanging
        internal static string appName = Strings.NameOfAplication;

        //settings
        internal static string templateProjectName = Strings.Untitled;

        internal static float baundWidth = 2;
        internal static Color baundColor = Color.Red;
        internal static Color baundColorTwo = Color.Green;
        internal static Color positionersColor = Color.Black;

        internal static string nameString = Strings.Name+":";
        internal static PdfFontFamily stringFont = PdfFontFamily.TimesRoman;
        internal static PdfFontStyle stringStyle = PdfFontStyle.Regular;

        internal static float sizeOfBoxF = 20;
        internal static float spaceBetweenBoxes =10;

        internal static float dpiExport = 600;
        internal static float dpiEvaluatePdf = 600;

        internal static float positionersWidth = (float)0.002;
        internal static float positionersMargin = (float)0.03; //relative to width!!    keep it greater then expected addup by scaning (0.03)
        internal static float positionersLegLength = (float)0.03; //relative to width (0.03)
        internal static float positionersEdgenessThresholdOld = (float)0.35;
        internal static float positionersEdgenessThreshold = (float)0.3;
        //internal static System.Windows.Thickness scanExpectedMargins = new(); //relative to page width
        //internal static System.Windows.Thickness scanExpectedMargins = new(0,0,0,0);//todo comment valek

        internal static double treshold = 0.7;

        internal static string roamingFolderName="StudentTesterLocalData";
        internal static string roamingFolder { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\"+roamingFolderName; } }
        internal static string tempDirectoryName = Path.GetTempPath() + "Stepan_Husa_Is_A_Genius";
        internal static string projectExtension = ".st0r"; //templateExtension
        internal static byte[] fileCode = "008800ff001100aa".StringToByteArray(); //8 bytes file format conformation

        //get variables
        internal static PdfPen positionersPen { get { return new PdfPen(positionersColor, positionersWidth); } }//unused
        internal static PdfPen baundPen     { get { return new PdfPen(baundColor, baundWidth); } }
        internal static PdfPen baundPenTwo  { get { return new PdfPen(baundColorTwo, baundWidth); } }
        internal static SizeF sizeOfBox     { get { return new SizeF(sizeOfBoxF, sizeOfBoxF); } set { sizeOfBoxF = value.Height; } }
        internal static int pagesOfDocument { get { return new PdfLoadedDocument(tempFile).Pages.Count; } }

        internal static List<string> tempFilesToDelete = new();

        //file specified info
        internal static string projectFileName;
        internal static string originalFile;
        internal static string tempFile;
        internal static string tempFileCopy;
        internal static List<string> versions = new();
        internal static string projectName = string.Empty;
        internal static bool IsLocked = false;






        //data variables
        internal static Tuple<int, RectangleF> nameField = null; //field made for name, date, ect.

        internal static List<Tuple<int, RectangleF>> Fields = new(); //of lists containing fields (tuple of rectangle and page index)

        //internal static List<List<Tuple<int, RectangleF,bool>>> boxesInQuestions = new();// the main listing tuple: <page index, rectangle on page, is the answer right>  (rectangle relative to page size)
        internal static List<List<Box>> boxesInQuestions = new();// (rectangle relative to page size)


        internal static List<RectangleF> positioners = null;

        internal static List<List<string>> scanPagesInWorks = new(); //outside list are the separete works and inside are pages
        internal static Matrix[][] matrixPagesInWorks = null; //outside list are the separete works and inside are pages

        //internal static List<List<List<bool>>> resultsInQuestionsInWorks; //list of results
        internal static List<ResultOfWork> allResults=null;
        //internal static List<Bitmap> namesScaned;



        //additional var

        internal static class QS
        {
            internal static int n = 4; //number of answers
            internal static float heightOfTB = 20;
            internal static float widthOfQTBs = 250;
            internal static float tab = 20;
            internal static float spaceUnderQ = 10;
            internal static float spaceBeforeBox = 50;

            internal static float indexFontSize = 10;
        }

        internal static class GradingSettings
        {
            internal static int[] GradeBottomBoarder = new int[4] { 90, 80, 70, 60 };
        }

        internal static double[,] LaplacianOWNEdgeFilter = new double[,]
        {
            { -1,-1 ,  -1 , -1 ,  -1 },
            { -1  , -1  ,-1 , -1 , -1 },
            { -1 , -1 , 24 , -1,  -1 },
            { -1  , -1  ,-1 , -1 , -1 },
            { -1, -1 ,  -1 , -1 ,  -1 }
        };

        internal static double[,] LaplFilterForPositioners = new double[,] //not used
        {
            { 1, 1 ,  1 , 1 , 1 },
            { 1  , 1  ,1 , 1 , 1 },
            { 1 , 1 , -24 , 1,  1 },
            { 1, 1 ,  1 , 1 , 1 },
            { 1  , 1  ,1 , 1 , 1 }
        };
        internal static double[,] LaplFilterForPositionersBetter = new double[,]
{
            { .2, .2 ,  .2 , .2 , .2 },
            { .2  , .2  ,.2 , .2 , .2 },
            { .2 , .2 , -4.8 , .2,  .2 },
            { .2, .2 ,  .2 , .2 , .2 },
            { .2  , .2  ,.2 , .2 , .2 },
};
        internal static double[,] LaplFilterForPositionersBetterLarger = new double[,]
{
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2, -16.2 , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },
            { .2 , .2 , .2, .2,  .2  , .2, .2, .2,  .2 },

};

        internal static double[,] LaplFilterForPositioners2 = new double[,]//not used
{
            { .1, .1 ,  .1 , .1 , .1 },
            { .1  , .1  ,.1 , .1 , .1 },
            { .1 , .1 , -2.4 , .1,  .1 },
            { .1, .1 ,  .1 , .1 , .1 },
            { .1  , .1  ,.1 , .1 , .1 }
};


    }
}
