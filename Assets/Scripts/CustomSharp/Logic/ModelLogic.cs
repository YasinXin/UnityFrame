/*
 - FileName:      	ModelLogic.cs
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

public class ModelLogic : Manager {

    public static MatrixVectorStruct m_MatrixVector;            //3D模型矩阵变化结构体
    private GameObject mountObj;
    private bool m_fristTimeTrack;
    private Material3DBean material3DBean;

    /// <summary>
    /// 加载3D模型素材
    /// </summary>
    public void InstanceModel(string path, string json, Action<bool, bool> action)
    {
        material3DBean = Util.GetJsonData<Material3DBean>(json, true);
        MaterialsMgr.OnInitModelManager(path, material3DBean.name, (end, success) =>
        {
            action(end, success);
        });
    }

    /// <summary>
    /// 清楚3D模型素材
    /// </summary>
    public void DestroyModel()
    {
        MaterialsMgr.DelectMountModel();
    }

    /// <summary>
    /// 函数初始化
    /// </summary>
    public void Init()
    {
        InitStruct();
        mountObj = FrameMgr.m_webCamera.modelRoot.gameObject;
        m_fristTimeTrack = true;
    }

    /// <summary>
    /// 初始化3D结构体
    /// </summary>
    void InitStruct()
    {
        Vector3 Point = Vector3.zero;
        Vector3 Scale = Vector3.one;
        Vector3 Angles = Vector3.zero;
        m_MatrixVector = new MatrixVectorStruct(Point, Scale, Angles);
    }

    /// <summary>
    /// 模型逻辑方法，检测中调用
    /// </summary>
    /// <param name="trackResult">If set to <c>true</c> track result.</param>
    /// <param name="track3dret">Track3dret.</param>
    public void LogicModel(bool trackResult, TRACK3DRET track3dret, int index)
    {
        ModelActiveLogic(trackResult, track3dret, index);

        //ModelTrackTrigger(trackResult, track3dret);
    }

    /// <summary>
    /// 模型检测显示逻辑，主逻辑
    /// </summary>
    /// <param name="bolarck">If set to <c>true</c> bolarck.</param>
    /// <param name="track3dret">Track3dret.</param>
    void ModelActiveLogic(bool boltarck, TRACK3DRET track3dret, int index)
    {
        //检测到人脸
        if (boltarck)
        {
            if (m_fristTimeTrack)
            {
                m_fristTimeTrack = false;
                FristTrackFaceMethod();
            }
            //循环检测
            OnTrackingFaceLoopMethod(boltarck, ref track3dret, "full", index);

        }
        else
        {
            m_fristTimeTrack = true;
            OnLostFaceMethod();
        }
    }

    /// <summary>
    /// 循环检测方法，需要检测人脸实时计算的逻辑
    /// </summary>
    /// <param name="track3dret">Track3dret.</param>
    //public static MatrixVectorStruct m_MatrixVector;
    void OnTrackingFaceLoopMethod(bool boltarck, ref TRACK3DRET track3dret, string viewType, int faceNum)
    {    
        //SDK返回矩阵值，重新计算u3d矩阵值，需要实时计算
        MaterialsMgr.GetTransformMatrix4x4(track3dret.allTrackPoints, track3dret.matrix, ref m_MatrixVector, viewType);
        m_MatrixVector.Point.z = 10f;      
        //模型跟随
        MaterialsMgr.MatrixToValue(mountObj, m_MatrixVector);       
    }

    /// <summary>
    /// 人脸丢失处理
    /// </summary>
    void OnLostFaceMethod()
    {
        mountObj.SetActive(false);
    }

    /// <summary>
    /// 首次检测到人脸处理
    /// </summary>
    void FristTrackFaceMethod()
    {
        mountObj.SetActive(true);
    }
}
