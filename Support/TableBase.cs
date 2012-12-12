using System.ComponentModel;

namespace Practic.Framework.Support
{
    public class TableBase : PropertyChangedBase, INotifyPropertyChanging
    {
        public event PropertyChangingEventHandler PropertyChanging;

        protected void OnPropertyChanging(string propertyName)
        {
            this.HandlePropertyChanging(PropertyChanging, propertyName);
        }

    }
}
