using BenchmarkDotNet.Attributes;
using SPT.MumbleLink.Services;
using UnityEngine;

namespace ControlFreak.MumbleLink.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[MonoJob("Mono x64", @"C:\Program Files\Mono\bin\mono.exe")]
[SimpleJob]
public class DataWrite
{
	private MumbleLinkConnection _connection;
	private const string Identity = "12345678";
	private const string Context = "SPT.FIKA.Benchmark";
	private static readonly Vector3 Up = Vector3.up;
	private static readonly Vector3 Forward = Vector3.forward;
	private static readonly Vector3 Position = new(100, 100, 100);

	[GlobalSetup]
	public void Setup()
	{
		_connection = new MumbleLinkConnection(Identity, Context);
	}

	[Benchmark]
	public void Update() => _connection.Update(Up, Forward, Position);
	

	[GlobalCleanup]
	public void Cleanup()
	{
		_connection.Dispose();
	}
}