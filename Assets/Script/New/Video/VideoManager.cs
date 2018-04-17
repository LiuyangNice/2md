using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RenderHeads.Media.AVProVideo;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using System.IO;
using System;
public class VideoManager : MonoBehaviour {
    public static VideoManager Inst;
    public Image isbuffering;
    public Text isbufferingtext;
    public Text canplayText;
    public Text debugText;

    public Text[] MovNames;
    [NonSerialized]
    public UnityEvent VRChange = new UnityEvent();
    public GameObject videoController;

    bool isVR = false;

    // Use this for initialization
    void Start () {
        Inst = this;
        StateInst();
        //MovePlay();

    }
	
	// Update is called once per frame
	void Update () {
        StateUpdate();

    }
    public IEnumerator MovePlay()
    {
        string path = Application.persistentDataPath + "/temp" + "/" + GameData.myUser.name + "/" + GameData.currentProj.name;
        if (File.Exists(path)&&GameData.currentProj.duration>=1)
        {
            debugText.text = "PlayInLocal";
            VideoPlayerController.inst.movPlayer.m_VideoPath = "temp/" + GameData.myUser.name + "/" + GameData.currentProj.name;
            VideoPlayerController.inst.movPlayer.m_VideoLocation = MediaPlayer.FileLocation.RelativeToPeristentDataFolder;

        }
        else
        {
            debugText.text = "paly in url";
            VideoPlayerController.inst.movPlayer.m_VideoPath = GameData.currentProj.videoUrl;
            VideoPlayerController.inst.movPlayer.m_VideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;


        }
        for (int i = 0; i < MovNames.Length; i++)
        {
            MovNames[i].text = GameData.currentProj.name;
        }
        if (VideoPlayerController.inst.movPlayer.OpenVideoFromFile(VideoPlayerController.inst.movPlayer.m_VideoLocation, VideoPlayerController.inst.movPlayer.m_VideoPath, true))
        {

        }
        else
        {

        }
        yield return new WaitUntil(()=> VideoPlayerController.inst.movPlayer.Info.GetDurationMs()>0);
        
    }
    public void StateInst()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        StartCoroutine(VrSwitchOn());
        VRChange.AddListener(VRSwitch);
        Windows.Inst.VideoPlayerShow(true);
        Windows.Inst.OnVRGameObject(true);
        
    }
    public void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            VRSwitch();
        }

        if (!XRSettings.enabled)
        {
            //text.text=InputTracking.GetLocalRotation(XRNode.CenterEye).ToString()+ "2";
            Camera.main.GetComponent<Transform>().localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(XRNode.Head);
        }
        else
        {
        }
        if (VideoPlayerController.inst.movPlayer != null)
        {
            if (VideoPlayerController.inst.movPlayer.Control.CanPlay())
            {
                canplayText.enabled = false;
            }
            else
            {
                canplayText.enabled = true;
                canplayText.text = "Loading..";

            }
            if (VideoPlayerController.inst.movPlayer.Control.IsBuffering())
            {
                isbuffering.enabled = true;
                isbufferingtext.enabled = true;
                isbufferingtext.text = (VideoPlayerController.inst.movPlayer.Control.GetBufferingProgress() * 100).ToString() + "%";

            }
            else
            {
                isbuffering.enabled = false;
                isbufferingtext.enabled = false;
            }
        }
    }
    public void VRSwitch()
    {
        isVR = !isVR;
        Windows.Inst.OnVRGameObject(isVR);
        XRSettings.enabled = isVR;
    }
    public void LastMov()
    {
        List<childProj> cjs = FileManager.Inst.current.dir.childProjects;
        for (int i = 0; i < cjs.Count; i++)
        {
            if (cjs[i].videoUrl == VideoPlayerController.inst.movPlayer.m_VideoPath)
            {
                if (i - 1 > -1)
                {
                    GameData.currentProj = cjs[i - 1];
                    MovePlay();

                }
                else
                {
                    GameData.currentProj = cjs[cjs.Count - 1];
                    MovePlay();
                }
                break;
            }
        }
    }
    public void NextMov()
    {
        List<childProj> cjs = FileManager.Inst.current.dir.childProjects;
        for (int i = 0; i < cjs.Count; i++)
        {
            if (cjs[i].videoUrl == VideoPlayerController.inst.movPlayer.m_VideoPath)
            {
                if (i + 1 < cjs.Count)
                {
                    GameData.currentProj = cjs[i + 1];
                    MovePlay();
                    Debug.Log(cjs[i + 1]);
                }
                else
                {
                    GameData.currentProj = cjs[0];
                    MovePlay();
                }
                break;
            }
        }
    }
    public void LoadDevice()
    {

    }
    IEnumerator VrSwitchOn()
    {
        if (String.Compare(XRSettings.loadedDeviceName, "cardboard", true) != 0)
        {
            XRSettings.LoadDeviceByName("Cardboard");
            yield return null;
            XRSettings.enabled = true;
        }
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.eulerAngles = Vector3.zero;
        InputTracking.Recenter();
    }
    IEnumerator VrSwitchOff()
    {
        if (String.Compare(XRSettings.loadedDeviceName, "None", true) != 0)
        {
            XRSettings.enabled = false;
            XRSettings.LoadDeviceByName("None");
        }
        yield return null;
    }
    public void OnBack()
    {
        StartCoroutine(VrSwitchOff());
        Windows.Inst.playerConPanel.SetActive(false);
        VideoPlayerController.inst.movPlayer.Control.CloseVideo();
        Windows.Inst.OnVRGameObject(false);
        GameData.currentProj = null;
        SceneManager.LoadScene(2);
    }
    
}
