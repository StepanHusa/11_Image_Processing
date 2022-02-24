using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _11_Image_Processing.Resources.Strings;


namespace _11_Image_Processing
{
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
        public Image Name { get; set; }

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

            Image wImage = new();
            wImage.Source = bitmap.BitmapToImageSource();
            this.Name = wImage;
        }

    }

    internal class Box //unused
    {
        public int Page { get; set; }

        public System.Drawing.RectangleF Rectangle { get; set; }

        public bool Correct { get; set; }
    }
    internal class Field //unused
    {
        public int Page { get; set; }

        public System.Drawing.RectangleF Rectangle { get; set; }
    }
}
