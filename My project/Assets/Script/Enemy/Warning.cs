using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private void Start() {
        anim = GetComponent<Animator>();
    }
   public GameObject cua;
   public void WarningFire(){
       anim.SetTrigger("warning");
   }
}
