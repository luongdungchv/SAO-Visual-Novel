using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class PreloadAsset : MonoBehaviour
{
    public AssetReference idManagerRef;
    public List<DataSlot> slotList;
    public Slider loadingBar;
    public Button startBtn;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        startBtn.interactable = false;
        var loadIdManagerOperation = Addressables.DownloadDependenciesAsync("Asset");
        StartCoroutine(VisualizeLoadProgress(loadIdManagerOperation));
        loadIdManagerOperation.Completed += obj =>
        {          
            startBtn.interactable = true;
        };

        Addressables.LoadAssetAsync<ObjectIdManager>(idManagerRef).Completed += obj =>
        {
            slotList.ForEach(n => {
                n.idManager = obj.Result;
                n.Setup();
            });
        };

    }
    IEnumerator VisualizeLoadProgress(AsyncOperationHandle input)
    {
        float percent = input.PercentComplete;
        //Vector3 newScale = new Vector3(percent, loadingBar.localScale.y, loadingBar.localScale.z);
        //loadingBar.localScale = newScale;
        loadingBar.value = percent;
        yield return null;
        if (percent < 1) StartCoroutine(VisualizeLoadProgress(input));

    }

   
}
