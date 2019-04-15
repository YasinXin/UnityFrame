/*
 - FileName:      	MatrixTool.cs
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

/// <summary>
/// 模型位置，旋转角，缩放
/// </summary>
public struct MatrixVectorStruct
{
    public Vector3 Point;
    public Vector3 Scale;
    public Vector3 Angles;
    public MatrixVectorStruct(Vector3 _Point, Vector3 _Scale, Vector3 _Angles)
    {
        this.Point = _Point;
        this.Scale = _Scale;
        this.Angles = _Angles;
    }
}

public static class MatrixTool
{
    /// 矩阵格式：
    /// m00,m01,m02,m03
    /// m10,m11,m12,m13
    /// m20,m21,m22,m23
    /// m30,m31,m32,m33
    /// 缩放：m00,m11,m22
    /// 位置：m03,m13,m23
    /// 旋转X：m11,m22,m21,m22
    /// 旋转Y：m00,m02,m20,m22
    /// 旋转Z：m00,m01,m10,m11

    private static Matrix4x4 m4;
    private static Quaternion q;
    public static void GetTransform(Matrix4x4 matrix, ref Vector3 scaleV3, ref Quaternion angleQua)
    {
        Util.Log("GetTransform==>" + matrix.m00 + "    " + matrix.m01 + "     " + matrix.m02);
        m4 = Matrix4x4.identity;
        scaleV3.x = Mathf.Sqrt(matrix.m00 * matrix.m00 + matrix.m01 * matrix.m01 + matrix.m02 * matrix.m02);
        scaleV3.y = Mathf.Sqrt(matrix.m10 * matrix.m10 + matrix.m11 * matrix.m11 + matrix.m12 * matrix.m12);
        scaleV3.z = Mathf.Sqrt(matrix.m20 * matrix.m20 + matrix.m21 * matrix.m21 + matrix.m22 * matrix.m22);

        m4.m00 = matrix.m00 / scaleV3.x;
        m4.m01 = matrix.m01 / scaleV3.y;
        m4.m02 = matrix.m02 / scaleV3.z;
        m4.m10 = matrix.m10 / scaleV3.x;
        m4.m11 = matrix.m11 / scaleV3.y;
        m4.m12 = matrix.m12 / scaleV3.z;
        m4.m20 = matrix.m20 / scaleV3.x;
        m4.m21 = matrix.m21 / scaleV3.y;
        m4.m22 = matrix.m22 / scaleV3.z;

        angleQua = QuaternionFromMatrix(m4);
    }

    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {

        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) * 0.5f;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) * 0.5f;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) * 0.5f;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) * 0.5f;

        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));

        return q;
    }

    private static Vector3 vAB;
    private static float cos1;
    private static float cosAngle;
    public static float GetDotDistance(Vector3 dotA, Vector3 dotB, float dis, float scaleX)
    {
        if (dotA != Vector3.zero && dotB != Vector3.zero)
        {
            vAB = new Vector3(dotB.x - dotA.x, dotB.y - dotA.y, dotB.z - dotA.z);
            cos1 = Mathf.Abs(vAB.z) / (Mathf.Sqrt(Mathf.Pow(vAB.x, 2) + Mathf.Pow(vAB.y, 2) + Mathf.Pow(vAB.z, 2)) * 1);  //向量vAB与向量z的余玄值
            cosAngle = Mathf.Sqrt(1 - Mathf.Pow(cos1, 2));
            return dis / cosAngle;
        }
        return 6900 * scaleX;
    }
}
