using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public static SystemManager ins;

    //public List<GameObject> keepObjects;

    private void Start()
    {
        ins = this;
        //DontDestroyOnLoad(this);
        //keepObjects.ForEach(n => DontDestroyOnLoad(n));
    }
    public void SwitchPanel(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf); 
    }

    
}
