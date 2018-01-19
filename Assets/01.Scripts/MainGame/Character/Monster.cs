using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{ 
    //Unity Functions

    void Start()
    {
        _type = eMapObjectType.MONSTER;
    }

    override public void InitState()
    {
        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new MoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new AttackState();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new DamagedState();
            state.Init(this);
            _stateMap[eStateType.DAMAGED] = state;
        }
        {
            State state = new DeadState();
            state.Init(this);
            _stateMap[eStateType.DEAD] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}
