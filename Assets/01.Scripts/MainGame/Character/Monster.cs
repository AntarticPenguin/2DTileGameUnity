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

    void Update()
    {
        if (false == _isLive)
            return;
    }
}
