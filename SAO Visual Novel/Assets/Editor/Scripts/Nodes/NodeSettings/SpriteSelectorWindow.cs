using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.AddressableAssets;
using System.Linq;

public class SpriteSelectorWindow : EditorWindow
{
    public ContentOption targetConfiguration;
    public string type;

    private void OnEnable()
    {
        rootVisualElement.style.flexDirection = FlexDirection.Row;
    }
    public void Setup(string name)
    {
        //Sprite[] sprites = Resources.LoadAll<Sprite>($"CharacterSprites/{name}");
        
        Addressables.LoadAssetAsync<IList<Sprite>>($"CharacterSprites/{name}.psb").Completed += obj =>
        {
            var sprites = obj.Result as List<Sprite>;
            Debug.Log(sprites.Count);
            for (int n = 0; n < sprites.Count; n++)
            {
                var i = sprites[n];
                Texture2D btnImage = new Texture2D((int)i.rect.width, (int)i.rect.height);
                var pixels = i.texture.GetPixels((int)Mathf.Round(i.rect.x),
                                                    (int)Mathf.Round(i.rect.y),
                                                    (int)Mathf.Round(i.rect.width),
                                                    (int)Mathf.Round(i.rect.height));
                btnImage.SetPixels(pixels);
                btnImage.Apply();

                rootVisualElement.Add(CreateButton(btnImage, n));
            }
        };
       

    }
    public Button CreateButton(Texture2D backgroundImage, int spriteIndex)
    {
        Button btn = new Button();
        btn.style.width = 120;
        btn.style.height = 120;
        btn.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
        btn.style.backgroundImage = backgroundImage;
        
        btn.clicked += () =>
        {
            targetConfiguration.SetImage(type, spriteIndex, backgroundImage);
            this.Close();
        };
        
        return btn;
    }
}
