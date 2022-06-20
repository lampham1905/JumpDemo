using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseBtn : Button
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    
    public override void OnPointerDown(PointerEventData eventData)
    {
         base.OnPointerDown(eventData);
         if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            BgSound.Instance.PlayTap();
        }
       
        Time.timeScale = 0;
        GameUIManager.instance.ShowPausePanel();
        //PlayerController.instance.canJump = false;
        
}
}
