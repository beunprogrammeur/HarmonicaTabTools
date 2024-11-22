using Harmonica.Core.Midi;

namespace Harmonica.Core.UnitTests
{
    [TestFixture]
    public class MidiUtilitiesTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [TestCase(60, Semitone.C, 4)]
        [TestCase(69, Semitone.A, 4)]
        [TestCase(42, Semitone.FSharp, 2)]
        [TestCase(61, Semitone.CSharp, 4)]
        [TestCase(48, Semitone.C, 3)]
        [TestCase(57, Semitone.A, 3)]
        [TestCase(75, Semitone.DSharp, 5)]
        [TestCase(90, Semitone.FSharp, 6)]
        public void Given_MidiUtilities_ConvertMidiPitchToNote_Returns_Correct_Note(int pitch, Semitone expectedSemitone, int expectedOctave)
        {
            // Act
            Note note = MidiUtilities.ConvertMidiPitchToNote(pitch);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(note.Octave, Is.EqualTo(expectedOctave));
                Assert.That(note.Semitone, Is.EqualTo(expectedSemitone));
            });
        }


        [TestCase(60, Semitone.C, 4)]
        [TestCase(69, Semitone.A, 4)]
        [TestCase(42, Semitone.FSharp, 2)]
        [TestCase(61, Semitone.CSharp, 4)]
        [TestCase(48, Semitone.C, 3)]
        [TestCase(57, Semitone.A, 3)]
        [TestCase(75, Semitone.DSharp, 5)]
        [TestCase(90, Semitone.FSharp, 6)]

        public void Given_MidiUtilities_ConvertMidiNoteToPitch_Returns_Correct_Note(int expectedPitch, Semitone semitone, int octave)
        {
            // Act
            int pitch = MidiUtilities.ConvertNoteToMidiPitch(semitone, octave);

            // Assert
            Assert.That(pitch, Is.EqualTo(expectedPitch));
        }
    }
}
