using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathImmediateBuildState : PathBuildState
{
    public override void Start()
    {
        base.Start();

        while(eStateType.MOVE != _nextState)
        {
            UpdateBuildPath();
        }
    }

}
