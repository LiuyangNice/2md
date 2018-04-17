using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LabelColorChange : MonoBehaviour {
    public Toggle tog;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (tog.isOn)
        {
            GetComponent<Text>().color = Color.gray;
        }
        else
        {
            GetComponent<Text>().color = Color.green;
        }
	}
}
