using System;
using System.Collections.Generic;
//using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_Image_Processing.Resources.Strings;
using System.Drawing;
using ImageProcessor.Imaging;

namespace _11_Image_Processing
{
    public enum DisplaiedWindow
    {
        Main,
        Edit,
        Results,
    }

    public class ResultOfQuestion
    {
        public int Number { get; set; }

        public string Checked { get; set; }

        public string RightAnswer { get; set; }

        public bool CorrectBool { get { return Checked == RightAnswer; } }

        public string Correct { get { if (Checked == RightAnswer) return Strings.Yes; else return Strings.No; } }

        public ResultOfQuestion(string Checked, string RightAnswer, int Number)
        {
            this.Checked = Checked;
            this.RightAnswer = RightAnswer;
            this.Number = Number;
        }
    }


    internal class ResultOfAllOne
    {
        public System.Windows.Controls.Image Name { get; set; }

        public int Index { get; set; } //from One so it can be binded to dispay 

        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public int Percents { get { return (int)Math.Round((double)((double)Points / MaxPoints * 100)); } }

        public int Grade
        {
            get
            {
                for (int i = 0; i < Settings.GradingSettings.GradeBottomBoarder.Length; i++)
                {
                    if (Percents >= Settings.GradingSettings.GradeBottomBoarder[i]) return i;
                }
                return 5;
            }
        }

        public ResultOfAllOne(int Index,int Points, int MaxPoints)
        {
            this.Index = Index;
            this.Points = Points;
            this.MaxPoints = MaxPoints;
        }
        public ResultOfAllOne(int Index, int Points, int MaxPoints, System.Drawing.Bitmap bitmap)
        {
            this.Index = Index;
            this.Points = Points;
            this.MaxPoints = MaxPoints;

            System.Windows.Controls.Image wImage = new();
            wImage.Source = bitmap.BitmapToImageSource();
            this.Name = wImage;
        }

    }

    public struct Box 
    {
        public int Page { get; set; }

        public System.Drawing.RectangleF Rectangle { get; set; }

        public float BoundWidth { get; set; }

        public bool IsCorrect { get; set; }

        public Box(int page, RectangleF rectangle, float boundWidth,bool isCorrect)
        {
            Page = page;
            Rectangle = rectangle;
            BoundWidth = boundWidth;
            IsCorrect = isCorrect;
        }
    }
    internal class Field //unused
    {
        public int Page { get; set; }

        public float BoundWidth { get; set; }

        public System.Drawing.RectangleF Rectangle { get; set; }
    }


    public class QuadrilateralF
    {
        public PointF p1 { get; set; }
        public PointF p2 { get; set; }
        public PointF p3 { get; set; }
        public PointF p4 { get; set; }

        public QuadrilateralF()
        {
        }
        public QuadrilateralF(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            this.p1 = point1;
            this.p2 = point2;
            this.p3 = point3;
            this.p4 = point4;

        }
    }
    /// <summary>
    /// line of ax+by+c=0
    /// </summary>
    public struct Line
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }

        public Line(double a,double b,double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }

    public class AplicationSettingsVariable
    {
        //general
        public string Language { get; set; }
        public string TempFolder { get; set; }
        public string TemplateProjectName { get; set; }
        public string ProjectExtension { get; set; }
        public string FileCode { get; set; }
        public string NameString { get; set; }
        public Syncfusion.Pdf.Graphics.PdfFontFamily StringFont = Syncfusion.Pdf.Graphics.PdfFontFamily.TimesRoman;
        public Syncfusion.Pdf.Graphics.PdfFontStyle stringStyle = Syncfusion.Pdf.Graphics.PdfFontStyle.Regular;

        //Edit
        public Color BaundColor { get; set; }
        public Color baundColorTwo { get; set; }
        public float SizeOfBox { get; set; }
        public float BoundWidth { get; set; }
        public int NumberOfBoxes { get; set; }

    }

    public class BitmapInGrayscale255
    {
        private byte[,] _pixels;
        public BitmapInGrayscale255(Bitmap bitmap)
        {
            _pixels = new byte[bitmap.Width, bitmap.Height];
            int h = bitmap.Height;

            using (var fbmp = new FastBitmap(bitmap))
            {
                //for (int i = 0; i < bitmap.Width; i++)
                //    for (int j = 0; j < bitmap.Height; j++)
                //    {
                //        _pixels[i, j] = fbmp.GetPixel(i, j).ColorToGrayscale();
                //    }
                Parallel.For(0, bitmap.Width, (i) => {
                    for (int j = 0; j < h; j++)
                        _pixels[i, j] = fbmp.GetPixel(i, j).ColorToGrayscale();
                });
            }
        }
        public byte this[int x,int y]
        {
            get
            {
                return _pixels[x,y];
            }

            set
            {
                _pixels[x, y] = value;
            }
        }
    }











}
