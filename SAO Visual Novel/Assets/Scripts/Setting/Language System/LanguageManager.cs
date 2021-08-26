using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LanguageManager : MonoBehaviour
{
    
    public static LanguageManager ins;
    public static event EventHandler OnLanguageChange;
    public LanguageEnum currentLanguage;
    private void Start()
    {
        ins = this;
        currentLanguage = (LanguageEnum)PlayerPrefs.GetInt("language", 0);
    }
    public void BroadcastLanguageChange(LanguageEnum newLanguage)
    {
        currentLanguage = newLanguage;
        PlayerPrefs.SetInt("language", (int)currentLanguage);
        OnLanguageChange?.Invoke(this, EventArgs.Empty);
    }
    public static int GetCurrentLanguage()
    {
        return (int)ins.currentLanguage;
    }
}
public enum LanguageEnum { English, Vietnamese }
