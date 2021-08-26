using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BaseNode : Node
{
    public string node_Guid;
    public List<Port> outputPortList;
    public List<Port> inputPortList;
    protected TestGraph graph;
    protected TestEditorWindow window;
    protected Vector2 defaultSize = new Vector2(200, 250);

    public BaseNode()
    {
        outputPortList = new List<Port>();
        inputPortList = new List<Port>();
    }
    public void AddOutputPort(string name, Port.Capacity cap = Port.Capacity.Single)
    {
        Port output = GetPortInstance(Direction.Output, cap);
        EdgeConnector<Edge> customConnector = new EdgeConnector<Edge>(new EdgeConnectorListener(graph, window));
        output.AddManipulator(customConnector);
        output.portName = name;
        outputContainer.Add(output);
        outputPortList.Add(output);
    }
    public void AddInputPort(string name, Port.Capacity cap = Port.Capacity.Multi)
    {
        Port input = GetPortInstance(Direction.Input, cap);
        input.portName = name;
        inputContainer.Add(input);
        inputPortList.Add(input);
        //portList.Add(input);
    }
    public Port GetPortInstance(Direction dir, Port.Capacity cap = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, dir, cap, typeof(float));
        //return new CustomPort(Orientation.Horizontal, dir, cap, typeof(float));

    }
    public virtual void LoadNodeData(BaseNodeData data)
    {

    }
}
