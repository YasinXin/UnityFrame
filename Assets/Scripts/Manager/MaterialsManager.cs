/*
 - FileName:      	MaterialsManager.cs
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

public class MaterialsManager : Manager {

    //模型缩放值
    Vector3 vScale = Vector3.zero;
    //模型角度
    Quaternion angleQua;
    //sdk中矩阵值
    Matrix4x4 matr = Matrix4x4.zero;
    //基准矩阵
    Matrix4x4 size_fit_matrix = Matrix4x4.zero;
    //用来存放结果的矩阵，axb的结果为a的行数和b的列数
    Matrix4x4 result = Matrix4x4.zero;
    //摄像机点原始默认orthographicSize  值
    float m_originalOrthographicSize = 1.0f;
    //摄像机点当前orthographicSize  值
    float m_newOrthographicSize = 1.0f;
    float m_screneScaleFactor = 0.0f;
    float m_screenWidth = 0.0f;

    GameObject faceObj;
    GameObject currentGlasses;

    /// <summary>
    /// 初始化程序
    /// </summary>
    public void OnAwakeUp()
    {
        faceObj = FrameMgr.m_webCamera.faceObj;
        if(FrameMgr.m_webCamera.glsssesTrans.childCount > 0)
        {
            currentGlasses = FrameMgr.m_webCamera.glsssesTrans.GetChild(0).gameObject;
        }
        CheckIntoDataInfo();
    }

    /// <summary>
    /// 初始化眼镜模型
    /// </summary>
    public void OnInitModelManager(string path, string mName, Action<bool, bool> action)
    {
        DelectMountModel();

        AssetBundle assetbundle_asset = null;
        Util.Log("OnInitModelManager==>" + path + "     " + mName);
        assetbundle_asset = AssetBundle.LoadFromFile(path + mName);
        GameObject[] prefabs = assetbundle_asset.LoadAllAssets<GameObject>();
        if(prefabs.Length > 0)
        {
            GameObject go = Instantiate(prefabs[0]);
            go.transform.SetParent(FrameMgr.m_webCamera.glsssesTrans);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.Euler(Vector3.zero);
            currentGlasses = go;
        }
        if(assetbundle_asset != null)
        {
            assetbundle_asset.Unload(false);
        }
    }

    /// <summary>
    /// 清楚之前的眼镜
    /// </summary>
    public void DelectMountModel()
    {
        if (currentGlasses)
        {
            currentGlasses.SetActive(false);
            Destroy(currentGlasses);
        }
    }

    /// <summary>
    /// 获取变换之后的矩阵数据
    /// 3D素材位置、旋转、大小的计算
    /// </summary>
    /// <param name="strArr">sdk出来的矩阵数据</param>
    /// <returns>变换之后的矩阵数据</returns>
    public static Vector3 faceScreenPos;
    public static Vector3 facePos;
    public static Vector3 faceAngle;
    public static Vector3 faceScale;
    private float screenRatio_w;
    private float screenRatio_h;
    private float dotDistance;
    Matrix4x4 mat;
    public void GetTransformMatrix4x4(TrackPointCoord[] allTrackPoints, float[] strArr, ref MatrixVectorStruct m_MatrixVector, string location)
    {
        GetSdkMatrix4x4(strArr);
        mat = Mutiply(size_fit_matrix, matr);
        mat.m23 = 185.5f;
        mat.m13 = mat.m13 + (m_originalOrthographicSize - m_newOrthographicSize);
        MatrixTool.GetTransform(mat, ref vScale, ref angleQua);

        #region 计算角度
        faceAngle.x = -angleQua.eulerAngles.x; //角度的微调整
        faceAngle.y = -angleQua.eulerAngles.y;
        faceAngle.z = angleQua.eulerAngles.z;
        m_MatrixVector.Angles = faceAngle;
        #endregion

        #region 计算大小
        dotDistance = MatrixTool.GetDotDistance(GetDot(location, 0), GetDot(location, 1), GetDistance(allTrackPoints[36], allTrackPoints[45]), vScale.x);

        vScale.z = vScale.x;
        faceScale = vScale * 2.2f * ((dotDistance / vScale.x) / 6900);      //调整眼镜大小
        //faceScale = vScale * 0.7f;      //调整眼镜大小
        m_MatrixVector.Scale = faceScale;
        #endregion

        #region 计算位置
        screenRatio_w = Screen.width / AppParameter.trackResolution.x;
        screenRatio_h = Screen.height / AppParameter.trackResolution.y;
        faceScreenPos.x = (matr[0, 3]) * screenRatio_w;
        faceScreenPos.y = (AppParameter.trackResolution.y - matr[1, 3]) * screenRatio_h;
        faceScreenPos.z = 0;
 
        faceScreenPos.x += (Camera.main.rect.x * Screen.width + Camera.main.rect.width * Screen.width / 2) - Screen.width / 2;  //相机偏移

        facePos = Camera.main.ScreenToWorldPoint(faceScreenPos);
        facePos.z = 10;
        m_MatrixVector.Point = facePos;
        #endregion
    }

    /// <summary>
    /// 获取sdk中矩阵数据
    /// </summary>
    /// <param name="strArr">String arr.</param>
    void GetSdkMatrix4x4(float[] strArr)
    {
        matr[0, 0] = strArr[0];
        matr[0, 1] = strArr[1];
        matr[0, 2] = strArr[2];
        matr[0, 3] = strArr[3];
        matr[1, 0] = strArr[4];
        matr[1, 1] = strArr[5];
        matr[1, 2] = strArr[6];
        matr[1, 3] = strArr[7];
        matr[2, 0] = strArr[8];
        matr[2, 1] = strArr[9];
        matr[2, 2] = strArr[10];
        matr[2, 3] = strArr[11];
        matr[3, 0] = strArr[12];
        matr[3, 1] = strArr[13];
        matr[3, 2] = strArr[14];
        matr[3, 3] = strArr[15];
    }


    /// <summary>
    /// 切换分辨率后重新计算初始化数据
    /// </summary>
    public void CheckIntoDataInfo()
    {
        float m_screenWidth = ((float)Screen.width / (float)Screen.height) * Camera.main.orthographicSize;
        m_newOrthographicSize = m_screenWidth * AppParameter.trackResolution.x / AppParameter.trackResolution.y;
 
        GetInitMatrix4x4(m_newOrthographicSize);
    }

    /// <summary>
    /// 获取初始缩放矩阵
    /// </summary>
    public void GetInitMatrix4x4(float cameraSize)
    {
        //		//获取正交相机下屏幕坐下脚（0，0）对应的3d坐标中位置点。
        Vector3 Point_0 = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Util.Log("Point_0" + Point_0.x + "Y:" + Point_0.y + "Z:" + Point_0.z);
        //		//由3d坐标点获取屏幕中其它3个脚点位置点
        //		//根据3d坐标点算出其在图坐标系中点像素点
        float x = ((Point_0.x * AppParameter.trackResolution.x + cameraSize * AppParameter.trackResolution.y) / cameraSize) * 0.5f;
        //由图坐标系中点像素点推导出其它3个像素点
        float s_factor, x_factor, y_factor;
        size_fit_matrix = Matrix4x4.identity;

        s_factor = (cameraSize * 2) / AppParameter.trackResolution.x;
        x_factor = Point_0.x - (s_factor * x);
        y_factor = cameraSize;

        size_fit_matrix.m00 = s_factor;
        size_fit_matrix.m03 = x_factor;
        size_fit_matrix.m11 = -s_factor;
        size_fit_matrix.m13 = y_factor;
        size_fit_matrix.m22 = -s_factor;
    }

    /// <summary>
    /// 矩阵相乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public Matrix4x4 Mutiply(Matrix4x4 a, Matrix4x4 b)
    {
        float c = 0;
        result = Matrix4x4.zero;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                c = 0;
                for (int k = 0; k < 4; k++)
                {
                    c += (a[i, k] * b[k, j]);
                }
                result[i, j] = c;
            }
        }
        return result;
    }

    /// <summary>
    /// 识别点的平面距离
    /// </summary>
    /// <param name="dot1"></param>
    /// <param name="dot2"></param>
    /// <returns></returns>
    float GetDistance(TrackPointCoord dot1, TrackPointCoord dot2)
    {
        return Mathf.Sqrt(Mathf.Pow(dot2.x - dot1.x, 2) + Mathf.Pow(dot2.y - dot1.y, 2));
    }

    /// <summary>
    /// 获取脸部3d的两点
    /// </summary>
    /// <param name="location"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    Vector3 GetDot(string location, int index)
    {
        if (faceObj != null)
        {
            return faceObj.transform.GetChild(0).GetChild(index).position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 更新模型的位置、旋转和缩放
    /// </summary>
    /// <param name="model"></param>
    /// <param name="m_MatrixVector"></param>
    public void MatrixToValue(GameObject model, MatrixVectorStruct m_MatrixVector)
    {
        model.transform.position = m_MatrixVector.Point;
        model.transform.eulerAngles = m_MatrixVector.Angles;
        model.transform.localScale = m_MatrixVector.Scale;
        //Util.Log("MatrixToValue => " + model.name + "  " + model.transform.eulerAngles.x + "  " + model.transform.eulerAngles.y + "   " + model.transform.position + "   " + model.transform.localScale.x + "   " + model.activeSelf);
    }
}
