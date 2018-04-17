using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData {
    public static UserInfo myUser = new UserInfo();
    public static directory currentDir;
    public static childProj currentProj;

}
public class UserInfo
{
    public int id;
    public string name;
    public string info;
    public int role;
}

