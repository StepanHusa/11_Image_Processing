using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_Image_Processing
{
    public struct ProjectInfo
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string DateLastOpened { get; set; }
        public string DateLastEdit { get; set; }
        public bool IsLocked { get; set; }

        public ProjectInfo( string name, string location, string dateLastOpened,string dateLastEdit,bool isLocked)
        {
            Location = location;
            Name = name;
            DateLastOpened = dateLastOpened;
            DateLastEdit = dateLastEdit;
            IsLocked = isLocked;
        }
    }
}
