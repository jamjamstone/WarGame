using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder : MonoBehaviour
{

    [MenuItem("Assets/Build AssetBundles")] //메뉴 아이템으로 생성
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles"; //주소 오타 없이!!
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, //경로
            BuildAssetBundleOptions.None, //압축방식
            BuildTarget.StandaloneWindows); //타겟 플랫폼
    }
}
