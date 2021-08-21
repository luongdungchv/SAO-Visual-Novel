using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "New SLot", menuName = "Data SLot")]
[Serializable]
public class DataSlot : ScriptableObject
{
    public ContentGroup group;
    public ObjectIdManager idManager;

    public string dataAddress;

    public int contentIndex;
    public string saveDate;

    public List<Sprite> charImageList;
    List<Character> savedImageData;

    public static event EventHandler OnDataChecked;

    private void OnEnable()
    {
        try
        {
            Data2 loadData = JsonUtility.FromJson<Data2>(GetJsonData());
            if (dataAddress == "dataslot3")
            {
                Debug.Log(GetJsonData());
                Debug.Log(loadData.imageData.Count);
            }
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
        //ContentManager manager = ContentManager.ins;
        //group = manager.group;
        //charImageList.Clear();

        //manager.currentCharImages.ForEach(n =>
        //{
        //    charImageList.Add(n);
        //});

        //Data saveData = new Data();
        //saveData.groupId = idManager.groupList.IndexOf(manager.group);
        //Debug.Log(manager.group);
        //saveData.contentIndex = manager.GetCurrentIndex();
        //saveData.saveDate = DateTime.Now.ToString();
        //saveData.imageIdList = new List<int>();
        //foreach (var n in manager.currentCharImages)
        //{
        //    saveData.imageIdList.Add(idManager.spriteList.IndexOf(n));
        //}
        //SaveJsonData(JsonUtility.ToJson(saveData));
        //GetData(saveData);

        var manager = ContentManager.ins;
        group = manager.group;
        Data2 saveData = new Data2();
        saveData.groupId = idManager.groupList.IndexOf(manager.group);
        saveData.contentIndex = manager.GetCurrentIndex();
        saveData.saveDate = DateTime.Now.ToString();
        saveData.imageData = new List<Character>();
        foreach (var i in AnimationPlayer.ins.originalRenderers)
        {
            if (i.character == null) { saveData.imageData.Add(null); continue; }
            saveData.imageData.Add(i.character);
        }
        Debug.Log(saveData.imageData.Count);
        string json = JsonUtility.ToJson(saveData);
        Debug.Log(AnimationPlayer.ins.originalRenderers[0]);
        PlayerPrefs.SetString(dataAddress, json);
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
            for (int i = 0; i < animPlayer.originalRenderers.Count; i++)
            {
                try { if (savedImageData[i] != null) animPlayer.Animate2(savedImageData[i], i); } catch { }
            }

        };
    }
    
    public void SaveJsonData(string value)
    {
        PlayerPrefs.SetString(dataAddress, value);
    }
    public void GetData(Data data)
    {
        group = idManager.groupList[data.groupId];
        contentIndex = data.contentIndex;
        saveDate = data.saveDate;
        charImageList.Clear();
        
        foreach (var i in data.imageIdList)
        {
            if (i < 0)
            {
                charImageList.Add(null);
                continue;
            }
            charImageList.Add(idManager.spriteList[i]);

        }
    }
    public void GetData(Data2 data)
    {
        group = idManager.groupList[data.groupId];
        contentIndex = data.contentIndex;
        saveDate = data.saveDate;
        savedImageData = new List<Character>(data.imageData);

        //group = idManager.groupList[0];
        //contentIndex = 0;
        //saveDate = "";
        //savedImageData.Clear();
        //savedImageData = new List<Character>();
    }
    public string GetJsonData()
    {
        return PlayerPrefs.GetString(dataAddress, "");
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

