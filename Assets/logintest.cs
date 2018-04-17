using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logintest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void login()
    {
        loginSnd ls = new loginSnd();
        ls.username = "boluo";
        ls.password = "123456";
        NetworkManager.inst.Login(ls);
    }
}
