using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScene : MonoBehaviour {

    public MainGameUI GameUI;
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

        Character player = DataManager.Instance.LoadPlayer();
        player.BecomeViewer();

    }
}
