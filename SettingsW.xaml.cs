using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using _11_Image_Processing.Resources.Strings;


namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for SettingsW.xaml
    /// </summary>
    public partial class SettingsW : Window
    {
        //general
        private List<System.Globalization.CultureInfo> avalibleLnguages = FileAndFolderExtensions.FindAvalibleLanguages();
        private string tempFolder = ST.tempDirectoryName;
        private string tempProjectName = ST.templateProjectName;
        //private string projectExtension = ST.projectExtension;
        //private byte[] fileCode = ST.fileCode;

        private string nameString = ST.nameString;
        private Syncfusion.Pdf.Graphics.PdfFontFamily font = ST.stringFont;



        //edit
        private int nOfBoxes = ST.QS.n;
        private float sizeOfBoxF = ST.sizeOfBoxF;
        private float spaceBetweenBoxes = ST.spaceBetweenBoxes;
        private float baundWidth = ST.baundWidth;

        private System.Drawing.Color baundColor =ST.baundColor;
        private System.Drawing.Color baundColorTwo=ST.baundColorTwo;

        //locators
        private System.Drawing.Color positionersColor = ST.positionersColor;
        private float positionersWidth = ST.positionersWidth;
        private float positionersMargin = ST.positionersMargin;
        private float positionersLegLength = ST.positionersLegLength;


        //more
        private float dpiExport = ST.dpiExport;
        private float dpiEvaluatePdf = ST.dpiEvaluatePdf;

        private bool evaluateSafely = ST.evaluateSafely;

        public SettingsW()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            //general
            languageCB.Items.Add(new ComboBoxItem() { Content = "Default"});
            for (int i = 1; i < avalibleLnguages.Count; i++)
            {
                ComboBoxItem it = new() { Content=avalibleLnguages[i].ToString()};
                languageCB.Items.Add(it);
            }
            for (int i = 0; i < avalibleLnguages.Count; i++)
            {
                if (avalibleLnguages[i] == Strings.Culture)
                    languageCB.SelectedIndex = i;
            }


            tempfolder.Text = tempFolder;
            tempprojectname.Text = tempProjectName;
            //projectextension.Text = projectExtension;
            //filecode.Text = fileCode.ByteArrayToString();

            namestring.Text = nameString;
            fonts.Items.Clear();
            fonts.Items.Add("Helvetica");//0
            fonts.Items.Add("Courier");
            fonts.Items.Add("TimesRoman");
            fonts.Items.Add("Symbol");
            fonts.Items.Add("ZapfDingbats");//4
            fonts.SelectedIndex = (int)font;

            //foreach (int i in Enum.GetValues(typeof(Syncfusion.Pdf.Graphics.PdfFontFamily)))
            //{
            //    fonts.Items.Add(i);
            //}  


            //edit
            color1.Background = new SolidColorBrush(ST.baundColor.ColorFromDrawing());
            color2.Background = new SolidColorBrush(ST.baundColorTwo.ColorFromDrawing());
            size.Text = sizeOfBoxF.ToString();
            width.Text = baundWidth.ToString();
            numberOfBoxes.Text = nOfBoxes.ToString();
            between.Text = spaceBetweenBoxes.ToString();

            //locators
            colorPosits.Background = new SolidColorBrush(ST.positionersColor.ColorFromDrawing());
            boundWithPositioners.Text = positionersWidth.ToString();
            positionersmargin.Text = positionersMargin.ToString();
            positionerslegLength.Text= positionersLegLength.ToString();

            //more
            //exportdpi.Text = dpiExport.ToString();
            //evaluatedpi.Text = dpiEvaluatePdf.ToString();
            //advanced
            safeevaluationcheckbox.IsChecked = evaluateSafely;
        }


        private void IfEnterMoveFocus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) {                
                //(sender as TextBox).MoveFocus(new TraversalRequest(0)); e.Handled = true;
            }
        }

        private void ag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            if (!(e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                e.Handled = true;
            }



        }

        private void ag_LostFocus(object sender, RoutedEventArgs e)
        {
            //while (ag.Text.Length < 8) ag.Text += "0";
            //if (ag.Text.Length != 8) MessageBox.Show("invalid number");
            //else
            //{
            //    Settings.fileCode = ag.Text.StringToByteArray();
            //}
            
        }

        private void btnDialogApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyToGlobalSettings();
            this.DialogResult = true;
            this.Close();
        }

        private void ApplyToGlobalSettings()
        {
            bool restart = false;
            //general
            int il = languageCB.SelectedIndex;
            if (il >= 0 && avalibleLnguages[il] != Strings.Culture)
            {
                LI.languageselection = avalibleLnguages[il];
                restart = true;
            }
            bool moveTemp = false;
            if (Directory.Exists(tempfolder.Text))
                moveTemp = true;
            else { try { Directory.CreateDirectory(tempfolder.Text); moveTemp=true; } catch { MessageBox.Show("tempfolder isn't valid"); } }
            if(tempprojectname.Text!="")
                tempProjectName = tempprojectname.Text;
            else MessageBox.Show("default project name isn't valid");

            if (moveTemp)
            {
                tempFolder = tempfolder.Text;
                var files = Directory.GetFiles(ST.tempDirectoryName);
                foreach (var file in files)
                {
                    try
                    {
                        string newfile = tempFolder + "\\" + System.IO.Path.GetFileName(file);
                        File.Move(file, newfile);
                    }
                    catch { }
                }
                ST.tempDirectoryName = tempFolder;
            }
            ST.templateProjectName = tempProjectName;
            //ST.projectExtension = projectExtension;
            //ST.fileCode = fileCode;


            nameString = namestring.Text;
            font=(Syncfusion.Pdf.Graphics.PdfFontFamily) fonts.SelectedIndex;

            ST.nameString = nameString;
            ST.stringFont = font;



            //edit
            try
            {
                int n = Int32.Parse(numberOfBoxes.Text);
                nOfBoxes = n;
            }
            catch { MessageBox.Show("number of boxes isn't valid"); }
            try
            {
                double f = double.Parse(size.Text);
                sizeOfBoxF = (float)f;
            }
            catch { MessageBox.Show("size of boxes isn't valid"); }
            try
            {
                double f = double.Parse(between.Text);
                spaceBetweenBoxes = (float)f;
            }
            catch { MessageBox.Show("space between boxes isn't valid"); }
            try
            {
                double f = double.Parse(width.Text);
                baundWidth = (float)f;
            }
            catch { MessageBox.Show("baund width isn't valid"); }
            ST.QS.n = nOfBoxes;
            ST.sizeOfBoxF = sizeOfBoxF;
            ST.spaceBetweenBoxes = spaceBetweenBoxes;
            ST.baundWidth = baundWidth;

            ST.baundColor = baundColor;
            ST.baundColorTwo = baundColorTwo;

            //locators
            ST.positionersColor = positionersColor;
            try
            {
                double f = double.Parse(boundWithPositioners.Text);
                positionersWidth = (float)f;
            }
            catch { MessageBox.Show("positioners width of boxes isn't valid"); }
            try
            {
                double f = double.Parse(positionersmargin.Text);
                positionersMargin = (float)f;
            }
            catch { MessageBox.Show("positioners margin between boxes isn't valid"); }
            try
            {
                double f = double.Parse(positionerslegLength.Text);
                positionersLegLength = (float)f;
            }
            catch { MessageBox.Show("positioners Leg Length margin between boxes isn't valid"); }

            ST.positionersWidth = positionersWidth;
            ST.positionersMargin = positionersMargin;
            ST.positionersLegLength = positionersLegLength;
            //more
            //ST.dpiExport = dpiExport;
            //ST.dpiEvaluatePdf = dpiEvaluatePdf;
            //advanced
            evaluateSafely = safeevaluationcheckbox.IsChecked.Value;
            ST.evaluateSafely = evaluateSafely;

            if (restart)
                MessageBox.Show("Some changes will take effect after restart.");
        }

        private void color1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            baundColor = cd.Color;
            color1.Background = new SolidColorBrush(baundColor.ColorFromDrawing());
        }

        private void color2_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            baundColorTwo = cd.Color;
            color2.Background = new SolidColorBrush(baundColorTwo.ColorFromDrawing());

        }


        private void AddLanguageButton(object sender, RoutedEventArgs e)
        {
            var open = new System.Windows.Forms.OpenFileDialog();
            if (open.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            //todo finish
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var open = new System.Windows.Forms.FolderBrowserDialog() { SelectedPath=ST.tempDirectoryName };
            if (open.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            tempFolder = open.SelectedPath;
            tempfolder.Text = tempFolder;
        }

        private void colorLoc_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            positionersColor = cd.Color;
            colorPosits.Background = new SolidColorBrush(baundColorTwo.ColorFromDrawing());

        }
    }
}

