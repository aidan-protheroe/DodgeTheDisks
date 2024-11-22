using Godot;
using System;
using System.Collections.Generic;

public partial class FiniteStateMachine : Node
{
	[Export] public NodePath initialState;
	
	public Dictionary<string, State> _states;
	public State _currentState;
	
	public override void _Ready() 
	{
		LoadStates();
	}
	
	public void LoadStates() {
		_states = new Dictionary<string, State>(); 
		foreach (Node node in GetChildren()) {
			if (node is State s) {
				
				_states[node.Name] = s;
				s.fsm = this;
				s.Player = GetParent<Player>();
				s.Ready();
				s.Exit(); //reset all states
			}
		}
		
		_currentState = GetNode<State>(initialState);
		_currentState.Enter();
	}

	public override void _Process(double delta)
	{
		_currentState.Update((float) delta);
	}
	
	public void TransitionTo(string key) {
		if (!_states.ContainsKey(key) || _currentState == _states[key]) {
			GD.Print("ERROR: NO STATE NAMED '" + key + "' OR ALREADY IN THAT STATE");
			return;
		}
		_currentState.Exit();
		_currentState = _states[key];
		_currentState.Enter();
	}
	
}
