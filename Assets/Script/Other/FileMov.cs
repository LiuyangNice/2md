using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using System.IO;
using UnityEngine.EventSystems;
using System.Net;
using UnityEngine.Networking;
public class FileMov : MonoBehaviour,IPointerUpHandler,IPointerClickHandler
{
    private childProj cj;
    public RawImage sp;
    public Text nameTxt;
    public Text timeTxt;
    public Text sizeTxt;
    public Button DownLoadbtn;
    public DoloadBtn d;
    public Slider downloadSlider;
    Button btn;
    public bool isDownloading;

    public childProj Cj
    {
        get
        {
            return cj;
        }

        set
        {
            cj = value;
            InfoUpdate();
        }
    }

    // Use this for initialization
    void Start () {
       
        btn = GetComponent<Button>();
        //btn.onClick.AddListener(OnPointerDown);
        DownLoadbtn.onClick.AddListener(DownloadBtnClick);
        if (GetComponentInChildren<Text>())
        {
            nameTxt = GetComponentInChildren<Text>();
        }
        UpdateThumb();
        string path = Application.persistentDataPath + "/temp/" + Cj.name ;
        DownLoadBtnInst();
    }
    void DownLoadBtnInst()
    {
        d.namestr = Cj.name;
        d.downloadPath =Cj.videoUrl;
        d.mysl = downloadSlider;
        cj.duration = d.http.progress;
        
    }
    void UpdateThumb()
    {
        StartCoroutine(LoadMovFile());
        //StartCoroutine(LoadMovFile());
        //FileStreamLoadTexture();
    }

    //IEnumerator localLoadMovFile()
    //{
    //    //Debug.Log("start loading thumnail");
    //    WWW www = new WWW("http://cdn3.ressvr.com/2md/forTestApp/o_1c5q9ar851fbr1lok1fnhaq9a3r8.jpg");
    //    yield return www;
    //    //Debug.Log("end loading thumnail");
    //    Sprite sps = WWWToSprite(100, 100, www);
    //    sp.sprite = sps;

    //}

    IEnumerator LoadMovFile()
    {
        WWW www = new WWW(Cj.thumbnail);
        yield return www;
        Texture sps = WWWToSprite(100, 100, www);
        sp.texture = sps;
        
    }
    
    Texture2D WWWToSprite(int x, int y, WWW w)
    {
        Texture2D tex = new Texture2D(x, y);

        //w.LoadImageIntoTexture(tex);
        tex = w.texture;
        return tex;
    }
 
    // Update is called once per frame
    void Update() {
        
    }
    void InfoUpdate()
    {
        nameTxt.text = Cj.name;
        timeTxt.text = Cj.updateTime;
        int a = (int)Cj.Size / 1024;
        int b = (int)a / 1024;
        if (b>10)
        {
            sizeTxt.text = b.ToString()+"M";
        }
        else
        {
            sizeTxt.text = a.ToString() + "K";
        }
        
    }
   
    void OnPointerDown()
    {
        //UIManager.Inst.debug.text="1";
       NetworkManager.inst.GetMovie(Cj);
    }
    void DownloadBtnClick()
    {
        //GetCachedWWW(cj.videoUrl,cj.name);
        //downloadSlider.gameObject.SetActive(true);
        //DownLoadbtn.gameObject.SetActive(false);
    }


    static bool CheckFileOutOfDate(string filePath)
    {
        System.DateTime written = File.GetLastWriteTimeUtc(filePath);
        System.DateTime now = System.DateTime.UtcNow;
        double totalHours = now.Subtract(written).TotalHours;
        return (totalHours > 300);
    }

    

    public void OnPointerUp(PointerEventData eventData)
    {
        //UIManager.Inst.debug.text = "213123123";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerDown();
    }
}
