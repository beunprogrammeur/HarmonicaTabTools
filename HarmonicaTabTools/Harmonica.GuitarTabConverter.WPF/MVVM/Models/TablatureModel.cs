using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core.Guitar;
using Harmonica.GuitarTabConverter.Core;
using Harmonica.Core.Midi;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.Models
{
    internal class TablatureModel : BaseModel
    {
        private readonly ConfigurationModel _configurationModel;

        public TablatureModel(ConfigurationModel configurationModel)
        {
            _configurationModel = configurationModel;
        }
        public string Convert(string tablature, 
            Semitone harmonicaKey, HarmonicaTuning harmonicaTuning, StringConfiguration guitarConfiguration)
        {
            HarmonicaTuningConfiguration harmonicaTuningConfiguration = _configurationModel
                .GetHarmonicaTuningInKey(harmonicaKey, harmonicaTuning);

            GuitarTuningConfiguration guitarTuning = _configurationModel.GetGuitarTuning(guitarConfiguration);

            TabbingConfiguration configuration = new()
            {
                HarmonicaTuning = harmonicaTuningConfiguration,
                GuitarTuning = guitarTuning
            };

            return Core.GuitarTabConverter.ConvertGuitarTab(configuration, tablature);
        }
    }
}
