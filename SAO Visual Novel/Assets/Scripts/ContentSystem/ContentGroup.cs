using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(fileName = "New Dialogue Group", menuName = "Dialogue Group")]
public class ContentGroup : ScriptableObject
{
    public Selection selection;
    public List<Content> contents;
    public Sprite background;
    public ContentGroup nextGroup;

    public string vieTransitionText;
    public string engTransitionText;

    public string nextChapter;

    public bool hasTrasition;
    public Color transitionColor;
    private void OnEnable()
    { 
    }
    public bool HasTransition()
    {
        if (nextGroup == null) return false;
        if (hasTrasition) return true;
        return false;
    }

    public bool HasSelection()
    {
       
        if (selection == null) return false;
        if (selection.SelectionCount() == 0) return false;
        return true;
    }
    public void PlayTransitionEffect(SpriteRenderer img, TextMeshPro text, Action complete)
    {
        if (vieTransitionText != "" && engTransitionText != "")
        {
            string currentTransitionText = "";
            if (SettingManager.settingData.currentLanguage == 0) currentTransitionText = engTransitionText;
            else if (SettingManager.settingData.currentLanguage == 1) currentTransitionText = vieTransitionText;
            text.text = currentTransitionText;
            TransitionManager.TransitionWithText(img, text, currentTransitionText, complete);
            return;
        }
        if (hasTrasition)
        {
            TransitionManager.FadeTransition("out", img, transitionColor, () => {
                TransitionManager.FadeTransition("in", img, transitionColor, complete);
            }); 
            return;
        }
        complete();
    }
    public void PlayTransitionEffect(SpriteRenderer img, TextMeshPro text,Action halfComplete, Action complete)
    {
        Debug.Log("yes");
        if (vieTransitionText != "" && engTransitionText != "")
        {
            string currentTransitionText = "";
            if (SettingManager.settingData.currentLanguage == 0) currentTransitionText = engTransitionText;
            else if (SettingManager.settingData.currentLanguage == 1) currentTransitionText = vieTransitionText;
            text.text = currentTransitionText;
            TransitionManager.TransitionWithText(img, text, currentTransitionText,halfComplete, complete);
            return;
        }
        if (hasTrasition)
        {
            TransitionManager.FadeTransition("out",img, transitionColor, () => {
                halfComplete();
                TransitionManager.FadeTransition("in", img, transitionColor, complete);
            });
            return;
        }
        complete();
    }
    
}
