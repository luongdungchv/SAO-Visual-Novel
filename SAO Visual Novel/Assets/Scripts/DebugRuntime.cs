using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugRuntime : MonoBehaviour
{
    public static DebugRuntime ins;
    public Object testObject;
    public DataSlot testSlot;
    public Text logger;
    private void Start()
    {
        ins = this;
        logger.text = testSlot.GetJsonData();
    }
    public void Log(object target)
    {
        logger.text = target.ToString();
    }
}
