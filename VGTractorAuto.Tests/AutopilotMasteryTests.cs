using Xunit;

namespace VGTractorAuto.Tests;

public sealed class AutopilotMasteryTests
{
    [Theory]
    [InlineData(0, 100, 3, 0, 0)]
    [InlineData(50, 100, 3, 1, 50)]
    [InlineData(99, 100, 3, 2, 99)]
    [InlineData(100, 100, 3, 3, 100)]
    [InlineData(200, 100, 3, 3, 100)]
    [InlineData(-1, 100, 3, 0, 0)]
    [InlineData(50, 0, 3, 0, 0)]
    [InlineData(50, -1, 3, 0, 0)]
    [InlineData(50, 100, 0, 0, 50)]
    public void MasteryConversionPreservesFloorAndClamp(int level, int cap, int beams, int extra, int percent)
    {
        Assert.Equal(extra, AutopilotMastery.ComputeExtraAuto(level, cap, beams));
        Assert.Equal(percent, AutopilotMastery.ComputePercent(level, cap));
    }
}
