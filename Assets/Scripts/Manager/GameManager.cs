using UnityEngine;

public class GameManager : Manager
{
    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        DontDestroyOnLoad(gameObject);  //防止销毁自己

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.GameFrameRate;
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            AppParameter.isEditorPlatform = true;
        }
        else
        {
            AppParameter.isEditorPlatform = false;
        }
        Util.Log("Init==>" + Application.platform);

        PlatformMgr.OnAwakeUp();                        //平台管理初始化     
        ReceiveMgr.OnAwakeUp();
        FrameMgr.OnAwakeUp();                           //画面识别更新管理初始化
        MaterialsMgr.OnAwakeUp();                       //材质管理初始化      
    }

    /// <summary>
    /// 接收外部消息
    /// </summary>
    /// <param name="msg"></param>
    public void message(string msg)
    {
        Util.Log("Messgae==> " + msg);
        ReceiveMgr.ReceiveMessage(msg);
    }
}
