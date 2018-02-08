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

    Character _player;

    void Init()
    {

    }

    public void SaveCharacter(Character character)
    {
        _player = character;
    }

    public Character LoadPlayer()
    {
        return _player;
    }
}
