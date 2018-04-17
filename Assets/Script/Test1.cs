using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class Test1 : MonoBehaviour,IPointerEnterHandler{
 
    public UnityEvent ev;
    public Text text;
    public void OnPointerEnter(PointerEventData eventData)
    {

        text.text = "OK";
        Debug.Log("成功");
    }

    // Use this for initialization
    void Start () {
        text = GetComponentInChildren<Text>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount==2)
        {
            ev.Invoke();
        }
        text.text = Input.touchCount.ToString();
    }
    
}
