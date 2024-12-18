using Harmonica.Core.Midi;

namespace Harmonica.Core.Tuning
{
    public class HarmonicaHoleConfiguration
    {
        private readonly Dictionary<PlayingTechnique, Note> _supportedNotes;

        /// <summary>
        /// The number or letter that appears in a tab to identify which hole has to be played.
        /// </summary>
        public string Identifier { get; init; }
        public Note BlowNote { get; init; }
        public Note DrawNote { get; init; }
        public IReadOnlyCollection<PlayingTechnique> SupportedTechniques => _supportedNotes.Keys;
        public IReadOnlyCollection<Note> SupportedNotes => _supportedNotes.Values;

        public Note? this[PlayingTechnique technique]
        {
            get
            {
                _supportedNotes.TryGetValue(technique, out var note);
                return note;
            }
        }

        public HarmonicaHoleConfiguration(string identifier, Note blowNote, Note drawNote, params IReadOnlyCollection<PlayingTechnique> techniques)
        {
            Identifier = identifier;
            BlowNote = blowNote;
            DrawNote = drawNote;

            _supportedNotes = new Dictionary<PlayingTechnique, Note>()
            {
                [PlayingTechnique.Blow] = blowNote,
                [PlayingTechnique.Draw] = drawNote,
            };

            MapTechniques(techniques);
        }

        private void MapTechniques(IReadOnlyCollection<PlayingTechnique> techniques)
        {

            foreach (var technique in techniques)
            {
                if (!_supportedNotes.ContainsKey(technique))
                {
                    // blow and draw are already handled in the constructor.
                    switch (technique)
                    {
                        case PlayingTechnique.DrawBend1:
                            _supportedNotes[technique] = new Note(DrawNote.MidiPitch - 1);
                            break;
                        case PlayingTechnique.DrawBend2:
                            _supportedNotes[technique] = new Note(DrawNote.MidiPitch - 2);
                            break;
                        case PlayingTechnique.DrawBend3:
                            _supportedNotes[technique] = new Note(DrawNote.MidiPitch - 3);
                            break;
                        case PlayingTechnique.OverDraw:
                            _supportedNotes[technique] = new Note(BlowNote.MidiPitch + 1);
                            break;
                        case PlayingTechnique.BlowBend1:
                            _supportedNotes[technique] = new Note(BlowNote.MidiPitch - 1);
                            break;
                        case PlayingTechnique.BlowBend2:
                            _supportedNotes[technique] = new Note(BlowNote.MidiPitch - 2);
                            break;
                        case PlayingTechnique.OverBlow:
                            _supportedNotes[technique] = new Note(DrawNote.MidiPitch + 1);
                            break;
                        case PlayingTechnique.BlowSlider:
                            _supportedNotes[technique] = new Note(BlowNote.MidiPitch + 1);
                            break;
                        case PlayingTechnique.DrawSlider:
                            _supportedNotes[technique] = new Note(DrawNote.MidiPitch + 1);
                            break;
                    }
                }
            }
        }
    }
}
