using System;
using System.ComponentModel;

namespace Practic.Framework.Support
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(String propertyName)
        {
            this.HandlePropertyChanged(PropertyChanged,propertyName);
        }
    }
}
