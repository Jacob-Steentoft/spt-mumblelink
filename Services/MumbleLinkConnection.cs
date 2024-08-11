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

	private readonly MumbleLinkDataWriter _dataWriter;

	public MumbleLinkConnection(string playerId, string playerGroup)
	{
		using var mappedFile = MemoryMappedFile.CreateOrOpen(MapName, MumbleLinkDataWriter.Bytes);
		var stream = mappedFile.CreateViewStream(0, MumbleLinkDataWriter.Bytes);
		_dataWriter = new MumbleLinkDataWriter(playerId, playerGroup, stream);
	}

	public void Update(Vector3 up, Vector3 forward, Vector3 position)
	{
		_dataWriter.Update(up, forward, position);

		_dataWriter.Write();
	}

	public void Dispose()
	{
		_dataWriter.Dispose();
	}

	[DllImport("libc")]
	private static extern uint getuid();
}