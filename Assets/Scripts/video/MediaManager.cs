/*
    播放器管理器：IOS ，android 统一接口
    时间：2017-1-18
    创造者：ChenXingHua
    脚本:单列管理
*/

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 播放器接口
/// </summary>
public interface MediaInterface {
    void InitMedia(string MediaPath);               //初始化media
    int GetVideoWidth();                            //获取视频的宽
    int GetVideHeight();                            //获取视频的高
    void SetVideoTextures(int[] textureids, int width, int heigth); //设置视频显示渲染纹理
    void Play();                                    //开始播放
    void Pause();                                   //暂停
    void RePlay();                                  //重新播放
    void ExitPlayer();                              //离开播放器
    int GetState();                                 //获取播放状态

    void CommonFormat();                            //普通原来格式
    void LeftRightVideoFormat();                    //左右格式视频
    void UpDownVideoFormat();                       //上下格式视频
    long GetVideoLength();                         //获取视频的长度
    void UpdateVideoData();                         //更新视频数据
    bool SeekTo(long pos);                         //快进
    long GetCurPos();                              //获取视频播放位置
    void Release();                                 //释放
}

public enum VideoState
{
    NOT_READY,
    READY,
    PLAYING,
    PAUSED,
    ERROR
}

public interface MediaOKInterface
{
    void InitOK();
    void SetTextureOK();
    void bufferPercentage(string percentage);
    void hasError();
}

public class MediaManager : MonoBehaviour, MediaInterface, MediaOKInterface
{

    private static MediaManager _instance;
    public static MediaManager Instance
    {
        get {
            GameObject obj = GameObject.Find("MediaManager");
            if (obj == null)
                obj = new GameObject("MediaManager");
            _instance = obj.GetComponent<MediaManager>();
            if (_instance == null)
                _instance = obj.AddComponent<MediaManager>();
            return _instance;
        }
    }
    private MediaInterface media;
    public MediaInterface _media {
        get {
            if (media != null)
                return media;
#if UNITY_ANDROID
            media = new MediaAndroid();
#endif

#if UNITY_IPHONE || UNITY_IOS
                media =  new MediaIOS();
#endif

#if UNITY_STANDALONE_WIN
                media =  new MediaPC();
#endif
            return media;
        }
    }

    //外部回调
    public delegate void _InitOKDelegate();
    public _InitOKDelegate _InitOK;

    public delegate void _SetTextureDelegate();
    public _SetTextureDelegate _SetTextureOK;

    public delegate void _BufferPercentageDelegate(int percentage);     //(废弃重android 过来，换成SrollerPer）
    public _BufferPercentageDelegate _BufferPercentage;

    public delegate void _SorllerPreDelegate(float per);                //滚动条实现per百分比
    public _SorllerPreDelegate _SorllerPre;

    public delegate void _VideoOverDelagete();  //视频播放完成
    public _VideoOverDelagete _videoOver;         

    public delegate void _hasErrorDelagete();
    public _hasErrorDelagete _hasError;

    //内部常数
    private long durtion = -1; //视频总长度

    ///==============================Back=============================
    public void InitOK()
    {
        if (_InitOK != null)
            _InitOK();
    }
    public void SetTextureOK()
    {
        canUpdate = true;
        if (_SetTextureOK != null)
            _SetTextureOK();
      
    }

    public void bufferPercentage(string percentage)
    {
        if (_BufferPercentage != null)
        {
            _BufferPercentage(int.Parse(percentage));
        }
    }

    //滚动条百分比实现
    public void sorllerPre()
    {

        long videoLength = GetVideoLength();        //视频长度
        long curVideoLength = GetCurPos();          //当前视频播放进度
        if (videoLength <= 0 ) return;
        if (curVideoLength < 0 || curVideoLength > videoLength) return;
        float per = (float)curVideoLength / videoLength;
        //滚动条
        if (_SorllerPre != null)
            _SorllerPre(per);
        //播放完成
        if (_videoOver != null && per > 0.96f)
        {
            _videoOver();
            canUpdate = false; //切换不能播放
        }
    }

    public void hasError()
    {
        if (_hasError != null)
        {
            _hasError();
        }
    }

    ///==============================call=============================
    public void InitMedia(string MediaPath)
    {
        _media.InitMedia(MediaPath);
    }

    public int GetVideoWidth()
    {
        return _media.GetVideoWidth();
    }

    public int GetVideHeight()
    {
        return _media.GetVideHeight();
    }

    public void SetVideoTextures(int[] textureids, int width, int heigth)
    {
        _media.SetVideoTextures(textureids,width,heigth);
    }

    public void Play()
    {
        if (canUpdate == false) return;
        _media.Play();
    }

    public void Pause()
    {
        if (canUpdate == false) return;
        _media.Pause();
    }

    public void RePlay()
    {
        if (canPlay())
        {
            canUpdate = true;
            _media.RePlay();
        } 
    }

    public void ExitPlayer()
    {
        canUpdate = false;
        if (_media != null)
            _media.ExitPlayer();
    }

    public void CommonFormat()
    {
        _media.CommonFormat();
    }

    public void LeftRightVideoFormat()
    {
        _media.LeftRightVideoFormat();
    }

    public void UpDownVideoFormat()
    {
        _media.UpDownVideoFormat();
    }

    public long GetVideoLength()
    {
        if(durtion <= -1)
            durtion = _media.GetVideoLength();
        return durtion;
    }

    public void UpdateVideoData()
    {
        _media.UpdateVideoData();
    }

    public bool SeekTo(long pos)
    {
       return _media.SeekTo(pos);
    }
    //重写 SeekTo(long pos) 接收百分比
    public bool SeekTo(float per)
    {
        long tm = GetVideoLength() * (long)per;
        return SeekTo(tm);
    }

    public long GetCurPos()
    {
        return _media.GetCurPos();
    } 

    public int GetState()
    {
        return _media.GetState();
    }

    /// <summary>
    /// 多构造出一个枚举转换函数
    /// </summary>
    /// <returns></returns>
    public VideoState GetStateEnum()
    {
        int state = GetState();
        if (state == 0)
            return VideoState.NOT_READY;
        if (state == 1)
            return VideoState.READY;
        if (state == 2)
            return VideoState.PLAYING;
        if (state == 3)
            return VideoState.PAUSED;
        if (state == 4)
            return VideoState.ERROR;
        return VideoState.ERROR;
        
    }
    public void Release()
    {
        _media.Release();
    }


    bool canUpdate = false; //是否能循环播放
    void Update()
    {
        if (canUpdate)
        {
            UpdateVideoData();
            sorllerPre();           //滚动百分百
        }
    }


    //进入后台的前状态
    VideoState beforeState = VideoState.NOT_READY;  
    //系统休眠强制暂停
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            beforeState = GetStateEnum();   //纪录上个
            Pause();                    //停止播放
        }
        else {
            //恢复到播放前的状态
            if (beforeState == VideoState.PLAYING)
            {
                Play();
            }
        }
    }

    private bool canPlay()
    {
        VideoState state = GetStateEnum();
        if (state != VideoState.ERROR || state != VideoState.NOT_READY)
            return true;
        return false;
    }
}
