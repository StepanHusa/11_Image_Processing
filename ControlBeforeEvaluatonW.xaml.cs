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
using _11_Image_Processing.Resources.Strings;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for ControlBeforeEvaluatonW.xaml
    /// </summary>
    public partial class ControlBeforeEvaluatonW : Window
    {
        public ControlBeforeEvaluatonW()
        {
            InitializeComponent();
            BuildMenu();
        }
        private void BuildMenu()
        {
            foreach (var work in Settings.scansInPagesInWorks)
            {
            var mI = new MenuItem();
                mI.Header = Settings.scansInPagesInWorks.IndexOf(work);
                foreach (string page in work)
                {
                    var mII = new MenuItem();
                    mII.Header = Strings.ViewPage + " " + work.IndexOf(page);
                    //mII.Click += ();
                    mI.Items.Add(mII);
                }
                menu.Items.Add(mI);
            }





        }
    }
}
