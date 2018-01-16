using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //Unity Functions
	
	void Start ()
    {
        
	}
	
	void Update ()
    {
        eMoveDirection moveDirection = eMoveDirection.NONE;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = eMoveDirection.LEFT;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = eMoveDirection.RIGHT;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = eMoveDirection.UP;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = eMoveDirection.DOWN;
        }

        if (eMoveDirection.NONE != moveDirection)
            Move(moveDirection);
    }


    //Move

    void Move(eMoveDirection moveDirection)
    {
        string animationTrigger = "up";

        int moveX = _tileX;
        int moveY = _tileY;

        switch(moveDirection)
        {
            case eMoveDirection.LEFT:
                moveX--;
                animationTrigger = "left";
                break;
            case eMoveDirection.RIGHT:
                moveX++;
                animationTrigger = "right";
                break;
            case eMoveDirection.UP:
                moveY++;
                animationTrigger = "up";
                break;
            case eMoveDirection.DOWN:
                moveY--;
                animationTrigger = "down";
                break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(moveX, moveY);
        if(0 == collisionList.Count)
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
        else
        {
            for(int i = 0; i< collisionList.Count; i++)
            {
                switch(collisionList[i].GetObjectType())
                {
                    case eMapObjectType.MONSTER:
                        Attack(collisionList[i]);
                        break;
                }
            }
        }

    }

    void Attack(MapObject enemy)
    {
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.message = "Attack";
        msgParam.attackPoint = _attackPoint;

        MessageSystem.Instance.Send(msgParam);
    }
}
