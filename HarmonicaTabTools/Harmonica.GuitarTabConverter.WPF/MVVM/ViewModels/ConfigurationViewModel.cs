using Harmonica.Core.Midi;
using Harmonica.GuitarTabConverter.WPF.MVVM.Models;
using System.Windows.Threading;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels
{
    internal class ConfigurationViewModel : BaseViewModel
    {
        private Semitone _selectedKey;
        private readonly ConfigurationModel _configurationModel;
        public IReadOnlyCollection<Semitone> Keys { get; init; }
        public Semitone SelectedKey { get => _selectedKey; set => SetProperty(ref _selectedKey, value); }


        public ConfigurationViewModel(Dispatcher dispatcher, ConfigurationModel configurationModel) : base(dispatcher)
        {
            Keys = Enum.GetValues<Semitone>().Where(x => x != Semitone.Unknown).ToArray();

            _configurationModel = configurationModel;
            _configurationModel.LoadTunings();
        }
    }
}
