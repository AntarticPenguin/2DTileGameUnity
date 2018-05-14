using System.Collections;
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
        if (eMapType.TOWN == GameManager.Instance.GetMapType())
            _pathfinder.FindPath(eFindMethod.ASTAR);
        else
        {
            _pathfinder.FindPath(eFindMethod.DISTANCE);
        }

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
