using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFrame : MonoBehaviour {
    private string sceneName = "scene_1";
    private string ABname = "scene_1/prefabs.ab";
    private string assetName = "FloorCube.prefab";
    // Use this for initialization
    void Start () 
	{//1、调用AB 包
        StartCoroutine(AssetBundleMgr.GetInstance().LoadAssetBundlePack(sceneName, ABname, LoadComplete));
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.A))
        {
            AssetBundleMgr.GetInstance().Dispose(sceneName);
        }
	}
    public void LoadComplete(string  abName)
    {
        //2、提取资源
       UnityEngine.Object   tmpObj= AssetBundleMgr.GetInstance().LoadAsset(sceneName, abName, assetName, true);

        if (tmpObj)
        {
            Instantiate(tmpObj);
        }
    }


   
}
