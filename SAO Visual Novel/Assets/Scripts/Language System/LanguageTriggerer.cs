using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LanguageTriggerer : MonoBehaviour
{
    public static LanguageTriggerer ins;
    public Toggle english;
    public Toggle vietnamese;

    public TMP_Dropdown dropDown;

    public LanguageManager languageManager;
    private void Start()
    {
        //Debug.Log(PlayerPrefs.GetString("dataslot3"));
        ins = this;
        Debug.Log(PlayerPrefs.GetString("dataslot3"));
        //Data2 data = new Data2()
        //{
        //    groupId = 0,
        //    contentIndex = 0,
        //    saveDate = DateTime.Now.ToString(),
        //    imageData = new List<Character> {null, null, null}
        //};
        //string json = JsonUtility.ToJson(data);
        //PlayerPrefs.SetString("dataslot3", )
        dropDown.value = PlayerPrefs.GetInt("language", 0);
        
    }
    public void Submit()
    {
        LanguageManager.LanguageEnum newLanguage = LanguageManager.LanguageEnum.English;
        int index = dropDown.value;
        if (index == 1) newLanguage = LanguageManager.LanguageEnum.Vietnamese;
        SettingManager.currentLanguage = (int)newLanguage;
        LanguageManager.ins.BroadcastLanguageChange(newLanguage);
    }

}
