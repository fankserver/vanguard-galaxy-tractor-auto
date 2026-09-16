using System;

namespace VGTractorAuto;

internal static class AutopilotMastery
{
    internal static int ComputeExtraAuto(int level, int cap, int amountOfBonusBeams)
    {
        if (cap <= 0 || amountOfBonusBeams <= 0) return 0;
        return (int)Math.Floor(Math.Max(0f, Math.Min(1f, (float)level / cap)) * amountOfBonusBeams);
    }
    internal static int ComputePercent(int level, int cap)
    {
        if (cap <= 0) return 0;
        return (int)Math.Floor(Math.Max(0f, Math.Min(1f, (float)level / cap)) * 100f);
    }
}
