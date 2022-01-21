using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _11_Image_Processing.Resources;

namespace _11_Image_Processing
{
    public static class Commands
    {
        public static readonly RoutedUICommand NameField = new RoutedUICommand
    (
        Strings.Name_Field,
        "NameField",
        typeof(Commands),
        new InputGestureCollection()
        {
                    new KeyGesture(Key.N, ModifierKeys.Control)
        }
    );
        public static readonly RoutedUICommand AnswerBoxes = new RoutedUICommand
(
Strings.Answer_Boxes,
"AnswerBoxes",
typeof(Commands),
new InputGestureCollection()
{
                    new KeyGesture(Key.B, ModifierKeys.Control)
}
);

    }
}
