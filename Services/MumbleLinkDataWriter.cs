using System;
using System.Buffers;
using System.IO;
using System.Text;
using UnityEngine;

namespace SPT.MumbleLink.Services;

public sealed class MumbleLinkDataWriter(string identity, string context, Stream stream) : IDisposable
{
	static MumbleLinkDataWriter()
	{
		var isUnix = Environment.OSVersion.Platform == PlatformID.Unix;
		PlatformEncoding = isUnix ? Encoding.UTF32 : Encoding.Unicode;
		PlatformByteSize = PlatformEncoding.GetByteCount(" ");
		UTF8ByteSize = Encoding.UTF8.GetByteCount(" ");
		Bytes = (256 * 2 + 2048) * PlatformByteSize + 340;
	}

	public static readonly int Bytes;

	private static readonly ArrayPool<byte> MemoryManager = ArrayPool<byte>.Create();
	private static readonly int PlatformByteSize;
	private static readonly int UTF8ByteSize;
	private static readonly Encoding PlatformEncoding;
	
	private const uint UIVersion = 2;
	private const string Name = "Spt.MumbleLink";
	private const string Description = "A Mumble link plugin for Single Player Tarkov.";

	private readonly BinaryWriter _writer =  new(stream, PlatformEncoding, false);
	
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

	public void Write()
	{
		_writer.Write(UIVersion);
		_writer.Write(_uiTick);
		WriteVec3(_writer, _fCameraPosition);
		WriteVec3(_writer, _fCameraFront);
		WriteVec3(_writer, _fCameraTop);
		WritePlatformString(_writer, 256, Name);
		WriteVec3(_writer, _fCameraPosition);
		WriteVec3(_writer, _fCameraFront);
		WriteVec3(_writer, _fCameraTop);
		WritePlatformString(_writer, 256, identity);
		WriteUtf8String(_writer, 256, context);
		WritePlatformString(_writer, 2048, Description);
		stream.Position = 0;
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

	public void Dispose()
	{
		_writer.Dispose();
	}
}