using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class EntryNode : BaseNode
{
    public EntryNode(Vector2 pos, TestEditorWindow window, TestGraph graph)
    {
        this.graph = graph;
        this.window = window;
        node_Guid = Guid.NewGuid().ToString();
        SetPosition(new Rect(pos, defaultSize));
        Setup();
    }
    public EntryNode()
    {

    }
    void Setup()
    {
        AddOutputPort("Entry", Port.Capacity.Single);
    }
    public override void LoadNodeData(BaseNodeData data)
    {
        var entryNodeData = data as EntryNodeData;
        node_Guid = entryNodeData.nodeGuid;
    }
}
