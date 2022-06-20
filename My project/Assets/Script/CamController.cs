using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController instance;
    private Camera cam;
     private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
     float lerpTime = 2f;
     float xOffset = 2f;
    bool m_canLerp ;
    float m_lerpXDist;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeft();
        CheckScreen();
    }
    private void MoveLeft(){
        float xPos = transform.position.x;
        xPos = Mathf.Lerp(xPos, m_lerpXDist ,lerpTime * Time.deltaTime);
         transform.position = new Vector3(xPos, transform.position.y, transform.position.z);

        if(transform.position.x >= (m_lerpXDist - xOffset))
        {
            m_canLerp = false;
        }
    }
     public void LerpTrigeer(float dist)
    {
        m_canLerp = true;
        m_lerpXDist = dist;
    }
    private void CheckScreen()
    {
        if(cam.aspect > 0.5f)
        {
            cam.orthographicSize = 5f;
            

        }
        if (cam.aspect <.5f)
        {
            cam.orthographicSize = 5.2f;
           
        } 
    }
     
}
