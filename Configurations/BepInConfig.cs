using BepInEx.Configuration;

namespace SPT.MumbleLink.Configurations;

public sealed class BepInConfig
{
	public static BepInConfig Instance { get; set; } = null!;
	public required ConfigEntry<bool> Enabled { get; init; }
	public required ConfigEntry<bool> DebugLogs { get; init; }
}