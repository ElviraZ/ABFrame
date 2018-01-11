
/*
 * 
 * 主流层
 * 多个AB包的管理
 * 1、获取AB 包的依赖和引用关系
 * 2、管理AB 包之间的自动连锁加载
 * 

 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiABMgr 
{
    private SingleABLoader curSingleABLoader;

    //AB包的实现类缓存(缓存，防止重复加载(AB 包的缓存集合))
    private Dictionary<string, SingleABLoader> dicSingleABLoader;

    private string curSceneName;//测试用的场景名称
//当前AB 包的名字
    private string curABName;
    //AB包对应的依赖关系集合
    private Dictionary<string, ABRelation> dicABRelation;
    private DelLoadComplete LoadAllPackageCompleteHandle;



    public MultiABMgr(string  sceneName,string abName,DelLoadComplete   delLoadComplete)
    {
        curSceneName = sceneName;
        curABName = abName;
        dicSingleABLoader = new Dictionary<string, SingleABLoader>();

        dicABRelation = new Dictionary<string, ABRelation>();
        LoadAllPackageCompleteHandle = delLoadComplete;

    }



    //完成指定AB包的调用
    public void CompleteAB(string  abName)
    {
        if (abName.Equals(curABName))
        {
            if (LoadAllPackageCompleteHandle!=null)
            {
                LoadAllPackageCompleteHandle(abName);
            }
        }
    }


    //加载AB包
    public IEnumerator LoadAssetBundle(string  abName)
    {
        //AB包关系的建立
        if (!dicABRelation.ContainsKey(abName))
        {
            ABRelation aBRelationObj = new ABRelation(abName);
            dicABRelation.Add(abName,aBRelationObj);
        }
        ABRelation tmpABRelation = dicABRelation[abName];

        //得到所有的依赖关系
        string[] dependencearray = ABManifestLoader.GetInstance().RetrivalDependence(abName);
        foreach (string item_dependence in dependencearray)
        {
            //添加依赖
            tmpABRelation.AddDependence(item_dependence);
            //添加引用
            yield return LoadRefence(item_dependence, abName);
        }
        //加载
        if (dicSingleABLoader.ContainsKey(abName))
        {
            yield return dicSingleABLoader[abName].LoadAssetBundle();
        }
        else
        {
            curSingleABLoader = new SingleABLoader(abName, CompleteAB);
            dicSingleABLoader.Add(abName,curSingleABLoader);
            yield return curSingleABLoader.LoadAssetBundle() ;
        }
        yield return null;
    }
    /// <summary>
    /// 加载引用AB包
    /// </summary>
    /// <param name="abName">AB包的名称</param>
    /// <param name="refABName">被引用AB包的名称</param>
    /// <returns></returns>
    private IEnumerator LoadRefence(string abName,string  refABName)
    {
        if (dicABRelation.ContainsKey(abName))
        {
            ABRelation tmpABRelation = dicABRelation[abName];
            tmpABRelation.AddReference(refABName);
        }
        else
        {
            ABRelation tmpABRelation = new ABRelation(abName);
            tmpABRelation.AddReference(refABName);
            dicABRelation.Add(abName,tmpABRelation);

            //开始加载依赖的包
            yield return LoadAssetBundle(  abName);
        }
        yield return null;
    }



    //加载AB包中的资源
    public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache)
    {
        foreach (string item_abName in dicSingleABLoader.Keys)
        {
            if (abName==item_abName)
            {
            return dicSingleABLoader[item_abName].LoadAsset(assetName,isCache);

            }
        }
        Debug.LogError("找不到资源，"+abName+"+"+assetName);
        return null;

    }

    /// <summary>
    /// 释放本场景中的所有资源,场景转换调用
    /// </summary>
    public void DisposeAllAsset()
    {
        try
        {
     foreach (SingleABLoader item in dicSingleABLoader.Values)
        {
            item.DisposeAll();
        }
        }
        finally 
        {

            dicSingleABLoader.Clear();
            dicSingleABLoader = null;

            dicABRelation.Clear();
            dicABRelation = null;


            curABName = null;
            curSceneName = null;
            LoadAllPackageCompleteHandle = null;

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
   
    }
}
