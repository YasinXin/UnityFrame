/*
 - FileName:      	AppParameter.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/21
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppParameter {
    public static string unityVersion;                                  //版本
    public static bool isEditorPlatform = true;                         //是否在编辑平台
    public static Vector2 trackResolution = new Vector2(640, 360);      //识别的画面尺寸
    public static bool refbolTarck = false;                             //人脸检查返回结果
    public static int faceNumber = 0;                                   //检测到人脸数
    public enum ViewType
    {
        full,                                                           //全屏
        right,                                                          //右屏
        left                                                            //左屏
    }
    public static ViewType viewType = ViewType.full;                    //屏幕挂载区域
}
