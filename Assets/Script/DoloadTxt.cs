using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DoloadTxt : MonoBehaviour {
    Slider slider;
    Text text;
 	// Use this for initialization
	void Start () {
        slider = transform.parent.GetComponent<Slider>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (slider.value==0)
        {
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            text.text = (slider.value).ToString("00%");
        }
        
	}
}
