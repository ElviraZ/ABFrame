using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private SingleABLoader load = null;


    private string ABname = "scene_1/prefabs.ab";
    private string assetName= "Cube.prefab";
	// Use this for initialization
	void Start () 
	{
        load = new SingleABLoader(ABname, LoadComplete);
        StartCoroutine(load.LoadAssetBundle());
	}
    /// <summary>
    /// 加载完成之后回调
    /// </summary>
    /// <param name="name"></param>
    private void LoadComplete(string   name)
    {

      UnityEngine.Object   tmpobj=  load.LoadAsset(assetName, true);
        Instantiate(tmpobj) ;
    }


}
