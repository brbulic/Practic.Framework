using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Practic.Framework.mvvm
{
    public class RemoteMethodCallCommand : ICommand
    {
        private readonly Action<object> _remoteCommand;
        private readonly Action<object> _canExecuteCommand;
        
        public RemoteMethodCallCommand(Action<object> executeRemote, Action<object> canExecuteCommand = null)
        {
            _remoteCommand = executeRemote;
            _canExecuteCommand = canExecuteCommand;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _remoteCommand(parameter);
        }
    }
}
