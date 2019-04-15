/*
 - FileName:      	TrackTool.cs
 - Author:       	#小Y#
 - CreateTime:    	2019/03/21
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System;
public struct TRACK3DRET
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
    public TrackPointCoord[] allTrackPoints;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
    public TrackPointCoord[] mountTrackPoints;

    public int faceShapeIndex;  //  index  0:aquare  1:round  2:slim
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public float[] matrix;

    public TRACK3DRET(TrackPointCoord[] _allTrackPoints, TrackPointCoord[] _mountTrackPoints, float[] _Matrix, int _faceShapeIndex)
    {
        this.allTrackPoints = _allTrackPoints;
        this.mountTrackPoints = _mountTrackPoints;
        this.matrix = _Matrix;
        this.faceShapeIndex = _faceShapeIndex;

    }
}
public struct TrackPointCoord
{
    public float x;
    public float y;
    public TrackPointCoord(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
    }
};

public struct TRACK2DRET
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 68)]
    public TrackPointCoord[] allTrackPoints;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
    public TrackPointCoord[] mountTrackPoints;
    public float scale;
    public float angle;

    public TRACK2DRET(TrackPointCoord[] _allTrackPoints, TrackPointCoord[] _mountTrackPoints, float _scale, float _angle)
    {
        this.allTrackPoints = _allTrackPoints;
        this.mountTrackPoints = _mountTrackPoints;
        this.scale = _scale;
        this.angle = _angle;
    }
};

public class TrackTool : Manager
{
    bool boltrack = false;                  //检测返回结果
    private float[] trackFloat;             //检测返回数组
    /// <summary>
    /// 初始化2d结构体
    /// </summary>
    public TRACK2DRET InitStruct()
    {
        TrackPointCoord[] _allTrackPoints, _mountTrackPoints;
        TrackPointCoord _TrackPointCoord = new TrackPointCoord(0, 0);
        _allTrackPoints = new TrackPointCoord[68];
        _mountTrackPoints = new TrackPointCoord[11];
        for (int i = 0; i < _allTrackPoints.Length; i++)
        {
            _allTrackPoints[i] = _TrackPointCoord;
        }
        for (int i = 0; i < _mountTrackPoints.Length; i++)
        {
            _mountTrackPoints[i] = _TrackPointCoord;
        }
        TRACK2DRET track2dret = new TRACK2DRET(_allTrackPoints, _mountTrackPoints, 0, 0);
        return track2dret;
    }

    /// <summary>
    /// 初始化3d结构体
    /// </summary>
    public TRACK3DRET InitTrackVector()
    {
        TrackPointCoord[] _allTrackPoints, _mountTrackPoints;
        TrackPointCoord _TrackPointCoord = new TrackPointCoord(0, 0);
        _allTrackPoints = new TrackPointCoord[68];
        _mountTrackPoints = new TrackPointCoord[11];

        for (int i = 0; i < _allTrackPoints.Length; i++)
        {
            _allTrackPoints[i] = _TrackPointCoord;
        }
        for (int i = 0; i < _mountTrackPoints.Length; i++)
        {
            _mountTrackPoints[i] = _TrackPointCoord;
        }

        float[] _Matrix;
        _Matrix = new float[16];
        for (int i = 0; i < _Matrix.Length; i++)
        {
            _Matrix[i] = 0.0f;
        }

        int _faceShapeIndex;
        _faceShapeIndex = 0;


        TRACK3DRET tv = new TRACK3DRET(_allTrackPoints, _mountTrackPoints, _Matrix, _faceShapeIndex);
        return tv;
    }

#if UNITY_ANDROID
    /// <summary>
    /// 1 人脸个数
    /// 68*2 是原始点位置(x,y)
    /// 16是姿态
    /// 22是 11个锚点位置(x,y)
    /// 1 脸型
    /// 1 2D大小
    /// 1 2D角度
    /// 一共是 1 + 68*2 + 16 + 11*2 +1 + 1+ 1=178个浮点数的数组
    /// </summary>
    public bool GetTrack(ref int faceNum, ref TRACK2DRET[] track2dretResult, ref TRACK3DRET[] track3dretResult, float trackSmoothValue)
    {
        try
        {
            trackFloat = SendPlatformManager.currentActivity.Call<float[]>("getFaceTracking", trackSmoothValue);
        }
        catch (Exception e)
        {
            Util.Log("track错误2！" + e.ToString());
            throw;
        }

        if (trackFloat == null || trackFloat.Length < 1)
        {
            return false;
        }
        else
        {
            faceNum = (int)trackFloat[0];
            if (faceNum > 0)
            {
                track3dretResult = new TRACK3DRET[faceNum];
                track2dretResult = new TRACK2DRET[faceNum];
                for (int j = 0; j < faceNum; j++)
                {
                    track3dretResult[j] = InitTrackVector();
                    track2dretResult[j] = InitStruct();

                    //68个识别点
                    for (int i = 1; i < 69; i++)
                    {
                        track3dretResult[j].allTrackPoints[i - 1] = new TrackPointCoord(trackFloat[2 * i - 1], trackFloat[2 * i]);
                        track2dretResult[j].allTrackPoints[i - 1] = track3dretResult[j].allTrackPoints[i - 1];
                    }

                    //16个3D矩阵数
                    for (int i = 137; i < 153; i++)
                    {
                        track3dretResult[j].matrix[i - 137] = trackFloat[i];
                    }

                    //11个2D挂载点
                    for (int i = 153; i < 164; i++)
                    {
                        track2dretResult[j].mountTrackPoints[i - 153] = new TrackPointCoord(trackFloat[2 * i - 153], trackFloat[2 * i - 152]);
                    }

                    //脸型参数
                    track3dretResult[j].faceShapeIndex = (int)trackFloat[175];
                    //2D素材大小
                    track2dretResult[j].scale = trackFloat[176];
                    //2D素材旋转
                    track2dretResult[j].angle = trackFloat[177];
                }
                boltrack = true;
            }
            else
            {
                boltrack = false;
            }
        }
        return boltrack;
    }
#endif
}