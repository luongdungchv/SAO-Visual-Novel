using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new Test SO", menuName = "new Test SO")]
public class TestEventSO : ScriptableObject
{
    public virtual void RunEvent()
    {
        Debug.Log("event called");
    }
}
