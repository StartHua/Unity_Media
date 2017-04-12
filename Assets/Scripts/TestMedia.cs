using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestMedia : MonoBehaviour {
    public GameObject LeftPlane;

    void Start() {
        MediaManager.Instance.InitMedia("http://res.utovr.com/common/player/javascript/UtoVRPlayer.swf?isShare=true&vcode=211403329269&referer=http%3a%2f%2fshare.utovr.com%2fvcode%2f211403329269%2fu.swf&parentReferer=");
        MediaManager.Instance._InitOK += InitOK;
        MediaManager.Instance._SetTextureOK += SetTextureOK;
        MediaManager.Instance._hasError += HasError;
        MediaManager.Instance._BufferPercentage += BuffPer;
    }

    public void InitOK()
    {
        int Width = MediaManager.Instance.GetVideoWidth();
        int height = MediaManager.Instance.GetVideHeight();
        if (Width > 640)
            Width = 640;
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
        Debug.Log("Unity==================== SetTextureOK");
        MediaManager.Instance.Play();
    }

    public void HasError()
    {
        Debug.Log("Unity==================== HasError");
    }
    public void BuffPer(int numPer)
    {
        //Debug.Log("Unity==================== Buffer : " + numPer); //测试滚动百分百
    
    }

    //====================Button===============
    public void StartPlay()
    {
        Debug.Log("startPlayer ===============================");
        MediaManager.Instance.Play();
    }

    public void PauseVideo()
    {
        MediaManager.Instance.Pause();
    }

    public void CommVideo()
    {
        MediaManager.Instance.CommonFormat();
    }

    public void LeftRightVideo()
    {
        MediaManager.Instance.LeftRightVideoFormat();
    }

    public void UpDownVideo()
    {
        MediaManager.Instance.UpDownVideoFormat();
    }


}
