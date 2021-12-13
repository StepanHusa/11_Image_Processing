using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _11_Image_Processing
{
            //TODO 02 add more commands
    static class CommandDefinitions
    {
        public static RoutedCommand MyCommand = new RoutedCommand();

        public static void Initialize()
        {
            MyCommand.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
        }
    
    }
}
