using System;
using System.Collections.Generic;
//using System.Drawing;
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
    /// Interaction logic for ViewResultW.xaml
    /// </summary>
    public partial class ViewResultW : Window
    {
        public ViewResultW()
        {
            InitializeComponent();

            SetupTabs(ST.scansInPagesInWorks);
        }

        public void SetupTabs(List<List<System.Drawing.Bitmap>> bitmaps)
        {
            int ww = 0;
            foreach (var work in bitmaps)
            {
                ww++;
                int bb = 0;
                TabItem z = new() { Header = ww };
                var tab = new TabControl() { TabStripPlacement = Dock.Left };
                foreach (var bitmap in work)
                {
                    bb++;
                    TabItem t = new() { Header = bb };

                    Image Image = new();
                    Image.Source = bitmap.BitmapToImageSource();
                    t.Content = Image;

                    tab.Items.Add(t);
                }
                z.Content = tab;
                tabsHorizontal.Items.Add(z);

            }



        }

    }
}
