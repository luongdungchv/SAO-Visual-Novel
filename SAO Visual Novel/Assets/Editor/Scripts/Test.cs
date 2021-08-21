using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(TestInherit)), CanEditMultipleObjects]
public class Test : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    base.OnInspectorGUI();
    //    serializedObject.FindProperty("id").intValue = 2;
    //    serializedObject.ApplyModifiedProperties();
    //}
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement container = new VisualElement();
        TextField field = new TextField();

        container.Add(field);

        return container;
    }
}
