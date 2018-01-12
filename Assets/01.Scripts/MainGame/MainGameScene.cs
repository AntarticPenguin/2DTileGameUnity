using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }


    void Init()
    {
        _tileMap.Init();
        GameManager.Instance.SetMap(_tileMap);

        //CreatePlayer("Player");
        //CreateMonster("Monster");
        Character player = CreateCharacter("Player", "character01");
        Character monster = CreateCharacter("Monster", "character02");
        player.BecomeViewer();
    }

    Character CreateCharacter(string fileName, string resourceName)
    {
        //string filePath = "Prefabs/CharacterFrame/" + fileName;
        string filePath = "Prefabs/CharacterFrame/Character";
        GameObject characterPrefabs = Resources.Load<GameObject>(filePath);

        GameObject characterGameObject = GameObject.Instantiate(characterPrefabs);
        characterGameObject.transform.SetParent(_tileMap.transform);
        characterGameObject.transform.localPosition = Vector3.zero;

        Character character = null;
        switch(fileName)
        {
            case "Player":
                //character = characterGameObject.GetComponent<Player>();
                character = characterGameObject.AddComponent<Player>();
                break;
            case "Monster":
                //character = characterGameObject.GetComponent<Monster>();
                character = characterGameObject.AddComponent<Monster>();
                break;

        }
        character.Init(resourceName);

        return character;
    }

    //void CreateMonster(string fileName)
    //{
    //    GameObject playerPrefabs = Resources.Load<GameObject>("Prefabs/CharacterFrame/Player");
    //    string filePath = "Prefabs/CharacterFrame/" + fileName;
    //    GameObject monsterPrefabs = Resources.Load<GameObject>(filePath);

    //    GameObject monsterGameObject = GameObject.Instantiate(monsterPrefabs);
    //    monsterGameObject.transform.SetParent(_tileMap.transform);
    //    monsterGameObject.transform.localPosition = Vector3.zero;

    //    Character monster = monsterGameObject.GetComponent<Monster>();
    //    monster.Init("character02");
    //}
}
