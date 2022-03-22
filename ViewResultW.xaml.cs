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
using _11_Image_Processing.Resources.Strings;


namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for ViewResultW.xaml
    /// </summary>
    public partial class ViewResultW : Window
    {
        public ViewResultW()
        {
            if (namesScaned == null & Settings.nameField != null)
            {
                namesScaned = new();
                foreach (var item in Settings.scanPagesInWorks)
                {
                    var rect = Settings.nameField.Item2;
                    var b = new System.Drawing.Bitmap(item[Settings.nameField.Item1]);
                    var mat = b.MakeTransformationMatrixFromPositioners(Settings.nameField.Item1);
                    var newRect = new System.Drawing.RectangleF(rect.Location.ApplyMatrix(mat), rect.Size.ApplyMatrix(mat));
                    namesScaned.Add(b.Crop(System.Drawing.Rectangle.Round(newRect)));
                }
            }

            InitializeComponent();

            //Settings.scansInPagesInWorks.DrowCorrect();
            SetupTabsOfView();
            SetUpAllResults();

        }
        internal static List<System.Drawing.Bitmap> namesScaned;


        public void SetupTabsOfView()
        {
            for (int i = 0; i < Settings.scanPagesInWorks.Count; i++)
            {
                TabItem z = new();
                if (namesScaned != null)
                {
                    Image wImage = new();
                    wImage.Source = namesScaned[i].BitmapToImageSource();
                    wImage.Height = 32;
                    StackPanel sp = new();
                    sp.Children.Add(wImage);
                    z.Header = sp;
                }
                else z.Header = i + 1;

                z.Content = GenerateLeftGrid(i);

                tabsHorizontal.Items.Add(z);
                //_TODO make memory suitable
            }
        }

        private Grid GenerateLeftGrid(int i)
        {
            //var imagesTabs = ViewWorks(i); 
            var imagesTabsButton = (Button)Resources["butView"];
            imagesTabsButton.Content = Strings.ViewScan;
            imagesTabsButton.Tag = i;
            imagesTabsButton.Click += ImagesTabsButton_Click;

            var table = ResultsList(i);
            var result = new StackPanel();
            var tupleTextCheckbox = new StackPanel();
            tupleTextCheckbox.Orientation = Orientation.Horizontal;
            tupleTextCheckbox.Children.Add(new Image());//TODO add evaluation of fields
            tupleTextCheckbox.Children.Add(new CheckBox());




            result.Children.Add(tupleTextCheckbox); 
            
            
            //TODO finish this section and add statistics

            Grid grid = new();
            grid.RowDefinitions.Add(new() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new());
            grid.RowDefinitions.Add(new() { Height = new GridLength(40) });
            grid.Children.Add(imagesTabsButton);
            grid.Children.Add(table);
            grid.Children.Add(result);
            Grid.SetRow(imagesTabsButton, 3);
            Grid.SetRow(table, 0);
            Grid.SetRow(result, 1);

            return grid;
        }

        private void ImagesTabsButton_Click(object sender, RoutedEventArgs e)
        {
            int i = (int)(sender as Button).Tag;
            new WorkImagesVeiwWindow(i).Show();
        }

        private ListView ResultsList(int workindex)
        {
            var lv = (ListView)Resources["lview"];
            var list = GetListToDisplay(workindex);
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
            int ii = 0;
            foreach (var question in Settings.boxesInQuestions)
            {
                ii++;
                int q = Settings.boxesInQuestions.IndexOf(question);
                string correct = string.Empty;
                string checkedd = string.Empty;

                foreach (var box in question)
                    if (box.Item3) correct += $"{question.IndexOf(box).IntToAlphabet()}, ";

                for (int i = 0; i < Settings.resultsInQuestionsInWorks[workindex][q].Count; i++)
                    if (Settings.resultsInQuestionsInWorks[workindex][q][i]) checkedd += $"{i.IntToAlphabet()}, ";

                list.Add(new(checkedd, correct, ii));
            }
            return list;
        }




        private void SetUpAllResults()
        {

            var s = GetListOfAll();
            allResults.ItemsSource = s;

            int height = 40; //same as allResults listview height of row

            foreach (var item in s)
            {
                if (item.Name != null)
                {
                    Image x = item.Name;
                    x.Height = height;
                    namesPanel.Children.Add(x);
                }
                else namesPanel.Children.Add(new TextBlock() {Text=item.Index.ToString() });
            }
        }

        private List<ResultOfAllOne> GetListOfAll()
        {
            var list = new List<ResultOfAllOne>();
            for (int i = 0; i < Settings.resultsInQuestionsInWorks.Count; i++)
            {
                var li = GetListToDisplay(i);

                int right = 0;
                foreach (var item in li)
                    if (item.CorrectBool)
                        right++;
                ResultOfAllOne r;
                if (namesScaned != null)
                    if (namesScaned.Count > i)
                        r = new(i + 1, right, li.Count, namesScaned[i]);
                    else r = new(i + 1, right, li.Count);
                else r = new(i + 1, right, li.Count);

                list.Add(r);
            }


            return list;
        }


    }


}
