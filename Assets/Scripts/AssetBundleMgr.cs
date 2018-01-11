

/*
 * 
 * 主流层
 * 所有场景的AB包的管理
 * 1、读取manifest清单文件，缓存本脚本
 * 2、以场景为单位，管理整个项目的AB包
 * 

 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleMgr:MonoBehaviour
{


    private static AssetBundleMgr _Instance;


    private Dictionary<string, MultiABMgr> _dicAllScene = new Dictionary<string, MultiABMgr>();

    private AssetBundleManifest manifesObj = null;


    private AssetBundleMgr()
    {

    }

    public static AssetBundleMgr GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new GameObject("AssetBundleMgr").AddComponent<AssetBundleMgr>() ;
        }
        return _Instance;
    }
    private void Awake()
    {
        //加载Manifest清单文件
        StartCoroutine(ABManifestLoader.GetInstance().LoadManifestFile());
    }
 
    /// <summary>
    /// 下载AB包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="abName"></param>
    /// <param name="delLoadComplete"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundlePack(string  sceneName,string   abName,DelLoadComplete  delLoadComplete)
    {
        if (string.IsNullOrEmpty(sceneName)||string.IsNullOrEmpty(abName))
        {
            Debug.LogError("sceneName   检查+abName    检查" );
         yield return null;
        }
        while (!ABManifestLoader.GetInstance().IsLoadFinish)
        {
            yield return null;
        }

        manifesObj = ABManifestLoader.GetInstance().GetABManifest();
        if (manifesObj==null)
        {
            Debug.LogError("请先加载清单文件");
            yield return null;
        }
        if (!_dicAllScene.ContainsKey(sceneName))
        {
            MultiABMgr multiABMgr = new MultiABMgr(sceneName,abName,delLoadComplete);
            _dicAllScene.Add(sceneName, multiABMgr);

        }
        MultiABMgr tmpmultiABMgr = _dicAllScene[sceneName];
        if (tmpmultiABMgr==null)
        {
            Debug.LogError("tmpmultiABMgr  is  null");
        }
        yield return tmpmultiABMgr.LoadAssetBundle(abName);
    }
    /// <summary>
    /// 加载AB包中的资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="abName"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string sceneName, string abName,string   assetName,bool  isCache)
    {


        if (_dicAllScene.ContainsKey(sceneName))
        {
            MultiABMgr multiAB = _dicAllScene[sceneName];
            return multiAB.LoadAsset(abName,assetName,isCache);
        }
        Debug.LogError("加载AB包中的资源失败"+ sceneName);
        return null;


    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose(string sceneName)
    {
        if (_dicAllScene.ContainsKey(sceneName))
        {
            MultiABMgr multiAB = _dicAllScene[sceneName];
             multiAB.DisposeAllAsset();
        }
        else
        {
            Debug.LogError("释放资源失败"+sceneName);
        }
    }
}
