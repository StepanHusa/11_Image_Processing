using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_StudentTester
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) PropertyChanged(this, e);
        }
        private Stream docStream;
        public Stream DocumentStream
        {
            get { return docStream; }
            set
            {
                docStream = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentStream"));
            }
        }
        public ViewModel()
        {
            string tempFileName = Settings.tempFile;
            //string tempFileName = @"D:\0_DW\result_20211015.pdf";
            Debug.Write(tempFileName);
            docStream = new FileStream(tempFileName, FileMode.OpenOrCreate);
        }
    }
}
