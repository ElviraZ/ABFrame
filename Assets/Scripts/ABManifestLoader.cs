

/*
 * 
 * 辅助类
 * 
读取依赖关系文件
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManifestLoader : System.IDisposable
{

    private static ABManifestLoader _Instance;
    // 系统类
    AssetBundleManifest _ManifestObj;
    //清单的下载路径
    private string ManifestPath;
    private AssetBundle ABReadManifest;

    private bool _isLoadFinish;
    public bool IsLoadFinish
    {
        get { return _isLoadFinish; }
    }

    private ABManifestLoader()
    {

        ManifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatFormName();
        _ManifestObj = null;
        ABReadManifest = null;
        _isLoadFinish = false;
    }

    public static ABManifestLoader GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new ABManifestLoader();
        }
        return _Instance;
    }
    //加载manifest清单文件
    public IEnumerator LoadManifestFile()
    {
        using (WWW www = new WWW(ManifestPath))
        {
            yield return www;
            if (www.progress >= 1)
            {
                AssetBundle abObj = www.assetBundle;
                if (abObj != null)
                {
                    ABReadManifest = abObj;
                    _ManifestObj = ABReadManifest.LoadAsset(ABDefine.AssetBundleManifest) as AssetBundleManifest;//AssetBundleManifest   为固定的常量
                    _isLoadFinish = true;
                }
                else
                {
                    Debug.LogError("www 下载manifest失败" + www.error);
                }
            }
        }
    }




    /// <summary>
    /// 获取AssetBundleManifest的实例
    /// </summary>
    /// <returns></returns>
    public AssetBundleManifest GetABManifest()
    {
        if (_isLoadFinish)
        {
            if (_ManifestObj)
            {
                return _ManifestObj;
            }
        }
        else
        {
            Debug.LogError("_ManifestObj失败");
        }
        return null;
    }
 
   /// <summary>
   /// 获取指定包名的所有依赖项
   /// </summary>
   /// <param name="abName">AB包的名字</param>
   /// <returns></returns>
    public string[] RetrivalDependence(string   abName)
    {
        if (_ManifestObj!=null&&!string.IsNullOrEmpty(abName))
        {
            return _ManifestObj.GetAllDependencies(abName);
        }
        return null;
    }

    /// <summary>
    /// 释放系统类的资源
    /// </summary>
    public void Dispose()
    {
        if (ABReadManifest!=null)
        {
            ABReadManifest.Unload(true);
        }
    }
}
