using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class Selection
{
    //public List<SelectionButton> buttons;
    public List<SelectionButton> selections;
    public string engQuestion;
    public string vieQuestion;

    public Selection()
    {
        selections = new List<SelectionButton>();
    }
    public int SelectionCount()
    {
        return selections.Count;
    }
    
    
    public IEnumerator AnimateQuestionEnum(TextMeshPro contentText, Action complete)
    {
        string currentQuestion = "";
        if (SettingManager.currentLanguage == 0) currentQuestion = engQuestion;
        else if (SettingManager.currentLanguage == 1) currentQuestion = vieQuestion;
        int index = 0;
        ContentManager.ins.isWriting = true;
        while(index < currentQuestion.Length)
        {
            string part = currentQuestion.Substring(index, 1);
            index++;
            contentText.text += part;
            yield return null;
        }
        ContentManager.ins.isWriting = false;
        complete();
    }
    
}
