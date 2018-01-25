using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //Unity Functions
	
	void Start ()
    {
        base.InitState();

        {
            State state = new PathfindingIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new PathfindingMoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new PathfindingState();
            state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }
        {
            State state = new PathBuildState();
            state.Init(this);
            _stateMap[eStateType.BUILD_PATH] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }
}
