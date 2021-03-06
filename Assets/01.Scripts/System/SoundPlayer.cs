﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

    //Singleton

    static SoundPlayer _instance;
    public static SoundPlayer Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = FindObjectOfType<SoundPlayer>();
                if (null == _instance)
                {
                    GameObject obj = new GameObject();
                    obj.name = "SoundPlayer";
                    _instance = obj.AddComponent<SoundPlayer>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

        
    //Unity Functions
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }


    //SoundPlayer

    AudioSource _audioSource;

    public void playEffect(string soundName)
    {
        string filePath = "Sounds/Effects/" + soundName;
        AudioClip clip = Resources.Load<AudioClip>(filePath);

        if (null != clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
