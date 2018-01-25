using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State {

    Stack<TileCell> _pathTileCellStack;

    override public void Start ()
    {
        base.Start();
        _pathTileCellStack = _character.GetPathTileCellStack();
	}

    public override void Stop()
    {
        base.Stop();
        _pathTileCellStack.Clear();
    }

    public override void Update()
    {
        base.Update();

        if( 0 != _pathTileCellStack.Count)
        {
            TileCell tileCell = _pathTileCellStack.Pop();

            int fromX = _character.GetTileX();
            int fromY = _character.GetTileY();
            int toX = tileCell.GetTileX();
            int toY = tileCell.GetTileY();

            if (toX < fromX)
                _character.SetNextDirection(eMoveDirection.LEFT);
            else if(fromX < toX)
                _character.SetNextDirection(eMoveDirection.RIGHT);
            else if (toY < fromY)
                _character.SetNextDirection(eMoveDirection.DOWN);
            else if (fromY < toY)
                _character.SetNextDirection(eMoveDirection.UP);

            _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY());
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }
}
