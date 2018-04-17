using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LogInState : State<FileCon>
{
    public override void EnterState(FileCon target)
    {
        WinManager.Inst.LogInPanel.SetActive(true);
        UIManager.Inst.InstInfo();
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public override void UpdateState(FileCon target)
    {
        
    }
    public override void ExitState(FileCon target)
    {
        WinManager.Inst.LogInPanel.SetActive(false);
    }
}
public class FilePanel : State<FileCon>
{
    RectTransform rect,imParentRect;
    public override void EnterState(FileCon target)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        rect = UIManager.Inst.reFreshBtn.transform as RectTransform;
        imParentRect = UIManager.Inst.ImParent.transform as RectTransform;
        WinManager.Inst.filePanel.SetActive(true);
        
    }
    public override void UpdateState(FileCon target)
    {
        if (Input.GetKeyUp(KeyCode.Escape)&& FileCon.Inst.myFile.Count>1)
        {
            UIManager.Inst.OnBackClick();
        }
        rect.localScale = new Vector3(1, -imParentRect.localPosition.y / 200, 1);
    }
    public override void ExitState(FileCon target)
    {
        WinManager.Inst.filePanel.SetActive(false);
    }
}
public class VideoPlayerPanel : State<UIManager>
{
    GameObject go = WinManager.Inst.playEndBtn;
    Camera cam = Camera.main;
    public StateMachine<VideoPlayerPanel> VideoStateM;
    Image isbufferIm;
    Text isbufferTe;
    Text canplay;
    Text debugText;
    public override void EnterState(UIManager target)
    {
        debugText = Camera.main.transform.Find("Load").Find("1").GetComponent<Text>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        VideoStateM = new StateMachine<VideoPlayerPanel>(this);
        UIManager.Inst.VrSwitchOnFun();
        WinManager.Inst.VideoPlayerShow(true);
        WinManager.Inst.OnVRGameObject(true);
        target.VRChange.AddListener(ChangeState);
        isbufferIm = target.isbuffering.GetComponent<Image>();
        isbufferTe = target.isbufferingtext.GetComponent<Text>();
        canplay = target.canplayText.GetComponent<Text>();
        VideoStateM.SetCourrentState(new VRModle());
    }
    public override void UpdateState(UIManager target)
    {

        debugText.text = XRSettings.loadedDeviceName;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState();
        }
        
        if (!XRSettings.enabled)
        {
            //text.text=InputTracking.GetLocalRotation(XRNode.CenterEye).ToString()+ "2";
            Camera.main.GetComponent<Transform>().localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(XRNode.Head);
        }
        else
        {
        }
        if (UIManager.Inst.player!=null)
        {
            if (UIManager.Inst.player.Control.CanPlay())
            {
                canplay.enabled = false;
            }
            else
            {
                canplay.enabled = true;
                canplay.text = "Loading..";

            }
            if (target.player.Control.IsBuffering())
            {
                isbufferIm.enabled = true;
                isbufferTe.enabled = true;
                isbufferTe.text = (target.player.Control.GetBufferingProgress() * 100).ToString() + "%";

            }
            else
            {
                isbufferIm.enabled = false;
                isbufferTe.enabled = false;
            }
        }
        
    }
    public override void ExitState(UIManager target)
    {
        UIManager.Inst.VrSwitchOffFun();   
        WinManager.Inst.playerConPanel.SetActive(false);
        VideoPlayerController.inst.movPlayer.Control.CloseVideo();
        WinManager.Inst.OnVRGameObject(false);
    }
    public void ChangeState()
    {
        VideoStateM.ChangeStateNext();
    }
    class VRModle : State<VideoPlayerPanel>
    {
        public override void EnterState(VideoPlayerPanel target)
        {
            target.VideoStateM.NextState = new NoVRModle();
            WinManager.Inst.OnVRGameObject(true);
            XRSettings.enabled = true;

        }

        public override void ExitState(VideoPlayerPanel target)
        {
            
        }

        public override void UpdateState(VideoPlayerPanel target)
        {
            
        }
    }
    class NoVRModle : State<VideoPlayerPanel>
    {
        public override void EnterState(VideoPlayerPanel target)
        {
            target.VideoStateM.NextState = new VRModle();
            WinManager.Inst.OnVRGameObject(false);
            XRSettings.enabled = false;
        }

        public override void ExitState(VideoPlayerPanel target)
        {

        }

        public override void UpdateState(VideoPlayerPanel target)
        {
            
        }
    }
}
