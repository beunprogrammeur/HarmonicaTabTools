namespace Harmonica.GuitarTabConverter.WPF.MVVM.Models
{
    internal class TablatureModel : BaseModel
    {
        private readonly SettingsModel _settingsModel;

        public TablatureModel(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }
        public string Convert(string tablature)
        {
            return tablature;
        }
    }
}
