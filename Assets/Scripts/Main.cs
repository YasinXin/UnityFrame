using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    [Tooltip("Unity版本")]
    public string unityversion = "Unity_E_V0.0";
    [Tooltip("是否显示 Unity Log")]
    public bool debugLog;

    private void Awake()
    {
        AppParameter.unityVersion = unityversion;
        Util.m_Log = debugLog;
    }

    void Start()
    {
        AppFacade.Instance.StartUp();   //启动游戏
    }
}