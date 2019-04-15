/*
 - FileName:      	Material3DBean.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/04/02
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/

using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 3D素材配置文件
/// 对应3d_config.txt内的配置文件
/// </summary>
[Serializable]
public class Material3DBean
{
    #region Property
    //3D unity资源文件，通过此文件加载打包的资源；
    //资源以.unity结尾
    public string name { get; set; }

    //该素材是否有需要实例化的资源。
    public bool NeedInstantiateAssets { get; set; }

    //判断模型 face
    public string faceTexture { get; set; }

    //判断模型 face 是否显示
    public bool showface { get; set; }

    //3D动画数据对象列表，配置基本属性、动画事件
    public List<Model3DBean> model { get; set; }

    #endregion
}
