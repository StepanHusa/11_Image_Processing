using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using _11_Image_Processing.Resources.Strings;

namespace _11_Image_Processing
{
    public static class Commands
    {

        public static readonly RoutedCommand NameField = new RoutedCommand("NameField", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });
        public static readonly RoutedCommand AnswerField = new RoutedCommand("AnswerField", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.F, ModifierKeys.Control) });
        public static readonly RoutedCommand AnswerBoxes = new RoutedCommand("AnswerBoxes", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.B, ModifierKeys.Control) });

        public static readonly RoutedCommand UndoQuestion = new("UndoQuestion", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.Z, ModifierKeys.Control) });

        public static readonly RoutedCommand UnloadProject = new RoutedCommand("UnloadProject", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.F4, ModifierKeys.Control) });
        public static readonly RoutedCommand SaveProject = new RoutedCommand("SaveProject", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });
        public static readonly RoutedCommand SaveProjectAs = new RoutedCommand("SaveProjectAs", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.S, (ModifierKeys)Enum.ToObject(typeof(ModifierKeys),6)) });

        public static readonly RoutedCommand MainView = new RoutedCommand("MainView", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.W, ModifierKeys.Control) });
        public static readonly RoutedCommand EditView = new RoutedCommand("EditView", typeof(Commands),new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control)});
        public static readonly RoutedCommand ResultView = new RoutedCommand("ResultView", typeof(Commands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });



    }
}
