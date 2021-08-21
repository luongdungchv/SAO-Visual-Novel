using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Manager", menuName = "Id Manager")]
public class ObjectIdManager : ScriptableObject
{
    public static ObjectIdManager ins;
    public List<ContentGroup> groupList;
    public List<Sprite> spriteList;
    public List<CharacterImageRenderer> imageList;
    public Dictionary<string, List<Sprite>> spriteMap;

    private void OnEnable()
    {
        if (ins == null) ins = this;
    }
    public static Sprite[] GetSprites(string name)
    {
        return Resources.LoadAll<Sprite>($"CharacterSprites/{name}");
    }
}
