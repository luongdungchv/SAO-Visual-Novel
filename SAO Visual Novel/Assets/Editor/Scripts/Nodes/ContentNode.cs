using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEditor;
using System.Linq;
using System.IO;


public class ContentNode : BaseNode
{
    public static bool canOpenView = true;

    public ObjectField groupField;
    public Button newGroupButton;

    private Toggle hasChoiceField;
    public Button modifyBtn, addChoiceBtn, removeChoiceBtn, addGroupBtn;
    VisualElement choiceManipulation;

    public int choiceCount;

    public ContentGroup group
    {
        get => (ContentGroup)groupField.value;
        set => groupField.SetValueWithoutNotify(value);
    }
    public bool hasChoice
    {
        get => hasChoiceField.value;
        set => hasChoiceField.value = value;
    }

    public ContentNode()
    {
        
    }
    public ContentNode(Vector2 pos, TestEditorWindow window, TestGraph graph)
    {
        this.window = window;
        this.graph = graph;

        title = "Content";
        SetPosition(new Rect(pos, defaultSize));
        node_Guid = Guid.NewGuid().ToString();

        InitializeComponents();
        styleSheets.Add(Resources.Load<StyleSheet>("NodeStyle"));
        groupField.RegisterValueChangedCallback(v =>
        {
            outputContainer.Clear();
            outputPortList.Clear();
            choiceCount = 0;
            if (group != null)
            {
                if (!group.HasSelection())
                {
                    hasChoice = false;
                    AddOutputPort("Next Group", Port.Capacity.Single);
                    return;
                }
                hasChoice = true;
                for (int i = 1; i <= group.selection.selections.Count; i++)
                {                   
                    choiceCount++;                  
                }
            }
            
        });
        
        modifyBtn.clicked += () =>
        {
            if (canOpenView)
            {
                ContentConfiguring config = EditorWindow.GetWindow<ContentConfiguring>(group.name);
                config.group = group;
                config.currentNode = this;
                config.Setup();
                inputContainer.Children().ToList();
                canOpenView = false;
            }
        };

        addChoiceBtn.clicked += () =>
        {
            if(group != null)
            {
                choiceCount++;
                AddOutputPort("Choice " + choiceCount.ToString(), Port.Capacity.Single);
                group.selection.selections.Add(new SelectionButton());
                RefreshExpandedState();
                RefreshPorts();
            }
        };
        removeChoiceBtn.clicked += () =>
        {
            if (group != null)
            {
                choiceCount--;
                //outputContainer.RemoveAt(outputContainer.childCount - 1);
                Port portToRemove = outputContainer.Children().ToList()[outputContainer.childCount - 1] as Port;
                Edge edgeToRemove = portToRemove.connections.ToList()[0];
                //edgeToRemove.input
                edgeToRemove.input.Disconnect(edgeToRemove);
                
                edgeToRemove.parent.Remove(edgeToRemove);
                Debug.Log(edgeToRemove.input.connected);
                outputContainer.Remove(portToRemove);

                group.selection.selections.RemoveAt(group.selection.selections.Count - 1);
                RefreshPorts();
                RefreshExpandedState();
            }
        };
        addGroupBtn.clicked += () =>
        {
            if (addGroupBtn.childCount > 0) return;
            TextField namingField = new TextField();
            addGroupBtn.Add(namingField);
            //addGroupBtn.SetEnabled(false);
            //addGroupBtn.clickable.active = false;
            namingField.RegisterCallback<KeyDownEvent>(v =>
            {
                if(v.keyCode == KeyCode.Return)
                {
                    if (!File.Exists($"Assets/ContentGroups/{namingField.value}.asset"))
                    {
                        ContentGroup newGroup = ScriptableObject.CreateInstance<ContentGroup>();
                        newGroup.contents = new List<Content>();
                        newGroup.selection = new Selection();

                        var groupName = namingField.value;
                        if (namingField.value == string.Empty) groupName = "New Group";
                        GraphSaveLoadData.AddGroupToPending(newGroup, groupName);
                        //AssetDatabase.CreateAsset(newGroup, $"Assets/ContentGroups/{groupName}.asset");
                        //EditorUtility.SetDirty(ObjectIdManager.ins);
                        //ObjectIdManager.ins.groupList.Add(newGroup);
                        groupField.SetValueWithoutNotify(newGroup);
                        //AssetDatabase.SaveAssets();
                    }
                    addGroupBtn.SetEnabled(true);
                    addGroupBtn.Remove(namingField);
                    
                }
            });
            graph.RegisterCallback<MouseUpEvent>(v =>
            {
                if (v.button == 0 && addGroupBtn.Contains(namingField)) addGroupBtn.Remove(namingField);
                
            });
        };
        

        choiceManipulation.Add(addChoiceBtn);
        choiceManipulation.Add(removeChoiceBtn);

        hasChoiceField.RegisterValueChangedCallback(v =>
        {
            outputPortList.ForEach(n => outputContainer.Remove(n));
            outputPortList.Clear();
            if (!v.newValue)
            {
                group.selection.selections.Clear();
                mainContainer.Remove(choiceManipulation);              
                AddOutputPort("Next Group", Port.Capacity.Single);               
            }
            else
            {
                group.selection.selections.Clear();
                mainContainer.Add(choiceManipulation);
                for (int i = 1; i <= choiceCount; i++)
                {
                    AddOutputPort("Choice " + i.ToString(), Port.Capacity.Single);
                    
                    group.selection.selections.Add(new SelectionButton());
                }
            }
        });
        mainContainer.Add(groupField);
        mainContainer.Add(addGroupBtn);
        mainContainer.Add(modifyBtn);
        mainContainer.Add(hasChoiceField);
        AddInputPort("Input", Port.Capacity.Multi);
    }
    void LoadNodeData(string guid, int choiceCount, ContentGroup newGroup, bool hasChoice)
    {
        node_Guid = guid;
        this.choiceCount = choiceCount;
        this.group = newGroup;
        this.hasChoice = hasChoice;
        if (!hasChoice)
        {
            AddOutputPort("Next Group", Port.Capacity.Single);
        }
        else
        {
            mainContainer.Add(choiceManipulation);
            for (int j = 1; j <= choiceCount; j++)
            {
                AddOutputPort($"Choice {j.ToString()}", Port.Capacity.Single);
            }
        }
    }
    public override void LoadNodeData(BaseNodeData data)
    {
        var contentNodeData = data as ContentNodeData;
        LoadNodeData(contentNodeData.nodeGuid, contentNodeData.choiceCount, contentNodeData.group, contentNodeData.hasChoice);
    }
    void InitializeComponents()
    {
        choiceManipulation = new VisualElement() {name = "choiceManipulation" };

        groupField = new ObjectField()
        {
            objectType = typeof(ContentGroup),
            allowSceneObjects = false
        };
        modifyBtn = new Button()
        {
            text = "View",
            name = "modifyBtn"
        };
        addChoiceBtn = new Button() { text = "Add", name = "addChoiceBtn" };
        removeChoiceBtn = new Button() { text = "Remove", name = "removeChoiceBtn" };
        hasChoiceField = new Toggle()
        {
            text = "Has Choice"
        };
        addGroupBtn = new Button()
        {
            text = "Add new group",
            name = "addGroupBtn"
        };
    }
}
