using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ABEditor
{
    #region 设置和删除AB      Label
    static bool isSet = true;
    #region 设置AB 的包名
    /// <summary>
    /// 设置AB 的包名
    /// </summary>
    [MenuItem("AssetBundle  Tools/1、Set AB  Labels")]
    public static void SetABLabel()
    {
        isSet = true;
        FindResources();
    }

    private static void FindResources()
    {
        //  1：定义文件夹根目录
        string root = string.Empty;
        //目录信息
        DirectoryInfo[] dirArray = null;
        //清空无用Labels
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //得到根目录
        //  root = Application.dataPath + "/"+ "AB_Res";
        root = Application.dataPath + "/" + PathTools.GetABResources();
        DirectoryInfo directoryInfo = new DirectoryInfo(root);
        dirArray = directoryInfo.GetDirectories();

        // 2：遍历文件夹
        foreach (DirectoryInfo curreentDir in dirArray)
        {
            //       a、如果是目录则继续递归深入
            string tmpdir = root + "/" + curreentDir.Name;//全路径
            //DirectoryInfo tmpdirinfo = new DirectoryInfo(tmpdir);//
            int tmpindex = tmpdir.LastIndexOf("/");//
            string tmpSceneName = tmpdir.Substring(tmpindex + 1);//得到场景名称
                                                                 //       b、找到后使用   AssetImporter标记包名和后缀名
            DirOrFile(curreentDir, tmpSceneName);
        }
        //3、刷新编辑器
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 判断是否是目录或者文件，修改label
    /// </summary>
    /// <param name="currentDir">当前文件信息</param>
    /// <param name="sceneName">场景名称</param>
    private static void DirOrFile(FileSystemInfo fileSystemInfo, string sceneName)
    {
        //参数检查
        if (!fileSystemInfo.Exists)
        {
            Debug.LogError("目录不存在" + fileSystemInfo);
        }
        //得到下一级
        DirectoryInfo dirinfoObj = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] filesysArray = dirinfoObj.GetFileSystemInfos();
        foreach (FileSystemInfo fileinfo in filesysArray)
        {
            FileInfo fileinfoObj = fileinfo as FileInfo;
            //文件类型
            if (fileinfoObj != null)
            {//修改AB Label
                SetFileABLabel(fileinfoObj, sceneName);
            }
            //目录类型
            else
            {
                DirOrFile(fileinfo, sceneName);
            }
        }
        if (isSet)
        {
            Debug.Log("设置完成");
        }
        else
        {
            Debug.Log("取消设置完成");
        }
    }

    /// <summary>
    /// 设置Label
    /// </summary>
    /// <param name="fileinfo">文件信息（包含绝对路径）</param>
    /// <param name="sceneName">场景名称</param>
    private static void SetFileABLabel(FileInfo fileinfoobj, string sceneName)
    {
        string ABName = string.Empty;
        string assetFilePath = string.Empty;
        if (fileinfoobj.Extension == ".meta") return;
        ABName = GetABName(fileinfoobj, sceneName);
        //获取文件的相对路径
        int tmpindex = fileinfoobj.FullName.IndexOf("Assets");
        assetFilePath = fileinfoobj.FullName.Substring(tmpindex);

        AssetImporter tmpImporter = AssetImporter.GetAtPath(assetFilePath);
        if (isSet == true)
        {
            tmpImporter.assetBundleName = ABName;
            if (fileinfoobj.Extension == ".unity")
            {
                tmpImporter.assetBundleVariant = "u3d";
            }
            else
            {
                tmpImporter.assetBundleVariant = "ab";
            }
      
        }
        else
        {
            tmpImporter.assetBundleVariant = null;
            tmpImporter.assetBundleName = null;
       
        }

 
    }

    private static string GetABName(FileInfo fileinfoobj, string sceneName)
    {
        string ABName = string.Empty;
        //Windows 路径
        string tmpwinPath = fileinfoobj.FullName;
        //Unity路径
        string tmpunityPath = tmpwinPath.Replace("\\", "/");
        //定位“场景名称”后面字符位置
        int tmpSceneNamePositon = tmpunityPath.IndexOf(sceneName) + sceneName.Length;
        string ABFileNameArea = tmpunityPath.Substring(tmpSceneNamePositon + 1);
        if (ABFileNameArea.Contains("/"))
        {
            string[] tmpArray = ABFileNameArea.Split('/');
            ABName = sceneName + "/" + tmpArray[0];
        }
        else
        {
            ABName = sceneName + "/" + sceneName;
        }
        return ABName;
    }
    #endregion
    #region 删除所有的AB 的包名
    [MenuItem("AssetBundle  Tools/2、Delete AB  Labels")]
    public static void DeleteABLabel()
    {
        isSet = false;
        FindResources();
    }


    #endregion
    #endregion
    #region 打AB包
    [MenuItem("AssetBundle  Tools/3、Build AB /For Win64")]
    public static void BuildAllAB()
    {
        //打包AB输出路径
        string strABOutPathDIR = PathTools.GetABOutPath();
        //判断生成输出目录文件夹
        if (!Directory.Exists(strABOutPathDIR))
        {
            Directory.CreateDirectory(strABOutPathDIR);
        }
        //打包生成
        BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("AssetBundle  Tools/3、Build AB /For Android")]
    public static void BuildAllABAndroid()
    {
        //打包AB输出路径
        string strABOutPathDIR = PathTools.GetABOutPath();
        //判断生成输出目录文件夹
        if (!Directory.Exists(strABOutPathDIR))
        {
            Directory.CreateDirectory(strABOutPathDIR);
        }
        //打包生成
        BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    [MenuItem("AssetBundle  Tools/3、Build AB /For IOS")]
    public static void BuildAllABIOS()
    {
        //打包AB输出路径
        string strABOutPathDIR = PathTools.GetABOutPath();
        //判断生成输出目录文件夹
        if (!Directory.Exists(strABOutPathDIR))
        {
            Directory.CreateDirectory(strABOutPathDIR);
        }
        //打包生成
        BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }

    #endregion
    #region 删除AB包
    [MenuItem("AssetBundle  Tools/4、Delete AB ")]
    public static void DeleteAllAB()
    {
        string deleteDir = PathTools.GetABOutPath();
        if (!string.IsNullOrEmpty(deleteDir))
        {
            Directory.Delete(deleteDir, true);//true表示可以删除非空目录

            File.Delete(deleteDir + ".meta");
            AssetDatabase.Refresh();
        }

    }
    #endregion

}
