using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;

public class ContentConfiguring : EditorWindow
{
    public static event EventHandler OnSave;
    public static bool selectionState;
    public static bool contentState;

    public ContentGroup group;
    public ContentNode currentNode;
    
    public Foldout selectionFold;
    public Foldout contentFold;
    public VisualElement otherElement;

    public ScrollView scrollView;

    private void OnEnable()
    {
        Button saveBtn = new Button();
        saveBtn.text = "Save";
        saveBtn.style.paddingRight = 5;
        saveBtn.clicked += () => OnSave?.Invoke(this, EventArgs.Empty);
        rootVisualElement.Add(saveBtn);
    }
    private void OnDisable()
    {
        selectionState = selectionFold.value;
        contentState = contentFold.value;
        ContentNode.canOpenView = true;
        EditorUtility.SetDirty(group);
        AssetDatabase.SaveAssets();
    }
    public void Setup()
    {
        selectionFold = new Foldout() { text = "Selections", value = selectionState };
        contentFold = new Foldout() {text = "Content", value = contentState};
        
        scrollView = new ScrollView();
        otherElement = new VisualElement();      

        int index = 0;
        group.selection.selections = group.selection.selections.Where(j => j.group != null).ToList();
        foreach (var i in group.selection.selections)
        {
            if (i.group == null) continue;
            index++;
            Foldout selectionOption = new Foldout();
            selectionOption.text = $"Selection {index}";
            selectionOption.style.marginLeft = 30;
            ObjectField groupField = new ObjectField()
            {
                objectType = typeof(ContentGroup),
                allowSceneObjects = false
            };
            TextField answerField = new TextField();

            groupField.SetValueWithoutNotify(i.group);
            answerField.SetValueWithoutNotify(i.answer);

            OnSave += (s, e) =>
            {
                i.group = groupField.value as ContentGroup;
                i.answer = answerField.value;
            };

            selectionOption.Add(groupField);
            selectionOption.Add(answerField);

            selectionFold.Add(selectionOption);
        }
   
        List<Content> contentsClone = group.contents == null ? new List<Content>() : new List<Content>(group.contents);
        IntegerField contentNumberField = new IntegerField() { label = "number", value = contentsClone.Count};
        int contentNumber = contentNumberField.value;
        contentNumberField.RegisterCallback<KeyDownEvent>(v =>
        {
            if(v.keyCode == KeyCode.Return)
            {
                contentNumber = contentNumberField.value;
                contentFold.Clear();
                contentFold.Add(contentNumberField);
                for (int i = 0; i < contentNumber; i++)
                {
                    if (i < contentsClone.Count)
                    {
                        RenderContentConfiguration(contentsClone[i], i + 1);
                    }
                    else
                    {
                        Content newContent = new Content();
                        newContent.character = new Character();
                        RenderContentConfiguration(newContent, i + 1);
                        contentsClone.Add(newContent);
                    }
                }
            }
        });
        contentFold.Add(contentNumberField);
        index = 0;
        foreach (var i in contentsClone)
        {
            index++;
            RenderContentConfiguration(i, index);
        }

        OnSave += (s, e) =>
        {
            group.contents.Clear();
            for(int i = 0; i < contentNumber; i++)
            {
                group.contents.Add(contentsClone[i]);
            }
        };

        scrollView.Add(selectionFold);
        scrollView.Add(contentFold);
        RenderOthers();
        rootVisualElement.Add(scrollView);
    }
    
    void RenderContentConfiguration(Content i, int index)
    {
        ContentOption contentOption = new ContentOption(this, "ContentOption", "ConfigStyle", i, index);
        contentFold.Add(contentOption.contentOption);    
    }
    void RenderOthers()
    {
        //VisualElement container = new VisualElement();
        Resources.Load<VisualTreeAsset>("OtherConfiguration").CloneTree(otherElement);

        otherElement.styleSheets.Add(Resources.Load<StyleSheet>("ConfigStyle"));

        VisualElement backgroundImageElement = otherElement.Query<VisualElement>("backgroundImage");
        VisualElement hasTransitionElement = otherElement.Query<VisualElement>("hasTransition");
        VisualElement hasTextTransitionElement = otherElement.Query<VisualElement>("hasTextTransition");
        VisualElement vieTranstionElement = otherElement.Query<VisualElement>("vieTransition");
        VisualElement engTransitionElement = otherElement.Query<VisualElement>("engTransition");

        ObjectField backgroundImageField = backgroundImageElement.Query<ObjectField>();
        Toggle hasTransitionToggle = hasTransitionElement.Query<Toggle>();
        Toggle hasTextTransitionToggle = hasTextTransitionElement.Query<Toggle>();
        TextField vieTransitionText = vieTranstionElement.Query<TextField>();
        TextField engTransitionText = engTransitionElement.Query<TextField>();
        ToolbarMenu colorSelector = otherElement.Query<ToolbarMenu>("colorSelector");

        backgroundImageField.objectType = typeof(Sprite);
        backgroundImageField.SetValueWithoutNotify(group.background);

        //if (group.nextGroup == null) otherElement.Remove(hasTransitionElement);
       
        hasTransitionToggle.value = group.hasTrasition;
        if (!hasTransitionToggle.value)
        {
            otherElement.Remove(colorSelector);
        }

        hasTransitionToggle.RegisterValueChangedCallback(v =>
        {
            if (v.newValue)
            {
                otherElement.Insert(otherElement.IndexOf(hasTransitionElement) + 1, colorSelector);
            }
            else otherElement.Remove(colorSelector);
        });
        hasTextTransitionToggle.value = group.vieTransitionText != "" && group.engTransitionText != "";
        if (!hasTextTransitionToggle.value)
        {
            otherElement.Remove(vieTranstionElement);
            otherElement.Remove(engTransitionElement);
        }
        hasTextTransitionToggle.RegisterValueChangedCallback(v =>
        {
            if (!v.newValue)
            {
                otherElement.Remove(vieTranstionElement);
                otherElement.Remove(engTransitionElement);
            }
            else
            {
                otherElement.Add(vieTranstionElement);
                otherElement.Add(engTransitionElement);
            }
        });
        vieTransitionText.value = group.vieTransitionText;
        engTransitionText.value = group.engTransitionText;

        Color transitionColor = group.transitionColor;
        colorSelector.text = transitionColor == Color.white ? "White" : "Black";
        colorSelector.menu.AppendAction("Black", new Action<DropdownMenuAction>(n =>
        {
            transitionColor = Color.black;
            colorSelector.text = "Black";
        }));
        colorSelector.menu.AppendAction("White", new Action<DropdownMenuAction>(n =>
        {
            transitionColor = Color.white;
            colorSelector.text = "White";
        }));

        OnSave += (s, e) =>
        {
            group.background = backgroundImageField.value as Sprite;
            group.hasTrasition = hasTransitionToggle.value;
            group.transitionColor = transitionColor;
            if (hasTextTransitionToggle.value)
            {
                group.vieTransitionText = vieTransitionText.value;
                group.engTransitionText = engTransitionText.value;
            }
            else
            {
                group.vieTransitionText = "";
                group.engTransitionText = "";
            }
        };      

        scrollView.Add(otherElement);       
    }
    public void OpenSpriteWindow(ContentOption option, string type, string charName)
    {
        SpriteSelectorWindow spriteWindow = GetWindow<SpriteSelectorWindow>("Sprite Selector");
        spriteWindow.targetConfiguration = option;
        spriteWindow.type = type;
        spriteWindow.Setup(charName);
    }
    public Texture2D GetTexture(Sprite sprite)
    {
        var tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)Mathf.Round(sprite.rect.x),
                                                (int)Mathf.Round(sprite.rect.y),
                                                (int)Mathf.Round(sprite.rect.width),
                                                (int)Mathf.Round(sprite.rect.height));
        Debug.Log($"{tex.width * tex.height} {pixels.Length}");
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }
    public void SetImage(string type, Texture2D image)
    {
        if(type == "body")
        {
            Image img = rootVisualElement.Query<Image>("bodyImage");
            img.image = image;
        }
        else
        {
            Image img = rootVisualElement.Query<Image>("emotionImage");
            img.image = image;
        }
    }
    
}
public class ContentOption
{
    public VisualElement contentOption;
    private ContentConfiguring parent;
    private int bodyIndex, emotionIndex;
    public ContentOption(ContentConfiguring parent, string uxmlAsset, string ussAsset, Content i, int index)
    {
        this.parent = parent;
        contentOption = new VisualElement();
        contentOption.styleSheets.Add(Resources.Load<StyleSheet>(ussAsset));
        var asset = Resources.Load<VisualTreeAsset>(uxmlAsset);
        asset.CloneTree(contentOption);

        TextField englishField = contentOption.Query<TextField>("englishField");
        TextField vietnameseField = contentOption.Query<TextField>("vietnameseField");
        TextField speakerField = contentOption.Query<TextField>("speakerField");
        ToolbarMenu renderPosMenu = contentOption.Query<ToolbarMenu>("renderPosMenu");

        Foldout elementContainer = contentOption.Query<Foldout>("container");
        Foldout characterStats = contentOption.Query<Foldout>("char");

        ToolbarMenu nameMenu = characterStats.Query<ToolbarMenu>();
        Image bodyImage = characterStats.Query<Image>("bodyImage");
        Button selectBodyImage = characterStats.Query<Button>("selectBodyImage");
        Image emotionImage = characterStats.Query<Image>("emotionImage");
        Button selectEmotionImage = characterStats.Query<Button>("selectEmotionImage");

        try
        {
            nameMenu.text = i.character.name;

            var aaSetting = AddressableAssetSettingsDefaultObject.Settings;
           
            Addressables.LoadAssetsAsync<Sprite>("sprite", null).Completed += obj =>
            {
                
            };
             
            Debug.Log("gg");
            var sprites = AddressablesEditor.LoadAllAsset<GameObject>($"CharacterSprites/{nameMenu.text}.psb");
            Debug.Log(sprites[0]);

            bodyIndex = i.character.bodyIndex;
            emotionIndex = i.character.emotionIndex;

            bodyImage.image = parent.GetTexture(sprites[bodyIndex].GetComponent<SpriteRenderer>().sprite);
            emotionImage.image = parent.GetTexture(sprites[emotionIndex].GetComponent<SpriteRenderer>().sprite);

            bodyImage.scaleMode = ScaleMode.ScaleToFit;
            emotionImage.scaleMode = ScaleMode.ScaleToFit;

        }
        catch (Exception e) { Debug.Log(e); }
        selectBodyImage.clicked += () =>
        {
            parent.OpenSpriteWindow(this, "body", nameMenu.text);
        };
        selectEmotionImage.clicked += () =>
        {
            parent.OpenSpriteWindow(this, "emotion", nameMenu.text);
        };
        Addressables.LoadAssetsAsync<GameObject>("prefab", null).Completed += obj =>
        {
            var spriteList = obj.Result as List<GameObject>;
            foreach (var n in spriteList)
            {
                nameMenu.menu.AppendAction(n.name, new Action<DropdownMenuAction>(v =>
                {
                    nameMenu.text = n.name;

                    bodyImage.image = Texture2D.whiteTexture;
                    emotionImage.image = Texture2D.whiteTexture;

                    bodyIndex = 0;
                    emotionIndex = 0;
                }));
            }
        };
        elementContainer.text = $"Content {index}";

        englishField.SetValueWithoutNotify(i.englishContent);
        vietnameseField.SetValueWithoutNotify(i.vietnameseContent);
        speakerField.SetValueWithoutNotify(i.speaker);
        renderPosMenu.text = i.renderPos.ToString();

        Content.SpriteRenderPos renderPos = i.renderPos;
        renderPosMenu.menu.AppendAction("Left", new Action<DropdownMenuAction>(n =>
        {
            renderPosMenu.text = n.name;
            renderPos = Content.SpriteRenderPos.Left;
            i.renderPos = renderPos;
        }));
        renderPosMenu.menu.AppendAction("Mid", new Action<DropdownMenuAction>(n =>
        {
            renderPosMenu.text = n.name;
            renderPos = Content.SpriteRenderPos.Mid;
            i.renderPos = renderPos;
        }));
        renderPosMenu.menu.AppendAction("Right", new Action<DropdownMenuAction>(n =>
        {
            renderPosMenu.text = n.name;
            renderPos = Content.SpriteRenderPos.Right;
            i.renderPos = renderPos;
        }));
        ContentConfiguring.OnSave += (s, e) =>
        {
            i.character.name = nameMenu.text;
            i.character.bodyIndex = bodyIndex;
            i.character.emotionIndex = emotionIndex;

            i.englishContent = englishField.value;
            i.vietnameseContent = vietnameseField.value;
            i.speaker = speakerField.value;
            i.renderPos = renderPos;
        };
    }
    public void SetImage(string type, int index, Texture2D tex)
    {
        if(type == "body")
        {
            bodyIndex = index;
            Image image = contentOption.Query<Image>("bodyImage");
            image.image = tex;
        }
        else
        {
            emotionIndex = index;
            Image image = contentOption.Query<Image>("emotionImage");
            image.image = tex;
        }
    }
}
