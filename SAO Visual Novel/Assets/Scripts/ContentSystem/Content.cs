using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Content  
{
    public enum SpriteRenderPos { Left , Mid, Right }
    public Sprite image;

    public Character character;

    [TextArea(10,15)]
    public string englishContent;
    [TextArea(10, 15)]
    public string vietnameseContent;
    [HideInInspector]
    public ContentManager manager;
    [HideInInspector]
    public TextMeshPro contentText;
    [HideInInspector]
    public string currentContent;
    int index = 0;
    public string speaker;
    
    public SpriteRenderPos renderPos;

    public void DisplayText(TextMeshPro tmp)
    {
        string currentContent = "";
        if (SettingManager.settingData.currentLanguage == 0) currentContent = englishContent;
        else if (SettingManager.settingData.currentLanguage == 1) currentContent = vietnameseContent;

        tmp.text = currentContent;
    }

    public IEnumerator AnimateText()
    {
        SetText();
        index = 0;
        manager.isWriting = true;
        while (index < currentContent.Length)
        {
            string part = currentContent.Substring(index, 1);
            index++;
            contentText.text += part;
            yield return null;
        }
        manager.isWriting = false;
    }
    public void SetText()
    {
        if (SettingManager.settingData.currentLanguage == 0) currentContent = englishContent;
        else if (SettingManager.settingData.currentLanguage == 1) currentContent = vietnameseContent;
    }
}
