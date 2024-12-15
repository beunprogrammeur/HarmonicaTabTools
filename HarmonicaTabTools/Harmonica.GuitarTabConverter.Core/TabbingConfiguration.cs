using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;
using Harmonica.GuitarTabConverter.Core.Guitar;

namespace Harmonica.GuitarTabConverter.Core
{
    public class TabbingConfiguration
    {
        /// <summary>
        /// If certain techniques are not mastered such as overdraw/blow
        /// We can "turn those off" by removing them from the list.
        /// </summary>
        public List<PlayingTechnique> AllowedHarmonicaTechniques { get; set; } = [];
        public List<GuitarString> StringOrder { get; set; } = [];
        public string UnknownNoteCharacter { get; set; } = "X";

        public HarmonicaTuningConfiguration HarmonicaTuning { get; set; }
        public GuitarTuningConfiguration GuitarTuning { get; set; }

        public TabbingConfiguration()
        {
            // The default playing techniques
            AllowedHarmonicaTechniques.AddRange(
                PlayingTechnique.Draw,
                PlayingTechnique.DrawBend1,
                PlayingTechnique.DrawBend2,
                PlayingTechnique.DrawBend3,
                PlayingTechnique.Blow,
                PlayingTechnique.BlowBend1,
                PlayingTechnique.BlowBend2,
                PlayingTechnique.OverBlow,
                PlayingTechnique.OverDraw);

            // The default string layout
            StringOrder.AddRange(
                GuitarString.HighE,
                GuitarString.B,
                GuitarString.G,
                GuitarString.D,
                GuitarString.A,
                GuitarString.LowE);
        }

        public int GetPitchForGuitarString(GuitarString guitarString)
        {
            if (GuitarTuning.StringConfiguration != StringConfiguration.Default)
            {
                throw new NotImplementedException();
            }

            return guitarString switch
            {
                GuitarString.LowE => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.E, 2),
                GuitarString.A => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.A, 2),
                GuitarString.D => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.D, 3),
                GuitarString.G => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.G, 3),
                GuitarString.B => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.B, 3),
                GuitarString.HighE => MidiConversionUtilities.ConvertNoteToMidiPitch(Semitone.E, 4),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
