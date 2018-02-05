using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State {

    Stack<TileCell> _pathTileCellStack;

    override public void Start ()
    {
        base.Start();
        _pathTileCellStack = _character.GetPathTileCellStack();
        _pathTileCellStack.Pop();       //처음위치 빼주기

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

            sPosition curPosition;
            curPosition.x = _character.GetTileX();
            curPosition.y = _character.GetTileY();

            sPosition nextPosition;
            nextPosition.x = tileCell.GetTileX();
            nextPosition.y = tileCell.GetTileY();

            eMoveDirection direction = GetDirection(curPosition, nextPosition);
            _character.SetNextDirection(direction);

            if (false == _character.MoveStart(tileCell.GetTileX(), tileCell.GetTileY()))
            {
                if (true == _character.IsAttackCoolDown())
                    _nextState = eStateType.ATTACK;
                else
                    _nextState = eStateType.IDLE;
            }
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }

    eMoveDirection GetDirection(sPosition curPosition, sPosition nextPosition)
    {
        if (nextPosition.x > curPosition.x)
            return eMoveDirection.RIGHT;
        else if (curPosition.x > nextPosition.x)
            return eMoveDirection.LEFT;
        else if (curPosition.y > nextPosition.y)
            return eMoveDirection.DOWN;
        else if (nextPosition.y > curPosition.y)
            return eMoveDirection.UP;

        return eMoveDirection.UP;
    }
}
