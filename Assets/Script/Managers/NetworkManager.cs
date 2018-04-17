using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using System.Net;

public class NetworkManager : MonoBehaviour {
    
    public static NetworkManager inst;
    string urlhead = "http://www.2minutedrill.us/";
    //string urlhead = "http://192.168.1.2:11500";
    

    string loginUrl = "/api/organization/login";
   
    string rootdirUrl = "/api/directory/root";
    string dirUrl = "/api/directory/"; //换成local要记得加回刚刚
    string localDirUrl = "api/directory/";
    public string projectUrl = "/api/project/";
    
    string downloadData;
    string jsonRead;
    string token;
    // Use this for initialization
    void Start () {
        inst = this;
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame  
	void Update () {
		
	}

    public void Login(loginSnd ls)
    {
        StartCoroutine(login(ls));

        //StartCoroutine(localLogin(ls));
    }
    public void GetDirectory()
    {
        StartCoroutine(getDir());
        //StartCoroutine(localGetDirectory());
    }
    public void GetDirectory(string id)
    {
        //StartCoroutine(localGetDirectory(id));
        StartCoroutine(getDir(id));
    }

    public void GetMovie(childProj c)
    {
        StartCoroutine(loadMovie(c));
        //StartCoroutine(localLoadMovie(c));
    }
    public void LoadThumbnail(childProj c)
    {
        StartCoroutine(loadThumbnail(c));
    }
    //开始改成本地的造假程序。
    #region 
    IEnumerator localLogin(loginSnd ls)
    {
        yield return 1f;
        
        loginRsp r = new loginRsp(); //JsonConvert.DeserializeObject<loginRsp>(json);
        r.is_successed = true;
        r.token = "1";
        r.organization.code = "BOLUO";
        r.organization.id = 15;
        r.organization.role = 0;
        r.organization.verboseName = "2MD Default Organization";
        token = r.token;
        UIManager.Inst.LogInTrue(r);
    }
    IEnumerator localGetDirectory()
    {
        WWW www = new WWW("http://cdn3.ressvr.com/2md/forTestApp/list3.json");
        yield return www;
        dirRsp d = JsonConvert.DeserializeObject<dirRsp>(www.text);
        FileManager.Inst.UpdateCurrent(d.directory);
    }
    IEnumerator localGetDirectory(string id)
    {
        WWW www = new WWW("http://cdn3.ressvr.com/2md/forTestApp/childList.json");
        yield return www;
        dirRsp d = JsonConvert.DeserializeObject<dirRsp>(www.text);
        d.directory.childProjects[0].videoUrl = "http://cdn3.ressvr.com/2md/forTestApp/o_1c5q9ar851fbr1lok1fnhaq9a3r8.mp4";
        FileManager.Inst.UpdateCurrent(d.directory);
    }
    IEnumerator localLoadJson(string filename)
    {
        string path = Application.streamingAssetsPath + "/" + filename;

        Debug.Log(path);
        WWW www = new WWW(path);
        yield return www;
        Debug.Log(www.text);
        jsonRead = www.text;    
    }
    IEnumerator localLoadMovie(childProj cp)
    {
        //string url = Application.streamingAssetsPath + "/" + cp.name + ".mp4";
        string url = "http://cdn3.ressvr.com/2md/forTestApp/o_1c5q9ar851fbr1lok1fnhaq9a3r8.mp4";
        cp.videoUrl = url;
        //Debug.Log("movie url is : "+url);
        yield return 1f;
        UIManager.Inst.MovePlay(cp);
        //StartCoroutine( localLoadAction(cp.id));
    }
    IEnumerator localLoadAction(string id)
    {
        //string filename = id + ".json";
        ////Debug.Log(filename);
        //yield return localLoadJson(filename);
        WWW www = new WWW("http://cdn3.ressvr.com/2md/forTestApp/data.json");
        yield return www;
        Actions actionlist = JsonConvert.DeserializeObject<Actions>(www.text);
        //Debug.Log(json);
        if (actionlist.actions.Count != 0)
        {
            ActionSortForTime(actionlist.actions);
            //Debug.Log("action list has " + actionlist.actions.Count + " actions");
            ActionManager.inst.ActionList = actionlist.actions;
            ActionManager.inst.StartCheckAction();
        }
    }
    #endregion
    IEnumerator login(loginSnd ls)
    {
        DownloadHandler downloader = new DownloadHandlerBuffer();

        WWWForm wf = new WWWForm();                       //一种格式，类似json。
        wf.AddField("username", ls.username);
        string newpsw = CalculateMD5Hash(ls.password);
        string truepsw = newpsw.ToLower();

        
        wf.AddField("password", truepsw);

        UploadHandler uploader2 = new UploadHandlerRaw(wf.data);
  
        using (UnityWebRequest uw = UnityWebRequest.Post(urlhead + loginUrl, wf))
        {
    
            uw.uploadHandler = uploader2;
            uw.downloadHandler = downloader;
            uploader2.contentType = "application/x-www-form-urlencoded";
            yield return uw.SendWebRequest();
     
            downloadData = uw.downloadHandler.text;
            
            if (uw.responseCode == 200)
            {
                loginRsp r = JsonConvert.DeserializeObject<loginRsp>(downloadData);
                Debug.Log(downloadData);
                token = r.token;
                LoginMA.Inst.LogInTrue(r);
            }
            else
            {
                //Debug.Log("respone code is : " + uw.responseCode);
                LoginMA.Inst.LogInFalse(downloadData);
            }
        };  
    }
    IEnumerator getDir()
    {
        using (UnityWebRequest uw = UnityWebRequest.Get(urlhead + rootdirUrl))
        {
            uw.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uw.SetRequestHeader("Authorization", "Bearer " + token);

            yield return uw.SendWebRequest();
            downloadData = uw.downloadHandler.text;

            if (uw.responseCode == 200)
            {
                dirRsp d = JsonConvert.DeserializeObject<dirRsp>(downloadData);
                
                FileManager.Inst.UpdateCurrent(d.directory);
            }
            else
            {
                Debug.Log("respone fail");
            }
        };

    }
    IEnumerator getDir(string id)
    {
        string url = dirUrl + id.ToString();
        Debug.Log("dir name is" + url);
        using (UnityWebRequest uw = UnityWebRequest.Get(urlhead + url))
        {
            uw.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uw.SetRequestHeader("Authorization", "Bearer " + token);

            yield return uw.SendWebRequest();

            if (uw.responseCode == 200)
            {
                downloadData = uw.downloadHandler.text;
                //CreateFile(dirUrl,downloadData,id+".json");
                dirRsp d = JsonConvert.DeserializeObject<dirRsp>(downloadData);
                FileManager.Inst.UpdateCurrent(d.directory);
                FileCon.Inst.refreshShow.transform.GetChild(0).GetComponent<Image>().enabled = false;
                FileCon.Inst.refreshShow.transform.GetChild(1).GetComponent<Image>().enabled = true;
                StartCoroutine(CloseRefreshShow());
            }
            else
            {
                FileCon.Inst.refreshShow.transform.GetChild(0).GetComponent<Image>().enabled = false;
                FileCon.Inst.refreshShow.transform.GetChild(2).GetComponent<Image>().enabled = true;
                Debug.Log("respone fail");
                StartCoroutine(CloseRefreshShow());
            }
        };
    }
    IEnumerator CloseRefreshShow()
    {
        RectTransform rect = FileCon.Inst.refreshShow.transform as RectTransform;
        yield return new WaitForSeconds(0.8f);
        rect.DOSizeDelta(new Vector2(rect.sizeDelta.x,0),0.3f).OnComplete(()=> {
            FileCon.Inst.refreshShow.transform.GetChild(1).GetComponent<Image>().enabled = false;
            FileCon.Inst.refreshShow.transform.GetChild(2).GetComponent<Image>().enabled = false;
            FileCon.Inst.refreshShow.SetActive(false);
        });
    }
    IEnumerator loadThumbnail(childProj c)
    {
        UnityWebRequest uw = new UnityWebRequest(c.thumbnail);
        
        yield return uw.SendWebRequest();
       
        Debug.Log("thumbnail = " + uw.downloadHandler.text);
        //FileManager.Inst.SpritUpDate(c,);
    }
    IEnumerator loadMovie(childProj cp)
    {
        string url = cp.videoUrl;
        //UIManager.Inst.debug.text = "2";
        //Debug.Log("movie url is : "+url);
        yield return FileCon.Inst.MovePlay(cp);
        yield return VideoManager.Inst.MovePlay();
        yield return loadActionData(cp.id);
    }
    IEnumerator loadActionData(string id)
    {
        string actionUrl = urlhead + "/api/project/"+ id +"/actions";
        using (UnityWebRequest uw = UnityWebRequest.Get(actionUrl))
        {
            uw.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uw.SetRequestHeader("Authorization", "Bearer " + token);

            yield return uw.SendWebRequest();
            if (uw.responseCode == 200)
            {
                downloadData = uw.downloadHandler.text;
                Debug.Log(downloadData);
                //CreateFile(downloadData,id+".json");
                Actions actionlist = JsonConvert.DeserializeObject<Actions>(downloadData);
                //actionlist.actions.Sort();
                if (actionlist.actions.Count != 0)
                {
                    ActionSortForTime(actionlist.actions);
                    Debug.Log("action list has " + actionlist.actions.Count + " actions");
                    ActionManager.inst.ActionList = actionlist.actions;
                    ActionManager.inst.StartCheckAction();
                }
            }
            else
            {
                Debug.Log("respone fail");
            }
        };
        yield return null;
    }
    void CreateFile(string address, string content,string name)
    {
        StreamWriter sw;
        string path = Application.dataPath + "/StreamingAssets" + address + name;
        FileInfo t = new FileInfo(path);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            Debug.Log(name + " file exist");
            sw = t.AppendText();
        }
        sw.WriteLine(content);
        Debug.Log("write file to " + path);
        sw.Close();
        sw.Dispose();
    }
    IEnumerator loadLocalActionData(string id)
    {
        string actionUrl = urlhead + "/api/project/" + id + "/actions";
        using (UnityWebRequest uw = UnityWebRequest.Get(actionUrl))
        {
            uw.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            uw.SetRequestHeader("Authorization", "Bearer " + token);

            yield return uw.SendWebRequest();
            if (uw.responseCode == 200)
            {
                downloadData = uw.downloadHandler.text;

                Actions actionlist = JsonConvert.DeserializeObject<Actions>(downloadData);
                //actionlist.actions.Sort();
                if (actionlist.actions.Count != 0)
                {
                    ActionSortForTime(actionlist.actions);
                    Debug.Log("action list has " + actionlist.actions.Count + " actions");
                    ActionManager.inst.ActionList = actionlist.actions;
                    ActionManager.inst.StartCheckAction();
                }
            }
            else
            {
                Debug.Log("respone fail");
            }
        };
        yield return null;
    }


    void ActionSortForTime(List<action> actions)
    {
        
        for (int i = 0; i < actions.Count; i++)
        {
            float a = float.MaxValue;
            int b=i;
            for (int j  = 0; j  < actions.Count-i; j ++)
            {
                if (actions[i+j].time<a)
                {
                    a = actions[i + j].time;
                    b = i+j;
                }
            }
            action m = actions[i];
            actions[i] = actions[b];
            actions[b] = m;
        }
    }
    public string CalculateMD5Hash(string input)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}

public class respo { }
public class loginSnd : respo
{
    public string username;
    public string password;
}
public class loginRsp : respo
{
    public organization organization = new organization();
    public string token;
    public bool is_successed;
    public string err_msg;
}
public class organization :respo
{
   public int id;
   public string code;
   public string verboseName;
   public int role;
}

public class dirRsp
{
    public directory directory;
}

public class directory : respo
{
    public string id;
    public string name;
    public string updateTime;
    public List<childDir> childDirs = new List<childDir>();
    public List<childProj> childProjects = new List<childProj>();
}
public class childDir
{
    public string id;
    public string name;
    public string updateTime;
}
public class childProj
{
    public string id;
    public string name;
    public string createTime;
    public string updateTime;
    public string thumbnail;
    public string videoUrl;
    public float duration;
    private int size;

    public int Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
        }

    }
   
}
public class Project
{
    public childProj project;
}
public class Actions
{
    public List<action> actions = new List<action>();
}
public class action
{
    public string id;
    public float time;
    public string type;
    public string content;
    
}
