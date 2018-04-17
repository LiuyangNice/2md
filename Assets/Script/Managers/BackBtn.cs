using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class BackBtn : MonoBehaviour,IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        FileCon.Inst.Back();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
