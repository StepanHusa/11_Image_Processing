using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using _11_Image_Processing.Resources.Strings;

namespace _11_Image_Processing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetLanguageDictionary();
        }

        private void SetLanguageDictionary()
        {
            System.Globalization.CultureInfo lang;
            if (LI.languageselection != null)
                lang = LI.languageselection;
            else lang = Thread.CurrentThread.CurrentCulture;

            var l = FileAndFolderExtensions.FindAvalibleLanguages();

            if (l.Contains(lang))
                Strings.Culture = lang;



        }
    }
}
