﻿using Harmonica.Core.Midi;

namespace Harmonica.Core.Tuning
{
    public class TuningConfiguration
    {
        private readonly Dictionary<string, HarmonicaHoleConfiguration> _holes;
        public HarmonicaTuning Tuning { get; init; }
        public Semitone Key { get; init; }
        public bool HasSlider { get; init; }

        internal IReadOnlyDictionary<string, HarmonicaHoleConfiguration> Holes => _holes;

        internal TuningConfiguration()
        {
            _holes = [];
        }

        internal TuningConfiguration(HarmonicaTuning tuning, Semitone key, bool hasSlider = false) : this()
        {
            Tuning = tuning;
            Key = key;
            HasSlider = hasSlider;
        }

        internal void AddHole(HarmonicaHoleConfiguration hole)
        {
            if (_holes.TryGetValue(hole.Identifier, out HarmonicaHoleConfiguration preExistingHole))
            {
                throw new InvalidOperationException("hole already exists on this configuration");
            }

            _holes[hole.Identifier] = hole;
        }
    }
}
