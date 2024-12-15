using CommunityToolkit.Mvvm.Input;
using Harmonica.GuitarTabConverter.WPF.MVVM.Models;
using System.Windows.Threading;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly TablatureModel _tablatureModel;
        private readonly ConfigurationModel _configurationModel;

        public TablatureViewModel InputTablatureViewModel { get; }
        public TablatureViewModel OutputTablatureViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        public IRelayCommand ConvertCommand { get; init; }

        public MainViewModel(Dispatcher dispatcher) : base(dispatcher)
        {
            ConvertCommand = new RelayCommand(OnConvert);

            InputTablatureViewModel = new(dispatcher);
            OutputTablatureViewModel = new(dispatcher)
            {
                IsReadOnly = true
            };

            _settingsModel = new SettingsModel();
            _configurationModel = new ConfigurationModel(_settingsModel);
            _tablatureModel = new TablatureModel(_configurationModel);
            ConfigurationViewModel = new ConfigurationViewModel(dispatcher, _configurationModel, _settingsModel);
        }

        public override void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }

            base.Dispose();
            _settingsModel.Dispose();
            _tablatureModel.Dispose();
        }

        private void OnConvert()
        {
            OutputTablatureViewModel.Document.Text = _tablatureModel.Convert(InputTablatureViewModel.Document.Text,
                    ConfigurationViewModel.SelectedHarmonicaKey,
                    ConfigurationViewModel.SelectedHarmonicaTuning,
                    ConfigurationViewModel.SelectedGuitarTuning);

            OutputTablatureViewModel.IsInFocus = true;
        }
    }
}
