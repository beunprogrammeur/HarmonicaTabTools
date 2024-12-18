using Harmonica.Core.Midi;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Harmonica.Core.Tuning
{
    public static partial class TuningLoader
    {
        /// <summary>
        /// Transposes the key of the tuning configuration up or down
        /// </summary>
        /// <param name="configuration">configuration to transpose (copy)</param>
        /// <param name="semitones">the amount of semitones to transpose (negative to transpose down)</param>
        /// <returns>a new transposed tuning configuration</returns>
        public static HarmonicaTuningConfiguration Transpose(HarmonicaTuningConfiguration configuration, int semitones)
        {
            HarmonicaTuningConfiguration newConfiguration = new HarmonicaTuningConfiguration(
                configuration.Tuning,
                TransposeSemitone(configuration.Key, semitones),
                configuration.HasSlider);

            foreach ((string identifier, HarmonicaHoleConfiguration holeConfig) in configuration.Holes)
            {
                HarmonicaHoleConfiguration newHoleConfig = new(identifier,
                    new Note(holeConfig.BlowNote.MidiPitch + semitones),
                    new Note(holeConfig.DrawNote.MidiPitch + semitones),
                    holeConfig.SupportedTechniques
                    );
                newConfiguration.AddHole(newHoleConfig);
            }

            return newConfiguration;
        }

        internal static Semitone TransposeSemitone(Semitone semitone, int steps)
        {
            int midiPitch = MidiConversionUtilities.ConvertNoteToMidiPitch(semitone, 4);
            return MidiConversionUtilities.ConvertMidiPitchToNote(midiPitch + steps).Item1;
        }

        public static HarmonicaTuningConfiguration LoadTuningConfiguration(string filepath)
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
                    throw new ArgumentOutOfRangeException("version is unsupported");
                case "1.0":
                    return LoadTuningConfigurationV1(tuning);
            }
        }

        private static HarmonicaTuningConfiguration LoadTuningConfigurationV1(XElement xml)
        {
            string tuning = xml.Attribute("tuning")?.Value ?? string.Empty;
            string key = xml.Attribute("key")?.Value ?? string.Empty;
            bool.TryParse(xml.Attribute("hasSlider")?.Value ?? "false", out bool hasSlider);
            HarmonicaTuningConfiguration tuningConfiguration = new(TuningFromString(tuning), SemitoneFromString(key), hasSlider);

            foreach (XElement hole in xml.Elements("hole"))
            {
                string identifier = hole.Attribute("id").Value;
                Note blowNote = NoteFromString(hole.Attribute("blow").Value);
                Note drawNote = NoteFromString(hole.Attribute("draw").Value);

                List<PlayingTechnique> techniques = hole
                    .Elements("technique")
                    .Select(x => PlayingTechniqueFromString(x.Value))
                    .ToList();

                if (hasSlider)
                {
                    techniques.AddRange(PlayingTechnique.BlowSlider, PlayingTechnique.DrawSlider);
                }

                HarmonicaHoleConfiguration holeConfiguration = new(identifier, blowNote, drawNote, techniques);

                tuningConfiguration.AddHole(holeConfiguration);
            }

            return tuningConfiguration;
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


        [GeneratedRegex(@"(\w+)(\d)", RegexOptions.Compiled)]
        private static partial Regex XmlNoteRegex();

        private static Note? NoteFromString(string input)
        {
            Match match = XmlNoteRegex().Match(input);
            if (match.Success &&
                match.Groups.Count == 3 &&
                Enum.TryParse(match.Groups[1].Value, true, out Semitone semitone) &&
                int.TryParse(match.Groups[2].Value, out int octave))
            {
                return new Note(semitone, octave);
            }

            return null;
        }

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
            "Solo" => HarmonicaTuning.Solo,
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
