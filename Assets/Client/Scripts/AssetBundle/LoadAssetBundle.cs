using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundle : MonoBehaviour
{
    private void Start()
    {
        LoadWithoutPreference();
    }

    /// <summary>
    /// AssetBundle没有依赖项的加载
    /// </summary>
    private void LoadWithoutPreference()
    {
        AssetBundle asset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + "model");

        //AssetBundle无法重复加载
        /*GameObject obj = asset.LoadAsset("BathTub", typeof(GameObject)) as GameObject;
        GameObject obj2 = asset.LoadAsset<GameObject>("Mirror");*/

        /*Instantiate(obj, transform);
        Instantiate(obj2, transform);*/

        //AB包卸载
        asset.Unload(false);
    }
}
