using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using JetBrains.Annotations;
using SPT.MumbleLink.Services;

namespace SPT.MumbleLink;

[BepInPlugin("ControlFreak.MumbleLink", "MumbleLink", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
	private static ManualLogSource _logSource;
	private static ConfigEntry<bool> _enabled;

	[CanBeNull] private MumbleLinkConnection _mumbleLink;

	private void Awake()
	{
		_enabled = Config.Bind("MumbleLink", "Enabled", true, "");
		_logSource = Logger;
		_logSource.LogInfo("MumbleLink loaded");
	}

	private void Update()
	{
		if (!_enabled.Value)
		{
			return;
		}
		
		var game = Singleton<AbstractGame>.Instance;
		var player = Singleton<GameWorld>.Instance.MainPlayer;

		if (game is null || player is null || !player.isActiveAndEnabled)
		{
			if (_mumbleLink is null)
			{
				return;
			}

			CloseLink();
			return;
		}
		
		var camera = player.CameraPosition;
		switch (game)
		{
			case { InRaid: true } when _mumbleLink is null:
			{
				_mumbleLink = new MumbleLinkConnection(player.AccountId, player.GroupId);
				return;
			}
			case { InRaid: true } when _mumbleLink is not null:
			{
				_mumbleLink.Update(camera.up, camera.forward, camera.position);
				return;
			}
			case { InRaid: false } when _mumbleLink is not null:
			{
				CloseLink();
				return;
			}
		}
	}

	private void CloseLink()
	{
		_mumbleLink?.Dispose();
		_mumbleLink = null;
	}
}