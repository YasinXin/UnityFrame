/*
 - FileName:      	FrameManager.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/20
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : Manager {
    [HideInInspector]
    public WebCamera m_webCamera;

    #region 人脸检测
    public bool m_refbolTarck = false;              //人脸检测返回结果    
    public static bool refbolTarck;
    private float m_waitForSeconds = 0.03f;         //人脸检测间隔时间  
    private bool bol_trackFace = true;              //人脸检查开关
    private bool bol_IsFirst = true;                //判断是否第一次启动
    public float trackSmoothValue;                  //人脸跟踪的平滑值(0.0f~3.0f)， 数值越大越抑制定位抖动,但是会导致跟踪延迟
    private TrackTool m_trackTool;
    private ModelLogic m_modelLogic;

    [HideInInspector]
    public TRACK2DRET[] track2dretResult;
    [HideInInspector]
    public TRACK3DRET[] track3dretResult;
    [HideInInspector]
    public int faceNum = 1;                         //当前检测到人脸数   
    private int beforeFaceNum = 0;                  //上一次检测到人脸数
    #endregion

    /// <summary>
    /// 初始化程序
    /// </summary>
    public void OnAwakeUp()
    {
        m_webCamera = new WebCamera();              //初始化相机类
        m_webCamera.Get2DPanelObj();
        m_trackTool = new TrackTool();              //初始化检测类
        m_modelLogic = new ModelLogic();            //初始化模型逻辑类
        m_modelLogic.Init();                        //初始化ModelLogic

        InitWebCamera();
        track3dretResult = new TRACK3DRET[faceNum];

        StartCoroutine(UpdateFrame());
        StartCoroutine(UpdateTrack());
    }

    /// <summary>
    /// 初始化相机设置
    /// </summary>
    void InitWebCamera()
    {
        if (AppParameter.isEditorPlatform)
        {
            return;
        }

        #region 测试工程初始化
        //SendPlatformManager.currentActivity.Call("InitOK");
        //m_webCamera.InitTexture();
        #endregion

        #region 正式工程初始化
        PlatformMgr.UnityStateInfo("Init");
        PlatformMgr.UnityStateInfo(AppParameter.unityVersion);
        #endregion
    }

    /// <summary>
    /// 人脸检查循环
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTrack()
    {
        while (true)
        {
            if (bol_trackFace && !AppParameter.isEditorPlatform)
            {
                TrackEvent();
            }

            if (m_refbolTarck)
            {
                m_waitForSeconds = 1 / AppConst.maxTraceRate;
            }
            else
            {
                m_waitForSeconds = 1 / AppConst.minTraceRate;
            }
            yield return new WaitForSeconds(m_waitForSeconds);
        }
    }

    /// <summary>
    /// 更新视频循环
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateFrame()
    {
        while (true)
        {
            #region 测试工程
            //m_webCamera.UpdateTexture();
            #endregion
            #region 正式工程
            m_webCamera.UpdateTexture2D();
            #endregion
            yield return new WaitForSeconds(1 / AppConst.GameFrameRate);
        }
    }

    /// <summary>
    /// 人脸检测
    /// </summary>
    void TrackEvent()
    {
        //返回是否识别到人脸
        if (m_refbolTarck != refbolTarck)
        {
            //Util.Log("TrackEvent==> " + m_refbolTarck);
            refbolTarck = m_refbolTarck;
            PlatformMgr.IsTrackFace(refbolTarck);
        }

        if (bol_IsFirst)
        {
            bol_IsFirst = false;
        }
        else
        {
            trackSmoothValue = AppConst.SmoothValue;
            m_refbolTarck = m_trackTool.GetTrack(ref faceNum, ref track2dretResult, ref track3dretResult, trackSmoothValue);         
        }

        if (track3dretResult.Length > 0)
        {
            m_modelLogic.LogicModel(refbolTarck, track3dretResult[0], 1);
        }
    }
}
