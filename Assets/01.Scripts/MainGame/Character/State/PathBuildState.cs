﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuildState : State
{
    TileCell _reverseTileCell = null;


    override public void Start ()
    {
        base.Start();

        _reverseTileCell = _character.GetTargetTileCell();
    }

    override public void Stop()
    {
        base.Stop();
        _character.ResetTargetTileCell();
    }

    override public void Update ()
    {
        UpdateBuildPath();
    }

    protected void UpdateBuildPath()
    {
        if (null != _reverseTileCell)
        {
            //경로를 그려준다
            //_reverseTileCell.DrawColor2();
            _character.PushPathTileCell(_reverseTileCell);
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }
}
