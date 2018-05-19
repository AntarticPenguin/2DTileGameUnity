using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{

    #region SINGLETON

    static UISystem _instance;
    public static UISystem Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = FindObjectOfType<UISystem>();
                if (null == _instance)
                {
                    GameObject obj = new GameObject();
                    obj.name = "UIManager";
                    _instance = obj.AddComponent<UISystem>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    #endregion

    //Unity Functions
    void Awake()
    {
        string filePath = "Prefabs/UI/";
        _hpGuagePrefabs = Resources.Load<GameObject>(filePath + "hpGuage");
        _cooltimeGuagePrefabs = Resources.Load<GameObject>(filePath + "cooltimeGuage");
        _levelTextPrefabs = Resources.Load<GameObject>(filePath + "LevelText");
        _battleMenuPrefabs = Resources.Load<GameObject>(filePath + "battleMenu");
    }

    void Start()
    {

    }

    void Update()
    {

    }

    #region LOAD UI

    GameObject _hpGuagePrefabs;
    GameObject _cooltimeGuagePrefabs;
    GameObject _levelTextPrefabs;
    GameObject _battleMenuPrefabs;

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
        GameObject sliderObject = Instantiate(SliderPrefabs);
        Slider slider = sliderObject.GetComponent<Slider>();
        return slider;
    }

    public Text CreateLevelText()
    {
        GameObject textObject = Instantiate(_levelTextPrefabs);
        Text text = textObject.GetComponent<Text>();
        return text;
    }

    public Canvas CreateBattleMenu()
    {
        GameObject canvasObject = Instantiate(_battleMenuPrefabs);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        return canvas;
    }

    #endregion
}
