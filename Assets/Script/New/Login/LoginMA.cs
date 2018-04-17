using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
public class LoginMA : MonoBehaviour {
    public static LoginMA Inst;
    public InputField IuserName;//账号输入
    public InputField Ipassword;//密码输入
    public Toggle isRember;//记住密码
    public Button logIn;//登录按钮
    public Text ErrorSow;
    public GameObject net;
    // Use this for initialization
    void Start () {
        Inst = this;
        logIn.onClick.AddListener(OnLogInClick);
        Screen.orientation = ScreenOrientation.Portrait;
        InstInfo();
    }
	
	// Update is called once per frame
	void Update () {
		
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
        //
        SceneManager.LoadScene(2);
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
    public void UserInfoUpdate(organization or)
    {
           
        GameData.myUser.id = or.id;
        GameData.myUser.info = or.verboseName;
        GameData.myUser.name = or.code;

    }
}
