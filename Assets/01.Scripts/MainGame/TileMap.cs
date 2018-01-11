using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{

    //Unity Functions
	
	void Start ()
    {
        
	}
	
	void Update ()
    {
		
	}


    //SpriteList

    Sprite[] _spriteArray;

    public void Init()
    {
        _spriteArray = Resources.LoadAll<Sprite>("Sprites/MapSprite");
        CreateTiles();
    }


    //Tile

    public GameObject TileObjectPrefabs;

    int _width;
    int _height;

    //List<TileCell> _tileCellList = new List<TileCell>();
    TileCell[,] _tileCellList;

    void CreateTiles()
    {
        float tileSize = 32.0f;

        //1층
        {
            TextAsset scriptAsset = Resources.Load<TextAsset>("Data/MapData_layer1");
            string[] records = scriptAsset.text.Split('\n');

            {
                string[] token = records[0].Split(',');
                _width = int.Parse(token[1]);
                _height = int.Parse(token[2]);
            }
            _tileCellList = new TileCell[_height, _width];

            for (int y = 0; y < _height; y++)
            {
                int line = y + 2;
                string[] token = records[line].Split(',');
                for (int x = 0; x < _width; x++)
                {
                    int spriteIndex = int.Parse(token[x]);

                    GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                    tileGameObject.transform.SetParent(transform);
                    tileGameObject.transform.localScale = Vector3.one;
                    tileGameObject.transform.localPosition = Vector3.zero;

                    TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                    tileObject.Init(_spriteArray[spriteIndex]);

                    /*
                    TileCell tileCell = new TileCell();
                    tileCell.Init();
                    tileCell.SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                    tileCell.AddObject(eTileLayer.GROUND, tileObject);
                    _tileCellList.Add(tileCell);
                    */
                    _tileCellList[y, x] = new TileCell();
                    GetTileCell(x, y).Init();
                    GetTileCell(x, y).SetPosition(x * tileSize / 100.0f, y * tileSize / 100.0f);
                    GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                }
            }
        }

        //2층
        {
            TextAsset scriptAsset = Resources.Load<TextAsset>("Data/MapData_layer2");
            string[] records = scriptAsset.text.Split('\n');
            for (int y = 0; y < _height; y++)
            {
                int line = y + 2;
                string[] token = records[line].Split(',');
                for (int x = 0; x < _width; x++)
                {
                    int spriteIndex = int.Parse(token[x]);
                    if(0<= spriteIndex)
                    {
                        GameObject tileGameObject = GameObject.Instantiate(TileObjectPrefabs);
                        tileGameObject.transform.SetParent(transform);
                        tileGameObject.transform.localScale = Vector3.one;
                        tileGameObject.transform.localPosition = Vector3.zero;

                        TileObject tileObject = tileGameObject.GetComponent<TileObject>();
                        tileObject.Init(_spriteArray[spriteIndex]);

                        /*
                        int cellIndex = (y * _width) + x;
                        _tileCellList[cellIndex].AddObject(eTileLayer.GROUND, tileObject);
                        */
                        GetTileCell(x, y).AddObject(eTileLayer.GROUND, tileObject);
                    }
                }
            }
        }
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

    public TileCell GetTileCell(int x, int y)
    {
        return _tileCellList[y, x];
    }
}
