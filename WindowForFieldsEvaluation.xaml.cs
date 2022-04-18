using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for WindowForFieldsEvaluation.xaml
    /// </summary>
    public partial class WindowForFieldsEvaluation : Window
    {
        private int work;
        private int question;


        public WindowForFieldsEvaluation(int indexofwork)
        {
            InitializeComponent();
            SetupGrid();
            work = indexofwork;
            question = 0;
            GetImageToDisplay();




        }

        private void SetupGrid()
        {

            for (int i = 0; i < ST.scanPagesInWorks.Count; i++)
            {
                long j = i;
                Button but = new();
                but.Click += (sen,e) => { work = (int)j; GetImageToDisplay(); };

                if (ST.nameField != null)
                {

                    var rect = ST.nameField.Item2;
                    var b = new System.Drawing.Bitmap(ST.scanPagesInWorks[i][ST.nameField.Item1]);
                    var mat = ST.matrixPagesInWorks[i][ST.nameField.Item1];
                    var newRect = new System.Drawing.RectangleF(rect.Location.ApplyMatrix(mat), rect.Size.ApplyMatrix(mat));
                    but.Content = new Image() { Source = b.Crop(System.Drawing.Rectangle.Round(newRect)).BitmapToImageSource() };
                }
                else but.Content = new TextBlock() { Height = 40, Text = (i + 1).ToString() };
                names.Children.Add(but);

            }


            for (int i = 0; i < ST.Fields.Count; i++)
            {
                long j = i;
                Button but = new();
                but.Click += (sen, e) => { question = (int)j; GetImageToDisplay(); };

                but.Content = new TextBlock() { Height = 40, Text = (i + 1).ToString() };
                questions.Children.Add(but);
            }


        }


        private void GetImageToDisplay()
        {

            var rect = ST.Fields[question].Item2;
            var b = new System.Drawing.Bitmap(ST.scanPagesInWorks[work][ST.Fields[question].Item1]);
            var mat = ST.matrixPagesInWorks[work][ST.Fields[question].Item1];
            var newRect = new System.Drawing.RectangleF(rect.Location.ApplyMatrix(mat), rect.Size.ApplyMatrix(mat));
            aQuestion.Source = b.Crop(System.Drawing.Rectangle.Round(newRect)).BitmapToImageSource();

            var ba = ST.allResults[work].FieldsBinary[question];
            if (ba.HasValue)
            {
                isWrong.IsChecked = !ba.Value;
                isRight.IsChecked = ba.Value;
            }
            else
            {
                isWrong.IsChecked = false;
                isRight.IsChecked = false;
            }

            foreach (Button chil in names.Children)
            {
                chil.BorderBrush =new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                chil.BorderThickness = new Thickness(1); 
            }

            var nam = names.Children[work] as Button;
            nam.BorderBrush =new SolidColorBrush(Color.FromArgb(255, 150, 255, 200));
            nam.BorderThickness = new Thickness(4);

            foreach (Button chil in questions.Children)
            {
                chil.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                chil.BorderThickness = new Thickness(1);
            }

            var que = questions.Children[question] as Button;
            que.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 150, 255, 200));
            que.BorderThickness = new Thickness(4);

        }

        private void AnswerClick(object sender, RoutedEventArgs e)
        {
            if (sender == isRight)
            {
                isWrong.IsChecked = false;
            }
            if (sender == isWrong)
            {
                isRight.IsChecked = false;
            }
            StoreValue();
            if (nextCheckbox.IsChecked.Value && (sender as CheckBox).IsChecked.Value)
            {
                System.Threading.Thread.Sleep(500);
                next_Click(null, null);
            }

        }
        private void Answer_Checked(object sender, RoutedEventArgs e)
        {

            if(sender==isRight)
            {
                isWrong.IsChecked = false;
            }
            if (sender == isWrong)
            {
                isRight.IsChecked = false;
            }
            StoreValue();
            if (nextCheckbox.IsChecked.Value)
            {
                next_Click(null, null);
            }
        }
        private void Answer_UnChecked(object sender, RoutedEventArgs e)
        {
            StoreValue();
        }

        private void StoreValue()
        {
            if (!isRight.IsChecked.Value && !isWrong.IsChecked.Value)
                ST.allResults[work].FieldsBinary[question] = new();
            if (isRight.IsChecked.Value)
                ST.allResults[work].FieldsBinary[question] = new(true); 
            if (isWrong.IsChecked.Value)
                ST.allResults[work].FieldsBinary[question] = new(false);
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (questionCheckbox.IsChecked.Value)
            {
                question++;
                if (question >= ST.Fields.Count)
                {
                    question = 0;
                    work++;
                    if (work >= ST.scanPagesInWorks.Count) work = 0;
                }
            }
            else
            {
                work++;
                if (work >= ST.scanPagesInWorks.Count)
                {
                    work = 0;
                    question++;
                    if (question >= ST.Fields.Count) question = 0;
                }

            }

            GetImageToDisplay();

        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (questionCheckbox.IsChecked.Value)
            {
                question--;
                if (question <0)
                {
                    question = ST.Fields.Count - 1;
                    work--;
                    if (work < 0) work = ST.scanPagesInWorks.Count-1;
                }
            }
            else
            {
                work--;
                if (work <0)
                {
                    work = ST.scanPagesInWorks.Count-1;
                    question--;
                    if (question <0) question = ST.Fields.Count-1;
                }

            }
            GetImageToDisplay();
        }


        private void questionCheckbox_Click(object sender, RoutedEventArgs e)
        {
            if (sender == questionCheckbox)
            {
                workCheckbox.IsChecked = !questionCheckbox.IsChecked.Value;
            }
            if (sender == workCheckbox)
            {
                questionCheckbox.IsChecked = !workCheckbox.IsChecked.Value;
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
