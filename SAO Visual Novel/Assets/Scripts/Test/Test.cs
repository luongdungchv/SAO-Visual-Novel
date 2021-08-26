using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test : MonoBehaviour
{
    public AssetReference refa;
    private void Start()
    {
        List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
        Addressables.DownloadDependenciesAsync("Asset").Completed += obj =>
        {
            obj.GetDependencies(list);
            Debug.Log(list[0]);
        };
        //Addressables.LoadAssetAsync<CharacterImageRenderer>(refa).Completed += obj =>
        //{
        //    Debug.Log(obj.Result);
        //};
        refa.LoadAssetAsync<GameObject>().Completed += obj =>
        {
            Debug.Log(obj.Result);
        };
    }
   
}
