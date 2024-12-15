using Harmonica.Core.Tuning;
using System.IO;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.Models
{
    internal class ConfigurationModel : BaseModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly List<HarmonicaTuningConfiguration> _harmonicaTuningConfigurations = [];

        public ConfigurationModel(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }

        public void LoadTunings()
        {
            string tunings = _settingsModel.GetSetting(SettingsModel.Setting.HarmonicaTuningLocation);
            if (Directory.Exists(tunings))
            {
                foreach (string file in Directory.GetFiles(tunings, "*.xml"))
                {
                    HarmonicaTuningConfiguration configuration = TuningLoader.LoadTuningConfiguration(file);
                    _harmonicaTuningConfigurations.Add(configuration);
                }
            }
        }

        public HarmonicaTuning[] GetTuningTypes()
        {
            return _harmonicaTuningConfigurations.Select(x => x.Tuning).ToArray();
        }
    }
}
