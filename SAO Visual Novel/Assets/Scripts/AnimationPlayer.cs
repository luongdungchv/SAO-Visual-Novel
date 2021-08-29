using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AnimationPlayer : MonoBehaviour
{
    public static AnimationPlayer ins;

    public List<CharacterImageRenderer> originalRenderers;

    private void Start()
    {
        ins = this;
    }
    
    public void Animate2(Character c, int posIndex)
    {
        Debug.Log(c.name);
        var load = Addressables.LoadAssetAsync<GameObject>($"CharacterPrefabs/{c.name}.prefab");

        var prefab = load.WaitForCompletion().GetComponent<CharacterImageRenderer>();
        var newImage = Instantiate(prefab, originalRenderers[posIndex].transform.position, Quaternion.identity);
        newImage.SetProperty(c);

        int transparentCount = 0;
        for (int i = 0; i < 3; i++)
        {
            var part = originalRenderers[posIndex].transform.GetChild(i).GetComponent<SpriteRenderer>();
            part.sortingLayerName = "first";
            if (part.color.a == 0) transparentCount++;
        }
        for (int i = 0; i < newImage.transform.childCount; i++)
        {
            var part = newImage.transform.GetChild(i).gameObject;
            part.GetComponent<SpriteRenderer>().sortingLayerName = "second";
            if (i == c.bodyIndex || i == c.emotionIndex || part.name == "hairline") continue;
            Destroy(part);
        }
        if (transparentCount == 3)
        {
            Destroy(originalRenderers[posIndex].gameObject);
            originalRenderers[posIndex] = newImage;
            //ContentManager.ins.slot.Save();
            return;
        }
        FadeImage(posIndex, .15f, newImage);
    }
    
    void FadeImage(int pos, float time, CharacterImageRenderer newImage)
    {
        StartCoroutine(FadeImgEnum(pos, time, newImage));
    }
    IEnumerator FadeImgEnum(int index ,float time, CharacterImageRenderer newImage)
    {
        float t = 0;

        var oldImage = originalRenderers[index];
        originalRenderers[index] = newImage;

        while ( t <= 1)
        {
            t += Time.deltaTime / time;
            Color newColor = new Color(1, 1, 1, 1);
            float a = Mathf.Lerp(1, 0, t);
            newColor.a = a;
            for(int i = 0; i < 3; i++)
            {
                oldImage.transform.GetChild(i).GetComponent<SpriteRenderer>().color = newColor;
            }
            yield return null;
        }
        Destroy(oldImage.gameObject);
        //originalRenderers[index] = newImage;
        //ContentManager.ins.slot.Save();
    }
    public IEnumerable<Character> GetCurrentImageDatas()
    {
        foreach (var i in originalRenderers)
        {
            yield return i.character;
        }
    }

    
}
