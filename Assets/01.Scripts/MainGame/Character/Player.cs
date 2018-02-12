using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //Unity Functions

    void Awake()
    {
        //character's info init
        _hp = 100;
        _level = 1;
        _attackPoint = _level * 50;

        _expPoint = 0;
        _nextLvExpStat = _level * 100;
        _curExpStat = 0;

        Debug.Log("CALL PLAYER Awake()");
    }

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
            State state = new PathfindingImmediateState();
            state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }
        {
            State state = new PathImmediateBuildState();
            state.Init(this);
            _stateMap[eStateType.BUILD_PATH] = state;
        }

        _state = _stateMap[eStateType.IDLE];
    }
}
