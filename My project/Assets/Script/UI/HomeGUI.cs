using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeGUI : MonoBehaviour
{
    public static HomeGUI instance;
    public GameObject MusicOff;
    public GameObject SoundOff;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    public GameObject HomePanel;
    public GameObject SettingPanel;

    void Start()
    {
        CheckMusicStart();
        CheckSoundStart();
        //PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckHideSettingPanel();
    }
    public void ShowSettingPanel(){
         if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            BgSound.Instance.PlayTap();
        }
        SettingPanel.SetActive(true);
        HomePanel.SetActive(false);
       
        Time.timeScale = 0;
    }
    public void HideSettingsPanel(){
         if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            BgSound.Instance.PlayTap();
        }
        SettingPanel.SetActive(false);
        HomePanel.SetActive(true);
     
        Time.timeScale = 1;
    }
   public void TurnOffMusic(){
       int isMusicOn = PlayerPrefs.GetInt("isMusicOn", 1);
       if(isMusicOn == 1){
            MusicOff.SetActive(true);
            BgSound.Instance.PauseMusic();
            PlayerPrefs.SetInt("isMusicOn", 0);
            PlayerPrefs.Save();
       }
       if(isMusicOn == 0){
            MusicOff.SetActive(false);
            BgSound.Instance.PlayMusic();
            PlayerPrefs.SetInt("isMusicOn", 1);
            PlayerPrefs.Save();
       }
   }
   public void CheckMusicStart(){
       if(PlayerPrefs.GetInt("isMusicOn", 1) == 1){
           BgSound.Instance.PlayMusic();
            MusicOff.SetActive(false);
       }
       else{
              MusicOff.SetActive(true);
       }
       
   }
   public void TurnOffSound(){
         int isSoundOn = PlayerPrefs.GetInt("isSoundOn", 1);
         if(isSoundOn == 1){
            SoundOff.SetActive(true);
            PlayerPrefs.SetInt("isSoundOn", 0);
            PlayerPrefs.Save();
         }
         if(isSoundOn == 0){
            SoundOff.SetActive(false);
            PlayerPrefs.SetInt("isSoundOn", 1);
            PlayerPrefs.Save();
         }
   }
   public void CheckSoundStart(){
         if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            SoundOff.SetActive(false);
         }
         else{
             SoundOff.SetActive(true);
         }
   }
   public void EasyLevel(){
        PlayerPrefs.SetInt("dokho", 0); 
        PlayerPrefs.Save();      
   }
   public void DifficultLevel(){
       PlayerPrefs.SetInt("dokho", 1); 
        PlayerPrefs.Save();
   }
   
}
