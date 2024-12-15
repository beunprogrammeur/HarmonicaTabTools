namespace Harmonica.Core.Midi
{
    public static class MidiConversionUtilities
    {
        public static int ConvertNoteToMidiPitch(Semitone semitone, int octave)
        {
            const int C4MidiNoteNumber = 60;
            const int semitonesPerOctave = 12;

            int baseNote = C4MidiNoteNumber + (octave - 4) * semitonesPerOctave;

            int midiNoteNumber = baseNote + (int)semitone - 1;
            return midiNoteNumber;
        }

        public static (Semitone, int) ConvertMidiPitchToNote(int pitch)
        {
            const int semitonesPerOctave = 12;

            int octave = (pitch / semitonesPerOctave) - 1;
            int semitoneValue = (pitch % semitonesPerOctave) + 1;
            Semitone semitone = (Semitone)semitoneValue;
            return (semitone, octave);
        }
    }
}
