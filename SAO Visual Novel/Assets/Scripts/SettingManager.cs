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
    // Start is called before the first frame update
    void Start()
    {
        ins = this;

        currentLanguage = PlayerPrefs.GetInt("language", 0);

        autoPlayToggle.isOn = PlayerPrefs.GetInt("autoplay", 0) == 1;
    }
    public void Submit()
    {
        isAutoPlay = autoPlayToggle.isOn;
        LanguageTriggerer.ins.Submit();
        //currentLanguage = (int)LanguageManager.ins.currentLanguage;
        PlayerPrefs.SetInt("autoplay", Convert.ToInt32(isAutoPlay));
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
        DebugRuntime.ins.Log(json);
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
