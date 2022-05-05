using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }
    public int currEndPoint;
    public GameObject endPointCurrent;
    public GameObject[] endPoint;
    // Start is called before the first frame update
    void Start()
    {
        currEndPoint = 0;
        endPointCurrent = endPoint[currEndPoint];
    }

    // Update is called once per frame
    void Update()
    {
        CheckEndPoint();
    }
    public void CheckEndPoint(){
        endPointCurrent = endPoint[currEndPoint];
    }
}
