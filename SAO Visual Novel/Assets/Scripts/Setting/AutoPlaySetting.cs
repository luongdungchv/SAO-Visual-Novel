using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlaySetting :Setting
{
    public Toggle toggle;

    public override void OnSubmit(SettingData data)
    {
        data.isAutoPlay = toggle.isOn;
    }
    public override void OnInit(SettingData data)
    {
        toggle.isOn = data.isAutoPlay;
    }
}
