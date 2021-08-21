using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public SettingManager manager;
    public GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {       
        manager = SettingManager.ins;
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => manager.SwitchPanel(targetObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
