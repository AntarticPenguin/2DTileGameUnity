using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : State
{
    override public void Update()
    {
        if (eStateType.NONE != _nextState)
        {
            _character.ChangeState(_nextState);
        }
    }
}
