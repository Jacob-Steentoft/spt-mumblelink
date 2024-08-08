using System.IO;
using System.Text;
using UnityEngine;

namespace SPT.MumbleLink.Services;

public sealed class MumbleLinkData(string identity, string context)
{
	public const int Size = 5460;
	private const uint UIVersion = 2;
	private const string Name = "Spt.MumbleLink";
	private const string Description = "A Mumble link plugin for Single Player Tarkov.";

	private uint _uiTick;
	private Vector3 _fAvatarPosition;
	private Vector3 _fAvatarFront;
	private Vector3 _fAvatarTop;
	private Vector3 _fCameraPosition;
	private Vector3 _fCameraFront;
	private Vector3 _fCameraTop;

	public void Update(Vector3 top, Vector3 front, Vector3 position)
	{
		_uiTick++;
		_fCameraTop = top;
		_fAvatarTop = top;
		
		_fCameraFront = front;
		_fAvatarFront = front;
		
		_fCameraPosition = position;
		_fAvatarPosition = position;
	}
	
	public void Write(Stream stream)
	{
		using var writer = new BinaryWriter(stream, Encoding.Unicode, true);
		writer.Write(UIVersion);
		writer.Write(_uiTick);
		WriteVec3(writer, _fAvatarPosition);
		WriteVec3(writer, _fAvatarFront);
		WriteVec3(writer, _fAvatarTop);
		WriteString(writer, 256, Name);
		WriteVec3(writer, _fCameraPosition);
		WriteVec3(writer, _fCameraFront);
		WriteVec3(writer, _fCameraTop);
		WriteString(writer, 256, identity);
		WriteString(writer, 256, context, Encoding.UTF8, true);
		WriteString(writer, 2048, Description);
	}
	
	private static void WriteVec3(BinaryWriter writer, Vector3 vector)
	{
		writer.Write(vector.x);
		writer.Write(vector.y);
		writer.Write(vector.z);
	}
		
	private static void WriteString(BinaryWriter writer, int size, string value, Encoding encoding = null, bool lengthPrefixed = false)
	{
		encoding ??= Encoding.Unicode;
		var bytes  = new byte[encoding.GetByteCount(" ") * size];
		var length = encoding.GetBytes(value, 0, value.Length, bytes, 0);
		if (lengthPrefixed) writer.Write((uint)length);
		writer.Write(bytes);
	}
}