using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;


public class Test : MonoBehaviour
{
    
    private void Start()
    {
        StartCoroutine(TestFunc());
        
    }
    IEnumerator TestFunc()
    {
        string uri = "https://drive.google.com/uc?id=1deP9AFZk_yPxtQfS-QuIDqK5qun2eOw6&export=download";
        Uri myUri = new Uri(uri);
        AssetBundleManifest manifest = AssetBundle.LoadFromFile("Assets/AssetBundles/AssetBundles")
            .LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        Hash128 hash = manifest.GetAssetBundleHash("chapter2.pack");
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, hash, 0);

        yield return request.SendWebRequest();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        var idManager = bundle.LoadAsset<ObjectIdManager>("chapter2IdManager");
        Debug.Log(idManager);
        bundle.Unload(false);
        string bundleName = System.IO.Path.GetFileNameWithoutExtension(myUri.AbsolutePath);
        Debug.Log(Caching.ClearAllCachedVersions(bundleName));
        
    }
}
