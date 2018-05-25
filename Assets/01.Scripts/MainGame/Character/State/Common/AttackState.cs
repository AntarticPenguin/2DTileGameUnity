using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	override public void Start ()
    {
        base.Start();

        SoundPlayer.Instance.playEffect("player_hit");

		Debug.Log("ATTACK");
        
        _nextState = eStateType.IDLE;
    }

	public override void Stop()
	{
		base.Stop();
		_character.SetCanBattle(false);
		_character.DecreaseBehavior(2);
	}
}
