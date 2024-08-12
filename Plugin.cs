using BepInEx;
using EFT.Communications;
using Fika.Core.Coop.Components;
using SPT.MumbleLink.Configurations;
using SPT.MumbleLink.Services;

namespace SPT.MumbleLink;

[BepInPlugin("com.ControlFreak.MumbleLink", "MumbleLink", Version)]
[BepInDependency("com.fika.core")]
public class Plugin : BaseUnityPlugin
{
	public const string Version = "1.1.0";

	private static CoopHandler? _coopHandler;
	private static MumbleLinkConnection? _mumbleLink;

	private void Awake()
	{
		BepInConfig.DebugLogs = Config.Bind("MumbleLink", "Enable Debug Logs", false);
		BepInConfig.Enabled = Config.Bind("MumbleLink", "Enable MumbleLink", true);

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
			if (_mumbleLink == null)
			{
				_mumbleLink = new MumbleLinkConnection(player.AccountId, player.GroupId);
				Toast("[SPT.MumbleLink] Data is now being shared with Mumble");
			}

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
		Toast("[SPT.MumbleLink] Data is no longer shared with Mumble.", true);
	}

	private static void Toast(string message, bool forLong = false)
	{
		NotificationManagerClass.DisplayMessageNotification(message, forLong ? ENotificationDurationType.Long : ENotificationDurationType.Default);
		EFT.UI.ConsoleScreen.Log(message);
	}
}