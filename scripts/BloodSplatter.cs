using Godot;
using System;

public partial class BloodSplatter : CpuParticles2D
{
	private void OnTimerTimeout()
	{
		SetProcess(false);
		SetPhysicsProcess(false);
		SetProcessInput(false);
		SetProcessInternal(false);
		SetProcessUnhandledInput(false);
		SetProcessUnhandledKeyInput(false);
	}
}
