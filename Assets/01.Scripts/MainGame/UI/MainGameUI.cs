using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {

    }


    //Play UI

    public GameObject HPGuagePrefabs;
    public GameObject CooltimeGuagePrefabs;

    public Slider CreateHPSlider()
    {
        return CreateSlider(HPGuagePrefabs);
    }

    public Slider CreateCooltimeSlider()
    {
        return CreateSlider(CooltimeGuagePrefabs);
    }

    public Slider CreateSlider(GameObject SliderPrefabs)
    {
        GameObject sliderObject = GameObject.Instantiate(SliderPrefabs);
        Slider slider = sliderObject.GetComponent<Slider>();
        return slider;
    }
}
