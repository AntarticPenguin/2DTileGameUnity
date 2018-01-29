using System.Collections;
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
        while(null != _reverseTileCell)
        {
            if (null != _reverseTileCell.GetPrevTileCell())
            {
                _character.PushPathTileCell(_reverseTileCell);

                //경로를 그려준다
                //_reverseTileCell.DrawColor2();
                _reverseTileCell = _reverseTileCell.GetPrevTileCell();
            }
            else
            {
                _nextState = eStateType.MOVE;
                break; ;
            }
        }
    }
}
