using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEditor;

[CreateAssetMenu(fileName = "New Graph", menuName = "New Graph")]
public class GraphDataContainer : ScriptableObject
{
    public EntryNodeData entryNodeData;
    public List<SubgraphNodeData> subgraphNodeDatas;
    public List<ContentNodeData> nodeDatas;
    public List<NodeEdgeData> nodeEdgeDatas;
    public Vector3 graphViewScale;
    public Vector3 graphViewPosition;

    public BaseNodeData GetNodeDataByGuid(string guid)
    {
        foreach(var i in nodeDatas)
        {
            if (i.nodeGuid == guid) return i;
        }
        foreach (var i in subgraphNodeDatas)
        {
            if (i.nodeGuid == guid) return i;
        }
        return entryNodeData;
    }
    public void SaveNode(BaseNode n)
    {
        var nodeData = CreateNodeData(n);
        if (n is EntryNode)
        {
            entryNodeData = nodeData as EntryNodeData;
            try
            {
                entryNodeData.firstNodeGuid = ((n.outputContainer.Children().ToList()[0] as Port)
                    .connections.ToList()[0].input.node as BaseNode).node_Guid;
            }
            catch { }          
        }
        if(n is SubgraphNode)
        {
            subgraphNodeDatas.Add(nodeData as SubgraphNodeData);
        }
        if(n is ContentNode)
        {
            nodeDatas.Add(nodeData as ContentNodeData);
        }
    }
    public void SaveEdge(Edge n)
    {
        nodeEdgeDatas.Add(CreateEdgeData(n));
    }
    public static BaseNodeData CreateNodeData(BaseNode n)
    {
        if (n is EntryNode)
            return new EntryNodeData()
            {
                position = n.GetPosition().position,
                nodeGuid = n.node_Guid,
            };
        if (n is SubgraphNode)
            return new SubgraphNodeData()
            {
                position = n.GetPosition().position,
                nodeGuid = n.node_Guid,
                data = (n as SubgraphNode).dataContainer
            };
        ContentNode tmpNode = n as ContentNode;
        //EditorUtility.SetDirty(tmpNode.group);
        return new ContentNodeData()
        {
            group = tmpNode.group,
            nodeGuid = tmpNode.node_Guid,
            choiceCount = tmpNode.hasChoice ? tmpNode.choiceCount : 0,
            position = n.GetPosition().position,
            hasChoice = tmpNode.hasChoice
        };
    }
    public static NodeEdgeData CreateEdgeData(Edge n)
    {
        return new NodeEdgeData()
        {
            startNodeGuid = (n.output.node as BaseNode).node_Guid,
            endNodeGuid = (n.input.node as BaseNode).node_Guid,
            startPortIndex = (n.output.node as BaseNode).outputPortList.IndexOf(n.output)
        };
    }
    private void OnEnable()
    {
        //entryNodeData = new EntryNodeData();
    }
}
[Serializable]
public class BaseNodeData
{
    public string nodeGuid;
    public Vector2 position;
}
[Serializable]
public class NodeEdgeData
{
    public string startNodeGuid;
    public string endNodeGuid;
    public int startPortIndex;
}
[Serializable]
public class ContentNodeData: BaseNodeData
{
    public ContentGroup group;
    public int choiceCount;
    public bool hasChoice;
}
[Serializable]
public class EntryNodeData: BaseNodeData
{
    public string firstNodeGuid;
   
}
[Serializable]
public class SubgraphNodeData: BaseNodeData
{
    public GraphDataContainer data;
}
[Serializable]
public class NodePortData
{
    public Port port;
    public string name;
}
