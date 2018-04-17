using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class FileManager : MonoBehaviour {
    public static FileManager Inst;
   
    public CurrentFolder current=new CurrentFolder();//当前文件夹
    public Button back;
    public Dictionary<string, directory> dirs;
    // Use this for initialization
    void Start () {
        dirs = new Dictionary<string, directory>();
        Inst = this;
        
    }
    public void Init()
    {

    }
    public void MovePlay(childProj p)
    {
        FileCon.Inst.MovePlay(p);
    }
    public void UpdateCurrent(directory dir)//更新资源
    {
        current.dir=dir;
        FileCon.Inst.UpdateCurrent(dir);
    }
    public void LogInTrue(loginRsp res)//账号密码正确
    {
        LoginMA.Inst.LogInTrue(res);
    }
    public void LogInFalse(string errorMsg)//账号密码错误
    {
        LoginMA.Inst.LogInFalse(errorMsg);
    }


    void Update () {
		
	}
    
  
}
