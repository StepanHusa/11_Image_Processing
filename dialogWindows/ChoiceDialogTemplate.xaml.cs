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

namespace _11_Image_Processing.dialogWindows
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class ChoiceDialogTemplate : Window
    {
        public ChoiceDialogTemplate()
        {
            // TODO: rewrite this to chice template, save it and implement into mainWindow.Read_Click

            InitializeComponent();

            string question = "question";
            string defaultAnswer = "answer";


            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
    }
}
