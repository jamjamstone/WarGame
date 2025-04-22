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

    // ������ ���� ������ �ٿ�ε��ϰ� �ε��ϴ� �ڷ�ƾ
    IEnumerator AssetBundleFromWeb(string url)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();  // �� ��û�� �Ϸ�� ������ ��ٸ�

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("���� ������ �ٿ�ε��� �� �����ϴ�: " + www.error);
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);  // ���� ���� ��������
        if (bundle == null)
        {
            Debug.LogError("���� ������ �ε��� �� �����ϴ�!");
            yield break;
        }

        // ���µ� �ε� �� Ȯ��
        string[] assetNames = bundle.GetAllAssetNames();
        Debug.Log(assetNames.Length);
        foreach (var name in assetNames)
        {
            Debug.Log("���鿡 ���Ե� ����: " + name);
        }

        //var barrel = bundle.LoadAsset<GameObject>("Barrel"); // ���� �ҷ��������� �׽�Ʈ
        //if (barrel != null)
        //{
        //    Instantiate(barrel);
        //}
        //else
        //{
        //    Debug.LogWarning("�������� ã�� �� �����ϴ�.");
        //}

        //var mat = bundle.LoadAsset<Material>("barrel-barrel 1"); // �Ϲ� ���͸��� ��������
        //if (mat != null)
        //{
        //    Debug.Log("��Ƽ���� �ε� ����!");
        //}
        //else
        //{
        //    Debug.LogWarning("��Ƽ������ ã�� �� �����ϴ�.");
        //}

        bundle.Unload(false); // ���� ����
    }
}
