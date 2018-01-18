using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public override void Start()
    {
        base.Start();

        int moveX = _character.GetTileX();
        int moveY = _character.GetTileY();

        switch (_character.GetNextDirection())
        {
            case eMoveDirection.LEFT:
                moveX--;
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                break;
            case eMoveDirection.UP:
                moveY++;
                break;
            case eMoveDirection.DOWN:
                moveY--;
                break;
        }
        if (false == _character.MoveStart(moveX, moveY))
        {
            _nextState = eStateType.ATTACK;

            /*
             * 어디다 AttackState를 넣을까? (구현하기 나름)
             * 1. 위에다 AttackState 두기 : 더 유연함, 하지만 해야할 일은 많아짐
            TileMap map = GameManager.Instance.GetMap();
            List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
            for (int i = 0; i < collisionList.Count; i++)
            {
                switch (collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        2. 여기에 AttackState 두기
                        _character.Attack(collisionList[i]);
                        break;
                }
            }
            */
        }
        else
        {
            _character.SetNextDirection(eMoveDirection.NONE);
            _nextState = eStateType.IDLE;
        }
    }
}
