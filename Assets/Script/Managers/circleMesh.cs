using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class circleMesh : MonoBehaviour {
    public Mesh mesh;
    public int segment;
    public GameObject sphere;
    public GameObject line;
    public bool startdraw = false;
    public GameObject arrowHead;
    // Use this for initialization
    void Start () {
        segment = 8;
        //draw(10,new Vector3(10,0,30));
        //drawDefaultArrow();
        //Vector3 head = new Vector3(-5,5,5);
        //Vector3 end = new Vector3(-5,-5,10);
        //reDrawArrow(end, head);
        StartCoroutine(networktest());

     }
    void Update()
    {
        
    }
    IEnumerator networktest()
    {
        UnityWebRequest uw = UnityWebRequest.Get("www.baidu.com");
        yield return uw.Send();
        Debug.Log(uw.downloadHandler.text);
    }
    void drawCircle(float r,Vector3 position)
    {
        Matrix4x4 ro = Matrix4x4.LookAt(position,Vector3.zero,Vector3.up);
        Matrix4x4 m = Matrix4x4.Translate(position);
        Vector3[] v = new Vector3[segment];
        
        for (int i = 0; i <v.Length ; i++)
        {
            v[i] = new Vector3(r * Mathf.Sin(2 * Mathf.PI * i / segment), r * Mathf.Cos(2 * Mathf.PI * i / segment)+r, 0);
            v[i] = ro.MultiplyPoint3x4(v[i]);
            v[i] = m.MultiplyPoint3x4(v[i]);
        }
    }
    void draw(float r,Vector3 position)
    {
        Matrix4x4 m = Matrix4x4.Translate(position);
        Matrix4x4 ro = Matrix4x4.LookAt(position,Vector3.zero,Vector3.up);
        for (int i = 0; i < segment; i++)
        {
            GameObject s = Instantiate(sphere);
            
            s.transform.position = new Vector3(r*Mathf.Sin(2*Mathf.PI*i /segment),r*Mathf.Cos(2*Mathf.PI * i / segment),0);
            s.transform.position = ro.MultiplyPoint3x4(s.transform.position);
            s.transform.position = m.MultiplyPoint3x4(s.transform.position);
        }
    }

    

    void drawArrow(Vector3 basePos, Vector3 headPos)//默认是个向上的箭头 //
    {
        float Size = 0.5f;
        float Space = 0.2f;
        float length = (headPos - basePos).magnitude;

        Vector3[] body = new Vector3[2];
        Vector3[] head = new Vector3[3];

        body[0] = new Vector3(0,0,0);  //起点 ，干脆原地画一个一毛一样大的箭头，然后移到指定位置。就不用考虑缩放了。
        head[1] = new Vector3(0, length, 0);

        body[1] = head[1] - new Vector3(0, Space, 0);
        head[0] = head[1] + new Vector3(-Size * Mathf.Cos(Mathf.PI/4), -Size * Mathf.Cos(Mathf.PI/4),0);
        head[2] = head[1] + new Vector3(Size * Mathf.Cos(Mathf.PI / 4), -Size * Mathf.Cos(Mathf.PI / 4), 0);

        Vector3 originDir = (head[1] - body[0]).normalized;
        Vector3 originCenter = (head[1] - body[0]) * 0.5f;

        Vector3 targetDir = (headPos - basePos).normalized;
        Vector3 targetCenter = (headPos - basePos)*0.5f;

        //上面计算弄默认坐标。下面开始计算位移。
        Vector3 t = targetCenter - originCenter;
        Quaternion r = Quaternion.FromToRotation(originDir,targetDir);


        Debug.Log("target Quaternion is : " + r);
        Matrix4x4 trs = Matrix4x4.TRS(t,r,Vector3.one);

        

        for (int i = 0; i < head.Length; i++)
        {
            head[i] = trs.MultiplyPoint3x4(head[i]);
        }
        for (int i = 0; i < body.Length; i++)
        {
            body[i] = trs.MultiplyPoint3x4(body[i]);
        }
        Debug.Log(trs);

        foreach (Vector3 v3 in head)  //画箭头
        {
           GameObject s = Instantiate(sphere); //画球
            s.transform.position = v3;
        }
        float width = 0.1f;
        LineRenderer LineHead = Instantiate(line).GetComponent<LineRenderer>();
        LineHead.positionCount = head.Length;
        LineHead.SetPositions(head);
        LineHead.startWidth = width;
        LineHead.endWidth = width;
        foreach (Vector3 v3 in body)
        {
            GameObject s = Instantiate(sphere);
            s.transform.position = v3;
        }
        LineRenderer LineBody = Instantiate(line).GetComponent<LineRenderer>();
        LineBody.positionCount = body.Length;
        LineBody.SetPositions(body);
        LineBody.startWidth = width;
        LineBody.endWidth = width;
    }

    void reDrawArrow(Vector3 basePos, Vector3 headPos)// 重新来一遍，这次思路是直接那坐标画身子，只把
    {
        float Size = 0.1f;
        float length = (headPos - basePos).magnitude;

        Vector3 targetDir = (headPos - basePos).normalized;
        Vector3[] targetBody = new Vector3[2];
      
        targetBody[0] = basePos;
        targetBody[1] = headPos - targetDir * Size;

        Quaternion r = Quaternion.FromToRotation(Vector3.forward, targetDir);   

        GameObject targetHead = Instantiate(arrowHead);

       
        targetHead.transform.position = headPos - targetDir * Size;
        targetHead.transform.rotation = r;
        targetHead.transform.localScale = Vector3.one * Size* 20;
        

        float width = 0.1f;

        LineRenderer LineBody = Instantiate(line).GetComponent<LineRenderer>();
        LineBody.positionCount = targetBody.Length;
        LineBody.SetPositions(targetBody);
        LineBody.startWidth = width;
        LineBody.endWidth = width;
    }
    
}
