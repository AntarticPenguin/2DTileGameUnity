using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : State
{
    Pathfinder _rangeViewer = new Pathfinder();

    public override void Init(Character character)
    {
        base.Init(character);
    }

    public override void Update()
    {
        base.Update();

		if (Input.GetMouseButtonDown(1))
			CloseUI();

		_character.ChargeBehaivor();
        SetNextStateByAction();
    }

    public override void Start()
    {
		base.Start();
		_character.ResetTargetTileCell();
        
		int range = GetViewRange();
		eFindMode mode = GetFindMode();

		_rangeViewer.Init(_character);
		_rangeViewer.SetRange(range);
		_rangeViewer.FindPath(mode, eFindMethod.DISTANCE);
    }

    public override void Stop()
    {
        base.Stop();

        _rangeViewer.Reset();
    }

	void CloseUI()
	{
		_character.CloseBattleMenu();
		_nextState = eStateType.IDLE;
	}

	int GetViewRange()
    {
		switch (_character.GetActionType())
		{
			case eActionType.MOVE:
				return _character.GetMoveRange();
			case eActionType.ATTACK:
				return _character.GetAttackRange();
			default:
				return 0;
		}
	}

	eFindMode GetFindMode()
	{
		switch (_character.GetActionType())
		{
			case eActionType.MOVE:
				return eFindMode.VIEW_MOVERANGE;
			case eActionType.ATTACK:
				return eFindMode.VIEW_ATTACKRANGE;
			default:
				return eFindMode.NONE;
		}
	}

	void SetNextStateByAction()
    {
        switch(_character.GetActionType())
        {
            case eActionType.MOVE:
                MoveAction();
                break;
			case eActionType.ATTACK:
				AttackAction();
				break;
            default:
                break;
        }
    }

    void MoveAction()
    {
        TileCell targetCell = _character.GetTargetTileCell();
		if (null == targetCell)
			return;

		if (!(targetCell.GetTileX() == _character.GetTileX()
            && targetCell.GetTileY() == _character.GetTileY()))
        {
            if (_rangeViewer.CheckRange(targetCell))
                _nextState = eStateType.PATHFINDING;
        }
    }

	void AttackAction()
	{
		TileCell targetCell = _character.GetTargetTileCell();
		if (null == targetCell)
			return;

		if(_rangeViewer.CheckRange(targetCell))
		{
			List<MapObject> collisonList = targetCell.GetCollsionList();
			for (int i = 0; i < collisonList.Count; i++)
			{
				MapObject mapObject = collisonList[i];
				if (eMapObjectType.MONSTER == mapObject.GetObjectType())
				{
					_character.SetTarget(mapObject);
					_nextState = eStateType.ATTACK;
					return;
				}
			}
		}
	}
}
