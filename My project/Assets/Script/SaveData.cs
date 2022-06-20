using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public  class SaveData : MonoBehaviour
{   
    public static SaveData instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }
    public int levelCur;
      
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
         AddLevelAndSave();
    }

    // Update is called once per frame
    void Update()
    {
          
    }
    public void AddLevelAndSave(){
        levelCur = PlayerPrefs.GetInt("levelCur", 0);
        if(levelCur < SceneManager.GetActiveScene().buildIndex){
            levelCur = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("levelCur", levelCur);
        }
        PlayerPrefs.Save();
    }
    public void CheckMusicOn(){
        
    }

}
