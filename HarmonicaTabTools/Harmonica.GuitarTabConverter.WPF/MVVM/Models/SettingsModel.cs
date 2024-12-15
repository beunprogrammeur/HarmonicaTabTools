using System.Xml.Linq;

namespace Harmonica.GuitarTabConverter.WPF.MVVM.Models
{
    internal class SettingsModel : BaseModel
    {
        private const string _settingsFile = "settings.xml";
        private XElement _settingsXML;

        public enum Setting
        {
            HarmonicaTuningLocation,
            LastUsedHarmonicaTuning,
            LastUsedHarmonicaKey
        }

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                base.Dispose();
                SaveSettings();
            }
        }

        private void LoadSettings()
        {
            try
            {
                _settingsXML = XElement.Load(_settingsFile);
            }
            catch
            {
                _settingsXML = new XElement("settings");
            }
        }

        private void SaveSettings()
        {
            _settingsXML?.Save(_settingsFile);
        }

        internal string GetSetting(Setting setting)
        {
            if (_settingsXML == null)
            {
                LoadSettings();
            }

            string name = setting.ToString();
            return _settingsXML?.Elements("setting")?.FirstOrDefault(x => x.Attribute("name").Value == name)?.Value ?? string.Empty;
        }

        internal void SetSetting(Setting setting, string value)
        {
            if (_settingsXML == null)
            {
                LoadSettings();
            }

            string name = setting.ToString();
            XElement node = _settingsXML?.Elements("setting")?.FirstOrDefault(x => x.Attribute("name").Value == name);
            if (node == null)
            {
                node = new XElement("setting", new XAttribute("name", name));
                _settingsXML?.Add(node);
            }

            node.Value = value;
        }
    }
}
