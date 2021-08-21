using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class EdgeConnectorListener : IEdgeConnectorListener
{
    public TestGraph graph;
    public TestEditorWindow window;
    public EdgeConnectorListener(TestGraph graph, TestEditorWindow window)
    {
        this.graph = graph;
        this.window = window;
    }
    public void OnDrop(GraphView graphView, Edge edge)
    {
        
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {

        //Vector2 graphMousePos = graph.contentViewContainer.WorldToLocal(position);
        //var node = graph.CreateContentNode(graphMousePos, false);
        //graph.AddElement(node);
        //var newEdge = edge.output.ConnectTo(node.inputPortList[0]);
        //graph.AddElement(newEdge);
        //Debug.Log("dfalsdhf");
        OpenSearchWindow(edge.output);
    }
    void OpenSearchWindow(Port startPort)
    {
        var searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Configure(window, graph, startPort);
        SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
    }
}
