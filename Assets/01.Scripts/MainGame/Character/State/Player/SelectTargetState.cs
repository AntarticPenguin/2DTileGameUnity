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
		_character.ChargeBehaivor();

        SetNextStateByAction();
    }

    public override void Start()
    {
        base.Start();
        _character.ResetTargetTileCell();
        
        int range = GetViewRange();

        _rangeViewer.Init(_character);
		_rangeViewer.SetRange(range);
        _rangeViewer.FindPath(eFindMode.VIEW_RANGE, eFindMethod.DISTANCE);
    }

    public override void Stop()
    {
        base.Stop();

        _rangeViewer.Reset();
    }

    int GetViewRange()
    {
		Debug.Log(_character.GetActionType().ToString());

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

    void SetNextStateByAction()
    {
        switch(_character.GetActionType())
        {
            case eActionType.MOVE:
                MoveAction();
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
}
