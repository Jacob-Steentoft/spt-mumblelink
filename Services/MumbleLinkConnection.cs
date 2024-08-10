using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SPT.MumbleLink.Services;

public sealed class MumbleLinkConnection : IDisposable
{
	private static readonly string MapName = Environment.OSVersion.Platform == PlatformID.Unix
		? $"/dev/shm/MumbleLink.{getuid()}"
		: "MumbleLink";

	private readonly MumbleLinkData _data;
	private readonly MemoryMappedViewStream _stream;

	public MumbleLinkConnection(string playerId, string playerGroup)
	{
		using var mappedFile = MemoryMappedFile.CreateOrOpen(MapName, MumbleLinkData.Size);
		_stream = mappedFile.CreateViewStream(0, MumbleLinkData.Size);
		_data = new MumbleLinkData(playerId, playerGroup);
	}

	public void Update(Vector3 up, Vector3 forward, Vector3 position)
	{
		_stream.Position = 0;

		_data.Update(up, forward, position);

		_data.Write(_stream);
	}

	public void Dispose()
	{
		_stream.Dispose();
	}

	[DllImport("libc")]
	private static extern uint getuid();
}