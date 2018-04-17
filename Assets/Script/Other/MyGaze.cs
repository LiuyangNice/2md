using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class MyGaze :MonoBehaviour, IPointerExitHandler,IGvrPointerHoverHandler {
    public void OnGvrPointerHover(PointerEventData eventData)
    {
        aimPoint.fillAmount += Time.deltaTime*0.3f;
        if (aimPoint.fillAmount>=1)
        {
            ev.Invoke();
            aimPoint.fillAmount = 0;
        }
    }
    Image aimPoint;
    public UnityEvent ev;
    // Use this for initialization
    void Start () {
        aimPoint = Camera.main.transform.Find("Canvas").Find("AimPoint").GetComponent<Image>();
        if (aimPoint.fillAmount!=0)
        {
            aimPoint.fillAmount = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        aimPoint.fillAmount = 0;
    }
}
