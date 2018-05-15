using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMoveDirection
{
    NONE,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}

public struct sCharacterInfo
{
    public int hp;
    public int attackPoint;
    public int level;
    public int expPoint;
    public int nextLvExpStat;
    public int curExpStat;
}

public class Character : MapObject
{ 
    protected GameObject _characterView;

    protected bool _isLive = true;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCharacter();
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

        int tileX = Random.Range(1, map.GetWidth() - 2);
        int tileY = Random.Range(1, map.GetWidth() - 2);
        while(false == map.CanMoveTile(tileX, tileY))
        {
            tileX = Random.Range(1, map.GetWidth() - 2);
            tileY = Random.Range(1, map.GetWidth() - 2);
        }
        _tileX = tileX;
        _tileY = tileY;
        map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);

        InitState();
    }

    virtual public void UpdateCharacter()
    {
        if (eStateType.NONE != _state.GetNextState())
            ChangeState(_state.GetNextState());

        UpdateAttackCoolTime();
        _state.Update();
        UpdateUI();
    }


    //State

    protected Dictionary<eStateType, State> _stateMap = new Dictionary<eStateType, State>();
    protected State _state;

    virtual public void InitState()
    {
        {
            State state = new PlayerIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new MoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new AttackState();
            state.Init(this);
            _stateMap[eStateType.ATTACK] = state;
        }
        {
            State state = new DamagedState();
            state.Init(this);
            _stateMap[eStateType.DAMAGED] = state;
        }
        {
            State state = new DeadState();
            state.Init(this);
            _stateMap[eStateType.DEAD] = state;
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

    eMoveDirection _nextDirection = eMoveDirection.NONE;

    public eMoveDirection GetNextDirection() { return _nextDirection; }
    public void SetNextDirection(eMoveDirection direction) { _nextDirection = direction; }

    public void MoveStart(int tileX, int tileY)
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

        {
            TileMap map = GameManager.Instance.GetMap();
            map.ResetObject(_tileX, _tileY, this);
            _tileX = tileX;
            _tileY = tileY;
            map.SetObject(_tileX, _tileY, this, eTileLayer.MIDDLE);
        }
    }


    //Message

    override public void ReceiveObjectMessage(MessageParam msgParam)
    {
        switch (msgParam.message)
        {
            case "Attack":
                _damagedPoint = msgParam.attackPoint;
                _whoAttackedMe = (Character)msgParam.sender;
                _state.NextState(eStateType.DAMAGED);
                break;
            case "Died":
                RaiseExp(msgParam.expPoint);
                break;
        }
    }


    //Character's Info

    protected int _hp = 100;
    protected int _attackPoint = 10;
    protected int _moveRange = 6;

    protected int _level = 1;
    protected int _expPoint = 0;
    protected int _nextLvExpStat = 0;
    protected int _curExpStat = 0;

    protected int _dropItemIndex = 0;

    public int GetMoveRange() { return _moveRange; }
    public int GetExpPoint() { return _expPoint; }

    void RaiseExp(int expPoint)
    {
        _curExpStat += expPoint;

        if(_nextLvExpStat <= _curExpStat)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        _level++;
        _curExpStat = _curExpStat - _nextLvExpStat;
        _nextLvExpStat = _level * 100;
        _attackPoint = _level * 50;
        Debug.Log("LEVEL UP!! current EXP is " + _curExpStat);
        Debug.Log("nextLvExp is " + _nextLvExpStat);
        Debug.Log("AttackPoint is " + _attackPoint);
    }

    public int GetItemIndex() { return _dropItemIndex; }

    public sCharacterInfo GetCharacterInfo()
    {
        sCharacterInfo info = new sCharacterInfo();
        info.hp = _hp;
        info.attackPoint = _attackPoint;
        info.level = _level;
        info.nextLvExpStat = _nextLvExpStat;
        info.curExpStat = _curExpStat;

        return info;
    }

    public void SetCharacterInfo(sCharacterInfo info)
    {
        _hp = info.hp;
        _attackPoint = info.attackPoint;
        _level = info.level;
        _nextLvExpStat = info.nextLvExpStat;
        _curExpStat = info.curExpStat;
    }

    //Attack

    float _attackCooltimeDuration = 0.0f;
    float _attackCooltime = 1.0f;
    int _damagedPoint = 0;

    Character _whoAttackedMe;

    public void Attack(MapObject enemy)
    {
        MessageParam msgParam = new MessageParam();
        msgParam.sender = this;
        msgParam.receiver = enemy;
        msgParam.message = "Attack";
        msgParam.attackPoint = _attackPoint;

        MessageSystem.Instance.Send(msgParam);
    }

    public Character WhoAttackedMe()
    {
        return _whoAttackedMe;
    }

    void UpdateAttackCoolTime()
    {
        if (_attackCooltimeDuration < _attackCooltime)
        {
            _attackCooltimeDuration += Time.deltaTime;
        }
        else
        {
            _attackCooltimeDuration = _attackCooltime;
        }
    }

    public bool IsAttackCoolDown()
    {
        if (_attackCooltime <= _attackCooltimeDuration)
            return true;
        return false;
    }

    public void ResetAttackCoolTime()
    {
        _attackCooltimeDuration = 0.0f;
    }

    public int GetDamagedPoint()
    {
        return _damagedPoint;
    }

    public void DecreaseHP(int damagedPoint)
    {
        string filePath = "Prefabs/Effect/DamageEffect";
        GameObject effectPrefab = Resources.Load<GameObject>(filePath);
        GameObject effectObject = GameObject.Instantiate(effectPrefab, transform.position, Quaternion.identity);
        GameObject.Destroy(effectObject, 1.0f);

        _characterView.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);

        _hp -= damagedPoint;
        if (_hp <= 0)
        {
            _hp = 0;
            _isLive = false;
        }
    }

    void ResetColor()
    {
        _characterView.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public bool IsLive()
    {
        return _isLive;
    }


    //UI

    Slider _hpGuage;
    Slider _cooltimeGuage;
    Text _levelText;

    public void LinkHPGuage(Slider hpGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        hpGuage.transform.SetParent(canvasObject.transform);
        hpGuage.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        hpGuage.transform.localScale = Vector3.one;

        _hpGuage = hpGuage;
        _hpGuage.value = _hp / 100.0f;
    }

    public void LinkCooltimeGuage(Slider cooltimeGuage)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        cooltimeGuage.transform.SetParent(canvasObject.transform);
        cooltimeGuage.transform.localPosition = Vector3.zero;
        cooltimeGuage.transform.localScale = Vector3.one;

        _cooltimeGuage = cooltimeGuage;
        _cooltimeGuage.value = _attackCooltimeDuration;
    }

    public void LinkLevelText(Text levelText)
    {
        GameObject canvasObject = transform.Find("Canvas").gameObject;
        levelText.transform.SetParent(canvasObject.transform);
        levelText.transform.localPosition = new Vector3(0.0f, 1.30f, 0.0f);
        levelText.transform.localScale = new Vector3(0.35f, 0.1f, 1.0f);

        _levelText = levelText;
        _levelText.text = "LEVEL " + _level;
    }

    void UpdateUI()
    {
        _hpGuage.value = _hp / 100.0f;
        _cooltimeGuage.value = _attackCooltimeDuration;
        _levelText.text = "LEVEL " + _level;
    }


    //Pathfinding

    TileCell _targetTileCell = null;
    Stack<TileCell> _pathTileCellStack = new Stack<TileCell>();

    public void SetTargetTileCell(int tileX, int tileY)
    {
        TileMap map = GameManager.Instance.GetMap();
        _targetTileCell = map.GetTileCell(tileX, tileY);
    }

    public TileCell GetTargetTileCell()
    {
        return _targetTileCell;
    }

    public void ResetTargetTileCell()
    {
        _targetTileCell = null;
    }

    public void PushPathTileCell(TileCell tileCell)
    {
        _pathTileCellStack.Push(tileCell);
    }

    public Stack<TileCell> GetPathTileCellStack()
    {
        return _pathTileCellStack;
    }

    public bool IsClickedCharacter(TileCell tileCell)
    {
        if (_tileX == tileCell.GetTileX() && _tileY == tileCell.GetTileY())
            return true;
        return false;
    }
}
