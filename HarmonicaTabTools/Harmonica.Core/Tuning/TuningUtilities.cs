namespace Harmonica.Core.Tuning
{
    internal static class TuningUtilities
    {
        private readonly static IReadOnlyDictionary<PlayingTechnique, int> _playingTechniquePitchDeviationMapping =
            new Dictionary<PlayingTechnique, int>()
            {
                {PlayingTechnique.Unknown, 0 },
                {PlayingTechnique.Blow, 0 },
                {PlayingTechnique.BlowBend1, -1 },
                {PlayingTechnique.BlowBend2, -2 },
                {PlayingTechnique.OverBlow, -1 },
                {PlayingTechnique.Draw, 0 },
                {PlayingTechnique.DrawBend1, -1 },
                {PlayingTechnique.DrawBend2, -2 },
                {PlayingTechnique.DrawBend3, -3 },
                {PlayingTechnique.OverDraw, -1 },
            };

        public static int GetPitchForTechnique(int pitch, PlayingTechnique technique)
        {
            return pitch + _playingTechniquePitchDeviationMapping[technique];
        }
    }
}
