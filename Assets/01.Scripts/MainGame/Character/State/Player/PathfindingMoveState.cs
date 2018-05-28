using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State {

    Stack<TileCell> _pathTileCellStack;
    float _movingDuration;

    public override void Start ()
    {
        base.Start();

        _movingDuration = 0.0f;

        _pathTileCellStack = _character.GetPathTileCellStack();
        if(0 < _pathTileCellStack.Count)
            _pathTileCellStack.Pop();       //처음위치 빼주기
    }

    public override void Stop()
    {
        base.Stop();
        _pathTileCellStack.Clear();

		if(eMapType.DUNGEON == GameManager.Instance.GetMapType())
			_character.DecreaseBehavior(2);
    }

    public override void Update()
    {
        base.Update();

        if (_character.GetMoveSpeed() <= _movingDuration)
        {
            _movingDuration = 0.0f;
            if (0 != _pathTileCellStack.Count)
            {
                TileCell tileCell = _pathTileCellStack.Pop();

                sPosition curPosition;
                curPosition.x = _character.GetTileX();
                curPosition.y = _character.GetTileY();

                sPosition nextPosition;
                nextPosition.x = tileCell.GetTileX();
                nextPosition.y = tileCell.GetTileY();

                eMoveDirection direction = GlobalUtility.GetDirection(curPosition, nextPosition);
                _character.SetNextDirection(direction);

                if (GameManager.Instance.GetMap().CanMoveTile(nextPosition.x, nextPosition.y))
                {
                    _character.MoveStart(nextPosition.x, nextPosition.y);
                }
                else
                {
                    _nextState = eStateType.IDLE;
                }
            }
            else
            {
                _nextState = eStateType.IDLE;
            }
        }
        else
        {
            _movingDuration += Time.deltaTime;
        }
    }
}
