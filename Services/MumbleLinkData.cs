using System;
using System.Buffers;
using System.IO;
using System.Text;
using UnityEngine;

namespace SPT.MumbleLink.Services;

public sealed class MumbleLinkData(string identity, string context)
{
	static MumbleLinkData()
	{
		var isUnix = Environment.OSVersion.Platform == PlatformID.Unix;
		PlatformEncoding = isUnix ? Encoding.UTF32 : Encoding.Unicode;
		Size = isUnix ? 10580 : 5460;
		PlatformByteSize = PlatformEncoding.GetByteCount(" ");
		UTF8ByteSize = Encoding.UTF8.GetByteCount(" ");
	}

	public static readonly int Size;

	private static readonly ArrayPool<byte> MemoryManager = ArrayPool<byte>.Create();
	private static readonly int PlatformByteSize;
	private static readonly int UTF8ByteSize;
	private static readonly Encoding PlatformEncoding;
	
	private const uint UIVersion = 2;
	private const string Name = "Spt.MumbleLink";
	private const string Description = "A Mumble link plugin for Single Player Tarkov.";

	private uint _uiTick;
	private Vector3 _fCameraPosition;
	private Vector3 _fCameraFront;
	private Vector3 _fCameraTop;

	public void Update(Vector3 top, Vector3 front, Vector3 position)
	{
		_uiTick++;
		_fCameraTop = top;
		_fCameraFront = front;
		_fCameraPosition = position;
	}

	public void Write(Stream stream)
	{
		using var writer = new BinaryWriter(stream, PlatformEncoding, true);
		writer.Write(UIVersion);
		writer.Write(_uiTick);
		WriteVec3(writer, _fCameraPosition);
		WriteVec3(writer, _fCameraFront);
		WriteVec3(writer, _fCameraTop);
		WritePlatformString(writer, 256, Name);
		WriteVec3(writer, _fCameraPosition);
		WriteVec3(writer, _fCameraFront);
		WriteVec3(writer, _fCameraTop);
		WritePlatformString(writer, 256, identity);
		WriteUtf8String(writer, 256, context);
		WritePlatformString(writer, 2048, Description);
	}

	private static void WriteVec3(BinaryWriter writer, Vector3 vector)
	{
		writer.Write(vector.x);
		writer.Write(vector.y);
		writer.Write(vector.z);
	}

	private static void WritePlatformString(BinaryWriter writer, int size, string value)
	{
		var bufferLength = PlatformByteSize * size;
		var buffer = MemoryManager.Rent(bufferLength);
		
		PlatformEncoding.GetBytes(value, 0, value.Length, buffer, 0);

		writer.Write(buffer, 0, bufferLength);
		MemoryManager.Return(buffer, true);
	}
	
	private static void WriteUtf8String(BinaryWriter writer, int size, string value)
	{
		var bufferLength = UTF8ByteSize * size;
		var buffer = MemoryManager.Rent(bufferLength);
		
		var encodedBufferLength = Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);

		writer.Write((uint)encodedBufferLength);
		writer.Write(buffer, 0, bufferLength);
		MemoryManager.Return(buffer, true);
	}
}