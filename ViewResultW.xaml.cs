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

            //ST.scansInPagesInWorks.DrowCorrect();

            SetupTabsOfView(ST.scansInPagesInWorks);
            SetupTabsOfResults(ST.resultsInQuestionsInWorks);
        }


        public void SetupTabsOfView(List<List<System.Drawing.Bitmap>> bitmaps)
        {
            int ww = 0;
            foreach (var work in bitmaps)
            {
                ww++;
                int bb = 0;
                TabItem z = new() { Header = ww };
                var tabs = new TabControl() { TabStripPlacement = Dock.Left };
                foreach (var bitmap in work)
                {
                    bb++;
                    TabItem t = new() { Header = bb };

                    Image wImage = new();
                    wImage.Source = bitmap.BitmapToImageSource();
                    t.Content = wImage;

                    tabs.Items.Add(t);
                }

                tabs.SelectionChanged += Tabs_SelectionChanged;
                z.Content = tabs;
                tabsHorizontal.Items.Add(z);

            }
        }

        public void SetupTabsOfResults(List<List<List<bool>>> results)
        {
            if (results == null) { MessageBox.Show("null results"); return; }
            foreach (var item in results)
            {
                var t = new TabItem();
                if (ST.namesScaned != null)
                {
                    Image wImage = new();
                    wImage.Source = ST.namesScaned[results.IndexOf(item)].BitmapToImageSource();
                    wImage.Height = 20;
                    StackPanel sp = new();
                    sp.Children.Add(wImage);
                    t.Header = sp;

                }
                else t.Header = results.IndexOf(item)+1;

                resTabsVertical.Items.Add(t);
            }
        }


        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sen = sender as TabControl;
            int ii = sen.SelectedIndex;

            for (int i = 0; i < tabsHorizontal.Items.Count; i++)
            {
                var tabs = (tabsHorizontal.Items[i] as TabItem).Content as TabControl;
                if (tabs.Items.Count > ii)
                    tabs.SelectedIndex = ii;
                else tabs.SelectedIndex = tabs.Items.Count - 1; 
            }




        }

    }
}
