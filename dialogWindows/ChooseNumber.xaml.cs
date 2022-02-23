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

namespace _11_StudentTester.dialogWindows
{
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class ChooseNumber : Window
    {
        public ChooseNumber()
        {
            InitializeComponent();

            string question = "Type number of lists to scane";
            string defaultAnswer = "1";


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

        public int Answer
        {
            get
            {
                if (txtAnswer.Text != String.Empty)
                    return Int16.Parse(txtAnswer.Text);
                else return 1;
            }
        }

        private void txtAnswer_TextChanged(object sender, TextChangedEventArgs e)
        {
            short i=0;
            if (!Int16.TryParse(txtAnswer.Text,out i))
            {
                if(txtAnswer.Text!=String.Empty)
                    Dispatcher.BeginInvoke(new Action(() => txtAnswer.Undo()));
            }
                
                
        }
    }
}
