namespace Harmonica.Core.Tuning
{
    [Flags]
    internal enum PlayingTechnique
    {
        Unknown = 0,
        Draw      = 1 << 1,
        Blow      = 1 << 2,
        DrawBend1 = 1 << 4,
        DrawBend2 = 1 << 8,
        DrawBend3 = 1 << 16,
        OverDraw  = 1 << 32,
        BlowBend1 = 1 << 64,
        BlowBend2 = 1 << 128,
        OverBlow  = 1 << 256,
    }
}
