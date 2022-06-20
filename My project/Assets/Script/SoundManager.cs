using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] mysounds;
    private AudioSource musicSource;
    private AudioSource tapSound;
    private AudioSource jumpSound;
    private AudioSource windSound;
    private AudioSource dieSound;
    private AudioSource hitSound;
    // Start is called before the first frame update
    void Start()
    {
        tapSound = mysounds[0];
        jumpSound = mysounds[1];
        windSound = mysounds[2];
        dieSound = mysounds[3];
        hitSound = mysounds[4];
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMusic(bool isPlaying){
            musicSource.Play();
       }
    public void PlayTap(bool isPlaying){
            tapSound.Play();
    }
    public void PlayWind(bool isPlaying){    
            windSound.Play();
    }
    public void Jump(bool isPlaying){
            jumpSound.Play();
    }


}
