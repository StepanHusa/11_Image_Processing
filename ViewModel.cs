using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Image_Processing
{
    public class ViewModel : INotifyPropertyChanged
    {
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
            set
            {
                docStream = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DocumentStream"));
            }

        }
        public ViewModel()
        {
            string fileName= "D:\\0_DW\\31006634.pdf";
            docStream = new FileStream(fileName, FileMode.Open);
        }
    }
}
