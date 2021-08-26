using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using System.Linq;

public class AddressablesEditor
{
    public static List<T> LoadAllAsset<T>(string address) where T: Object
    {
        if (IsFile(address))
        {
            var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(GetAssetPath(address));
            var res = assets.Where(n => n is T).Cast<T>().ToList();
            return res;
            
        }
        var files = Directory.GetFiles(GetAssetPath(address)).Where(n => !n.Contains("meta"));
        List<T> returnList = new List<T>();
        foreach (var i in files)
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(i);
            if (asset == null) continue;
            returnList.Add(asset);
        }
        return returnList;
    }
    public static T LoadAsset<T>(string address) where T:Object
    {
        return AssetDatabase.LoadAssetAtPath<T>(GetAssetPath(address));
    }
    public static string GetAssetPath(string address)
    {
        List<AddressableAssetEntry> entryList = new List<AddressableAssetEntry>();
        var setting = AddressableAssetSettingsDefaultObject.Settings;
        setting.GetAllAssets(entryList, true);
        foreach(var i in entryList)
        {
            if (i.address == address) return i.AssetPath;
            if (i.ParentEntry.address == address) return i.ParentEntry.AssetPath; 
        }
        return "";
    }
    static bool IsFile(string input)
    {
        if (Path.GetExtension(input) != "") return true;
        return false;
    }
}
