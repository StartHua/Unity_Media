using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class D2Video : MonoBehaviour {

    private GameObject LeftPlane;

    private GameObject _canvas;
    private GameObject _start;
    private GameObject _pause;
    private GameObject _Scroller;
    private Image _preScroller;
    private Camera _mainCamer;

    private Image _localVideo;
    private Image _netVideo;

    private Sprite _hightVideoSprite;
    private Sprite _oldVideoSprite;

    void Start()
    {
        LeftPlane = GameObject.Find("LeftPlane");
        _canvas = GameObject.Find("Canvas");
        _start = _canvas.transform.Find("start").gameObject;
        _pause = _canvas.transform.Find("Pause").gameObject;
        _Scroller = _canvas.transform.Find("Scroller").Find("Scroll").gameObject;
        _preScroller = _Scroller.GetComponent<Image>();
        _mainCamer = GameObject.Find("Main Camera").GetComponent<Camera>();
        RegisterEvent();
        Init();
        InitMedia("http://res.utovr.com/common/player/javascript/UtoVRPlayer.swf?isShare=true&vcode=211403329269&referer=http%3a%2f%2fshare.utovr.com%2fvcode%2f211403329269%2fu.swf&parentReferer=");//地址也许失效了
    }


    void RegisterEvent()
    {
        UUIEventListener.Get(_start).onClick = StartBtn;
        UUIEventListener.Get(_pause).onClick = PauseBtn;

        GameObject quit = _canvas.transform.Find("quit").gameObject;
        UUIEventListener.Get(quit).onClick = QuitBtn;

        GameObject com = _canvas.transform.Find("com").gameObject;
        UUIEventListener.Get(com).onClick = ComBtn;

        GameObject leftRight = _canvas.transform.Find("LeftRight").gameObject;
        UUIEventListener.Get(leftRight).onClick = leftRightBtn;

        GameObject UpDown = _canvas.transform.Find("UpDown").gameObject;
        UUIEventListener.Get(UpDown).onClick = UpdownBtn;

        GameObject replay = _canvas.transform.Find("rePlay").gameObject;
        UUIEventListener.Get(replay).onClick = ReplayBtn;

        GameObject localVideo = _canvas.transform.Find("LocalVideo").gameObject;
        UUIEventListener.Get(localVideo).onClick = LocalVideoBtn;
        _hightVideoSprite = localVideo.GetComponent<Image>().sprite;
        _localVideo = localVideo.GetComponent<Image>();
     

       GameObject netVideo = _canvas.transform.Find("NetVideo").gameObject;
       UUIEventListener.Get(netVideo).onClick = NetVideoBtn;
       _netVideo = netVideo.GetComponent<Image>();
       _oldVideoSprite = _netVideo.sprite;
       _netVideo.sprite = _hightVideoSprite; //默认是网络
        _localVideo.sprite = _oldVideoSprite;
    }

    private void Init()
    {
        _pause.SetActive(false);        //停止播放先隐藏
        _preScroller.fillAmount = 0.0f; //初始化播放器的进度条
    }
    private void InitMedia(string Url)
    {
        MediaManager.Instance.InitMedia(Url);
        MediaManager.Instance._InitOK += InitOK;
        MediaManager.Instance._SetTextureOK += SetTextureOK;
        MediaManager.Instance._hasError += HasError;
        MediaManager.Instance._BufferPercentage += BuffPer;
        MediaManager.Instance._SorllerPre += ScrollerPer;
        MediaManager.Instance._videoOver += OVerVideo;
    }
    private void ResMediaEvent()
    {
        MediaManager.Instance._InitOK -= InitOK;
        MediaManager.Instance._SetTextureOK -= SetTextureOK;
        MediaManager.Instance._hasError -= HasError;
        MediaManager.Instance._BufferPercentage -= BuffPer;
        MediaManager.Instance._SorllerPre -= ScrollerPer;
        MediaManager.Instance._videoOver -= OVerVideo;
    }

    //==========================播放器回调========================
    public void InitOK()
    {
        int Width = MediaManager.Instance.GetVideoWidth();
        int height = MediaManager.Instance.GetVideHeight();
        if (Width > 1136)
            Width = 1136;
        if (height > 640)
            height = 640;

        Texture2D texture = new Texture2D(Width, height, TextureFormat.BGRA32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;

        Renderer render = LeftPlane.GetComponent<Renderer>();
        render.material.mainTexture = texture;

        int id = (int)texture.GetNativeTexturePtr().ToInt32();
        MediaManager.Instance.SetVideoTextures(new int[] { id }, Width, height);
    }

    public void SetTextureOK()
    {
        MediaManager.Instance.CommonFormat();
        StartBtn(null);
    }

    public void HasError()
    {

    }

    public void BuffPer(int per)
    {
        //Debug.Log("Unity Buffer : " + per);
    }

    public void ScrollerPer(float per)
    {
        _preScroller.fillAmount = per;
    }

    public void OVerVideo()
    {
        Debug.Log("播放完成");
    }
    //=========================按钮事件回调=======================
    //开始
    public void StartBtn(GameObject obj)
    {
        Debug.Log("开始播放======");
        _start.SetActive(false);
        _pause.SetActive(true);
        MediaManager.Instance.Play();
    }
    //暂停
    public void PauseBtn(GameObject obj)
    {
        Debug.Log("停止播放=====");
        _start.SetActive(true);
        _pause.SetActive(false);
        MediaManager.Instance.Pause();
    }
    //关闭
    public void QuitBtn(GameObject obj)
    {
        Debug.Log("关闭=====");
        Application.Quit();
    }
    public void ComBtn(GameObject obj)
    {
        Debug.Log("普通模式");
        MediaManager.Instance.CommonFormat();
    }
    public void leftRightBtn(GameObject obj)
    {
        Debug.Log("左右模式");
        MediaManager.Instance.LeftRightVideoFormat();
    }
    public void UpdownBtn(GameObject obj)
    {
        Debug.Log("上下模式");
        MediaManager.Instance.UpDownVideoFormat();
    }
    public void ReplayBtn(GameObject obj)
    {
        Debug.Log("重新播放");
        MediaManager.Instance.RePlay();
    }
    public void LocalVideoBtn(GameObject obj)
    {
        Debug.Log("本地视频");

        ResMediaEvent();

        MediaManager.Instance.ExitPlayer();

        InitMedia("VuforiaSizzleReel_1.mp4");

        _localVideo.sprite = _hightVideoSprite;
        _netVideo.sprite = _oldVideoSprite;
    }
    public void NetVideoBtn(GameObject obj)
    {
        Debug.Log("网络播放");

        ResMediaEvent();

        MediaManager.Instance.ExitPlayer();

        InitMedia("http://video.dlodlo.com/mp4_2k/2016/05/30/hNO4DA2Y7wnLELUXQXw950X__2k.mp4?auth_key=1486955906-0-0-46ad866810aa5832bc1d4f548b8ed3e8");

        _localVideo.sprite = _oldVideoSprite;
        _netVideo.sprite = _hightVideoSprite;
    }

}
      