using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ControlFreak.MumbleLink.Benchmarks.Benchmarks;

namespace ControlFreak.MumbleLink.Benchmarks;

public class Program
{
	public static void Main(string[] args)
	{
		BenchmarkRunner.Run<DataWrite>(DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator));
	}
}
