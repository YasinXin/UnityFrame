/*
 - FileName:      	WebCamera.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/20
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamera : MonoBehaviour {
    private Transform panelCanvas;                              //ui节点
    private RawImage viewTexture;                               //图像显示面板
    public Camera viewCamera;                                   //视频图像相机
    public Camera modelCamera;                                  //3D物体相机
    public Transform modelRoot;                                 //3D物体节点
    public Transform glsssesTrans;
    public GameObject faceObj;

    private Vector2 viewSize = new Vector2(1920, 1080);         //图像分辨率
    private Texture2D nativeTexture = null;                     //获取传输过来的图像
    private Int32 texPtrs = -1;                                 //图像ID

    /// <summary>
    /// 获取2D面板里的对象
    /// </summary>
    public void Get2DPanelObj()
    {
        panelCanvas = GameObject.Find("PanelCanvas").transform;
        viewTexture = panelCanvas.Find("PanelRoot/viewTexture").GetComponent<RawImage>();
        viewCamera = panelCanvas.Find("viewCamera").GetComponent<Camera>();

        modelCamera = GameObject.Find("modelCamera").GetComponent<Camera>();
        modelRoot = GameObject.Find("modelRoot").transform;
        glsssesTrans = GameObject.Find("glassesTrans").transform;
        faceObj = modelRoot.Find("face").gameObject;
    }
   
    /// <summary>
    /// 更新画面
    /// </summary>
    public void UpdateTexture2D()
    {
        if (SendPlatformManager.currentActivity == null)
        {
            return;
        }
        try
        {
            if (texPtrs < 0)
            {
                texPtrs = SendPlatformManager.currentActivity.Call<Int32>("getTextureId");
                Util.Log("UpdateTexture2D==>" + texPtrs + "    " + viewSize);
                nativeTexture = Texture2D.CreateExternalTexture((int)viewSize.x, (int)viewSize.y, TextureFormat.ARGB32, false, false, new System.IntPtr(texPtrs));
                viewTexture.texture = nativeTexture;
            }
            SendPlatformManager.currentActivity.Call("DrawFrame");
            GL.InvalidateState();
        }
        catch (Exception)
        {
            Util.Log("DrawFrame出错！");
        }
    }

    /// <summary>
    /// 测试工程更新画面
    /// </summary>
    /// 
    public void InitTexture()
    {
        texPtrs = SendPlatformManager.currentActivity.Call<Int32>("GetTexturePtr");
        nativeTexture = Texture2D.CreateExternalTexture((int)viewSize.x, (int)viewSize.y, TextureFormat.ARGB32, false, false, new System.IntPtr(texPtrs));
        viewTexture.texture = nativeTexture;
    }

    public void UpdateTexture()
    {
        if (SendPlatformManager.currentActivity == null)
        {
            return;
        }
        try
        {
            SendPlatformManager.currentActivity.Call("DrawFrame");
            GL.InvalidateState();
        }
        catch (Exception)
        {
            Util.Log("DrawFrame出错！");
        }
    }
}
