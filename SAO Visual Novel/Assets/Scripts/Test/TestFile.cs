using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestFile : MonoBehaviour
{
    public BoxCollider2D box;
    public BoxCollider2D box2;
    public SpriteRenderer mesh;
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        var b1 = obj.transform.GetChild(1).GetComponent<BoxCollider2D>();
        var b2 = obj.transform.GetChild(2).GetComponent<BoxCollider2D>();
        Debug.Log(b1.bounds.size);
        //var tex = MergeImage(b1, b2);
        //var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        //mesh.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Texture2D GetTexture(Sprite i)
    {
        var baseTex = new Texture2D((int)i.rect.width, (int)i.rect.height);
        var pixels = i.texture.GetPixels((int)Mathf.Round(i.rect.x),
                                                (int)Mathf.Round(i.rect.y),
                                                (int)Mathf.Round(i.rect.width),
                                                (int)Mathf.Round(i.rect.height));
        baseTex.SetPixels(pixels);
        baseTex.Apply();
        return baseTex;
    }
    public Texture2D MergeImage(BoxCollider2D box, BoxCollider2D box2)
    {
        var i = box.GetComponent<SpriteRenderer>().sprite;
        var baseTex = GetTexture(i);

        var basePointWorld = box.transform.position - Vector3.up * box.bounds.extents.y;
        var dir = (box2.transform.position - Vector3.up * box2.bounds.extents.y - Vector3.right * box2.bounds.extents.x) - basePointWorld;

        var proportion = baseTex.width / box.bounds.size.x;

        var dirTex = dir * proportion;
        Vector2 pos = new Vector2(baseTex.width / 2, 0) + (Vector2)dirTex;

        var additionalTex = GetTexture(box2.GetComponent<SpriteRenderer>().sprite);
        Debug.Log(additionalTex.GetPixel(0, 0).a);
        for (var a = pos.x; a <= pos.x + additionalTex.width; a++)
        {
            for (var b = pos.y; b <= pos.y + additionalTex.height; b++)
            {
                var newPixel = additionalTex.GetPixel((int)(a - pos.x), (int)(b - pos.y));
                if (newPixel.a == 0) continue;
                baseTex.SetPixel((int)a, (int)b, newPixel);
            }
        }
        baseTex.Apply();
        //mesh.material.mainTexture = baseTex;
        return baseTex;
    }
}
