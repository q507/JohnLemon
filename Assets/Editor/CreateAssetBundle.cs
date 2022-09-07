using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// ����AssetBundle����
/// </summary>
public class CreateAssetBundle : MonoBehaviour
{
    [MenuItem("AssetBundles/Build AsserBundles")]
    static void BuildAllAssetBundles()
    {
        //AssetBundle �洢·��
        string _AssetBundleDirectory = Application.streamingAssetsPath + "//AssetBundles";//Application.dataPath + "/Resources";

        if (!Directory.Exists(_AssetBundleDirectory))
        {
            Directory.CreateDirectory(_AssetBundleDirectory);
        }

        //�����༭��ָ���������ʲ��� ������ ������ַ�� �ʲ�������ѡ�Ŀ�깹��ƽ̨��//ChunkBasedCompression
        BuildPipeline.BuildAssetBundles(_AssetBundleDirectory, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

#if UNITY_EDITOR

        //��Դˢ��
        AssetDatabase.Refresh();

#endif
    }
}
