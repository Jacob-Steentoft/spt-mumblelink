using BepInEx;
using Fika.Core.Coop.Components;
using SPT.MumbleLink.Configurations;
using SPT.MumbleLink.Services;

namespace SPT.MumbleLink;

[BepInPlugin("com.ControlFreak.MumbleLink", "MumbleLink", Version)]
[BepInDependency("com.fika.core")]
public class Plugin : BaseUnityPlugin
{
	public const string Version = "1.0.2";
	
	private static readonly BepInConfig BepInConfig = BepInConfig.Instance;

	private static CoopHandler? _coopHandler;
	private static MumbleLinkConnection? _mumbleLink;

	private void Awake()
	{
		BepInConfig.Instance = new BepInConfig
		{
			DebugLogs = Config.Bind("MumbleLink", "Enable Debug Logs", false, string.Empty),
			Enabled = Config.Bind("MumbleLink", "Enable MumbleLink", true, string.Empty),
		};

		Logger.LogInfo($"MumbleLink version '{Version}' loaded");
	}

	private void Update()
	{
		if (!BepInConfig.Enabled.Value)
		{
			return;
		}

		if (_coopHandler == null)
		{
			if (CoopHandler.GetCoopHandler() is not { } coopHandler)
			{
				return;
			}

			_coopHandler = coopHandler;
		}

		var player = _coopHandler.MyPlayer;

		if (BepInConfig.DebugLogs.Value)
		{
			Logger.LogMessage("IsPlayerAlive: " + player?.HealthController.IsAlive);
			Logger.LogMessage("HasExtracted: " + !_coopHandler.ExtractedPlayers.Contains(player?.NetId ?? 0));
		}

		if (player is { HealthController.IsAlive: true } && !_coopHandler.ExtractedPlayers.Contains(player.NetId))
		{
			_mumbleLink ??= new MumbleLinkConnection(player.AccountId, player.GroupId);
			var camera = player.CameraPosition;
			_mumbleLink.Update(camera.up, camera.forward, camera.position);
			return;
		}

		if (_mumbleLink is null)
		{
			return;
		}

		_mumbleLink.Dispose();
		_mumbleLink = null;
	}
}