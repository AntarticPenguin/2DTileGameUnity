using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : Character
{
    //Unity Functions

    void Awake()
    {
        //character's info init
        _hp = 100;
        _level = 1;
        _attackPoint = _level * 50;

        _expPoint = 0;
        _nextLvExpStat = _level * 100;
        _curExpStat = 0;
    }

    void Start ()
    {
        base.InitState();

        {
            State state = new PlayerIdleState();
            state.Init(this);
            _stateMap[eStateType.IDLE] = state;
        }
        {
            State state = new PathfindingMoveState();
            state.Init(this);
            _stateMap[eStateType.MOVE] = state;
        }
        {
            State state = new PathfindingState();
            state.Init(this);
            _stateMap[eStateType.PATHFINDING] = state;
        }
        _state = _stateMap[eStateType.IDLE];
    }

    void Update()
    {
        UpdateCharacter();
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();

        if (Input.GetMouseButtonDown(0))
        {
			Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Ray2D ray = new Ray2D(worldPosition, Vector2.zero);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (null != hit.collider)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on the UI");
                    return;
                }

                string filePath = "Prefabs/Effect/DamageEffect";

                Vector3 pos = new Vector3(hit.transform.position.x, hit.transform.position.y, 1);

                GameObject effectPrefab = Resources.Load<GameObject>(filePath);
                GameObject effectObject = GameObject.Instantiate(effectPrefab, pos, Quaternion.identity);
                GameObject.Destroy(effectObject, 1.0f);

                TileObject hitTile = hit.transform.GetComponent<TileObject>();
                int tileX = hitTile.GetTileX();
                int tileY = hitTile.GetTileY();

                TileCell hitCell = GameManager.Instance.GetMap().GetTileCell(tileX, tileY);
                if (null == hitCell)
                    return;

                if(eMapType.TOWN == GameManager.Instance.GetMapType())
                {
                    SetTargetTileCell(tileX, tileY);
                    ChangeState(eStateType.PATHFINDING);
                }
                else if(eMapType.DUNGEON == GameManager.Instance.GetMapType())
                {
                    if(IsClickedCharacter(hitCell))
                    {
                        //SetTargetTileCell(tileX, tileY);
                        ChangeState(eStateType.PATHFINDING);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            DataManager.Instance.SaveCharacter(GetCharacterInfo());
            SceneManager.LoadScene("Map01");
        }
    }
}
