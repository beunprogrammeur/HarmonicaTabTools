namespace Harmonica.GuitarTabConverter.WPF.MVVM.Models
{
    internal class BaseModel : IDisposable
    {
        protected bool IsDisposed { get; private set; }
        public virtual void Dispose()
        {
            IsDisposed = true;
        }
    }
}
