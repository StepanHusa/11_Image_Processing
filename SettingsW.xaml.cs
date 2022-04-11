using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        private string languagefile= ST.Language;
        private string language = Strings.THIS_LANGUAGE;
        private List<string> evalibleLnguages=new();
        private string tempFolder = ST.tempDirectoryName;
        private string tempProjectName = ST.templateProjectName;
        private string projectExtension = ST.projectExtension;
        private byte[] fileCode = ST.fileCode;

        private string nameString = ST.nameString;
        private Syncfusion.Pdf.Graphics.PdfFontFamily font = ST.stringFont;



        //edit
        private int nOfBoxes = ST.QS.n;
        private float sizeOfBoxF = ST.sizeOfBoxF;
        private float spaceBetweenBoxes = ST.spaceBetweenBoxes;
        private float baundWidth = ST.baundWidth;

        private System.Drawing.Color baundColor;
        private System.Drawing.Color baundColorTwo;


        //more
        private float dpiExport = ST.dpiExport;
        private float dpiEvaluatePdf = ST.dpiEvaluatePdf;


        public SettingsW()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            //general

            tempfolder.Text = tempFolder;
            tempprojectname.Text = tempProjectName;
            projectextension.Text = projectExtension;
            filecode.Text = fileCode.ByteArrayToString();

            namestring.Text = nameString;
            fonts.Items.Clear();
            fonts.Items.Add("Helvetica");//0
            fonts.Items.Add("Courier");
            fonts.Items.Add("TimesRoman");
            fonts.Items.Add("Symbol");
            fonts.Items.Add("ZapfDingbats");//4

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

            //more
            exportdpi.Text = dpiExport.ToString();
            evaluatedpi.Text = dpiEvaluatePdf.ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
            this.DialogResult = true;
            this.Close();
        }

        private void ApplyToGlobalSettings()
        {

        }

        private void color1_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog cd = new();
            if (cd.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            baundColor = cd.Color;
            color1.Background= new SolidColorBrush(baundColor.ColorFromDrawing());
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

        }
    }
}

