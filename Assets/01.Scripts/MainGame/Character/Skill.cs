using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
	string _skillName;
	string _spriteName;
	int _castingRange;
	int _attackRange;

	public void Init(string skillName, string spriteName)
	{
		_skillName = skillName;
		_spriteName = spriteName;
	}

	public void SetCastingRange(int range) { _castingRange = range; }
	public void SetAttackRange(int range) { _attackRange = range; }

}
