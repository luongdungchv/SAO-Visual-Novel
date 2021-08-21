using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTex : MonoBehaviour
{
    public Sprite sprite;
    public Sprite sprite2;
    public MeshRenderer quad;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(sprite.texture.width);
        Debug.Log(sprite.texture.height);
        Debug.Log($"{sprite.rect.width} {sprite.rect.height}");

        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)Mathf.Round(sprite.rect.x),
                                                (int)Mathf.Round(sprite.rect.y),
                                                (int)Mathf.Round(sprite.rect.width),
                                                (int)Mathf.Round(sprite.rect.height));
        Debug.Log(pixels.Length);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        quad.material.mainTexture = croppedTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
