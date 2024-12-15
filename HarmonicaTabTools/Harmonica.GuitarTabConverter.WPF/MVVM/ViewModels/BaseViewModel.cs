using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels
{
    internal abstract class BaseViewModel : IDisposable, INotifyPropertyChanged
    {
        protected Dispatcher Dispatcher { get; init; }
        protected bool IsDisposed { get; private set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected BaseViewModel(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public virtual void Dispose()
        {
            IsDisposed = true;
        }

        protected bool SetProperty<T>(ref T old, T @new, [CallerMemberName] string propertyName = null) where T : IComparable
        {
            if (old == null && @new != null || old != null && @new == null ||old.CompareTo(@new) != 0)
            {
                old = @new;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
