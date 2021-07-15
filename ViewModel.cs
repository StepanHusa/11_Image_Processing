using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace _11_Image_Processing
{
    public class ViewModel : INotifyPropertyChanged
    {
        string path = @"D:\0_DW\marketa.duskova_2021-06-04_08-50-23.pdf";
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private Stream docStream;
        public Stream DocumentStream
        {
            get { return docStream; }
            set { docStream = value; OnPropertyChanged(new PropertyChangedEventArgs("DocumentStream")); }
        }


        public ViewModel()
        {

            docStream = new FileStream(path, FileMode.OpenOrCreate);
        }

    }
}
