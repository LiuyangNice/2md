using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogIn : State<FileCon>
{
    public override void EnterState(FileCon target)
    {
        WinMa.Inst.loginPage.SetActive(true);
        //FileCon.Inst.InstInfo();
        
    }
    public override void UpdateState(FileCon target)
    {
        
    }
    public override void ExitState(FileCon target)
    {
        WinMa.Inst.loginPage.SetActive(false);
    }

    
}
public class FileShow : State<FileCon>
{
    RectTransform rect, imParentRect;
    public override void EnterState(FileCon target)
    {
        Screen.orientation = ScreenOrientation.Portrait;
        rect = FileCon.Inst.reFreshBtn.transform as RectTransform;
        imParentRect = FileCon.Inst.ImParent.transform as RectTransform;
        WinMa.Inst.filePage.SetActive(true);

    }
    public override void UpdateState(FileCon target)
    {
        if (Input.GetKeyUp(KeyCode.Escape) && FileCon.Inst.myFile.Count > 1)
        {
            FileCon.Inst.Back();
        }
        rect.localScale = new Vector3(1, -imParentRect.localPosition.y / 200, 1);
    }
    public override void ExitState(FileCon target)
    {
        WinMa.Inst.filePage.SetActive(false);
    }
}
