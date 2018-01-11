


/*
 * 
 * 辅助类
 * 
维护包和包之间的关系
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABRelation
{
    //当期AB的名称
    private   string ABName;

    //    本包所有的依赖包的集合
    private List<string> listAllDependenceAB;

    //    本包所有的引用的包的集合
    private List<string> listAllReferenceAB;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="abName"></param>
    public ABRelation(string  abName)
    {
        if (!string.IsNullOrEmpty(abName))
        {
            ABName  = abName;
        }
        listAllDependenceAB = new List<string>();
        listAllReferenceAB = new List<string>();
    }

    public void AddDependence(string  abName)
    {
        if (!listAllDependenceAB.Contains(abName))
        {
            listAllDependenceAB.Add(abName);
        }
    }

    /// <summary>
    /// 返回：
    /// true:此包没有依赖项
    /// false：此包还有依赖项  
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public bool RemoveDependence(string  abName)
    {
        if (listAllDependenceAB.Contains(abName))
        {
            listAllDependenceAB.Remove(abName);
        }
        if (listAllDependenceAB.Count>0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public List<string> GetAllDependence()
    {
        return listAllDependenceAB;


    }



    public void AddReference(string abName)
    {
        if (!listAllReferenceAB.Contains(abName))
        {
            listAllReferenceAB.Add(abName);
        }
    }
    /// <summary>
    /// 返回：
    /// true:此包没有引用项
    /// false：此包还有引用项  
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public bool RemoveReference(string  abName)
    {
        if (listAllReferenceAB.Contains(abName))
        {
            listAllReferenceAB.Remove(abName);
        }
        if (listAllReferenceAB.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public List<string> GetAllReference()
    {
        return listAllReferenceAB;
    }
}
