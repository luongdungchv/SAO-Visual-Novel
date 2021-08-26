using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SettingManager : MonoBehaviour
{
    public static SettingManager ins;
    public static bool isAutoPlay;
    public static int currentLanguage;

    public Toggle autoPlayToggle;

    public static SettingData settingData;
    public List<Setting> settingList;
    // Start is called before the first frame update
    void Start()
    {
        ins = this;
        string jsonData = PlayerPrefs.GetString("SettingData", "");
        if (jsonData == "") settingData = new SettingData();
        else settingData = JsonUtility.FromJson<SettingData>(jsonData);
        settingList.ForEach(n => n.OnInit(settingData));
    }
    public void Submit()
    {
        settingList.ForEach(n => n.OnSubmit(settingData));
        string jsonData = JsonUtility.ToJson(settingData);
        PlayerPrefs.SetString("SettingData", jsonData);
    }
    public void ResetData()
    {
        Debug.Log("clicked");
        Data2 data = new Data2()
        {
            groupId = 0,
            contentIndex = 0,
            saveDate = DateTime.Now.ToString(),
            imageData = new List<Character> { null, null, null }
        };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("dataslot3", json);
    }
    public void SwitchPanel(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
