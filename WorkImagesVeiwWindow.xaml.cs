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
    /// Interaction logic for WorkImagesVeiwWindow.xaml
    /// </summary>
    public partial class WorkImagesVeiwWindow : Window
    {
        public WorkImagesVeiwWindow(int indexOfWork)
        {
            InitializeComponent();
            grid.Children.Add(ViewWorks(indexOfWork));
        }

        private TabControl ViewWorks(int i)//TODO maybe remake for scroling (key= scr)
        {
            var tabs = new TabControl() { TabStripPlacement = Dock.Left };
            int bb = 0;
            foreach (var imageFile in Settings.scanPagesInWorks[i])
            {
                bb++;
                TabItem t = new() { Header = bb };

                Image wImage = new();
                var bi = new BitmapImage(new Uri(imageFile));
                bi.CacheOption = BitmapCacheOption.OnLoad;
                wImage.Source = bi;
                t.Content = wImage;

                tabs.Items.Add(t);
            }
            return tabs;
        }
    }
}
