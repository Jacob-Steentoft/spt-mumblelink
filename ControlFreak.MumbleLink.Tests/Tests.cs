using SPT.MumbleLink.Services;
using UnityEngine;
using Xunit;

namespace SPT.MumbleLink.Tests;

public class Tests
{
	[Fact]
	public void CanCreateAndUseMumbleLink()
	{
		using var connection = new MumbleLinkConnection("test", "testing");
		connection.Update(Vector3.up, Vector3.forward, Vector3.zero);
		
		Assert.True(true);
	}
}