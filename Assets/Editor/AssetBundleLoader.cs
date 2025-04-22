using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    //https://docs.google.com/uc?export=download&id=123MzPSZp58DPhPu_DEukS8x_rHsP8QOW
    void Start()
    {
        string url = "https://docs.google.com/uc?export=download&id=123MzPSZp58DPhPu_DEukS8x_rHsP8QOW";
        StartCoroutine(AssetBundleFromWeb(url));
    }

    // 웹에서 에셋 번들을 다운로드하고 로드하는 코루틴
    IEnumerator AssetBundleFromWeb(string url)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();  // 웹 요청이 완료될 때까지 기다림

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("에셋 번들을 다운로드할 수 없습니다: " + www.error);
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);  // 에셋 번들 가져오기
        if (bundle == null)
        {
            Debug.LogError("에셋 번들을 로드할 수 없습니다!");
            yield break;
        }

        // 에셋들 로드 및 확인
        string[] assetNames = bundle.GetAllAssetNames();
        Debug.Log(assetNames.Length);
        foreach (var name in assetNames)
        {
            Debug.Log("번들에 포함된 에셋: " + name);
        }

        //var barrel = bundle.LoadAsset<GameObject>("Barrel"); // 베럴 불러와지는지 테스트
        //if (barrel != null)
        //{
        //    Instantiate(barrel);
        //}
        //else
        //{
        //    Debug.LogWarning("프리팹을 찾을 수 없습니다.");
        //}

        //var mat = bundle.LoadAsset<Material>("barrel-barrel 1"); // 일반 메터리얼도 마찬가지
        //if (mat != null)
        //{
        //    Debug.Log("머티리얼 로드 성공!");
        //}
        //else
        //{
        //    Debug.LogWarning("머티리얼을 찾을 수 없습니다.");
        //}

        bundle.Unload(false); // 번들 해제
    }
}
