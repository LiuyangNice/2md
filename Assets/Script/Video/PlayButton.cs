using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayButton : MonoBehaviour {
    public Sprite Play;
    public Sprite Pause;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSprite();
    }
    void UpdateSprite()
    {
        
        RenderHeads.Media.AVProVideo.MediaPlayer movPlayer=VideoPlayerController.inst.movPlayer;
        if (movPlayer.Control.IsFinished())
        {
            GetComponent<Image>().sprite = Play;
        }
        else if (movPlayer.Control.IsPlaying())
        {
            GetComponent<Image>().sprite = Pause;
        }
        else if(movPlayer.Control.IsPaused())
        {
            GetComponent<Image>().sprite = Play;
        }
        
    }
}
