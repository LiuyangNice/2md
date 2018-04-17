using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InfoUpdate : MonoBehaviour,IPointerUpHandler,IEndDragHandler {
    public RectTransform rectTransform ;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rectTransform.localPosition.y < -50)
        {
            NetworkManager.inst.GetDirectory(FileManager.Inst.current.dir.id);
            FileCon.Inst.refreshShow.SetActive(true);
            FileCon.Inst.refreshShow.transform.GetChild(0).GetComponent<Image>().enabled = true;
            RectTransform rect= FileCon.Inst.refreshShow.transform as RectTransform;
            rect.sizeDelta = new Vector2(1075, 200);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
       
	}
    
}
