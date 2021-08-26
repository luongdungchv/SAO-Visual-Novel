using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
public class ContentManager : MonoBehaviour
{
    public static ContentManager ins;

    public DataSlot slot;
    public List<Sprite> currentCharImages;

    [HideInInspector]
    public List<GameObject> buttonList;
    [HideInInspector]
    public bool isWriting;

    [SerializeField] private SpriteRenderer fadePanel;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer speechBackground;

    [SerializeField] private Transform selectionBtnContainer;

    [SerializeField] private TextMeshPro contentText;
    [SerializeField] private TextMeshPro speakerText;
    [SerializeField] private TextMeshPro animationText;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectionButtonPrefab;

    private ContentGroup group;
    private Queue<Content> contentQueue;
    private Content currentContent;

    private Coroutine writeRoutine;

    private void Start()
    {
        LanguageSetting.OnLanguageChange += (s, e) => {
            RenderContent(currentContent);
            if (SettingManager.settingData.isAutoPlay) StartCoroutine(PlayNext());
        };

        ins = this;

        nextButton.onClick.AddListener(PlayContent);
        Debug.Log($"{group.nextGroup} {group}");
        TransitionManager.Fade("in", fadePanel, () => 
        {
             PlayContent();           
        });
    }

    Coroutine playCoroutine;
    public void PlayContent()
    {
        fadePanel.color = new Color(0, 0, 0, 0);
        if(playCoroutine != null) StopCoroutine(playCoroutine);
        
        if (contentQueue.Count >= 0)
        {
            
            //Debug.Log(group.nextGroup);
            if (isWriting)
            {
                isWriting = false;
                StopCoroutine(writeRoutine);
                currentContent.DisplayText(contentText);
            }
            else if (!isWriting && contentQueue.Count > 0)
            {
                RenderContent(contentQueue.Dequeue());
            }

            else if (!isWriting && contentQueue.Count == 0)
            {
                
                if (group.HasSelection())
                {
                    RenderSelection(group.selection);
                    return;
                }
                if (group.nextGroup != null)
                {
                    Debug.Log("g");
                    group.nextGroup.PlayTransitionEffect(fadePanel, animationText, () => ReloadScene(group.nextGroup), () =>
                    {
                        ReloadScene(group);
                        PlayContent();
                    });
                }
            }
        }
        if (SettingManager.settingData.isAutoPlay)
        {           
            playCoroutine = StartCoroutine(PlayNext());
        }
    }
    IEnumerator PlayNext()
    {
        yield return new WaitForSeconds(4f);
        PlayContent();
    }
   
    public void RenderContent(Content c)
    {   
        currentContent = c;

        //AnimationPlayer.ins.Animate(c.image, c.renderPos);
        AnimationPlayer.ins.Animate2(c.character, (int)c.renderPos);

        contentText.text = "";

        speakerText.text = c.speaker;

        writeRoutine = StartCoroutine(c.AnimateText());
    }
    public int GetCurrentIndex()
    {
        return group.contents.IndexOf(currentContent);
    }
    public void RenderSelection(Selection s)
    {
        nextButton.onClick.RemoveAllListeners();

        contentText.text = "";

        StartCoroutine(s.AnimateQuestionEnum(contentText, () => RenderButton(s)));   
    }
    void RenderButton(Selection s)
    {
        foreach (var i in s.selections)
        {
            var newButton = Instantiate(selectionButtonPrefab);

            newButton.GetComponentInChildren<TextMeshProUGUI>().text = i.answer;
            buttonList.Add(newButton.gameObject);
            newButton.transform.SetParent(selectionBtnContainer);
            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.onClick.AddListener(() =>
            {
                nextButton.onClick.AddListener(PlayContent);
                i.group.PlayTransitionEffect(fadePanel, animationText, () => ReloadScene(i.group), () => {
                    ReloadScene(i.group);
                    PlayContent();
                });
            });
        }
    }
    public void GenerateContentQueue(int startIndex)
    {
        Debug.Log(startIndex);
        contentQueue = new Queue<Content>();
        List<Content> contents = group.contents;
        for (int i = startIndex; i < contents.Count; i++)
        {
            contents[i].manager = this ;
            contents[i].contentText = contentText;
            contentQueue.Enqueue(contents[i]);
        }
    }
    public void LoadNewGroup(ContentGroup newGroup, int startIndex)
    {
        
        group = newGroup;
        GenerateContentQueue(startIndex);     
        background.sprite = group.background;
    }
    public ContentGroup GetCurrentGroup()
    {
        return group;
    }
    void ReloadScene(ContentGroup group)
    {
        contentText.text = "";
        for (int n = 0; n < 3; n++) currentCharImages[n] = null;

        AnimationPlayer.ins.originalRenderers.ForEach(n =>
        {
            n.character = null;
            for(int i = 0; i < n.transform.childCount; i++)
            {
                n.transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        });
        Debug.Log(group);
        LoadNewGroup(group, 0);
        buttonList.ForEach(n => Destroy(n));

    }
}
[Serializable]
public class Character
{
    public string name;
    public int emotionIndex;
    public int bodyIndex;
    
}
