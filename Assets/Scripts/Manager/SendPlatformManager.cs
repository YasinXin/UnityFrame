/*
 - FileName:      	SendPlatformManager.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/20
 - Email:         	#279444122@qq.com#
 - Description:     传送信息到其他平台
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPlatformManager : Manager {

#if UNITY_ANDROID
    AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
#endif
    public void OnAwakeUp()
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
       //AppConst.AndroidPath = getFilePath();
        //Util.Log("PlatformMgr AndroidPath===>" + AppConst.AndroidPath);
#endif
#endif
    }

    //传送Unity的状态信息
    public void UnityStateInfo(string info)
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        currentActivity.Call("UnityStateInfo", info);
#endif
#endif
    }

    /// <summary>
    /// 是否识别到人脸
    /// </summary>
    /// <param name="bol"></param>
    public void IsTrackFace(bool bol)
    {
#if !UNITY_EDITOR
#if UNITY_ANDROID
        currentActivity.Call("IsTrackFace", bol);  // bol  为true：识别到  为false：未识别到
#endif
#endif
    }
}
