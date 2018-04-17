using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Trans : MonoBehaviour {
    float a;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        a += 180 * Time.deltaTime;
        transform.DOLocalRotate(new Vector3(0, 0, -a), Time.deltaTime);
	}
}
