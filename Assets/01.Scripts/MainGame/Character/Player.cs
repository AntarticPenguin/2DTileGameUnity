using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //Unity Functions
	
	void Start ()
    {
        
	}
	
	void Update ()
    {
        if (false == _isLive)
            return;

        eMoveDirection moveDirection = _state.Update();
    }
}
