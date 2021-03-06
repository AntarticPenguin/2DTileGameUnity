﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStateType
{
    NONE,
    IDLE,
    MOVE,
    ATTACK,
    DAMAGED,
    PATHFINDING,
    SELECT_TARGET,
    DEAD,
}

public class State
{
    protected eStateType _nextState = eStateType.NONE;
    protected Character _character;

    virtual public void Init(Character character)
    {
        _character = character;
    }

    virtual public void Start()
    {
        _nextState = eStateType.NONE;
    }

    virtual public void Stop()
    {

    }

    virtual public void Update()
    {
        
    }

    public void NextState(eStateType nextState)
    {
        _nextState = nextState;
    }

    public eStateType GetNextState()
    {
        return _nextState;
    }
}
