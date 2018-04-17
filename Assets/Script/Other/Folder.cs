using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Folder : MonoBehaviour,IPointerClickHandler
{
    public childDir cd;
    public Text nameTxt;
    public Text timeTxt;
    public Text sizeTxt;
    public Button DownLoadbtn;
    public Slider downloadSlider;
    Button btn;
    
    // Use this for initialization
    void Start()
    {

        btn = GetComponent<Button>();
        //btn.onClick.AddListener(Clicked);
        DownLoadbtn.onClick.AddListener(DownLoadClick);
        if (GetComponentInChildren<Text>())
        {
            nameTxt = GetComponentInChildren<Text>();
        }
       
	}    
	
	// Update is called once per frame
	void Update () {
        InfoUpdate();

    }
    void InfoUpdate()
    {
        nameTxt.text = cd.name;
        timeTxt.text = cd.updateTime;
        sizeTxt.text = "";
    }
    void Clicked()
    {
        //FileManager.Inst.ParentsId.Add(FileManager.Inst.current);
        if (FileManager.Inst.dirs.ContainsKey(cd.id))
        {
            FileCon.Inst.UpdateCurrent(FileManager.Inst.dirs[cd.id]);
        }
        else
        {
            NetworkManager.inst.GetDirectory(cd.id);
        }
        

    }
    void DownLoadClick()
    {   
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked();
    }
}
