using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTargetState : State
{
    Pathfinder _rangeViewer = new Pathfinder();

    public override void Init(Character character)
    {
        base.Init(character);

        _rangeViewer.Init(_character);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Start()
    {
        base.Start();
        
        int range = GetViewRange();
        _rangeViewer.FindPath(eFindMethod.DISTANCE);
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
}
