using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuoiHoaAnThit : MonoBehaviour
{
    // Start is called before the first frame update
    bool IsEnoughLengh = false;
    public bool canAttack = true;
    void Start()
    {
        //StartCoroutine(Attack(3f));
       InvokeRepeating("CanAttack", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
       if(canAttack) {
           Lengthen();
       }
       if(transform.localScale.y < 0) {
           
           canAttack = false;
       }
    
    }
    private IEnumerator AttackPlayer(float waitTIme){
        while(true){
            yield return new WaitForSeconds(waitTIme);
           Lengthen();
        }
    }
    private IEnumerator Attack(float waitTIme){
        while(true){
            yield return new WaitForSeconds(waitTIme);
            StartCoroutine(AttackPlayer(0.0000001f));
            IsEnoughLengh = false;
            
        }
    }
    private void Lengthen(){
        if(transform.localScale.y >= 10) IsEnoughLengh = true;
        if(!IsEnoughLengh){
            transform.localScale += new Vector3(0, 0.05f, 0);
        }
        if(IsEnoughLengh  && transform.localScale.y > 0){
            transform.localScale -= new Vector3(0, 0.05f, 0);
        }
    } 
    private void CanAttack(){
        canAttack = true;
        IsEnoughLengh = false;
    }
}
