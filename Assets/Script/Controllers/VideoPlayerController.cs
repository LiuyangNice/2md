using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
public class VideoPlayerController : MonoBehaviour {
    public static VideoPlayerController inst;
    public MediaPlayer movPlayer;
    public Text seeks;
    public Slider seekSlider;
    private float _setVideoSeekSliderValue;
    private bool _wasPlayingOnScrub;
    public Button volume;
    
    public Slider _audioVolumeSlider;
    private float _setAudioVolumeSliderValue;
    public float videoDuration;
    public float stateTime = 10;
    float timer ;
    public Animator[] animator;
    //控制界面各按钮
    
    private bool isPlaying;//是否正在播放
    public bool IsPlaying { get { return movPlayer.Control.IsPlaying(); }
        set { isPlaying = value; } }
    float currentTime;//当前播放时间
    public float CurrentTime { get { return movPlayer.Control.GetCurrentTimeMs()/1000; } set { currentTime = value; } }

    
    private void Awake()
    {
            
        inst = this;
    }
    // Use this for initialization
    void Start () {
        timer = stateTime;
        volume.onClick.AddListener(OnVolumeClick);
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].Play("On", 0);
        }
                                                                                                                                                                                                                                                                                                                                                     
    }
	
	// Update is called once per frame
	void Update () {
        if (_audioVolumeSlider.isActiveAndEnabled)
        {
            Inst();
        }
        if (movPlayer.isActiveAndEnabled)
        {
            TimeUpdate();
            float time = movPlayer.Control.GetCurrentTimeMs();
            float d = time / movPlayer.Info.GetDurationMs();
            _setVideoSeekSliderValue = d;
            seekSlider.value = d;
        }
        HideController();
        if (Input.GetMouseButtonDown(0))
        {
            timer = stateTime;
            for (int i = 0; i < animator.Length; i++)
            {
                animator[i].Play("On", 0);
            }
            
        }
    }
    void HideController()
    {
        
        timer -= Time.deltaTime;
        if (timer<=0)
        {
            for (int i = 0; i < animator.Length; i++)
            {
                animator[i].Play("Off", 0);
            }
        }
        
    }
    public void Inst()
    {
        if (_audioVolumeSlider)
        {
            // Volume
            if (movPlayer.Control != null)
            {
                float volume = movPlayer.Control.GetVolume();
                _setAudioVolumeSliderValue = volume;
                _audioVolumeSlider.value = volume;
            }
        }
    }
    void OnVolumeClick()//声音调整开关
    {
        GameObject go = volume.transform.GetChild(0).gameObject;
        go.SetActive(!go.activeSelf);
    }
    public void Pause()//暂停
    {
        movPlayer.Pause();
        
    }
    public void Play()//播放
    {
        if (movPlayer.Control.IsFinished())
        {
            movPlayer.Rewind(false);
            return;
        }
        movPlayer.Play();
    }
    public void OnPlayButton()
    {
        if (movPlayer.Control.IsFinished())
        {
            //movPlayer.Control.Rewind();
            movPlayer.Rewind(false);
            movPlayer.Play();
            ActionManager.inst.stopChecking();
            ActionManager.inst.StartCheckAction();

        }
        else if (movPlayer.Control.IsPlaying())
        {
            movPlayer.Pause();

        }
        else if (movPlayer.Control.IsPaused())
        {
            Debug.Log("qqqqqqqqq");
            movPlayer.Play();
            ActionManager.inst.stopChecking();
            ActionManager.inst.StartCheckAction();
        }
        
    }
    void TimeUpdate()
    {
        int scd = (int)((movPlayer.Info.GetDurationMs() / 1000) % 60);
        int min = (int)((movPlayer.Info.GetDurationMs() / 1000) / 60);
        string a = "/" + min.ToString("00") + ":" + scd.ToString("00");
        int scds = (int)((movPlayer.Control.GetCurrentTimeMs() / 1000) % 60);
        int mins = (int)((movPlayer.Control.GetCurrentTimeMs() / 1000) / 60);
        string b = mins.ToString("00") + ":" + scds.ToString("00");
        seeks.text = b + a;


    }
    //video滑条
    public void OnVideoSeekSlider()
    {
        if (movPlayer && seekSlider && seekSlider.value != _setVideoSeekSliderValue)
        {
            movPlayer.Control.Seek(seekSlider.value * movPlayer.Info.GetDurationMs());
        }
    }
    public void OnVideoSliderDown()
    {
        if (movPlayer)
        {
            _wasPlayingOnScrub = movPlayer.Control.IsPlaying();
            if (_wasPlayingOnScrub)
            {
                movPlayer.Control.Pause();
                //SetButtonEnabled( "PauseButton", false );
                //SetButtonEnabled( "PlayButton", true );
            }
            OnVideoSeekSlider();
        }
    }
    public void OnVideoSliderUp()
    {
        if (movPlayer && _wasPlayingOnScrub)
        {
            movPlayer.Control.Play();
            _wasPlayingOnScrub = false;

            //SetButtonEnabled( "PlayButton", false );
            //SetButtonEnabled( "PauseButton", true );
        }
    }
    //声音
    public void OnAudioVolumeSlider()
    {
        if (movPlayer && _audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
        {           
            movPlayer.Control.SetVolume(_audioVolumeSlider.value);
        }
        if (movPlayer && _audioVolumeSlider && _audioVolumeSlider.value != _setAudioVolumeSliderValue)
        {
            movPlayer.Control.SetVolume(_audioVolumeSlider.value);
        }
    }
    

}
