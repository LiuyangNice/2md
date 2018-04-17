using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class FileCon : MonoBehaviour {
    public static FileCon Inst;
    

    public GameObject UserinfoPage;

    public Button LogOut;
    public Button back;


    public GameObject folderPrefab;
    public GameObject movPrefab;
    public GameObject reFreshBtn;
    public GameObject refreshShow;
    public Text[] MovNames;
    public GameObject ImParent;//文件父transform
    public List<GameObject> files = new List<GameObject>();//文件夹及文件
    public Stack<directory> myFile = new Stack<directory>();
    // Use this for initialization
    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        Inst = this;
        LogOut.onClick.AddListener(OnLogOutClick);
        UserInfoUpdate();
        if (GameData.currentDir==null)
        {
            NetworkManager.inst.GetDirectory();
        }
        else
        {
            NetworkManager.inst.GetDirectory(GameData.currentDir.id);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
   
    public void UpdateCurrent(directory dir)
    {
        //Screen.orientation = ScreenOrientation.Portrait;

        if (myFile.Count == 0)
        { myFile.Push(dir); }
        else if (myFile.Peek().id != dir.id)
        {
            myFile.Push(dir);
        }
        else if (myFile.Peek().id == dir.id)
        {
            myFile.Pop();
            myFile.Push(dir);
        }

        if (!FileManager.Inst.dirs.ContainsKey(dir.id))
        {
            GameData.currentDir =dir;
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
    public void OnLogOutClick()
    {
        myFile.Clear();
        GameData.currentDir = null;
        SceneManager.LoadScene(1);
    }
    public void UserInfoUpdate()
    {
        Text prot = UserinfoPage.transform.Find("UserHead").Find("Image").Find("Text").GetComponent<Text>();
        Text userName = UserinfoPage.transform.Find("Name").GetComponent<Text>();
        Text userInfo = UserinfoPage.transform.Find("School").GetComponent<Text>();
        
        prot.text = GameData.myUser.name.Substring(0, 2).ToUpper();
        userName.text = GameData.myUser.name;
        userInfo.text = GameData.myUser.info;

        

    }
    
    public IEnumerator MovePlay(childProj p)
    {
        GameData.currentProj = p;
        SceneManager.LoadScene(3);
        yield return 1f;

    }
    public void OnBackClick()
    {
        //myMachine.ChangeState(new FilePanel());
        ActionManager.inst.stopChecking();
    }
}
