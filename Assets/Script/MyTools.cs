using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Net;
public class MyJson
{
    private static MyJson inst;

    public static MyJson Inst
    {
        get
        {
            if (inst == null)
            {
                inst = new MyJson();
            }
            return inst;
        }
    }
    public User ReadJson()//读json文件
    {
        string filePath = Application.persistentDataPath + "/2MD.json";
        string json = ReadFile(filePath);
        return JsonConvert.DeserializeObject<User>(json);
    }
    public void WriteJson(object obj)//写json文件
    {
        string path = Application.persistentDataPath;
        string filePath = path + "/" + "2MD" + ".json";
        string json = JsonConvert.SerializeObject(obj);
        WriteFile(filePath, json);
    }
    void WriteFile(string path, string json)
    {
        FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(json);
        sw.Close();
        fs.Close();

    }
    string ReadFile(string path)
    {
        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
        StreamReader sw = new StreamReader(fs);
        string a = sw.ReadToEnd();
        sw.Close();
        fs.Close();
        return a;
    }
}
public class User
{
    public string user;
    public string password;
    public bool isRember;
}
public enum VideoPlayerState
{
    LoginPanel,
    FilePanel,
    PlayerPanel
}
public class MyHttp
{
    /// <summary>
    /// 下载进度(百分比)
    /// </summary>
    public float progress { get; private set; }
    private bool isStop;
    private Thread thread;
    /// <summary>
    /// 下载文件(断点续传)
    /// </summary>
    /// <param name="_url">下载地址</param>
    /// <param name="_filePath">本地文件存储目录</param>
    public void Download(string _url,string _fileDirectory,string nameStr)
    {
        isStop = false;
        thread = new Thread(delegate ()
        {
            if (!Directory.Exists(_fileDirectory))
                Directory.CreateDirectory(_fileDirectory);
            string filePath;
            filePath = _fileDirectory + "/"+ nameStr;
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            long fileLength = fileStream.Length;
            float totalLength = GetLength(_url);
            
            if (fileLength < totalLength)
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_url);
                request.AddRange((int)fileLength);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                fileStream.Seek(fileLength, SeekOrigin.Begin);
                Stream httpStream = response.GetResponseStream();
                byte[] buffer = new byte[1024];
                int length = httpStream.Read(buffer, 0, buffer.Length);
                while (length > 0)
                {
                    if (isStop)
                        break;
                    fileStream.Write(buffer, 0, length);
                    fileLength += length;
                    progress = fileLength / totalLength ;
                    fileStream.Flush();
                    length = httpStream.Read(buffer, 0, buffer.Length);
                }
                httpStream.Close();
                httpStream.Dispose();
                response.Close();
                request.Abort();
                
            }
            else
            {
                Debug.Log(fileLength + " : " + totalLength);
                progress = fileLength / totalLength;
            }
                
            fileStream.Close();
            fileStream.Dispose();
        });
        thread.IsBackground = true;
        thread.Start();
    }
    /// <summary>
    /// 关闭线程
    /// </summary>
    public void Close()
    {
        isStop = true;
    }
    long GetLength(string _fileUrl)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(_fileUrl);
        //request.Method = "HEAD";
        HttpWebResponse res = (HttpWebResponse)request.GetResponse();
        long a = res.ContentLength;
        res.Close();
        res = null;
        request.Abort();
        request = null;
        return a;
    }
}