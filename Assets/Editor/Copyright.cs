/*
 - FileName:      	Copyright.cs
 - Author:       	#小Y#
 - CreateTime:    	#CreateTime#
 - Email:         	#279444122@qq.com#
 - Description:   
 -  (C) Copyright 2017 - 2019, 眼镜小鸽,Inc.
 -  All Rights Reserved.
*/
using System.IO;

public class Copyright : UnityEditor.AssetModificationProcessor
{
    private const string DateFormat = "yyyy/MM/dd";
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string allText = File.ReadAllText(path);
            allText = allText.Replace("#CreateTime#", System.DateTime.Now.ToString(DateFormat));
            File.WriteAllText(path, allText);
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
