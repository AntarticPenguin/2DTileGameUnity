using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingImmediateState : PathfindingState
{

    override public void Start()
    {
        base.Start();
        
        //탐색
        while(0 != _pathfindingQueue.Count)
        {
            if (eStateType.BUILD_PATH == _nextState)
                break;

            UpdatePathfinding();
        }

        //경로
        _nextState = eStateType.BUILD_PATH;
    }

    override public void Stop()
    {
        base.Stop();
    }

    override public void Update()
    {

    }
}
