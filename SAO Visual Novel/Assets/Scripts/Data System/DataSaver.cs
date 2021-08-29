using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DataSaver : MonoBehaviour
{
    public int slotIndex;
    public DataSlot slot;
    Button button;
    TextMeshProUGUI dateText;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => { slot.Save(); });
        button.onClick.AddListener(() => { SystemManager.ins.SwitchPanel(transform.parent.gameObject); });
        
    }
    void OnEnable()
    {
        dateText = transform.GetComponentInChildren<TextMeshProUGUI>();
        slot.SetSaver(this);
        Setup();
    }
    public void Setup()
    {
        Debug.Log("Setup");
        if (slot.HasData())
            dateText.text = slot.saveDate;
        else
            dateText.text = "No Data";
    }
}
