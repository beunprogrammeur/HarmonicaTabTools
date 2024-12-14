using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;

namespace Harmonica.Core.UnitTests
{
    [TestFixture]
    public class TuningLoaderTests
    {
        [TestCase(Semitone.D, 1, Semitone.DSharp)]
        [TestCase(Semitone.D, 0, Semitone.D)]
        [TestCase(Semitone.D, -1, Semitone.CSharp)]
        [TestCase(Semitone.C, -1, Semitone.B)]
        [TestCase(Semitone.GSharp, 1, Semitone.A)]
        [TestCase(Semitone.F, 12, Semitone.F)]
        public void Given_TuningLoader_Transposes_SemitoneCorrectly(Semitone semitone, int steps, Semitone expected)
        {
            // Act
            Semitone output = TuningLoader.TransposeSemitone(semitone, steps);

            // Assert
            Assert.That(output, Is.EqualTo(expected));
        }
    }
}
