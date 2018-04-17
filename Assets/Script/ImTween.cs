using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ImTween : MonoBehaviour {

    public float ts = 0.8f;
    public float timer = 0.5f;
    public Ease ease;
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localScale==new Vector3(ts,ts,ts))
        {
            transform.DOScale(new Vector3(1, 1, 1), timer);
        }
        if (transform.localScale==new Vector3(1,1,1))
        {
            transform.DOScale(new Vector3(ts, ts, ts), timer);
        }
	}
}
