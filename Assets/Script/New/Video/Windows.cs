using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RenderHeads.Media.AVProVideo;

public class Windows : MonoBehaviour
{
    public static Windows Inst;

    //public GameObject videoPlayer;
    public GameObject playerConPanel;
    public GameObject AllPanels;
    public GameObject[] VRCanvas;
    public GameObject[] VRgameObjects;
    public GameObject eventSys;
    //public GameObject playEndBtn;
    // Use this for initialization
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        
    }
    public void Init()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetCanvas(bool isShow)
    {
        AllPanels.SetActive(isShow);
        foreach (GameObject item in VRCanvas)
        {
            item.SetActive(!isShow);
        }
    }
    public void VideoPlayerShow(bool isShow)
    {
        //videoPlayer.SetActive(isShow);
        playerConPanel.SetActive(isShow);
    }
    public void OnVRGameObject(bool isShow)
    {
        foreach (GameObject item in VRgameObjects)
        {
            item.SetActive(isShow);

        }
        eventSys.SetActive(!isShow);
        //UIManager.Inst.text.text = "111";
        AllPanels.SetActive(!isShow); //UIManager.Inst.text.text = "222";
        Camera.main.GetComponent<PhysicsRaycaster>().enabled = !isShow; //UIManager.Inst.text.text = "333";
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = isShow; //UIManager.Inst.text.text = "666";
    }
    void SetResolution()
    {

    }
}
