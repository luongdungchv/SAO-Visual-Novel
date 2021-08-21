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
        //contents = new List<Content>();
    }
    public bool HasTransition()
    {
        //bool res = nextGroup == null && !hasTrasition;
        //return !res;

        if (nextGroup == null) return false;
        if (hasTrasition) return true;
        return false;
    }

    public bool HasSelection()
    {
        //bool res = selection.SelectionCount() == 0 || selection == null;
        //return !res;
        if (selection == null) return false;
        if (selection.SelectionCount() == 0) return false;
        return true;
    }
    public void PlayTransitionEffect(SpriteRenderer img, TextMeshPro text, Action complete)
    {
        if (vieTransitionText != "" && engTransitionText != "")
        {
            string currentTransitionText = "";
            if (SettingManager.currentLanguage == 0) currentTransitionText = engTransitionText;
            else if (SettingManager.currentLanguage == 1) currentTransitionText = vieTransitionText;
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
            if (SettingManager.currentLanguage == 0) currentTransitionText = engTransitionText;
            else if (SettingManager.currentLanguage == 1) currentTransitionText = vieTransitionText;
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
