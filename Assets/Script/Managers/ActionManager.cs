using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Vectrosity;

public class ActionManager : MonoBehaviour {

    public static ActionManager inst;
    public List<action> ActionList = new List<action>();
    public GameObject arrowPref;
    public LineRenderer linePref;
    public Camera cam;
    public GameObject textPref;

    public Coroutine checkingC;
    VideoPlayerController player;
    public int segment = 32;
    public List<GameObject> CurrentActionPool = new List<GameObject>(); //现在的做法是碰到谁画谁，一旦继续进行播放就释放掉原来的action。
    
    public Color actionColor = Color.blue;
    float timer = 0;
    // Use this for initialization
    private void Awake()
    {
        inst = this;
    }
    void Start () {

        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void StartCheckAction()
    {
        Debug.Log("StartCheck");
       player = VideoPlayerController.inst;
       checkingC = StartCoroutine(CheckAction());          
    }

    public IEnumerator CheckAction()
    {
        bool isChecking = true;
        yield return 1f;
        //Debug.Log(player.CurrentTime + "   " + player.movPlayer.Info.GetDurationMs() / 1000);
        //Debug.Log(isChecking);
        while (player.CurrentTime < player.movPlayer.Info.GetDurationMs()/1000 && isChecking)
        {
            //Debug.Log(player.CurrentTime + " duration " + player.videoDuration);
            for (int i = 0; i < ActionList.Count; i++)
            {
                if (player.CurrentTime > ActionList[i].time  && player.CurrentTime - Time.deltaTime < ActionList[i].time && isChecking)    //先是正着波，波
                {
                    player.Pause();
                    drawAction();
                    isChecking = false;
                }
            }
            yield return 1f;
        }
    }

    void drawAction()
    {
        for (int i = 0; i < ActionList.Count; i++)
        {
            if (ActionList[i].time < player.CurrentTime )
            {
                decypherCode(ActionList[i]);
                ActionList.RemoveAt(i);
                
                Debug.Log("draw action " + i);
            }
        }
    }
    void CleanAction()
    {
        foreach (GameObject action in CurrentActionPool)
        {
            Destroy(action);
        }
        CurrentActionPool.Clear();
    }
    public void stopChecking()
    {
        if (checkingC!= null)
        {
            StopCoroutine(checkingC);
        }
        if (CurrentActionPool.Count!= 0)
        {
            CleanAction();
        }
    }
    public void decypherCode(action act)
    {
        switch (act.type)
        {
            case "BRUSH":
                Brush brush = JsonConvert.DeserializeObject<Brush>(act.content);
                DrawBrush(brush);
                break;
            case "ARROW":
                Arrow arrow = JsonConvert.DeserializeObject<Arrow>(act.content);
                DrawArrow(arrow);
                break;
            case "HIGHLIGHT":
                Highlight highlight = JsonConvert.DeserializeObject<Highlight>(act.content);
                DrawHighLight(highlight);
                break;
            case "TEXT":
                Texts texts = JsonConvert.DeserializeObject<Texts>(act.content);
                DrawTexts(texts);
                break;
            default:
                break;
        }
    }
    public void DrawBrush(Brush brush)
    {
        LineRenderer line = Instantiate(linePref.gameObject).GetComponent<LineRenderer>();
        for (int i = 0; i < brush.vertices.Length; i++)
        {
            brush.vertices[i] = vFilter(brush.vertices[i]);
        }
        line.positionCount = brush.vertices.Length;
        line.SetPositions(brush.vertices);
        line.startColor = actionColor;
        line.endColor = actionColor;

        CurrentActionPool.Add(line.gameObject);
    }

    void DrawArrow(Arrow arrow)// 重新来一遍，这次思路是直接那坐标画身子，只把
    {
        Vector3 basePos = vFilter(arrow.basePos);
        Vector3 headPos = vFilter(arrow.headPos);

        float Size = 0.5f;
        float length = (headPos - basePos).magnitude;

        Vector3 targetDir = (headPos - basePos).normalized;
        Vector3[] targetBody = new Vector3[2];

        targetBody[0] = basePos;
        targetBody[1] = headPos - targetDir * Size;

        Quaternion r = Quaternion.FromToRotation(Vector3.forward, targetDir);

        GameObject a = Instantiate(arrowPref);
        GameObject targetHead = a.GetComponent<ArrowPref>().arrowHead;

        targetHead.transform.position = headPos - targetDir * Size;
        targetHead.transform.rotation = r;
        targetHead.transform.localScale = Vector3.one * Size * 20;
        targetHead.GetComponent<MeshRenderer>().material.color = actionColor;

        float width = 0.3f;

        LineRenderer LineBody = a.GetComponent<ArrowPref>().arrowBody;
        LineBody.positionCount = targetBody.Length;
        LineBody.SetPositions(targetBody);
        LineBody.startWidth = width;
        LineBody.endWidth = width;
        LineBody.startColor = actionColor;
        LineBody.endColor = actionColor;

        CurrentActionPool.Add(a);
    }

    void DrawHighLight(Highlight hl)
    {
        
        float r = hl.radius;
        Vector3 position = vFilter(hl.position);
        Matrix4x4 ro = Matrix4x4.LookAt(position.normalized, Vector3.zero, Vector3.up);
        Matrix4x4 m = Matrix4x4.Translate(position);
        Vector3[] v = new Vector3[segment];

        for (int i = 0; i < v.Length; i++)
        {
            v[i] = new Vector3(r * Mathf.Sin(2 * Mathf.PI * i / segment), r * Mathf.Cos(2 * Mathf.PI * i / segment)+r, 0);
            v[i] = ro.MultiplyPoint3x4(v[i]);
            v[i] = m.MultiplyPoint3x4(v[i]);
        }

        LineRenderer circle = Instantiate(linePref.gameObject).GetComponent<LineRenderer>();
        
        circle.positionCount = v.Length;
        circle.SetPositions(v);
        circle.loop = true;
        circle.startColor = actionColor;
        circle.endColor = actionColor;

        CurrentActionPool.Add(circle.gameObject);
    }
    public void DrawTexts(Texts texts) //字多的时候可能有bug。
    {
        Vector3 pos = vFilter(texts.position);

        GameObject text = Instantiate(textPref);
        text.GetComponent<textObj>().textmesh.text = texts.text;
        text.GetComponent<textObj>().background.transform.localScale = new Vector3(texts.width, texts.height,0);
        text.transform.position = pos;
        Vector3 direction = Vector3.zero - pos;
        text.transform.LookAt(Vector3.zero);
        text.transform.Rotate(0,180f,0);

        CurrentActionPool.Add(text);
    }

    Vector3 vFilter(Vector3 vector)
    {
        Vector3 newv = new Vector3(-vector.z, vector.y,-vector.x);
        return newv;
    }
}
public class ActionInst
{
    public float time;
    public GameObject action;
}
public class Arrow : BaseHint
{
    public Vector3 position;
    public Quaternion quaternion;
    public float length;
    public Vector3 headPos;
    public Vector3 basePos;
}
public class Highlight : BaseHint
{
    public Vector3 position;
    public float radius;
}
public class Brush : BaseHint
{
    public Vector3[] vertices;
}
public class Texts : BaseHint
{
    public string text;
    public float width;
    public float height;
    public Vector3 position;
}
public class BaseHint
{
    public string uuid;
    public string forWebData;
}

