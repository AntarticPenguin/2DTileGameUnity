using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<DataManager>();
                if (null == _instance)
                {
                    GameObject obj = new GameObject();
                    obj.name = "DataManager";
                    _instance = obj.AddComponent<DataManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    //Unity Functions

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    sCharacterInfo _playerInfo = new sCharacterInfo();

    void Init()
    {

    }

    public void SaveCharacter(sCharacterInfo info)
    {
        _playerInfo.hp = info.hp;
        _playerInfo.attackPoint = info.attackPoint;
        _playerInfo.level = info.level;
        _playerInfo.expPoint = info.expPoint;
        _playerInfo.nextLvExpStat = info.nextLvExpStat;
        _playerInfo.curExpStat = info.curExpStat;
}

    public sCharacterInfo LoadData()
    {
        return _playerInfo;
    }
}
