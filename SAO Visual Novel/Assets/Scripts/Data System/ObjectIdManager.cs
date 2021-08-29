using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
[CreateAssetMenu(fileName = "New Manager", menuName = "Id Manager")]
public class ObjectIdManager : ScriptableObject
{
    public static ObjectIdManager ins;
    public List<ContentGroup> groupList;

    private void OnEnable()
    {
        if (ins == null) ins = this;
        groupList = groupList.Where(n => n != null).ToList(); 
    }
    public static Sprite[] GetSprites(string name)
    {
        return Resources.LoadAll<Sprite>($"CharacterSprites/{name}");
    }
    public ContentGroup GetGroup(int index)
    {
        return groupList[index];
    }
    public int GetGroupIndex(ContentGroup group)
    {
        return groupList.IndexOf(group);
    }
    public void AddGroup(ContentGroup group)
    {
        groupList.Add(group);
    }
}
