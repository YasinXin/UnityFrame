using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppConst
{
    public const bool DebugMode = false;                       //调试模式-用于内部测试
                                                               /// <summary>
                                                               /// 如果想删掉框架自带的例子，那这个例子模式必须要
                                                               /// 关闭，否则会出现一些错误。
                                                               /// </summary>
    public const bool ExampleMode = true;                       //例子模式 

    public const int TimerInterval = 1;
    public const int GameFrameRate = 28;                        //游戏帧频
    public const int maxTraceRate = 28;                         //人脸检测（最大帧）
    public const int minTraceRate = 3;                          //人脸检测（最小帧）
    public const float SmoothValue = 1.5f;                        //人脸跟踪平滑值

    public const string AppName = "GlassesBird";                //应用程序名称
    public const string LuaTempDir = "Lua/";                    //临时目录
    public const string AppPrefix = AppName + "_";              //应用程序前缀
    public const string ExtName = ".unity3d";                   //素材扩展名
    public const string AssetDir = "StreamingAssets";           //素材目录 

    public static string FrameworkRoot
    {
        get
        {
            return Application.dataPath + "/" + AppName;
        }
        
    }
}
