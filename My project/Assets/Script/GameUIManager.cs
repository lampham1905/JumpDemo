using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameUIManager : MonoBehaviour
{
    public GameObject GamePlayPanel;
    public GameObject VictoryPanel;
    public GameObject DeadPanel;
    public GameObject PausePanel;
    public GameObject SettingPanel;
    public GameObject LeftWind;
    public GameObject RightWind;
    public GameObject soundOff;
    public GameObject musicOff;
    public static GameUIManager instance;
     bool isShowSettingPanel = false;
     public TextMeshProUGUI LevelText;
    private void Awake() {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        GamePlayPanel.SetActive(true);
        LevelText.text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex).ToString();
        CheckMusicStart();
        CheckSoundStart();

    }

    // Update is called once per frame
   
    public void ShowVictoryPanel(){
        VictoryPanel.SetActive(true);
        GamePlayPanel.SetActive(false);
        Time.timeScale = 0;
    }
    public void ShowDeadPanel(){
        DeadPanel.SetActive(true);
        GamePlayPanel.SetActive(false);
        Time.timeScale = 0;
    }
    public void ShowPausePanel(){
        PausePanel.SetActive(true);
        GamePlayPanel.SetActive(false);
        PlayerController.instance.canJump = false;
    }
    public void HidePauseGamePanel(){
         PausePanel.SetActive(false);
        GamePlayPanel.SetActive(true);
       Invoke("ResumeGame", 0.01f);
    }
    //  void CanJumpCounters(){
    //     PlayerController.instance.canJump = true;
    // }
    public void ShowSettingPanel(){
    
        SettingPanel.SetActive(true);
        GamePlayPanel.SetActive(false);
       

    }
    public void HideSettingsPanel(){
        SettingPanel.SetActive(false);
        GamePlayPanel.SetActive(true);
    }
   
    void ResumeGame(){
       Time.timeScale = 1; 
    }
    public void ShowLeftWind(){
        LeftWind.SetActive(true);
        RightWind.SetActive(false);
    }
    public void ShowRightWind(){
        LeftWind.SetActive(false);
        RightWind.SetActive(true);
    }
    public void TurnOffSound(){
        int isSoundOn = PlayerPrefs.GetInt("isSoundOn", 1);
        if(isSoundOn == 1){
             soundOff.SetActive(true);
             PlayerPrefs.SetInt("isSoundOn", 0);
             PlayerPrefs.Save();
        }
        if(isSoundOn == 0){
             soundOff.SetActive(false);
             PlayerPrefs.SetInt("isSoundOn", 1);
             PlayerPrefs.Save();
        }
    }
    public void CheckSoundStart(){
        if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            soundOff.SetActive(false);
        }
        else{
            soundOff.SetActive(true);
        }
    }
    public void TurnOffMusic(){
        int isMusicOn = PlayerPrefs.GetInt("isMusicOn", 1);
        if(isMusicOn == 1){
             musicOff.SetActive(true);
             BgSound.Instance.PauseMusic();
             PlayerPrefs.SetInt("isMusicOn", 0);
             PlayerPrefs.Save();
        }
        if(isMusicOn == 0){
             musicOff.SetActive(false);
             BgSound.Instance.PlayMusic();
             PlayerPrefs.SetInt("isMusicOn", 1);
             PlayerPrefs.Save();
        }
    }
    public void CheckMusicStart(){
        if(PlayerPrefs.GetInt("isMusicOn", 1) == 1){
            musicOff.SetActive(false);
        }
        else{
               musicOff.SetActive(true);
        }
    }
}
