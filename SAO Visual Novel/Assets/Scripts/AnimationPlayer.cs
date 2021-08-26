using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AnimationPlayer : MonoBehaviour
{
    public static AnimationPlayer ins;

    public List<CharacterImageRenderer> originalRenderers;
    [SerializeField] private List<SpriteRenderer> renderPositions;

    private void Start()
    {
        ins = this;
    }
    
    public void Animate(Sprite t, Content.SpriteRenderPos pos)
    {
        var current = renderPositions[(int)pos].sprite == null || renderPositions[(int)pos].color.a == 0 ?
            renderPositions[(int)pos].transform.GetChild(0).GetComponent<SpriteRenderer>() :
            renderPositions[(int)pos].GetComponent<SpriteRenderer>();
        var other = current.transform.childCount == 0 ?
            current.transform.parent.GetComponent<SpriteRenderer>() :
            current.transform.GetChild(0).GetComponent<SpriteRenderer>();

        ContentManager.ins.currentCharImages[(int)pos] = t;
        other.sprite = t;
        other.color = new Color(1, 1, 1, 1);
        current.sortingLayerName = "first";
        other.sortingLayerName = "second";

        current.GetComponent<Animation>().Play();
    }
    public void Animate2(Character c, int posIndex)
    {
        Debug.Log(c.name);
        Addressables.LoadAssetAsync<GameObject>($"CharacterPrefabs/{c.name}.prefab").Completed += obj =>
        {
            var prefab = obj.Result.GetComponent<CharacterImageRenderer>();
            var newImage = Instantiate(prefab, originalRenderers[posIndex].transform.position, Quaternion.identity);
            newImage.character.name = c.name;
            newImage.character.bodyIndex = c.bodyIndex;
            newImage.character.emotionIndex = c.emotionIndex;

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
            //var currentPart = originalRenderers[index].transform.GetChild(i).GetComponent<SpriteRenderer>()
            if (transparentCount == 3)
            {
                Destroy(originalRenderers[posIndex].gameObject);
                originalRenderers[posIndex] = newImage;
                ContentManager.ins.slot.Save();
                return;
            }
            FadeImage(posIndex, .15f, newImage);
        };
    }
    
    void FadeImage(int pos, float time, CharacterImageRenderer newImage)
    {
        StartCoroutine(FadeImgEnum(pos, time, newImage));
    }
    IEnumerator FadeImgEnum(int index ,float time, CharacterImageRenderer newImage)
    {
        float t = 0;
        while( t <= 1)
        {
            t += Time.deltaTime / time;
            Color newColor = new Color(1, 1, 1, 1);
            float a = Mathf.Lerp(1, 0, t);
            newColor.a = a;
            for(int i = 0; i < 3; i++)
            {
                originalRenderers[index].transform.GetChild(i).GetComponent<SpriteRenderer>().color = newColor;
            }
            yield return null;
        }
        Destroy(originalRenderers[index].gameObject);
        originalRenderers[index] = newImage;
        ContentManager.ins.slot.Save();
    }
}
