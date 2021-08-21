using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class SubgraphNode : BaseNode
{
    private GraphDataContainer _dataContainer;

    public GraphDataContainer dataContainer
    {
        get => _dataContainer;
        set
        {
            _dataContainer = value;
            dataContainerField.SetValueWithoutNotify(value);
        }
    }
    public ObjectField dataContainerField;

    public SubgraphNode(Vector2 pos, TestEditorWindow window, TestGraph graph)
    {
        this.graph = graph;
        this.window = window;
        SetPosition(new Rect(pos, defaultSize));
        node_Guid = Guid.NewGuid().ToString();
        Setup();
    }
    public SubgraphNode()
    {

    }
    void Setup()
    {
        style.width = 150;
        title = "Subgraph";



        dataContainerField = new ObjectField()
        {
            objectType = typeof(GraphDataContainer),
            allowSceneObjects = false,
            //label = "Data"
        };
        dataContainerField.RegisterValueChangedCallback(v =>
        {
            dataContainer = v.newValue as GraphDataContainer;
        });
        AddInputPort("In", Port.Capacity.Multi);
        mainContainer.Add(dataContainerField);
    }
    public override void LoadNodeData(BaseNodeData data)
    {
        var subgraphNodeData = data as SubgraphNodeData;
        if(subgraphNodeData.data != null) dataContainer = subgraphNodeData.data;
        node_Guid = subgraphNodeData.nodeGuid;
    }
}
