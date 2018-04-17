using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using GoogleVR.Demos;
using Newtonsoft.Json;
using System;
using System.IO;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour {
    public GameObject VRCamera;
    public Camera cam1, cam2;
    public static UIManager Inst;
    public InputField IuserName;//账号输入
    public InputField Ipassword;//密码输入
    public Toggle isRember;//记住密码
    public Button logIn;//登录按钮
    public Text ErrorSow;
    public MediaPlayer player;

    public GameObject UserinfoPage;
    Text prot, userName, userInfo;
    public Button LogOut;
    public Button back;


    public GameObject folderPrefab;
    public GameObject movPrefab;

    public GameObject reFreshBtn;
    public GameObject refreshShow;

    public GameObject isbuffering;
    public GameObject isbufferingtext;
    public GameObject canplayText;

    public Text[] MovNames;
    public GameObject ImParent;//文件父transform
    public StateMachine<UIManager> myMachine;
    public List<GameObject> files = new List<GameObject>();//文件夹及文件
    public Stack<directory> myFile = new Stack<directory>();
    [NonSerialized]
    public UnityEvent VRChange=new UnityEvent();
    //public Text debug;
    void Start () {

        Inst = this;
        myMachine = new StateMachine<UIManager>(Inst);
        //myMachine.SetCourrentState(new LogInState());
        logIn.onClick.AddListener(OnLogInClick);
        LogOut.onClick.AddListener(OnLogOutClick);
        InstInfo();
        //back.onClick.AddListener(Back);
        //Cardboard.SDK.VRModeEnabled = false;
        //Screen.orientation = ScreenOrientation.PortraitUpsideDown;
    }
    public void UserInfoInst()
    {
      
    }
    // Update is called once per frame
    void Update () {
        myMachine.UpdateFsm();
               
    }
      public void OnLogOutClick()
    {
        myFile.Clear();
        //myMachine.ChangeState(new LogInState());   
    }
    public void UpdateCurrent(directory dir)
    {
        //Screen.orientation = ScreenOrientation.Portrait;
        
        if (myFile.Count == 0)
        { myFile.Push(dir); }
        else if (myFile.Peek().id!=dir.id)
        {
            myFile.Push(dir);
        }
        else if(myFile.Peek().id == dir.id)
        {
            myFile.Pop();
            myFile.Push(dir);
        }

        if (!FileManager.Inst.dirs.ContainsKey(dir.id))
        {
            FileManager.Inst.dirs.Add(dir.id, dir);
        }        
        for (int i = 0; i < files.Count; i++)
        {
            Destroy(files[i].gameObject);
        }
        files = new List<GameObject>();
        for (int i = 0; i < dir.childDirs.Count; i++)
        {
            DirToFolder(dir.childDirs[i]);

        }
        //HeadUpdate();
        foreach (childProj cp in dir.childProjects)
        {
                GameObject go = Instantiate(movPrefab, ImParent.transform);
                FileMov f = go.GetComponent<FileMov>();
                f.Cj = cp;
                files.Add(go);
            

            //UImanager funct
        }
        BackBtnUpdate();
    }
    void DirToFolder(childDir child)//文件夹生成
    {

        GameObject go = Instantiate(folderPrefab, ImParent.transform);
        Folder a = go.GetComponent<Folder>();
        a.cd = child;
        files.Add(go);
    }
  
    Sprite WWWToSprite(int x, int y, WWW w)
    {
        Texture2D tex = new Texture2D(x, y);
        w.LoadImageIntoTexture(tex);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, x, y), Vector2.zero);
        return sprite;
    }
   
    public void Back()//返回上一级
    {

        //发送parentId中最后一个id 并在列表中移除
        //ParentsId.Remove(ParentsId.Count-1);
        myFile.Pop();
        if (myFile.Count == 1)
        {            
            NetworkManager.inst.GetDirectory();
        }
        else if (myFile.Count > 1)
        {
           
            NetworkManager.inst.GetDirectory(myFile.Pop().id);
        }
        
        BackBtnUpdate();
    }
    void BackBtnUpdate()
    {
        
        if (myFile.Count > 1)
        {
            back.gameObject.SetActive(true);
        }
        else
        {
            back.gameObject.SetActive(false);
        }
        back.transform.SetAsLastSibling();
    }
    public void InstInfo()//信息初始化
    {
        if (File.Exists(Application.persistentDataPath + "/2MD.json"))
        {
            User current = MyJson.Inst.ReadJson();
            IuserName.text = current.user;
            Ipassword.text = current.password;
            isRember.isOn = current.isRember;
        }
       
    }
    public void LogInTrue(loginRsp res)//账号密码正确
    {
        AddUser();
        UserInfoUpdate(res.organization);
        NetworkManager.inst.GetDirectory();
        //myMachine.ChangeState(new FilePanel());
    }
    public void UserInfoUpdate(organization or)
    {
        Text prot = UserinfoPage.transform.Find("UserHead").Find("Image").Find("Text").GetComponent<Text>();
        Text userName = UserinfoPage.transform.Find("Name").GetComponent<Text>();
        Text userInfo = UserinfoPage.transform.Find("School").GetComponent<Text>();
        prot.text = or.code.Substring(0,2).ToUpper();
        userName.text = or.code;
        userInfo.text = or.verboseName;

    }
    public void LogInFalse(string errorMsg)//账号密码错误
    {
        ErrorSow.text = errorMsg;
    }
    public void OnLogInClick()//登录处理
    {
        loginSnd login = new loginSnd();
        login.username = IuserName.text;
        login.password = Ipassword.text;
        NetworkManager.inst.Login(login);
    }
    void AddUser()
    {
        User a = new User();
        a.user = IuserName.text;        
        a.isRember = isRember.isOn;
        if (!isRember.isOn)
        {
            Ipassword.text = null;
        }
        a.password = Ipassword.text;
        
        MyJson.Inst.WriteJson(a);
    }
   
    public void MovePlay(childProj p)
    {
        Text debugText = Camera.main.transform.Find("Load").Find("1").GetComponent<Text>();
        myMachine.ChangeState(new VideoPlayerPanel());
        
        //VideoPlayerController.inst.videoDuration = p.duration;
        string path= Application.persistentDataPath + "/temp"+"/"+IuserName.text+"/"+p.name;         
        if (File.Exists(path))
        {
            Debug.Log("Play in local");
            player.m_VideoPath = "temp/"+IuserName.text + "/"+p.name;
            player.m_VideoLocation = MediaPlayer.FileLocation.RelativeToPeristentDataFolder;
            
        }
        else
        {
            Debug.Log("paly in url");
            player.m_VideoPath = p.videoUrl;
            player.m_VideoLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;
            
            
        }
        for (int i = 0; i < MovNames.Length; i++)
        {
            MovNames[i].text = p.name;
        }
        if (player.OpenVideoFromFile(player.m_VideoLocation, player.m_VideoPath, true))
        {
            debugText.text = "成功打开！";
            player.Play();
        }
        else
        {
            debugText.text = "打开失败！";
        }
        ;
    }
    public void InputModuleChange(bool isShow)
    {
        
    }
    public void OnNoVRClick()
    {
        VRChange.Invoke();
        //WinManager.Inst.OnVRGameObject(!VrModle);
        ////StartCoroutine(LoadDevice(VrModle));
        ////XRSettings.enabled = !VrModle;

        //VrModle = !VrModle;
        //XRSettings.enabled = !XRSettings.enabled;
        //if (XRSettings.enabled)
        //{
        //    Camera.main.transform.position = Vector3.zero;
        //    Camera.main.transform.eulerAngles = Vector3.zero;
        //    //InputTracking.Recenter();
        //}

    }
    IEnumerator LoadDevice(bool vrModle)
    {
        XRSettings.LoadDeviceByName(XRSettings.supportedDevices[1]);
        yield return null;
        
    }
    public void OnBackClick()
    {
        //myMachine.ChangeState(new FilePanel());
        ActionManager.inst.stopChecking();
    }
    public void NextMov()
    {
        List<childProj> cjs = FileManager.Inst.current.dir.childProjects;
        for (int i = 0; i < cjs.Count; i++)
        {
            if (cjs[i].videoUrl == VideoPlayerController.inst.movPlayer.m_VideoPath)
            {
                if (i+1<cjs.Count)
                {
                    MovePlay(cjs[i + 1]);
                    Debug.Log(cjs[i + 1]);
                }
                else
                {
                    MovePlay(cjs[0]);
                }
                break;
            }
        }
       
    }
    public void LasrMov()
    {
        List<childProj> cjs = FileManager.Inst.current.dir.childProjects;
        for (int i = 0; i < cjs.Count; i++)
        {
            if (cjs[i].videoUrl == VideoPlayerController.inst.movPlayer.m_VideoPath)
            {
                if (i -1 > -1)
                {
                    MovePlay(cjs[i-1]);
                    
                }
                else
                {
                    MovePlay(cjs[cjs.Count-1]);
                }
                break;
            }
        }
    }
    public void VrSwitchOnFun()
    {
        StartCoroutine(VrSwitchOn());
    }
    public void VrSwitchOffFun()
    {
        StartCoroutine(VrSwitchOff());
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
}

