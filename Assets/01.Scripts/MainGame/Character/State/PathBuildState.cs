using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBuildState : State
{
    TileCell _reverseTileCell;


    override public void Start ()
    {
        base.Start();

        _reverseTileCell = _character.GetTargetTileCell();
    }

    public override void Stop()
    {
        base.Stop();
    }

    override public void Update ()
    {
        base.Update();

        if (null != _reverseTileCell)
        {
            _character.PushPathTileCell(_reverseTileCell);

            //경로를 그려준다
            _reverseTileCell.DrawColor2();
            _reverseTileCell = _reverseTileCell.GetPrevTileCell();
        }
        else
        {
            _nextState = eStateType.MOVE;
        }
    }
}
