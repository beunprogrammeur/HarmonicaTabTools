using Harmonica.Core.Tuning;
using System.Xml.Linq;

namespace Harmonica.Core.Midi
{
    internal static class MidiUtilities
    {
        public static int ConvertNoteToMidiPitch(Semitone semitone, int octave)
        {
            const int C4MidiNoteNumber = 60;
            const int semitonesPerOctave = 12;

            int baseNote = C4MidiNoteNumber + (octave - 4) * semitonesPerOctave;

            int midiNoteNumber = baseNote + (int)semitone - 1;
            return midiNoteNumber;
        }

        public static Note ConvertMidiPitchToNote(int pitch)
        {
            const int C4MidiNoteNumber = 60; 
            const int semitonesPerOctave = 12; 
            int octave = (pitch / semitonesPerOctave) - 1; 
            int semitoneValue = (pitch % semitonesPerOctave) + 1; 
            Semitone semitone = (Semitone)semitoneValue; 
            return new Note(semitone, octave);
        }


        public static TuningConfiguration LoadTuningConfiguration(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException(filepath);
            }

            XElement tuning = XElement.Load(filepath);
            string tuningVersion = tuning.Attribute("version")?.Value ?? string.Empty;

            switch (tuningVersion)
            {
                default:
                case "1.0":
                    return LoadTuningConfigurationV1(tuning);
            }
        }

        private static TuningConfiguration LoadTuningConfigurationV1(XElement xml)
        {
            string tuning = xml.Attribute("tuning")?.Value ?? string.Empty;
            string key = xml.Attribute("key")?.Value ?? string.Empty;

            TuningConfiguration harmonicaConfiguration = new(TuningFromString(tuning), SemitoneFromString(key));

            foreach(XElement hole in xml.Elements("hole"))
            {
                string id = hole.Attribute("id")?.Value ?? string.Empty;

                HarmonicaHoleConfiguration holeConfiguration = new(id);
                harmonicaConfiguration.AddHole(holeConfiguration);

                foreach(XElement techniqueType in hole.Elements())
                {
                    PlayingTechnique playingTechnique = PlayingTechniqueFromString(techniqueType.Name.ToString());
                    int octave = int.Parse(techniqueType.Attribute("octave").Value);
                    Semitone semitone = SemitoneFromString(techniqueType.Attribute("semitone").Value);
                    int pitch = ConvertNoteToMidiPitch(semitone, octave);

                    holeConfiguration.Add(pitch, playingTechnique);
                }
            }

            return harmonicaConfiguration;
        }

        private static Semitone SemitoneFromString(string semitone) => semitone switch
        {
            "C" => Semitone.C,
            "CSharp" => Semitone.CSharp,
            "DFlat" => Semitone.DFlat,
            "D" => Semitone.D,
            "DSharp" => Semitone.DSharp,
            "EFlat" => Semitone.EFlat,
            "E" => Semitone.E,
            "F" => Semitone.F,
            "FSharp" => Semitone.FSharp,
            "GFlat" => Semitone.GFlat,
            "G" => Semitone.G,
            "GSharp" => Semitone.GSharp,
            "AFlat" => Semitone.AFlat,
            "A" => Semitone.A,
            "ASharp" => Semitone.ASharp,
            "BFlat" => Semitone.BFlat,
            "B" => Semitone.B,
            _ => throw new ArgumentOutOfRangeException(semitone),
        };

        private static HarmonicaTuning TuningFromString(string tuning) => tuning switch
        {
            "Richter" => HarmonicaTuning.Richter,
            "PaddyRichter" => HarmonicaTuning.PaddyRichter,
            "PowerBender" => HarmonicaTuning.PowerBender,
            "PowerBenderLucky13" => HarmonicaTuning.PowerBenderLucky13,
            "PowerDraw" => HarmonicaTuning.PowerDraw,
            "PowerDrawLucky13" => HarmonicaTuning.PowerDrawLucky13,
            "Wilde" => HarmonicaTuning.Wilde,
            "Country" => HarmonicaTuning.Country,
            "MelodyMaker" => HarmonicaTuning.MelodyMaker,
            "NaturalMinor" => HarmonicaTuning.NaturalMinor,
            "HarmonicMinor" => HarmonicaTuning.HarmonicMinor,
            "Chromatic" => HarmonicaTuning.Chromatic,
            _ => throw new ArgumentOutOfRangeException(tuning),
        };

        private static PlayingTechnique PlayingTechniqueFromString(string technique) => technique.ToLower() switch
        {
            "draw" => PlayingTechnique.Draw,
            "blow" => PlayingTechnique.Blow,
            "drawbend1" => PlayingTechnique.DrawBend1,
            "drawbend2" => PlayingTechnique.DrawBend2,
            "drawbend3" => PlayingTechnique.DrawBend3,
            "overdraw" => PlayingTechnique.OverDraw,
            "blowbend1" => PlayingTechnique.BlowBend1,
            "blowbend2" => PlayingTechnique.BlowBend2,
            "overblow" => PlayingTechnique.OverBlow,
            _ => throw new ArgumentOutOfRangeException(technique),
        };
    }
}
