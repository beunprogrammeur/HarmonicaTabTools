using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core.Guitar;
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

        public HarmonicaTuningConfiguration GetHarmonicaTuningInKey(Semitone key, HarmonicaTuning tuning)
        {
            HarmonicaTuningConfiguration tuningConfig = _harmonicaTuningConfigurations.FirstOrDefault(x => x.Tuning == tuning);

            int distance = Math.Abs((int)tuningConfig.Key - (int)key);
            if(key > Semitone.F)
            {
                distance -= 12; // F is usually the highest tuned harmonica, therefore a G is in the previous octave.
            }


            return TuningLoader.Transpose(tuningConfig, distance);
        }

        public GuitarTuningConfiguration GetGuitarTuning(StringConfiguration stringConfiguration)
        {
            return new(stringConfiguration);
        }
    }
}
