
/*
 * 
 * AB资源加载类
 * 
 * 
 *1 、管理和加载AB
 * 2、加载具有缓存功能的资源，带参数
 * 3、卸载‘释放资源
 * 4、查看当前AB 资源’
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : System.IDisposable
{
    //当前AB
    private AssetBundle curAssetBundle;
    private Hashtable _Ht;


    public AssetLoader(AssetBundle abObj)
    {
        if (abObj != null)
        {
            curAssetBundle = abObj;
            _Ht = new Hashtable();
        }
        else
        {
            Debug.LogError("AssetBundle 构造函数参数检查");
        }

    }
    /// <summary>
    /// 加载当前包中的指定资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="isCache">是否带缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
    {
        return LoadResources<UnityEngine.Object>(assetName,isCache);
    }
    private T LoadResources<T>(string assetName, bool isCache = false) where T: UnityEngine.Object
        {
        //判断缓存是否有了
        if (_Ht.Contains(assetName))
        {
            return _Ht[assetName] as T;
        }
        //正式加载
        T tmpResources = curAssetBundle.LoadAsset<T>(assetName);
        if (tmpResources!=null&&isCache)
        {
            _Ht.Add(assetName,tmpResources);
        }
        else
        {
            Debug.LogError("参数   tmpResources  检查");
        }
        return tmpResources;
}




   /// <summary>
   /// 卸载指定的资源
   /// </summary>
   /// <param name="asset">名称</param>
   /// <returns></returns>
    public bool UnLoadAsset(UnityEngine.Object asset)
    {
        if (asset!=null)
        {
            Resources.UnloadAsset(asset);
            return true;
        }
        Debug.LogError("参数 UnLoadAsset. asset  检查");

        return false;
    }
    /// <summary>
    /// 释放内存镜像资源
    /// </summary>
    public void Dispose()
    {
        curAssetBundle.Unload(false);
    }
    /// <summary>
    /// 释放内存镜像资源和内存资源
    /// </summary>
    public void DisposeAll()
    {
        curAssetBundle.Unload(false);

    }

    /// <summary>
    /// 查询当前AB 包中 的所有资源
    /// </summary>
    /// <returns></returns>
    public string[] RetriAllAssetName()
    {
        return curAssetBundle.GetAllAssetNames();
    }
}
