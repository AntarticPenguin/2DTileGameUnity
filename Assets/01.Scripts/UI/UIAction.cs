using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAction : MonoBehaviour
{
	Character _actor;

	void Start()
    {
		_actor = gameObject.GetComponentInParent<Character>();
		Debug.Log("UIACTION START");
    }

    void Update()
    {

    }

    public void UIMove()
	{
        if (10 == _actor.GetBehaviorPoint())
        {
            _actor.CloseBattleMenu();
            _actor.SetActionType(eActionType.MOVE);
            _actor.ChangeState(eStateType.SELECT_TARGET);
        }
    }
}
