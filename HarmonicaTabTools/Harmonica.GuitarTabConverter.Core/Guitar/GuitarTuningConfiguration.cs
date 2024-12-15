using Harmonica.Core.Midi;

namespace Harmonica.GuitarTabConverter.Core.Guitar
{
    public class GuitarTuningConfiguration
    {
        private Dictionary<GuitarString, Note> _notesForStrings { get; init; }

        public StringConfiguration StringConfiguration { get; init; }
        public IReadOnlyDictionary<GuitarString, Note> NotesForStrings => _notesForStrings;

        public GuitarTuningConfiguration(StringConfiguration stringConfiguration)
        {
            StringConfiguration = stringConfiguration;
            _notesForStrings = new Dictionary<GuitarString, Note>();
            InitialiseGuitarStringPitches();
        }

        private void InitialiseGuitarStringPitches()
        {
            void AssignPitches(Note lowE, Note a, Note d, Note g, Note b, Note highE)
            {
                _notesForStrings[GuitarString.LowE] = lowE;
                _notesForStrings[GuitarString.A] = a;
                _notesForStrings[GuitarString.D] = d;
                _notesForStrings[GuitarString.G] = g;
                _notesForStrings[GuitarString.B] = b;
                _notesForStrings[GuitarString.HighE] = highE;
            }

            switch (StringConfiguration)
            {
                case StringConfiguration.Default:
                    AssignPitches(
                        new Note(Semitone.E, 2),
                        new Note(Semitone.A, 2),
                        new Note(Semitone.D, 3),
                        new Note(Semitone.G, 3),
                        new Note(Semitone.B, 3),
                        new Note(Semitone.E, 4));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
