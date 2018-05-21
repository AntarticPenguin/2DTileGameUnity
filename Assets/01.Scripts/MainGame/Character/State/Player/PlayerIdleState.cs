using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerIdleState : State
{
    float _chargeDuration;

    public override void Start()
    {
        base.Start();

        _chargeDuration = 0.0f;
    }

    override public void Update()
    {
        base.Update();

        if (_character.GetChargeTime() <= _chargeDuration)
        {
            _chargeDuration = 0.0f;
            _character.ChargeBehaivor();
        }
        else
            _chargeDuration += Time.deltaTime;
	}
}
