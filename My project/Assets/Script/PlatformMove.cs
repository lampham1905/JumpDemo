using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private bool isMoveUp = true;
    private bool isMoveLeft = true;
    public float  bottomPoint;
    public float topPoint;
    public float leftPoint;
    public float rightPoint;
    public bool IsMoveUpDown = false;
    public bool IsMoveLeftRight = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       

    }

    // Update is called once per frame
    void Update()
    {
        if(IsMoveUpDown){
            MoveUpDown();
        }
        if(IsMoveLeftRight){
            MoveLeftRight();
        }
    }
    private  void MoveUpDown(){
        if(isMoveUp){
            if(transform.position.y > bottomPoint){
                    rb.velocity = new Vector2 (0, -speed);
            }
            else{
                isMoveUp = false;
            }
        }
        else{
            if(transform.position.y < topPoint){
                  rb.velocity = new Vector2(0, speed);  
            }
            else{
                isMoveUp = true;
            }
    }
    }
    private void MoveLeftRight(){
        if(isMoveLeft){
            if(transform.position.x > leftPoint){
                rb.velocity = new Vector2(-speed, 0);
            }
            else{
                isMoveLeft = false;
            }
        }
        else{
            if(transform.position.x < rightPoint){
                rb.velocity = new Vector2(speed, 0);
            }
            else{
                isMoveLeft = true;
            }
        }
    }
}
