using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class ConBtnCancas : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IGvrPointerHoverHandler{
    Image[] images;
    Vector3 myPos;
    Quaternion myQua;
    public Material material;
    // Use this for initialization
    void Start () {

        images = transform.GetComponentsInChildren<Image>();
        //transform.LookAt(Camera.main.transform);
        //transform.localEulerAngles = new Vector3(transform.eulerAngles.x , transform.eulerAngles.y + 180, transform.eulerAngles.z);
        myPos = transform.position;
        myQua = transform.rotation;
        Hide();
	}
	
	// Update is called once per frame
	void Update () {
        if (VideoPlayerController.inst.movPlayer.Control.IsFinished())
        {
            if (material.color.a>0.05f&&material.color.a<0.15f)
            {
                Show();
                transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5 + Camera.main.transform.up;
                transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, -Camera.main.transform.eulerAngles.z);
            }
        }
        else
        {
            if (transform.position!=myPos)
            {
                transform.position = myPos;
                transform.rotation = myQua;
                Hide();
            }
        }
        float a = VideoPlayerController.inst.movPlayer.Control.GetCurrentTimeMs();
        float b = VideoPlayerController.inst.movPlayer.Info.GetDurationMs();
        transform.Find("Slider").GetComponent<Slider>().value=a/b;
        
    }
    void Show()
    {
        material.DOColor(new Color(material.color.r, material.color.g, material.color.b, 1), 0.5f);
    }
    void Hide()
    {
        material.DOColor(new Color(material.color.r, material.color.g, material.color.b, 0.1f), 0.5f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (VideoPlayerController.inst.movPlayer.Control.IsPlaying())
        {
            Show();
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (VideoPlayerController.inst.movPlayer.Control.IsPlaying())
        {
            Hide();
        }
    }

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
    }
}
