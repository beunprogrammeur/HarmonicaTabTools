namespace Harmonica.Core.Midi
{
    internal class Note 
    {
        public int MidiPitch { get; init; }
        public int Octave { get; init; }
        public Semitone Semitone { get; init; }

        public Note(Semitone semitone, int octave)
        {
            Semitone = semitone;
            Octave = octave;

            MidiPitch = MidiConversionUtilities.ConvertNoteToMidiPitch(semitone, octave);
        }

        public Note(int midiPitch)
        {
            MidiPitch = midiPitch;
            
            (Semitone semitone, int octave) = MidiConversionUtilities.ConvertMidiPitchToNote(midiPitch);
            Semitone = semitone;
            Octave = octave;
        }
    }
}
