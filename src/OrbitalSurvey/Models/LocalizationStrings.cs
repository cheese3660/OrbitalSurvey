﻿using I2.Loc;

namespace OrbitalSurvey.Models;

public static class LocalizationStrings
{
    public static readonly Dictionary<Status, LocalizedString> STATUS = new()
    {
        { Status.Disabled, "OrbitalSurvey/Status/Disabled" },
        { Status.Scanning, "OrbitalSurvey/Status/Scanning" },
        { Status.Idle, "OrbitalSurvey/Status/Idle" },
        { Status.Complete, "OrbitalSurvey/Status/Complete" },
        { Status.NoPower, "OrbitalSurvey/Status/NoPower" },
    };

    public static readonly Dictionary<State, LocalizedString> STATE = new()
    {
        { State.BelowMin, "OrbitalSurvey/State/BelowMin" },
        { State.BelowIdeal, "OrbitalSurvey/State/BelowIdeal" },
        { State.Ideal, "OrbitalSurvey/State/Ideal" },
        { State.AboveIdeal, "OrbitalSurvey/State/AboveIdeal" },
        { State.AboveMax, "OrbitalSurvey/State/AboveMax" },
    };

    public static readonly LocalizedString COMPLETE = "OrbitalSurvey/UI/Complete";

    public static readonly Dictionary<string, LocalizedString> OAB_DESCRIPTION = new()
    {
        { "ModuleDescription", "OrbitalSurvey/OAB/Description" },
        { "ResourcesRequired", "OrbitalSurvey/OAB/ResourcesRequiredTitle" },
        { "ElectricCharge", "OrbitalSurvey/OAB/ResourcesRequiredEntry" }
    };

    public static readonly Dictionary<string, LocalizedString> PARTMODULES = new()
    {
        {"Name", "PartModules/OrbitalSurvey/Name"},
        {"Status", "PartModules/OrbitalSurvey/Status"},
        {"Enabled", "PartModules/OrbitalSurvey/Enabled"},
        {"Mode", "PartModules/OrbitalSurvey/Mode"},
        {"State", "PartModules/OrbitalSurvey/State"},
        {"ScanningFOV", "PartModules/OrbitalSurvey/ScanningFOV"},
        {"ScanningFOVDebug", "PartModules/OrbitalSurvey/ScanningFOVDebug"},
        {"MinAltitude", "PartModules/OrbitalSurvey/MinAltitude"},
        {"IdealAltitude", "PartModules/OrbitalSurvey/IdealAltitude"},
        {"MaxAltitude", "PartModules/OrbitalSurvey/MaxAltitude"},
        {"PercentComplete", "PartModules/OrbitalSurvey/PercentComplete"},
        {"OpenGui", "PartModules/OrbitalSurvey/OpenGui"},
    };
}