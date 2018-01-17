﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}

public class Character : MapObject
{
    protected GameObject _characterView;

    protected int _tileX = 0;
    protected int _tileY = 0;

    protected bool _isLive = true;
    protected int _hp = 20;

    protected int _attackPoint = 10;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //Init

    public void Init(string viewName)
    {
        //View를 붙인다(실제로 보일 모습(이미지), 카메라 아님)
        //Attach Player's View.
        string filePath = "Prefabs/CharacterView/" + viewName;
        GameObject characterViewPrefabs = Resources.Load<GameObject>(filePath);

        _characterView = GameObject.Instantiate(characterViewPrefabs);
        _characterView.transform.SetParent(transform);
        _characterView.transform.localPosition = Vector3.zero;
        _characterView.transform.localScale = Vector3.one;

        SetCanMove(false);

        TileMap map = GameManager.Instance.GetMap();

        _tileX = Random.Range(1, map.GetWidth() - 2);
        _tileY = Random.Range(1, map.GetHeight() - 2);
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        InitState();
    }


    //State

    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;

    void InitState()
    {
        //_stateMap[eStateType.IDLE] = new IdleState();
        {
            State state = new IdleState();
            state.Init(this);
        }
        {
            State state = new MoveState();
            state.Init(this);
        }
        _state = _stateMap[eStateType.IDLE];
    }

    public void ChangeState(eStateType nextState)
    {
        if(null != _state)
            _state.Stop();
        _state = _stateMap[nextState];
        _state.Start();
    }

    override public void SetSortingOrder(eTileLayer layer, int sortingOrder)
    {
        _curLayer = layer;

        int sortingID = SortingLayer.NameToID(layer.ToString());
        _characterView.GetComponent<SpriteRenderer>().sortingLayerID = sortingID;
        _characterView.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }

    public int GetTileX() { return _tileX; }
    public int GetTileY() { return _tileY; }

    eMoveDirection _nextDirection = eMoveDirection.NONE;

    public eMoveDirection GetNextDirection() { return _nextDirection; }
    public void SetNextDirection(eMoveDirection direction) { _nextDirection = direction; }

    public bool MoveStart(int tileX, int tileY)
    {
        string animationTrigger = "up";

        switch (_nextDirection)
        {
            case eMoveDirection.LEFT: animationTrigger = "left";
                break;
            case eMoveDirection.RIGHT: animationTrigger = "right";
                break;
            case eMoveDirection.UP: animationTrigger = "up";
                break;
            case eMoveDirection.DOWN: animationTrigger = "down";
                break;
        }

        _characterView.GetComponent<Animator>().SetTrigger(animationTrigger);

        TileMap map = GameManager.Instance.GetMap();
        List<MapObject> collisionList = map.GetCollisionList(tileX, tileY);
        if (0 == collisionList.Count)
        {
            map.ResetObject(_tileX, _tileY, this);
            _tileX = tileX;
            _tileY = tileY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

            return true;
        }
        return false;
    }


    //Message

    override public void ReceiveObjectMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "Attack":
                //Debug.Log("Receive AttackMessage: " + msgParam.attackPoint);
                Damaged(msgParam.attackPoint);
                break;
        }
    }

    public void Attack(MapObject enemy)
    {
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.message = "Attack";
        msgParam.attackPoint = _attackPoint;

        MessageSystem.Instance.Send(msgParam);
    }

    void Damaged(int attackPoint)
    {
        _hp -= attackPoint;
        if (_hp <= 0)
        {
            _hp = 0;
            _isLive = true;
            _canMove = true;
        }
    }
}
