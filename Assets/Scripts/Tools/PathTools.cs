using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTools
{

    public const string AB_Resources = "AB_Res";
    //public const string AB_OutPath = Application.streamingAssetsPath;
    public static   string GetABResources()
    {

        return AB_Resources;
            }

    /// <summary>
    /// 输出路径
    /// 1、平台路径
    /// 2、平台的名称
    /// </summary>
    /// <returns></returns>
    public static string GetABOutPath()
    {

        return GetPlatFormPath()+"/"+ GetPlatFormName();
    }
    public static string GetPlatFormPath()
    {

        string outPath = string.Empty;
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                outPath = Application.streamingAssetsPath;
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                outPath = Application.persistentDataPath;
                break;
            default:
                break;
        }
        return outPath;
    }
    public static string GetPlatFormName()
    {
        string outName = string.Empty;
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                outName = "Windows";
                break; 
            case RuntimePlatform.IPhonePlayer:
                outName = "IOS";
                break;
            case RuntimePlatform.Android:
                outName = "Android";
                break;
            default:
                break;
        }
        return outName;
    }

    /// <summary>
    /// 获取AB 包的www 的下载路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWPath()
    {
        string returnWWWPath = string.Empty ;
        switch (Application.platform)
        {

            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                returnWWWPath = "file://" + GetABOutPath();
                break;
            case RuntimePlatform.IPhonePlayer:
                returnWWWPath = "file://" + GetABOutPath();
                break;
            case RuntimePlatform.Android:
                returnWWWPath = "jar:file://" + GetABOutPath();
                break;
            default:
                break;
        }
        return returnWWWPath;
    }
}
