using System;
using System.IO.MemoryMappedFiles;
using UnityEngine;

namespace SPT.MumbleLink.Services;

public sealed class MumbleLinkConnection : IDisposable
{
	private readonly MumbleLinkData _data;
	private readonly MemoryMappedViewStream _stream;

	public MumbleLinkConnection(string playerId, string playerGroup)
	{
		using var mappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", MumbleLinkData.Size);
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
		_stream?.Dispose();
	}
}