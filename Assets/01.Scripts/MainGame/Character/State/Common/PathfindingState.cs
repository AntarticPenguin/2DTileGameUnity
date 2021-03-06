﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingState : State
{
    Pathfinder _pathfinder = new Pathfinder();

    override public void Start()
    {
        base.Start();

        _pathfinder.Init(_character);

        //경로 탐색
        _pathfinder.FindPath(eFindMode.FIND_PATH, eFindMethod.ASTAR);

        //경로 도출
        _pathfinder.BuildPath();
        _nextState = eStateType.MOVE;
    }

    override public void Stop()
    {
        base.Stop();
        _pathfinder.Reset();
    }
}
