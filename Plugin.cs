using BepInEx;
using BepInEx.Configuration;
using Fika.Core.Coop.Components;
using SPT.MumbleLink.Services;

namespace SPT.MumbleLink;

[BepInPlugin("com.ControlFreak.MumbleLink", "MumbleLink", Version)]
[BepInDependency("com.fika.core")]
public class Plugin : BaseUnityPlugin
{
	public const string Version = "1.0.0";
	private static ConfigEntry<bool> _enabled = null!;
	private static ConfigEntry<bool> _debug = null!;
	private CoopHandler? _coopHandler;


	private static MumbleLinkConnection? _mumbleLink;

	private void Awake()
	{
		_debug = Config.Bind("MumbleLink", "Enable Debug Logs", false, string.Empty);
		_enabled = Config.Bind("MumbleLink", "Enable MumbleLink", true, string.Empty);
		Logger.LogInfo($"MumbleLink version '{Version}' loaded");
	}

	private void Update()
	{
		if (!_enabled.Value)
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

		if (_debug.Value)
		{
			Logger.LogMessage("IsYourPlayer: " + player?.IsYourPlayer);
			Logger.LogMessage("IsAlive: " + player?.HealthController.IsAlive);
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