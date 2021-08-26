using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public bool isAutoPlay;
    public int currentLanguage;
    public SettingData()
    {
        isAutoPlay = false;
        currentLanguage = 0;
    }
}
