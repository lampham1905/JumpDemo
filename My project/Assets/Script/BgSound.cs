using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSound : MonoBehaviour
{
    public AudioSource[] mysounds;
  
    private AudioSource tapSound;
    private AudioSource jumpSound;
    private AudioSource windSound;
    private AudioSource dieSound;
    private AudioSource hitSound;
    private AudioSource jumpDownSound;
    public AudioSource musicSource;
    void Start()
    {
        
        if(PlayerPrefs.GetInt("isMusicOn", 1) == 1){
            PlayMusic();
        }

        tapSound = mysounds[0];
        jumpSound = mysounds[1];
        windSound = mysounds[2];
        dieSound = mysounds[3];
        hitSound = mysounds[4];
        jumpDownSound = mysounds[5];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private static BgSound instance = null;
    public static BgSound Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake() {
        if(instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void PlayMusic(){
            musicSource.Play();
       }
    public void PauseMusic(){
        musicSource.Pause();
    }
    public void ResumeMusic(){
        musicSource.UnPause();
    }
    
    public void PlayTap(){
            tapSound.Play();
    }
    public void PlayWind(){    
            windSound.Play();
    }
    public void PlayJumpUp(){
            jumpSound.Play();
    }
    public void PlayJumpDown(){
            jumpDownSound.Play();
    }
    public void PlayHit(){
        hitSound.Play();
    }
    public void PlayDie(){
        dieSound.Play();
    }
    
  
}
