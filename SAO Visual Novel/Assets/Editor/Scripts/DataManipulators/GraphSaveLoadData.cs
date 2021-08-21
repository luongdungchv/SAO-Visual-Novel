using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEditor;
using System;


public class GraphSaveLoadData 
{
    public static event EventHandler OnSave;
    private TestGraph graph;
    List<Edge> edgeList => graph.edges.ToList();
    List<BaseNode> nodeList => graph.nodes.ToList().Where(n => n is BaseNode).Cast<BaseNode>().ToList();

    public GraphSaveLoadData(TestGraph graph)
    {
        this.graph = graph;
        
    }
    public void Save(GraphDataContainer dataContainer)
    {
        SaveEdges(dataContainer);
        SaveNode(dataContainer);
        SaveData();

        EditorUtility.SetDirty(dataContainer);
        AssetDatabase.SaveAssets();
    }
    public void Load(GraphDataContainer dataContainer)
    {
        LoadNodes(dataContainer);
    }
    void SaveEdges(GraphDataContainer dataContainer)
    {
        dataContainer.nodeEdgeDatas.Clear();
        
        edgeList.ForEach(n =>
        {
            dataContainer.SaveEdge(n);
        });
    }
    void SaveNode(GraphDataContainer dataContainer)
    {
        dataContainer.nodeDatas.Clear();
        dataContainer.subgraphNodeDatas.Clear();
        foreach (var n in nodeList)
        {
            
            dataContainer.SaveNode(n);
        }
    }
    void SaveData()
    {        
        foreach(var n in nodeList)
        {
            if (n is EntryNode || n is SubgraphNode) continue;

            ContentNode node = n as ContentNode;
            if (node.group == null) continue;
            List<SelectionButton> tmpChoiceList = new List<SelectionButton>();
            for (int i = 0; i < n.outputPortList.Count; i++)
            {
                SelectionButton choice = new SelectionButton();
                tmpChoiceList.Add(choice);

                List<Edge> connectionList = n.outputPortList[i].connections.ToList();

                if (connectionList.Count == 0) continue;

                var connectedNode = connectionList[0].input.node;
                if(connectedNode is ContentNode)
                {
                    if (node.hasChoice)
                    {
                        choice.group = (connectedNode as ContentNode).group;
                        choice.answer = node.group.selection.selections[i].answer;
                    }
                    else node.group.nextGroup = (connectedNode as ContentNode).group;
                }
                if(connectedNode is SubgraphNode)
                {
                    var castedConnectedNode = connectedNode as SubgraphNode;

                    if (castedConnectedNode.dataContainer == null) continue;

                    var targetContentNodeGuid = castedConnectedNode.dataContainer.entryNodeData.firstNodeGuid;
                    if (targetContentNodeGuid == "") continue;
                    var targetContentNodeData = castedConnectedNode.dataContainer.nodeDatas.First(j => j.nodeGuid == targetContentNodeGuid);
                    Debug.Log(targetContentNodeData.group);
                    if (node.hasChoice)
                    {
                        choice.group = targetContentNodeData.group;
                        choice.answer = node.group.selection.selections[i].answer;
                    }
                    else node.group.nextGroup = targetContentNodeData.group;
                }
            }
            node.group.selection.selections.Clear();
            if (node.hasChoice) node.group.selection.selections = tmpChoiceList.Where(j => j != null).ToList();
        }
    }
    
    void LoadNodes(GraphDataContainer dataContainer)
    {
        if(dataContainer.entryNodeData.nodeGuid != null && dataContainer.entryNodeData.nodeGuid.Length > 4)
        {
            var node = graph.CreateEntryNode(dataContainer.entryNodeData.position);
            node.LoadNodeData(dataContainer.entryNodeData);
            graph.AddElement(node);
        }
        if (dataContainer.subgraphNodeDatas != null)
        {
            foreach (var i in dataContainer.subgraphNodeDatas)
            {
                if(i.nodeGuid != "")
                {
                    var node = graph.CreateSubgraphNode(i.position);
                    //node.node_Guid = i.nodeGuid;
                    //node.dataContainer = i.data;
                    node.LoadNodeData(i);
                    graph.AddElement(node);
                } 
            }
        }

        if(dataContainer.nodeDatas != null)
        {
            foreach (var i in dataContainer.nodeDatas)
            {
                ContentNode node = graph.CreateContentNode(i.position, true);
                //node.LoadNodeData(i.nodeGuid, i.choiceCount, i.group, i.hasChoice);
                node.LoadNodeData(i);
                graph.AddElement(node);
            }
        }
        ConnectNodes(dataContainer);
    }
    void ConnectNodes(GraphDataContainer dataContainer)
    {
        if (dataContainer.nodeEdgeDatas == null) return;
        List<BaseNode> nodeList = graph.nodes.ToList().Where(n => n is BaseNode).Cast<BaseNode>().ToList();
        foreach (var i in dataContainer.nodeEdgeDatas)
        {
            foreach (var j in graph.nodes.ToList())
            {
                var node = j as BaseNode;
                if (node.node_Guid == i.startNodeGuid)
                {
                    var endNode = nodeList.First(n => n.node_Guid == i.endNodeGuid);
                    Edge connection = node.outputPortList[i.startPortIndex].ConnectTo(endNode.inputContainer[0] as Port);
                    graph.AddElement(connection);
                }
            }
        }
    }
}
