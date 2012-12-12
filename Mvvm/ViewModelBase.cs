using System.ComponentModel;
using Practic.Framework.Support;

namespace Practic.Framework.mvvm
{
    public abstract class ViewModelBase : PropertyChangedBase
    {

        ~ViewModelBase()
        {
            DebugUtils.SignalObjectCollected(GetType());
        }

    }
}
