using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using System;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEngine.AddressableAssets;

public class TestGraph : GraphView
{
    private string styleSheetName = "GraphViewStyleSheet";
    public TestEditorWindow window;
    private NodeSearchWindow searchWindow;
    public static BaseNodeData copiedNodeData;
    public static CopiedData copiedData;
    public List<ContentGroup> pendingSaveGroups;

    public TestGraph(TestEditorWindow input)
    {
        window = input;
        
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheetName));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger()); 
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());
        
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        AddSearchWindow();

        serializeGraphElements = (s) =>
        {
            copiedData = new CopiedData();
            
            Debug.Log(selection.Count);
            selection.ForEach(n =>
            {
                if (n is BaseNode)
                {
                    var clonedNodeData = GraphDataContainer.CreateNodeData(n as BaseNode);
                    copiedData.AddNodeData(clonedNodeData);
                }
                if(n is Edge)
                {
                    var clonedEdgeData = GraphDataContainer.CreateEdgeData(n as Edge);
                    copiedData.AddEdgeData(clonedEdgeData);
                }
            });
            Debug.Log($"{copiedData.nodeDatas.Count} {copiedData.edgeDatas.Count}");
            //ClearSelection();
            return "gsdfg";
        };
        unserializeAndPaste = (s, e) =>
        {
            ClearSelection();
            var centerPos = contentViewContainer.WorldToLocal(layout.center);
            copiedData.CalculateMiddlePoint();
            List<BaseNode> clonedNodeList = new List<BaseNode>();
            List<Edge> clonedEdgeList = new List<Edge>();
            copiedData.nodeDatas.ForEach(n =>
            {
                var renderPos = centerPos + n.position;
                BaseNode node = new BaseNode();
                if (n is EntryNodeData)
                {
                    node = CreateEntryNode(renderPos);
                    node.LoadNodeData(n);
                    AddElement(node);
                    clonedNodeList.Add(node);
                }
                if (n is SubgraphNodeData)
                {
                    node = CreateSubgraphNode(renderPos);
                    node.LoadNodeData(n);
                    AddElement(node);
                    clonedNodeList.Add(node);
                }
                if (n is ContentNodeData)
                {
                    node = CreateContentNode(renderPos, true);
                    node.LoadNodeData(n);
                    AddElement(node);
                    clonedNodeList.Add(node);
                }
                //AddToSelection(node);
            });
            copiedData.edgeDatas.ForEach(n =>
            {
                var edge = new Edge();               
                try
                {
                    var startNode = clonedNodeList.First(i => i.node_Guid == n.startNodeGuid);
                    var endNode = clonedNodeList.First(i => i.node_Guid == n.endNodeGuid);
                    edge = startNode.outputPortList[n.startPortIndex].ConnectTo(endNode.inputPortList[0]);
                    AddElement(edge);
                    clonedEdgeList.Add(edge);
                }
                catch
                {

                }
                
            });

            clonedNodeList.ForEach(n =>
            {
                n.node_Guid = Guid.NewGuid().ToString();
                //AddToSelection(n);
            });
            clonedEdgeList.ForEach(n => AddToSelection(n));
            Debug.Log(selection[0]);
        };
        TestAddressable();
    }

    

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        
        List<Port> portList = new List<Port>();
        Port startPortView = startPort;
        
        ports.ForEach(n =>
        {
            Port portView = n;
            if(startPortView != portView && startPortView.node != portView.node && startPortView.direction != portView.direction)
            {
                portList.Add(n);               
            }
        });
        return portList;
    }

    private void AddSearchWindow()
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Configure(window, this, null);
       
        nodeCreationRequest = ctx => SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), searchWindow);
    }

    public ContentNode CreateContentNode(Vector2 pos, bool isLoad)
    {
        ContentNode res = new ContentNode(pos, window, this);
        if (!isLoad) res.AddOutputPort("Next Group", Port.Capacity.Single);
        return res;
    }
    public EntryNode CreateEntryNode(Vector2 pos)
    {
        var res = new EntryNode(pos, window, this);
        return res;
    }
    public SubgraphNode CreateSubgraphNode(Vector2 pos)
    {
        var res = new SubgraphNode(pos, window, this);
        return res;
    }
    void TestAddressable()
    {
        var op = Addressables.LoadAssetAsync<GameObject>("CharacterPrefabs/hoshi2 1.prefab");
        var res = op.WaitForCompletion();
    }
     
}
public class CopiedData 
{
    public List<BaseNodeData> nodeDatas; 
    public List<NodeEdgeData> edgeDatas;
    public CopiedData()
    {
        nodeDatas = new List<BaseNodeData>();
        edgeDatas = new List<NodeEdgeData>(); 
    }
    public void AddNodeData(BaseNodeData data)
    {
        nodeDatas.Add(data);
    }
    public void AddEdgeData(NodeEdgeData data)
    {
        edgeDatas.Add(data);
    }
    public Vector2 CalculateMiddlePoint()
    {
        Vector2 res = Vector2.zero;
        nodeDatas.ForEach(n => 
        {
            res += n.position;
        });
        res /= nodeDatas.Count;
        nodeDatas.ForEach(n =>
        {
            n.position -= res;
        });
        return res;
    }
}
