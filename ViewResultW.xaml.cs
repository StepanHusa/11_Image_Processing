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

            SetupTabsOfView();
        }


        public void SetupTabsOfView()
        {
            for (int i = 0; i < ST.scansInPagesInWorks.Count; i++)
            {
                TabItem z = new();
                if (ST.namesScaned != null)
                {
                    Image wImage = new();
                    wImage.Source = ST.namesScaned[i].BitmapToImageSource();
                    wImage.Height = 20;
                    StackPanel sp = new();
                    sp.Children.Add(wImage);
                    z.Header = sp;
                }
                else z.Header = i + 1;

                var tabs = ViewWorks(i); //maybe remake for scroling (key= scr)
                //tabs.SelectionChanged += Tabs_SelectionChanged; 

                var y = ResultsList(i);

                Grid grid = new();
                grid.ColumnDefinitions.Add(new());
                grid.ColumnDefinitions.Add(new());
                grid.Children.Add(tabs);
                grid.Children.Add(y);
                Grid.SetColumn(tabs, 0);
                Grid.SetColumn(y, 1);
                z.Content = grid;
                tabsHorizontal.Items.Add(z);

            }
        }
        private TabControl ViewWorks(int i)
        {
            var tabs = new TabControl() { TabStripPlacement = Dock.Left };
            int bb = 0;
            foreach (var bitmap in ST.scansInPagesInWorks[i])
            {
                bb++;
                TabItem t = new() { Header = bb };

                Image wImage = new();
                wImage.Source = bitmap.BitmapToImageSource();
                t.Content = wImage;

                tabs.Items.Add(t);
            }
            return tabs;
        }

        private ListView ResultsList(int workindex)
        {
            var lv = (ListView)Resources["lview"];
            var list= GetListToDisplay(workindex);
            lv.ItemsSource = list;

            return lv;
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

        private List<ResultOfQuestion> GetListToDisplay(int workindex)
        {
            var list = new List<ResultOfQuestion>();
            foreach (var question in ST.boxesInQuestions)
            {
                int q = ST.boxesInQuestions.IndexOf(question);
                string correct = string.Empty;
                string checkedd = string.Empty;

                foreach (var box in question)
                    if (box.Item3)  correct += $"{question.IndexOf(box).IntToAlphabet()}, ";

                for (int i = 0; i < ST.resultsInQuestionsInWorks[workindex][q].Count; i++)
                    if(ST.resultsInQuestionsInWorks[workindex][q][i]) checkedd += $"{i.IntToAlphabet()}, ";

                list.Add(new(checkedd, correct));
            }
            return list;
        }

    }
    public class ResultOfQuestion
    {
        public string Checked { get; set; }

        public string Correct { get; set; }

        public string IsTheAnswerRight { get { if (Checked == Correct) return "Yes"; else return "No"; } }

        public ResultOfQuestion(string Checked, string Correct)
        {
            this.Checked = Checked;
            this.Correct = Correct;
        }
    }
}
