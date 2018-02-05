using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public override void Start()
    {
        base.Start();

        sPosition position;
        position.x = _character.GetTileX();
        position.y = _character.GetTileY();

        sPosition nextPosition = GlobalUtility.GetPositionByDirection(position, _character.GetNextDirection());
        eMoveDirection nextDirection = GlobalUtility.GetDirection(position, nextPosition);
        _character.SetNextDirection(nextDirection);

        List<MapObject> collisionList = GameManager.Instance.GetMap().GetCollisionList(nextPosition.x, nextPosition.y);
        if (0 != collisionList.Count)
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjectType())
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

        //if (false == _character.MoveStart(nextPosition.x, nextPosition.y))
        //{
        //    if (true == _character.IsAttackCoolDown())
        //        _nextState = eStateType.ATTACK;
        //    else
        //        _nextState = eStateType.IDLE;
        //}
        //else
        //{
        //    _character.SetNextDirection(eMoveDirection.NONE);
        //    _nextState = eStateType.IDLE;
        //}
    }
}
