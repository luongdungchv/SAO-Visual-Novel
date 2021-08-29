using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "New Data SLot", menuName = "Data SLot")]
[Serializable]
public class DataSlot : ScriptableObject
{
    public ContentGroup group;

    public AssetReference idManagerRef;
    public ObjectIdManager idManager;

    public string dataAddress;

    public int contentIndex;
    public string saveDate;

    public List<Sprite> charImageList;
    List<Character> savedImageData;

    public static event EventHandler OnDataChecked;

    public bool isMain;

    private DataLoader loader;
    private DataSaver saver;

    public void OnEnable()
    {
        if (idManager == null) return;
        Setup();    
    }
    public void Setup()
    {
        try
        {
            Data2 loadData = JsonUtility.FromJson<Data2>(GetJsonData());
            GetData(loadData);
        }
        catch
        {
            Data2 data = new Data2()
            {
                groupId = 0,
                contentIndex = 0,
                saveDate = DateTime.Now.ToString(),
                imageData = new List<Character> { null, null, null }
            };
            GetData(data);
        }
    }    

    public bool HasData()
    {
        bool hasData = group == null || group.contents.Count == 0;
        
        return !hasData;
    }
    public void Save()
    {
        var manager = ContentManager.ins;
        group = manager.GetCurrentGroup();
        Data2 saveData = new Data2()
        {
            groupId = idManager.GetGroupIndex(manager.GetCurrentGroup()),
            contentIndex = manager.GetCurrentIndex(),
            saveDate = DateTime.Now.ToString(),
            imageData = new List<Character>()
        };
        
        foreach (var i in AnimationPlayer.ins.originalRenderers)
        {
            if (i.character == null) { saveData.imageData.Add(null); continue; }
            saveData.imageData.Add(i.character);
        }
        string json = JsonUtility.ToJson(saveData);
        SaveJsonData(json);

        GetData(saveData);

        if (loader != null) loader.Setup();
        if (saver != null) saver.Setup();
    }
    public void Load()
    {
        Data2 loadData = JsonUtility.FromJson<Data2>(GetJsonData());
        try
        {
            GetData(loadData);
        }
        catch
        {
            Data2 data = new Data2()
            {
                groupId = 0,
                contentIndex = 0,
                saveDate = DateTime.Now.ToString(),
                imageData = new List<Character> { null, null, null }
            };
            GetData(data);
        }

        SceneManager.LoadSceneAsync("SampleScene").completed += (obj) =>
        {
            ContentManager manager = GameObject.Find("ContentManager").GetComponent<ContentManager>();
            manager.LoadNewGroup(group, contentIndex);
            AnimationPlayer animPlayer = GameObject.Find("AnimationPlayer").GetComponent<AnimationPlayer>();
            Debug.Log(savedImageData.Count);
            for (int i = 0; i < animPlayer.originalRenderers.Count; i++)
            {
                try 
                { 
                    if (savedImageData[i] != null && savedImageData[i].name != "") 
                        animPlayer.Animate2(savedImageData[i], i); 
                } 
                catch { }
            }
        };
    }
    
    public void SaveJsonData(string value)
    {
        PlayerPrefs.SetString(dataAddress, value);
    }
   
    public void GetData(Data2 data)
    {
        group = idManager.GetGroup(data.groupId);
        contentIndex = data.contentIndex;
        saveDate = data.saveDate;
        savedImageData = new List<Character>(data.imageData);
    }
    public string GetJsonData()
    {
        return PlayerPrefs.GetString(dataAddress, "");
    }
    public void SetLoader(DataLoader _loader)
    {
        loader = _loader;
    }
    public void SetSaver(DataSaver _saver)
    {
        saver = _saver;
    }
}
[Serializable]
public class Data
{
    public int groupId;
    public int contentIndex;
    public string saveDate;
    public List<int> imageIdList;
}
[Serializable]
public class Data2
{
    public int groupId;
    public int contentIndex;
    public string saveDate;
    public List<Character> imageData;
}
public class ImageData
{
    public string name;
    public int bodyIndex, emotionIndex;
    public ImageData(string n, int b, int e)
    {
        name = n;
        bodyIndex = b;
        emotionIndex = e;
    }
}

