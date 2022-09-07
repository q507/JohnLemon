using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 构建AssetBundle包集
/// </summary>
public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("AssetBundles/Build AsserBundles")]
    static void BuildAllAssetBundles()
    {
        //AssetBundle 存储路径
        string _AssetBundleDirectory = Application.streamingAssetsPath + "//AssetBundles";//Application.dataPath + "/Resources";

        if (!Directory.Exists(_AssetBundleDirectory))
        {
            Directory.CreateDirectory(_AssetBundleDirectory);
        }

        //构建编辑中指定的所有资产包 变量（ 构建地址、 资产包构建选项、目标构建平台）//ChunkBasedCompression
        BuildPipeline.BuildAssetBundles(_AssetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

#if UNITY_EDITOR

        //资源刷新
        AssetDatabase.Refresh();

#endif
    }
}
