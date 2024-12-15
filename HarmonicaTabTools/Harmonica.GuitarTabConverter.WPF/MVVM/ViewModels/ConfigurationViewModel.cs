using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core.Guitar;
using Harmonica.GuitarTabConverter.WPF.MVVM.Models;
using System.Windows.Threading;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.ViewModels
{
    internal class ConfigurationViewModel : BaseViewModel
    {
        private readonly ConfigurationModel _configurationModel;
        private readonly SettingsModel _settingsModel;

        private Semitone _selectedHarmonicaKey;
        private HarmonicaTuning _selectedHarmonicaTuning;


        public IReadOnlyCollection<Semitone> HarmmonicaKeys { get; init; }
        public Semitone SelectedHarmonicaKey { get => _selectedHarmonicaKey; set => SetProperty(ref _selectedHarmonicaKey, value); }
        public HarmonicaTuning SelectedHarmonicaTuning { get => _selectedHarmonicaTuning; set => SetProperty(ref _selectedHarmonicaTuning, value); }
        public HarmonicaTuning[] SupportedHarmonicaTunings => _configurationModel.GetTuningTypes();
        public StringConfiguration[] SupportedGuitarTunings => Enum.GetValues<StringConfiguration>();
        public StringConfiguration SelectedGuitarTuning { get; set; }

        public ConfigurationViewModel(Dispatcher dispatcher, ConfigurationModel configurationModel, SettingsModel settingsModel) : base(dispatcher)
        {
            HarmmonicaKeys = Enum.GetValues<Semitone>().Where(x => x != Semitone.Unknown).ToArray();
            _settingsModel = settingsModel;
            _configurationModel = configurationModel;
            _configurationModel.LoadTunings();

            PropertyChanged += OnPropertyChanged;

            LoadStateFromSettings();
        }
        
        private void LoadStateFromSettings()
        {
            SelectedHarmonicaKey = Enum.TryParse(_settingsModel.GetSetting(SettingsModel.Setting.LastUsedHarmonicaKey), out Semitone semitone) ? semitone : Semitone.C;
            SelectedHarmonicaTuning = Enum.TryParse(_settingsModel.GetSetting(SettingsModel.Setting.LastUsedHarmonicaTuning), out HarmonicaTuning tuning) ? tuning : HarmonicaTuning.Richter;
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedHarmonicaKey):
                    _settingsModel.SetSetting(SettingsModel.Setting.LastUsedHarmonicaKey, SelectedHarmonicaKey.ToString());
                    break;
                case nameof(SelectedHarmonicaTuning):
                    _settingsModel.SetSetting(SettingsModel.Setting.LastUsedHarmonicaTuning, SelectedHarmonicaTuning.ToString());
                    break;
            }
        }
    }
}
