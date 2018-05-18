using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : State
{
    Pathfinder _rangeViewer = new Pathfinder();

    public override void Start()
    {
        base.Start();

        _character.OpenBattleMenu();
        _rangeViewer.Init(_character);
    }
}
