using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HideSettingBtn : Button
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
          if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            BgSound.Instance.PlayTap();
        }
        Time.timeScale = 1;
        GameUIManager.instance.HideSettingsPanel();
        //PlayerController.instance.canJump = true;
    }
}
