using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _11_Image_Processing
{
    class Command : ICommand
    {
        Action<object> executeMethod;
        Func<object, bool> canexecuteMethod;

        public Command(Action<object> executeMethod, Func<object, bool> canexecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canexecuteMethod = canexecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
    //TODO 02 add more commands
    static class CM // rename to Commands
    {
        public static RoutedCommand MyCommand = new RoutedCommand();

        public static void Initialize()
        {
            MyCommand.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
        }
    

    }
}
