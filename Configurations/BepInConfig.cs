using BepInEx.Configuration;

namespace SPT.MumbleLink.Configurations;

public static class BepInConfig
{
	public static ConfigEntry<bool> Enabled = null!;
	public static ConfigEntry<bool> DebugLogs = null!;
}