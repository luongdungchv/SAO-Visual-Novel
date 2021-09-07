﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;

public class TestEditorWindow : EditorWindow
{
    public TestGraph graph;
    public Button saveBtn;
    private GraphDataContainer container;
    private GraphSaveLoadData dataManipulator;
    public static bool isLoad = true;

    //[MenuItem("Assets/Test Window")]
    [OnOpenAsset(0)]
    public static bool ShowWindow(int instanceId, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceId);
        if(asset is GraphDataContainer)
        {
            isLoad = false;
            var window = GetWindow<TestEditorWindow>("Test");
            //window.Setup(asset as GraphDataContainer);
            window.container = asset as GraphDataContainer;
            PlayerPrefs.SetString("Asset path", AssetDatabase.GetAssetPath(asset));
            window.Setup();
            isLoad = true;
        } 

        
        return false;
    }
    
    private void OnDisable()
    {
        rootVisualElement.Remove(graph);
        
    }
    
    private void OnEnable()
    {
        if (isLoad)
        {
            try
            {
                container = AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString("Asset path"), typeof(GraphDataContainer)) as GraphDataContainer;
                Setup();
            }
            catch(Exception e)
            {
                Debug.Log("No saved data path " + e.ToString());
            }
        }
    }
    public void Setup(GraphDataContainer data)
    {
        container = data;
        Setup();
    }
    public void Setup()
    {
        rootVisualElement.Clear();
        GenerateGraph();
        dataManipulator = new GraphSaveLoadData(graph);
        rootVisualElement.Add(CreateToolbar());
        Load();
        graph.viewTransformChanged = v =>
        {
            graph.OnChange();
            //Debug.Log(v.viewTransform.scale); 
        };
    }
    VisualElement CreateToolbar()
    {
        VisualElement root = new VisualElement();
        root.style.flexDirection = FlexDirection.Row;

        saveBtn = new Button(Save)
        {
            text = "Save",          
        };

        
        saveBtn.style.width = 80;

        Button manageGroupBtn = new Button(OpenGroupManager)
        {
            text = "Manage Groups"
        };

        root.Add(saveBtn);
        root.Add(manageGroupBtn);

        return root;
    }
    

    private void GenerateGraph()
    {
        graph = new TestGraph(this);
        graph.StretchToParentSize();
        rootVisualElement.Add(graph);
    }
    private void Save()
    {
        dataManipulator.Save(container);
        Setup();
    }
    private void Load()
    {
        dataManipulator.Load(container);
    }
    void OpenGroupManager()
    {
        var managerWindow = EditorWindow.GetWindow<GroupManagerWindow>("Group Manager");
        managerWindow.Setup();
    }
}
