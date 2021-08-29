using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data.Common;
using TMPro;

public class DataLoader : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    public SpriteRenderer img;
    public DataSlot slot;
    public bool isMain;

    private void Awake()
    {        
        button = GetComponent<Button>();

        if(transform.parent != null)
        {
            button.onClick.AddListener(() => { SystemManager.ins.SwitchPanel(transform.parent.gameObject); });

        }

        button.onClick.AddListener(OnButtonClick);

        text = GetComponentInChildren<TextMeshProUGUI>();

        //Debug.Log(slot.isMain);
    }
    private void OnEnable()
    {
        slot.SetLoader(this);
        Setup();
    }

    public void OnButtonClick()
    {
        if (!slot.HasData())
        {
            Debug.Log("No Data");
            return;
        }
        TransitionManager.FadeTransition("out", img, Color.black, () =>
        {          
            slot.Load();
        });

    }
    public void Setup()
    {
        if (!slot.isMain)
        {
            if (slot.HasData())
                text.text = slot.saveDate;
            else
                text.text = "No Data";
        }
    }
   
}
