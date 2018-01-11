

/*
 * 
 * AB资源加载类
 * 
 * 单一加载
 *www加载
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleABLoader : System.IDisposable
{
    private AssetLoader assetLoader;
    //委托
    private DelLoadComplete loadCompleteHandle;

    //assetbundle名称
    private string ABName;
    //assetbundle下载的路径
    private string ABDownLoadPath;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="abName"></param>
    public SingleABLoader(string  abName,DelLoadComplete   loadComplete)
    {
        assetLoader = null;
           ABName = abName;
        loadCompleteHandle = loadComplete;
        
       ABDownLoadPath=PathTools.GetWWWPath()+"/"+ABName;

    }
    public IEnumerator LoadAssetBundle()
    {
        using (WWW www = new WWW(ABDownLoadPath))
        {
    yield return  www;
            if (www.progress>=1)
            {
                //下载完成
                AssetBundle abobj = www.assetBundle;
                if (abobj!=null)
                {
                    assetLoader = new AssetLoader(abobj);
                    //AB 下载完毕
                    if (loadCompleteHandle!=null)
                    {
                        loadCompleteHandle(ABName);
                    }

                }
                else
                {
                    Debug.LogError("www 下载失败");
                }
            }
        }
      
    

    }
    /// <summary>
    /// 加载AB包里面的资源
    /// </summary>
    /// <param name="assetName">名字</param>
    /// <param name="isCacha">缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string assetName,bool  isCache)
    {
        if (assetLoader!=null)
        {
            return assetLoader.LoadAsset(assetName, isCache);
        }
        Debug.LogError("assetLoader参数检查");
        return null;
    }


    public void UnLoadAsset(UnityEngine.Object   asset)
    {
        if (assetLoader!=null)
        {
            assetLoader.UnLoadAsset(asset);
        }
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (assetLoader != null)
        {
            assetLoader.Dispose();
            assetLoader = null;
        }
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    public void DisposeAll()
    {
        if (assetLoader != null)
        {
            assetLoader.DisposeAll();
            assetLoader = null;
        }
    }
    /// <summary>
    /// 查询当前AB 包中 的所有资源
    /// </summary>
    /// <returns></returns>
    public string[] RetriAllAssetName()
    {
        if (assetLoader != null)
        {
       return     assetLoader.RetriAllAssetName();

        }
        return null;
    }
}
