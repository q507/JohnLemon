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
    /// AssetBundleû��������ļ���
    /// </summary>
    private void LoadWithoutPreference()
    {
        AssetBundle asset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/AssetBundles/" + "model");

        //AssetBundle�޷��ظ�����
        /*GameObject obj = asset.LoadAsset("BathTub", typeof(GameObject)) as GameObject;
        GameObject obj2 = asset.LoadAsset<GameObject>("Mirror");*/

        /*Instantiate(obj, transform);
        Instantiate(obj2, transform);*/

        //AB��ж��
        asset.Unload(false);
    }
}
