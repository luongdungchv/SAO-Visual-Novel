using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Language : MonoBehaviour
{
    public string english;
    public string vietnamese;
    private void Start()
    {
        LanguageSetting.OnLanguageChange += OnLanguageChangeDelegate;
        ChangeText();

    }
    void OnLanguageChangeDelegate(object sender, EventArgs e)
    {
        ChangeText();
    }
    public void ChangeText()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        
        if (SettingManager.settingData.currentLanguage == 0) text.text = english;
        else if (SettingManager.settingData.currentLanguage == 1) text.text = vietnamese;
    }
    private void OnDestroy()
    {
        LanguageSetting.OnLanguageChange -= OnLanguageChangeDelegate;
    }
}
