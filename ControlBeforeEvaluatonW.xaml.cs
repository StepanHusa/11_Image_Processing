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
//using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Shapes;
using _11_Image_Processing.Resources.Strings;

namespace _11_Image_Processing
{

    
    /// <summary>
    /// Interaction logic for ControlBeforeEvaluatonW.xaml
    /// </summary>
    public partial class ControlBeforeEvaluatonW : Window
    {


        private string currentImage = null;
        public ControlBeforeEvaluatonW()
        {
            InitializeComponent();
            BuildMenu();
        }
        private void BuildMenu()
        {
            for (int i = 0; i < Settings.scanPagesInWorks.Count; i++)
         {
            var mI = new MenuItem();
                mI.Header =(i+1).ToString();
                for (int j = 0; j < Settings.scanPagesInWorks[i].Count; j++)
                {
                    var mII = new MenuItem();
                    mII.Header = Strings.ViewPage + " " + j;
                    mII.Tag = Settings.scanPagesInWorks[i][j];
                    mII.Click += MII_Click;
                    mI.Items.Add(mII);
                }
                menu.Items.Add(mI);
            }
        }

        private void MII_Click(object sender, RoutedEventArgs e)
        {
            string file = (sender as MenuItem).Tag as string;
            var f = new Bitmap(file);
            image.Source = f.BitmapToImageSource();
            f.Dispose();
            currentImage = file;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void rotate_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage!=null)
            {
                var bm =new Bitmap(currentImage);
                bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                bm.Save(currentImage);
                image.Source = bm.BitmapToImageSource();
                bm.Dispose();
            }
        }

        private void rotateAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var work in Settings.scanPagesInWorks)
            {   
                foreach (string file in work)
                {
                    var bm = new Bitmap(file);
                    bm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    bm.Save(file);
                    bm.Dispose();
                }
            }

            if (currentImage != null)
            {
                var bm = new Bitmap(currentImage);
                image.Source = bm.BitmapToImageSource();
                bm.Dispose();
            }

        }
    }
}
