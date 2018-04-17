using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeadIcon : MonoBehaviour {
    public directory dir;
    Text txt;
    Button btn;
	// Use this for initialization
	void Start () {
        txt = transform.GetComponentInChildren<Text>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
	}
	
	// Update is called once per frame
	void Update () {
        txt.text = dir.name;
	}
    void OnClick()
    {
        //UIManager.Inst.OnHeadClick(dir);
    }
}
