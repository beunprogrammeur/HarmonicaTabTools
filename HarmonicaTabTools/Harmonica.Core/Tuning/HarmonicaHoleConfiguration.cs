namespace Harmonica.Core.Tuning
{
    internal class HarmonicaHoleConfiguration
    {
        private Dictionary<int, PlayingTechnique> _configurations = [];

        public string HoleIdentifier { get; init; }

        public IReadOnlyList<int> Pitches => [.. _configurations.Keys];
        public IReadOnlyList<PlayingTechnique> Techniques => [.. _configurations.Values];

        public HarmonicaHoleConfiguration(string hole) 
        {
            HoleIdentifier = hole;
        }

        public void Add(int pitch, PlayingTechnique technique)
        {
            _configurations.Add(pitch, technique);
        }

        public PlayingTechnique TechniqueFromPitch(int pitch)
        {
            if (_configurations.TryGetValue(pitch, out PlayingTechnique technique))
            {
                return technique;
            }

            return PlayingTechnique.Unknown;
        }

        public int PitchFromTechnique(PlayingTechnique technique)
        {
            if (_configurations.Values.Any(x => x == technique))
            {
                return _configurations.First(x => x.Value == technique).Key;
            }

            return 0;
        }
    }
}
