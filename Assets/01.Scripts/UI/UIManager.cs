using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    #region SINGLETON

    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new UIManager();
                _instance.Init();
            }
            return _instance;
        }
    }

    #endregion

    #region LOAD UI

    GameObject _hpGuagePrefabs;
    GameObject _cooltimeGuagePrefabs;
    GameObject _levelTextPrefabs;

    GameObject _battleMenuPrefabs;

    void Init()
    {
        string filePath = "Prefabs/UI/";
        _hpGuagePrefabs = Resources.Load<GameObject>(filePath + "hpGuage");
        _cooltimeGuagePrefabs = Resources.Load<GameObject>(filePath + "cooltimeGuage");
        _levelTextPrefabs = Resources.Load<GameObject>(filePath + "LevelText");
        _battleMenuPrefabs = Resources.Load<GameObject>(filePath + "battleMenu");
    }

    public Slider CreateHPSlider()
    {
        return CreateSlider(_hpGuagePrefabs);
    }

    public Slider CreateCooltimeSlider()
    {
        return CreateSlider(_cooltimeGuagePrefabs);
    }

    public Slider CreateSlider(GameObject SliderPrefabs)
    {
        GameObject sliderObject = Object.Instantiate(SliderPrefabs);
        Slider slider = sliderObject.GetComponent<Slider>();
        return slider;
    }

    public Text CreateLevelText()
    {
        GameObject textObject = Object.Instantiate(_levelTextPrefabs);
        Text text = textObject.GetComponent<Text>();
        return text;
    }

    public Canvas CreateBattleMenu()
    {
        GameObject canvasObject = Object.Instantiate(_battleMenuPrefabs);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        return canvas;
    }

    #endregion
}
