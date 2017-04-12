using UnityEngine;
using System.Collections;
using System;

public class MediaAndroid : MediaInterface
{
    private string javaClassCom = "com.HuaMedia.MediaHelp";  //android 类
    private AndroidJavaObject _javaObj = null;              //android 实例
    private AndroidJavaObject GetJavaObject()
    {
        if (_javaObj == null)
            _javaObj = new AndroidJavaObject(javaClassCom);
        return _javaObj;
    }

    private AndroidJavaObject GetCurActivity()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        return jo;
    }

    public void InitMedia(string MediaPath)
    {
        GetJavaObject().Call("InitMedia",GetCurActivity(), MediaPath);
    }

    public int GetVideoWidth()
    {
        return GetJavaObject().Call<int>("GetVideoWidth");
    }

    public int GetVideHeight()
    {
        return GetJavaObject().Call<int>("GetVideHeight");
    }

    public void SetVideoTextures(int[] textureids, int width, int heigth)
    {
        GetJavaObject().Call("SetVideoTextures", textureids, width, heigth);
        GL.InvalidateState();
    }

    public void Play()
    {
        GetJavaObject().Call("Play");
    }

    public void Pause()
    {
        GetJavaObject().Call("Pause");
    }
    public void ExitPlayer()
    {
        GetJavaObject().Call("ExitPlayer");
    }


    public void RePlay()
    {
        GetJavaObject().Call("RePlay");
    }

    public void CommonFormat()
    {
        GetJavaObject().Call("CommonFormat");
    }


    public void LeftRightVideoFormat()
    {
        GetJavaObject().Call("LeftRightVideoFormat");
    }

    public void UpDownVideoFormat()
    {
        GetJavaObject().Call("UpDownVideoFormat");
    }

    public long GetVideoLength()
    {
        return GetJavaObject().Call<long>("GetVideoLength");
    }

    public void UpdateVideoData()
    {
        GetJavaObject().Call("UpdateVideoData");
        GL.InvalidateState();
    }

    public bool SeekTo(long pos)
    {
        return GetJavaObject().Call<bool>("SeekTo",pos);
    }

    public long GetCurPos()
    {
        return GetJavaObject().Call<long>("GetCurPos");
    }

    public int GetState()
    {
        return GetJavaObject().Call<int>("GetState");
    }

    public void Release()
    {
        GetJavaObject().Call("Release");
    }
}
