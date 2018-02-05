using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
	override public void Start ()
    {
        base.Start();

        SoundPlayer.Instance.playEffect("player_hit");

        sPosition position;
        position.x = _character.GetTileX();
        position.y = _character.GetTileY();
        
        sPosition nextPosition = GlobalUtility.GetPositionByDirection(position, _character.GetNextDirection());

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(nextPosition.x, nextPosition.y);
        for (int i = 0; i < collisionList.Count; i++)
        {
            switch (collisionList[i].GetObjectType())
            {
                case eMapObjectType.MONSTER:
                    _character.Attack(collisionList[i]);
                    break;
            }
        }

        _character.ResetAttackCoolTime();
        _character.SetNextDirection(eMoveDirection.NONE);
        _nextState = eStateType.IDLE;
    }
}
