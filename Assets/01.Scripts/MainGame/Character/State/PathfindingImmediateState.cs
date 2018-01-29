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
            UpdatePathfinding();
        }

        //구축

    }

    override public void Stop()
    {
        base.Stop();
    }
}
