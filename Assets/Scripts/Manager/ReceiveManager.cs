/*
 - FileName:      	ReceiveManager.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/21
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类型 
/// </summary>
public enum AssetType
{
    None = -1,  //空
    Spine2D,    //2D素材
    Model3D     //3D素材
}

public class ReceiveManager : Manager {
    readonly string m_spineJsonfileName = "2d_config.txt";        //spineJson文件名
    readonly string m_modelJsonfileName = "3d_config.txt";        //modelJson文件名

    readonly string functionname = "FunctionName";       //消息函数名
    readonly string bol = "Bool";                        //bool值
    readonly string recorder = "Recorder";               //录屏
    readonly string path = "Path";                       //路径
    readonly string context = "Context";                 //上下文
    readonly string value = "Value";                     //数值

    private ModelLogic modelLogic;

    public AssetType assetType;                         //素材类型
    public bool initEnd = true;                         //素材是否加载结束

    public void OnAwakeUp()
    {
        assetType = AssetType.None;
        modelLogic = new ModelLogic();
    }

    public void ReceiveMessage(string parms)
    {
        Dictionary<string, string> paraDic = new Dictionary<string, string>();
        string[] parmList = parms.Split('|');
        foreach (string parm in parmList)
        {
            string[] keyValueList = parm.Split('=');
            string key = keyValueList[0];
            string value = keyValueList[1];
            if (keyValueList.Length > 2)
            {
                for (int i = 2; i < keyValueList.Length; i++)
                {
                    value += "=" + keyValueList[i];
                }
            }
            paraDic.Add(key, value);
        }
        string functionName = paraDic[functionname];
        switch (functionName)
        {
            case "SpinePath"://spine素材包路径
                string mType = paraDic["Type"];
                if (initEnd)
                {
                    AnalyzeResources((end, success) => {
                        initEnd = end;
                        Util.Log("挂载" + success.ToString());
                    }, paraDic[path], mType);
                }
                break;
        }
    }

    /// <summary>
    /// 挂载控制
    /// </summary>
    void AnalyzeResources(Action<bool, bool> action, string filePath, string mType)
    {
        string configName = "";
        if (mType.Equals("2D"))
        {
            assetType = AssetType.Spine2D;
            configName = m_spineJsonfileName;
        }
        else if (mType.Equals("3D"))
        {
            assetType = AssetType.Model3D;
            configName = m_modelJsonfileName;
        }

        if (!string.IsNullOrEmpty(filePath.Replace("file://", "")))
        {
            string fileUrl = filePath.Replace("file://", "");
            string configPath = fileUrl + "/" + configName;
            if (Util.FileIsExistence(configPath))
            {
                string strjson = Util.GetFileText(configPath);
                if (null != strjson && "" != strjson)
                {
                    MountLogic((end, success) => {action(end, success);}, fileUrl, strjson);
                }
                else
                {
                    action(true, false);
                }
            }
            else
            {
                action(true, false);
            }
        }
        else
        {
            action(true, true);
        }
    }

    void MountLogic(Action<bool, bool> action, string path = "", string strjson = "")
    {
        switch (assetType)
        {
            case AssetType.Spine2D:

                break;

            case AssetType.Model3D:
                if (string.IsNullOrEmpty(path))
                {
                    modelLogic.DestroyModel();
                }
                else
                {
                    modelLogic.InstanceModel(path, strjson, (end, success) =>
                    {
                        action(end, success);
                    });
                }
                break;
            default:
                break;
        }
    }
}
