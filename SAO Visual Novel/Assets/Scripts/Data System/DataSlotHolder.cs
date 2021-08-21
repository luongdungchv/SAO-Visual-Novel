using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataSlotHolder : MonoBehaviour
{
    public static DataSlotHolder ins;
    public List<DataSlot> slots;
    public int slotIndex;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    

    private void Start()
    {
        ins = this;
        DontDestroyOnLoad(this.gameObject);
        slots.ForEach(n =>
        {
            //EditorUtility.SetDirty(n);
            //Undo.RecordObject(n, "slot");
            //n.CheckData();
        });
        //Debug.Log(PlayerPrefs.GetString(slots[2].address));
        btn1.onClick.AddListener(() => { PlayerPrefs.SetString("slot2", DateTime.Now.ToString()); });
        btn3.onClick.AddListener(() => { PlayerPrefs.SetInt("slot2", 3); });

        btn2.onClick.AddListener(() => { Debug.Log(PlayerPrefs.GetString("slot2")); });
        
    }

    //public void LoadData()
    //{
    //    DataSlot slot = slots[slotIndex];
    //    ContentManager manager = ContentManager.ins;
        
    //    manager.group = slot.group;
    //    manager.GenerateContentQueue(slot.GetContentIndex());
    //    //manager.RenderContent(manager.contentQueue.Dequeue());
    //}

    //public void SaveData(int index)
    //{
    //    DataSlot slot = slots[index];
    //    ContentManager manager = ContentManager.ins;
    //    slot.SetSavedDate(DateTime.Now.ToString());

    //    slot.group = manager.group;
    //    slot.SetContentIndex(manager.currentIndex);
    //    slot.CheckData();
    //}
    
}
