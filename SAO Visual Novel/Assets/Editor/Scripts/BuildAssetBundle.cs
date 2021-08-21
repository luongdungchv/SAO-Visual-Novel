using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAssetBundle 
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllBundles()
    {
        string directory = "Assets/AssetBundles";

        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/ClearCache")]
    static void ClearCache()
    {
        Caching.ClearAllCachedVersions("chapter2.pack");
        AssetBundle.UnloadAllAssetBundles(true);
        //Caching.ClearCache();
    }
}
