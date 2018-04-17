using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class DoloadBtn : MonoBehaviour {
    public Sprite download;
    public Sprite ctn;
    public Sprite stop;
    public Image Myim;
    public Slider mysl;
    public string downloadPath;
    public string namestr;
    bool stD;
    public MyHttp http = new MyHttp();
    FileMov myMov;
    // Use this for initialization
    void Start () {
        myMov = transform.GetComponentInParent<FileMov>();
        namestr = myMov.Cj.name;
        Myim.sprite=download;
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(DownloadBtn);
        }
        Inst();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void Inst()
    {
        string myFilePath = Application.persistentDataPath + "/temp/" +GameData.myUser.name+"/"+ namestr;

        if (File.Exists(myFilePath))
        {
            DownloadBtn();
            http.Close();
            Myim.sprite = ctn;
        }


    }
    void DownloadBtn()
    {
        myMov.isDownloading = true;
        //http.Download(downloadPath, Application.persistentDataPath + "/temp",namestr);
        StartCoroutine(SliderUpdate());
        if (Myim.sprite == download||Myim.sprite==ctn)
        {
            http.Download(downloadPath, Application.persistentDataPath + "/temp"+"/" + GameData.myUser.name, namestr);
            Myim.sprite = stop;
        }
        else if (Myim.sprite == stop)
        {
            http.Close();
            //myMov.isDownloading = false;
            Myim.sprite = ctn;
        }
    }
    IEnumerator SliderUpdate()
    {
        while (mysl.value<1 && myMov.isDownloading)
        {
            mysl.value = http.progress;           
            yield return new WaitForEndOfFrame();
        }
        mysl.value = 1;
        myMov.Cj.duration = mysl.value;
        myMov.isDownloading = false;
        gameObject.SetActive(false);
        
    }
    private void OnDisable()
    {
        http.Close();
    }
}
