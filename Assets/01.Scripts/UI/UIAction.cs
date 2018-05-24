using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAnimationMenu
{
	MOVE,
	ITEM,
	ATTACK,
	MAGIC,
	REST,
	MAX_COUNT,
}

public class UIAction : MonoBehaviour
{
	Character _actor;
	Animator[] _animator;
	Dictionary<eAnimationMenu, Animator> _animationMap = new Dictionary<eAnimationMenu, Animator>();

	void Start()
    {
		_actor = gameObject.GetComponentInParent<Character>();
		_animationMap[eAnimationMenu.MOVE] = transform.Find("move").gameObject.GetComponent<Animator>();
		_animationMap[eAnimationMenu.ITEM] = transform.Find("item").gameObject.GetComponent<Animator>();
		_animationMap[eAnimationMenu.ATTACK] = transform.Find("attack").gameObject.GetComponent<Animator>();
		_animationMap[eAnimationMenu.MAGIC] = transform.Find("magic").gameObject.GetComponent<Animator>();
		_animationMap[eAnimationMenu.REST] = transform.Find("rest").gameObject.GetComponent<Animator>();
	}

    void Update()
    {
		if (_actor.GetBehaviorPoint() < 10)
			_animationMap[eAnimationMenu.MOVE].SetTrigger("disabled");
		else
			_animationMap[eAnimationMenu.MOVE].SetTrigger("ready");

		if (_actor.canBattle())
			SetBattleUITrigger("ready");
		else
			SetBattleUITrigger("disabled");
    }

	void SetBattleUITrigger(string trigger)
	{
		for (int i = (int)eAnimationMenu.ITEM; i < (int)eAnimationMenu.MAX_COUNT; i++)
		{
			_animationMap[(eAnimationMenu)i].SetTrigger(trigger);
		}
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

    public void UIAttack()
    {
        if(_actor.canBattle())
		{
			_actor.CloseBattleMenu();
			_actor.SetActionType(eActionType.ATTACK);
			_actor.ChangeState(eStateType.SELECT_TARGET);
		}
    }
}
