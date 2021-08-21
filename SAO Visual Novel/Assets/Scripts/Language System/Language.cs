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
        LanguageManager.OnLanguageChange += OnLanguageChangeDelegate;
        ChangeText();

    }
    void OnLanguageChangeDelegate(object sender, EventArgs e)
    {
        ChangeText();
    }
    public void ChangeText()
    {
        Debug.Log(this);
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        int currentLangIndex = (int)LanguageManager.ins.currentLanguage;
        if (currentLangIndex == 0) text.text = english;
        else if (currentLangIndex == 1) text.text = vietnamese;
    }
    private void OnDestroy()
    {
        LanguageManager.OnLanguageChange -= OnLanguageChangeDelegate;
    }
}
