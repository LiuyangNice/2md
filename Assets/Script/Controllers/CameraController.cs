using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour {
    float rotationX;
    float rotationY;
    public float sensitivityY;
    public float sensitivityX;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Cursor.lockState = CursorLockMode.Locked;   //锁定光标不能动    注：可按Esc解锁光标
            RotateForCam();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }    
    }
    void RotateForCam()//旋转
    {
        rotationX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
        transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
    }
   
}
