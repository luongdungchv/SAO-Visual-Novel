using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LanguageSetting : Setting
{
    public static event EventHandler OnLanguageChange;
    public static LanguageSetting ins;
    public ContentGroup group;
   
    public TMP_Dropdown dropDown;

    public LanguageManager languageManager;
    private void Start()
    {
        ins = this;
        Debug.Log(group.nextGroup);
    }
    
    public override void OnSubmit(SettingData data)
    {
        data.currentLanguage = dropDown.value;
        OnLanguageChange?.Invoke(this, EventArgs.Empty);
    }
    public override void OnInit(SettingData data)
    {
        dropDown.value = data.currentLanguage;
    }
}
