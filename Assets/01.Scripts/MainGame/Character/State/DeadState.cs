using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
	// Use this for initialization
	override public void Start ()
    {
        base.Start();

        _character.SetCanMove(true);
    }

}
