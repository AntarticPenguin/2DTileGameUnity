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
            case eMoveDirection.LEFT: moveX--; break;
            case eMoveDirection.RIGHT: moveX++; break;
            case eMoveDirection.UP: moveY++; break;
            case eMoveDirection.DOWN: moveY--; break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        if (map.CanMoveTile(moveX, moveY))
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = moveX;
            _tileY = moveY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
    }
}
