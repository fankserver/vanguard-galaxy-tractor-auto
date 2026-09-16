using System;
using BepInEx;
using BepInEx.Configuration;
using VGModAPI;

namespace VGTractorAuto;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInProcess("VanguardGalaxy.exe")]
[BepInDependency(ModApi.PluginId)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "vgtractorauto";
    public const string PluginName = "Tractor Auto";
    public const string PluginVersion = "0.2.0";
    private ConfigEntry<bool> _enabled = null!;
    private ISkillTreeService _skillTrees = null!;
    private IDisposable? _targeting, _moduleTooltip, _masteryTooltip;

    private void Awake()
    {
        _enabled = Config.Bind("General", "Enabled", true,
            "Let the player ship's Manual Tractor Beams also auto-tractor, scaled 0-100% by " +
            "Autopilot (Engineering) skill-tree mastery. At 0 mastery automatic targeting stays vanilla; at " +
            "the level cap, all manual beams auto-tractor. Manual targeting can still claim any " +
            "free beam. When false, vanilla behavior is fully restored.");
        var api = ModApi.Services;
        _skillTrees = api.SkillTrees;
        _targeting = api.Equipment.ConfigurePlayerTractorModules(PluginGuid, ConfigureTractor);
        _moduleTooltip = api.Tooltips.RegisterTractorModule(PluginGuid, DescribeTractor);
        _masteryTooltip = api.Tooltips.RegisterSkillTree(PluginGuid, DescribeMastery);
        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded (VGModAPI equipment integration)");
    }

    private TractorTargeting? ConfigureTractor(TractorModule module)
    {
        if (!_enabled.Value) return null;
        var tree = _skillTrees.Get(CommanderSpecialization.Engineering);
        int extra = AutopilotMastery.ComputeExtraAuto(tree?.MasteryLevel ?? 0, tree?.MaximumLevel ?? 0, module.ManualBeamCount);
        return new TractorTargeting(module.BeamCount + extra, allowManualBorrowing: true);
    }

    private void DescribeTractor(TractorModule module, Tooltip tooltip)
    {
        if (_enabled.Value && module.ManualBeamCount > 0)
            tooltip.AddLine("Manual beams also auto-tractor, scales with Autopilot mastery (VGTractorAuto)");
    }

    private void DescribeMastery(SkillTree tree, Tooltip tooltip)
    {
        if (!_enabled.Value || tree.Specialization != CommanderSpecialization.Engineering) return;
        int percent = AutopilotMastery.ComputePercent(tree.MasteryLevel, tree.MaximumLevel);
        tooltip.AddLine($"Manual Tractor Beams act as automatic: {percent}%", TooltipTextStyle.Bonus)
            .Append(" (VGTractorAuto)", TooltipTextStyle.Details);
    }

    private void OnDestroy()
    {
        _masteryTooltip?.Dispose(); _moduleTooltip?.Dispose(); _targeting?.Dispose();
        _masteryTooltip = _moduleTooltip = _targeting = null;
    }
}
