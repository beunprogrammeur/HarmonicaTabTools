using Harmonica.Core.Midi;
using Harmonica.Core.Tuning;

namespace Harmonica.Core.UnitTests
{
    [TestFixture]
    internal class HarmonicaHoleConfigurationTests
    {
        [Test]
        public void Given_HarmonicaHoleConfiguration_Configures_All_Techniques()
        {
            Note blowNote = new(Semitone.G, 3);
            Note drawNote = new(Semitone.B, 3);

            HarmonicaHoleConfiguration configuration = new("3", blowNote, drawNote,
                PlayingTechnique.DrawBend1,
                PlayingTechnique.DrawBend2,
                PlayingTechnique.DrawBend3,
                PlayingTechnique.BlowBend1,
                PlayingTechnique.BlowBend2,
                PlayingTechnique.OverDraw,
                PlayingTechnique.OverBlow,
                PlayingTechnique.Unknown); // unsupported and should be missing

            Assert.Multiple(() =>
            {
                Assert.That(configuration.Identifier, Is.EqualTo("3"));
                Assert.That(blowNote, Is.EqualTo(configuration.BlowNote));
                Assert.That(drawNote, Is.EqualTo(configuration.DrawNote));
                Assert.That(configuration.SupportedTechniques,
                    Is.EquivalentTo(new PlayingTechnique[] {
                        PlayingTechnique.DrawBend1,
                        PlayingTechnique.DrawBend2,
                        PlayingTechnique.DrawBend3,
                        PlayingTechnique.BlowBend1,
                        PlayingTechnique.BlowBend2,
                        PlayingTechnique.OverDraw,
                        PlayingTechnique.OverBlow,
                        PlayingTechnique.Draw,
                        PlayingTechnique.Blow
                }));

                Assert.That(configuration[PlayingTechnique.Blow]!.Semitone, Is.EqualTo(Semitone.G));
                Assert.That(configuration[PlayingTechnique.BlowBend1]!.Semitone, Is.EqualTo(Semitone.FSharp));
                Assert.That(configuration[PlayingTechnique.BlowBend2]!.Semitone, Is.EqualTo(Semitone.F));
                Assert.That(configuration[PlayingTechnique.OverBlow]!.Semitone, Is.EqualTo(Semitone.C));
                Assert.That(configuration[PlayingTechnique.Draw]!.Semitone, Is.EqualTo(Semitone.B));
                Assert.That(configuration[PlayingTechnique.DrawBend1]!.Semitone, Is.EqualTo(Semitone.ASharp));
                Assert.That(configuration[PlayingTechnique.DrawBend2]!.Semitone, Is.EqualTo(Semitone.A));
                Assert.That(configuration[PlayingTechnique.DrawBend3]!.Semitone, Is.EqualTo(Semitone.GSharp));
                Assert.That(configuration[PlayingTechnique.OverDraw]!.Semitone, Is.EqualTo(Semitone.GSharp));
            });
        }
    }
}
