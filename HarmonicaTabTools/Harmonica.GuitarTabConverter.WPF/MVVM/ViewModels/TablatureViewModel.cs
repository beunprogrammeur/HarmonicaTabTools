using System.Windows.Threading;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels
{
    internal class TablatureViewModel : BaseViewModel
    {
        private bool _isReadOnly;
        private bool _isInFocus;
        public ICSharpCode.AvalonEdit.Document.TextDocument Document { get; }
        public bool IsInFocus { get => _isInFocus; set => SetProperty(ref _isInFocus, value); }
        public bool IsReadOnly { get => _isReadOnly; set => SetProperty(ref _isReadOnly, value); }
        public TablatureViewModel(Dispatcher dispatcher) : base(dispatcher)
        {
            Document = new ICSharpCode.AvalonEdit.Document.TextDocument();
        }
    }
}
