using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Character
{
    //Unity Functions

    private void Awake()
    {
        _type = eMapObjectType.MONSTER;
        _expPoint = 101;
        _dropItemIndex = 10;
    }

    void Start()
    {
        
    }

    override public void InitState()
    {
        base.InitState();
        {
            State state = new MonsterIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }
}
