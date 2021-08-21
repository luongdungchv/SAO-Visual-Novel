using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;
using System.IO;
using System.Linq;

public class GroupManagerWindow : EditorWindow
{
    Button deleteBtn;
    string selectedGroup;
    public void SetupTest()
    {
        
    }
    public void Setup()
    {
        deleteBtn = new Button() {text = "Delete" };
        deleteBtn.clicked += () =>
        {
            AssetDatabase.DeleteAsset($"Assets/ContentGroups/{selectedGroup}");
            rootVisualElement.Remove(rootVisualElement.Query<ListView>());
            GenerateListView();
        };
        deleteBtn.SetEnabled(false);
        rootVisualElement.Insert(0, deleteBtn);
        GenerateListView();      
    }

    void GenerateListView()
    {
        var files = Directory.GetFiles("Assets/ContentGroups", "*.asset").Select(f => Path.GetFileName(f));

        var items = files.ToList();
        Func<VisualElement> makeItem = () => new Label();
        Action<VisualElement, int> bindItem = (s, i) => (s as Label).text = items[i];
        const int itemHeight = 16;

        var listView = new ListView(items, itemHeight, makeItem, bindItem);

        listView.selectionType = SelectionType.Single;

        listView.onSelectionChange += objects => {
            var value = objects.ToList()[0];
            Debug.Log(value.ToString());
            selectedGroup = value.ToString();
            deleteBtn.SetEnabled(true);
        };

        listView.style.flexGrow = 1.0f;

        rootVisualElement.Add(listView);
    }
}
