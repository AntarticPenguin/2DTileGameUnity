using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingMoveState : State {

    Stack<TileCell> _pathTileCellStack;

    override public void Start ()
    {
        base.Start();
        _pathTileCellStack = _character.GetPathTileCellStack();
        
        if(0 < _pathTileCellStack.Count)
            _pathTileCellStack.Pop();       //처음위치 빼주기
    }

    public override void Stop()
    {
        base.Stop();
        _pathTileCellStack.Clear();
        _character.ResetTargetTileCell();
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

            eMoveDirection direction = GlobalUtility.GetDirection(curPosition, nextPosition);
            _character.SetNextDirection(direction);

            List<MapObject> collisionList = GameManager.Instance.GetMap().GetCollisionList(nextPosition.x, nextPosition.y);
            if (0 != collisionList.Count)
            {
                for(int i = 0; i < collisionList.Count; i++)
                {
                    switch(collisionList[i].GetObjectType())
                    {
                        case eMapObjectType.MONSTER:
                            if (true == _character.IsAttackCoolDown())
                                _nextState = eStateType.ATTACK;
                            else
                                _nextState = eStateType.IDLE;
                            break;

                        default:
                            _nextState = eStateType.IDLE;
                            break;
                    }
                }
            }
            else
            {
                _character.MoveStart(nextPosition.x, nextPosition.y);
            }
        }
        else
        {
            _nextState = eStateType.IDLE;
        }
    }
}
