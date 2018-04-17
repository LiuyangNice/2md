using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class PlayFuns : MonoBehaviour,IPointerClickHandler{
    public UnityEvent ev;
    
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData point)
    {
        ev.Invoke();
    }
    public void PointClick()
    {
        ev.Invoke();
    }
 
}
