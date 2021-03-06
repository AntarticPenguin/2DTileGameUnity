﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameScene : MonoBehaviour
{
    public TileMap _tileMap;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        MessageSystem.Instance.ProcessMessage();
    }

    
    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);
        GameManager.Instance.SetMapType(eMapType.TOWN);

        Character player = CreateCharacter("Player", "character03");
        Character monster = CreateCharacter("Monster", "character02");
        player.BecomeViewer();
    }

    Character CreateCharacter(string fileName, string resourceName)
    {
        string filePath = "Prefabs/CharacterFrame/Character";
        GameObject characterPrefabs = Resources.Load<GameObject>(filePath);

        GameObject characterGameObject = Object.Instantiate(characterPrefabs);
        characterGameObject.transform.SetParent(_tileMap.transform);
        characterGameObject.transform.localPosition = Vector3.zero;

        Character character = null;
        switch(fileName)
        {
            case "Player":
                character = characterGameObject.AddComponent<Player>();
                break;
            case "Monster":
                character = characterGameObject.AddComponent<Monster>();
                break;

        }
        character.Init(resourceName);

		Slider hpGuage = UISystem.Instance.CreateHPSlider();
		character.LinkHPGuage(hpGuage);

		Slider cooltimeGuage = UISystem.Instance.CreateCooltimeSlider();
		character.LinkCooltimeGuage(cooltimeGuage);

		Text levelText = UISystem.Instance.CreateLevelText();
		character.LinkLevelText(levelText);

		return character;
    }
}
