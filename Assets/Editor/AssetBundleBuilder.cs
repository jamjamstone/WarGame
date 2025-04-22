using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : MonoBehaviour
{

    [MenuItem("Assets/Build AssetBundles")] //�޴� ���������� ����
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles"; //�ּ� ��Ÿ ����!!
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, //���
            BuildAssetBundleOptions.None, //������
            BuildTarget.StandaloneWindows); //Ÿ�� �÷���
    }
}
