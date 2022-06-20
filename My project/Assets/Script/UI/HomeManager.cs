using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadSceneCurrent(){
        
          if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            BgSound.Instance.PlayTap();
        }
        int LevelCur = PlayerPrefs.GetInt("levelCur", 1);
        SceneManager.LoadScene("level" + LevelCur);
    }
}
